﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Repository;
using Mix.Identity.Constants;
using Mix.Identity.Dtos;
using Mix.Identity.Helpers;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.ViewModels;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Models;
using Mix.Shared.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Identity.Services
{
    public class MixIdentityService
    {
        private readonly UserManager<MixUser> _userManager;
        private readonly SignInManager<MixUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MixAppSettingService _mixAppSettingService;
        private readonly Repository<MixCmsAccountContext, AspNetRoles, Guid> _roleRepo;
        private readonly Repository<MixCmsAccountContext, RefreshTokens, Guid> _refreshTokenRepo;
        public readonly MixIdentityHelper _idHelper;
        public List<RoleViewModel> Roles { get; set; }
        public MixIdentityService(
            UserManager<MixUser> userManager,
            SignInManager<MixUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            MixIdentityHelper helper,
            MixAppSettingService mixService,
            Repository<MixCmsAccountContext, AspNetRoles, Guid> roleRepo, 
            Repository<MixCmsAccountContext, RefreshTokens, Guid> refreshTokenRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _idHelper = helper;
            _mixAppSettingService = mixService;
            _roleRepo = roleRepo;
            LoadRoles();
            _refreshTokenRepo = refreshTokenRepo;
        }

        public async Task<JObject> Login(LoginViewModel model)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(
                model.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true).ConfigureAwait(false);

            if (result.IsLockedOut)
            {
                throw new MixException(MixErrorStatus.Badrequest, "This account has been locked out, please try again later.");
            }

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
                return await GetAuthData(user, model.RememberMe);
            }
            else
            {
                throw new MixException(MixErrorStatus.Badrequest, "Login failed");
            }
        }

        public async Task<JObject> GetAuthData(MixUser user, bool rememberMe)
        {
            var rsaKeys = RSAEncryptionHelper.GenerateKeys();
            var aesKey = AesEncryptionHelper.GenerateCombinedKeys(256);
            var token = await GenerateAccessTokenAsync(user, rememberMe, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
            if (token != null)
            {
                var plainText = JObject.FromObject(token).ToString(Formatting.None).Replace("\r\n", string.Empty);
                var encryptedInfo = AesEncryptionHelper.EncryptString(plainText, aesKey);

                var resp = new JObject()
                        {
                            new JProperty(MixEncryptKeywords.AESKey, aesKey),
                            new JProperty(MixEncryptKeywords.RSAKey, rsaKeys[MixConstants.CONST_RSA_PRIVATE_KEY]),
                            new JProperty(MixEncryptKeywords.Message, encryptedInfo)
                        };
                return resp;
            }
            return default;
        }

        public async Task<AccessTokenViewModel> GenerateAccessTokenAsync(MixUser user, bool isRemember, string aesKey, string rsaPublicKey)
        {
            try
            {
                var dtIssued = DateTime.UtcNow;
                var dtExpired = dtIssued.AddMinutes(_mixAppSettingService.AuthConfigurations.AccessTokenExpiration);
                var dtRefreshTokenExpired = dtIssued.AddMinutes(_mixAppSettingService.AuthConfigurations.RefreshTokenExpiration);
                var refreshTokenId = Guid.Empty;
                var refreshToken = Guid.Empty;
                if (isRemember)
                {
                    refreshToken = Guid.NewGuid();
                    RefreshTokenViewModel vmRefreshToken = new RefreshTokenViewModel(
                                new RefreshTokens()
                                {
                                    Id = refreshToken,
                                    Email = user.Email,
                                    IssuedUtc = dtIssued,
                                    ClientId = _mixAppSettingService.AuthConfigurations.ClientId,
                                    Username = user.UserName,
                                    ExpiresUtc = dtRefreshTokenExpired
                                });

                    var saveRefreshTokenResult = await vmRefreshToken.SaveAsync();
                    refreshTokenId = saveRefreshTokenResult;
                }

                AccessTokenViewModel token = new AccessTokenViewModel()
                {
                    AccessToken = await _idHelper.GenerateTokenAsync(
                        user, dtExpired, refreshToken, aesKey, rsaPublicKey, _mixAppSettingService.AuthConfigurations),
                    RefreshToken = refreshTokenId,
                    TokenType = _mixAppSettingService.AuthConfigurations.TokenType,
                    ExpiresIn = _mixAppSettingService.AuthConfigurations.AccessTokenExpiration,
                    Issued = dtIssued,
                    Expires = dtExpired,
                    LastUpdateConfiguration = _mixAppSettingService.GetConfig<DateTime?>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.LastUpdateConfiguration)
                };
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<JObject> ExternalLogin(RegisterExternalBindingModel model)
        {
            var verifiedAccessToken = await _idHelper.VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken, _mixAppSettingService.AuthConfigurations);
            if (verifiedAccessToken != null)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                // return local token if already register
                if (user != null)
                {
                    return await GetAuthData(user, true);
                }
                else if (!string.IsNullOrEmpty(model.Email))// register new account
                {
                    user = new MixUser()
                    {
                        Email = model.Email,
                        UserName = model.Email
                    };
                    await _userManager.CreateAsync(user);
                    return await GetAuthData(user, true);
                }
                else
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Login Failed");
                }
            }
            return default;
        }

        public async Task<JObject> RenewTokenAsync(RenewTokenDto refreshTokenDto)
        {
            JObject result = new();
            var oldToken = await _refreshTokenRepo.GetSingleViewAsync<RefreshTokenViewModel>(t => t.Id == refreshTokenDto.RefreshToken);
            if (oldToken != null)
            {
                if (oldToken.ExpiresUtc > DateTime.UtcNow)
                {

                    var principle = _idHelper.GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken, _mixAppSettingService.AuthConfigurations);
                    if (principle != null && oldToken.Username == _idHelper.GetClaim(principle, MixClaims.Username))
                    {
                        var user = await _userManager.FindByEmailAsync(oldToken.Email);
                        await _signInManager.SignInAsync(user, true).ConfigureAwait(false);

                        var token = await GetAuthData(user, true);
                        if (token != null)
                        {
                            await oldToken.DeleteAsync();
                            result = token;
                        }
                    }
                    else
                    {
                        throw new MixException(MixErrorStatus.Badrequest, "Invalid Token");
                    }

                    return result;
                }
                else
                {
                    await oldToken.DeleteAsync();
                    throw new MixException(MixErrorStatus.Badrequest, "Token expired");
                }
            }
            else
            {
                throw new MixException(MixErrorStatus.Badrequest, "Token expired");
            }
        }

        public bool CheckEndpointPermission(ClaimsPrincipal user, PathString path, string method)
        {
            return true;

            // TODO: 
            //var roles = _idHelper.GetClaims(user, MixClaims.Role);
            //if (roles.Any(r => r == MixDefaultRoles.SuperAdmin || r == MixDefaultRoles.Admin))
            //{
            //    return true;
            //}
            //var endpoint = $"{method}-{path}";
            //var role = Roles.Find(r => r.Name == roles.First());
            //return role.MixPermissions.Any(
            //        p => p.Property<JArray>("endpoints")
            //                .Any(e => new Regex(e["endpoint"].Value<string>()).Match(path).Success
            //                        && e["method"].Value<string>() == method.ToUpper())
            //        );
        }

        public async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(MixExternalLoginProviders provider, string accessToken, MixAuthenticationConfigurations appConfigs)
        {
            ParsedExternalAccessToken parsedToken = null;

            string verifyTokenEndPoint;
            switch (provider)
            {
                case MixExternalLoginProviders.Facebook:
                    //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                    //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook

                    var appToken = $"{appConfigs.Facebook.AppId}|{appConfigs.Facebook.AppSecret}";
                    verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
                    break;
                case MixExternalLoginProviders.Google:
                    verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
                    break;
                case MixExternalLoginProviders.Twitter:
                case MixExternalLoginProviders.Microsoft:
                default:
                    return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = JObject.Parse(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == MixExternalLoginProviders.Facebook)
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(appConfigs.Facebook.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == MixExternalLoginProviders.Google)
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(appConfigs.Google.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                }

            }

            return parsedToken;
        }

        public async Task<string> GenerateTokenAsync(
            MixUser user,
            DateTime expires,
            string refreshToken,
            string aesKey,
            string rsaPublicKey,
            MixAuthenticationConfigurations appConfigs)
        {
            List<Claim> claims = await GetClaimsAsync(user, appConfigs);
            claims.AddRange(new[]
                {
                    new Claim(MixClaims.Id, user.Id.ToString()),
                    new Claim(MixClaims.Username, user.UserName),
                    new Claim(MixClaims.RefreshToken, refreshToken),
                    new Claim(MixClaims.AESKey, aesKey),
                    new Claim(MixClaims.RSAPublicKey, rsaPublicKey)
                });

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: appConfigs.Issuer,
                audience: appConfigs.Audience,
                claims: claims,
                notBefore: expires.AddMinutes(-appConfigs.AccessTokenExpiration),
                expires: expires,
                signingCredentials: new SigningCredentials(JwtSecurityKey.Create(appConfigs.SecretKey), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public async Task<List<Claim>> GetClaimsAsync(MixUser user, MixAuthenticationConfigurations appConfigs)
        {
            List<Claim> claims = new List<Claim>();
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var claim in user.Claims)
            {
                claims.Add(CreateClaim(claim.ClaimType, claim.ClaimValue));
            }

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }

        public Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

        public string GetClaim(ClaimsPrincipal User, string claimType)
        {
            return User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }

        public IEnumerable<string> GetClaims(ClaimsPrincipal User, string claimType)
        {
            return User.Claims.Where(c => c.Type == claimType).Select(c => c.Value);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, MixAuthenticationConfigurations appConfigs)
        {
            var tokenValidationParameters = GetValidationParameters(appConfigs, false);
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }

        public TokenValidationParameters GetValidationParameters(MixAuthenticationConfigurations appConfigs, bool validateLifetime)
        {
            return new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,
                ValidateIssuer = appConfigs.ValidateIssuer,
                ValidateAudience = appConfigs.ValidateAudience,
                ValidateLifetime = validateLifetime,
                ValidateIssuerSigningKey = appConfigs.ValidateIssuerSigningKey,
                IssuerSigningKey = JwtSecurityKey.Create(appConfigs.SecretKey)
            };
        }

        public static class JwtSecurityKey
        {
            public static SymmetricSecurityKey Create(string secret)
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            }
        }

        private void LoadRoles()
        {
            if (!_mixAppSettingService.GetConfig<bool>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.IsInit))
            {
                Roles = _roleRepo.GetListViewAsync<RoleViewModel>(m => true).GetAwaiter().GetResult();
                using (var ctx = new MixCmsContext())
                {
                    var transaction = ctx.Database.BeginTransaction();
                    // TODO:
                    //Roles.ForEach(m => m.LoadMixPermissions(ctx, transaction).GetAwaiter().GetResult());
                }
            }
        }
    }
}