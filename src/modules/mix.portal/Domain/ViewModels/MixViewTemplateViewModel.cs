﻿using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using Mix.Shared.Enums;

namespace Mix.Portal.Domain.ViewModels
{
    [GenerateRestApiController]
    public class MixViewTemplateViewModel 
        : ViewModelBase<MixCmsContext, MixTemplate, int, MixViewTemplateViewModel>
    {
        #region Properties

        public string Content { get; set; }
        public string Extension { get; set; }
        public string FileFolder { get; set; }
        public string FileName { get; set; }
        public MixTemplateFolderType FolderType { get; set; }
        public string Scripts { get; set; }
        public string SpaContent { get; set; }
        public string MobileContent { get; set; }
        public string Styles { get; set; }

        #endregion

        #region Contructors

        public MixViewTemplateViewModel()
        {
        }

        public MixViewTemplateViewModel(MixTemplate entity,
            MixCacheService cacheService = null,
            UnitOfWorkInfo uowInfo = null) : base(entity, cacheService, uowInfo)
        {
        }

        public MixViewTemplateViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        #endregion

        #region Overrides

        #endregion
    }
}