﻿using Microsoft.AspNetCore.Mvc;
using Mix.Database.Services;
using Mix.Lib.Services;
using Mix.Shared.Services;

namespace Mixcore.Domain.Bases
{
    public class MvcBaseController : MixControllerBase
    {
        protected UnitOfWorkInfo _uow;
        protected readonly MixCmsContext _context;
        protected readonly TranslatorService _translator;
        protected readonly MixDatabaseService _databaseService;
        public MvcBaseController(
            IPSecurityConfigService ipSecurityConfigService,
            MixService mixService,
            TranslatorService translator,
            MixDatabaseService databaseService,
            MixCmsContext context) : base(mixService, ipSecurityConfigService)
        {
            _context = context;
            _uow = new(_context);
            _translator = translator;
            _databaseService = databaseService;
            _context = context;
        }

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (GlobalConfigService.Instance.AppSettings.IsInit)
            {
                isValid = false;
                if (string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = "Init";
                }
                else
                {
                    var status = GlobalConfigService.Instance.AppSettings.InitStatus;
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        #region Helper
        protected async Task<IActionResult> Page(int pageId, string keyword = null)
        {
            // Home Page
            var pageRepo = PageContentViewModel.GetRepository(_uow);
            var page = await pageRepo.GetSingleAsync(pageId);
            ViewData["Title"] = page.SeoTitle;
            ViewData["Description"] = page.SeoDescription;
            ViewData["Keywords"] = page.SeoKeywords;
            ViewData["Image"] = page.Image;
            ViewData["Layout"] = page.Layout.FilePath;
            ViewData["BodyClass"] = page.ClassName;
            ViewData["ViewMode"] = MixMvcViewMode.Page;
            ViewData["Keyword"] = keyword;

            ViewBag.viewMode = MixMvcViewMode.Page;
            return View(page);
        }
        #endregion
    }
}