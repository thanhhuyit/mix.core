﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Shared.Constants;
using Mix.Lib.Controllers;
using Mix.Shared.Enums;
using Mix.Lib.Services;
using System.Threading.Tasks;
using Mix.Shared.Services;
using Mix.Theme.Domain.Dtos;
using Mix.Theme.Domain.Enums;
using Mix.Theme.Domain.Services;
using Mix.Identity.Models.AccountViewModels;

namespace Mix.Theme.Controllers.v2
{
    [Route("api/v2/mix-theme/setup")]
    public class InitController : MixApiControllerBase
    {
        private readonly InitCmsService _initCmsService;

        public InitController(ILogger<MixApiControllerBase> logger,
            MixAppSettingService appSettingService,
            MixService mixService,
            TranslatorService translator,
            InitCmsService initCmsService
            ) : base(logger, appSettingService, mixService, translator)
        {
            _initCmsService = initCmsService;
        }


        #region Post

        /// <summary>
        /// When status = 0
        ///     - Init Cms Database
        ///     - Init Cms Site
        ///     - Init Selected Culture as default
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("init-site")]
        public async Task<ActionResult<bool>> InitSite([FromBody] InitCmsDto model)
        {
            if (model != null
                && _appSettingService.GetConfig<int>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.InitStatus) == 0)
            {
                await _initCmsService.InitSiteAsync(model);
                _appSettingService.SetConfig(
                    MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.InitStatus, InitStep.InitSite, true);
                return NoContent();
            }
            return BadRequest();
        }
        
        /// <summary>
        /// When status = 1
        ///     - Init Account Database
        ///     - Init Superadmin Account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("init-account")]
        public async Task<ActionResult<bool>> InitAccount([FromBody] RegisterViewModel model)
        {
            if (model != null
                && _appSettingService.GetEnumConfig<InitStep>(
                    MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.InitStatus) == InitStep.InitSite)
            {
                await _initCmsService.InitAccountAsync(model);
                _appSettingService.SetConfig(
                    MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.InitStatus, InitStep.InitAccount, true);
                return NoContent();
            }
            return BadRequest();
        }




        #endregion Helpers
    }
}