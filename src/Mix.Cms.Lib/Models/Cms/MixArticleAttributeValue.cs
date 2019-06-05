﻿using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixArticleAttributeValue
    {
        public string Id { get; set; }
        public string Specificulture { get; set; }
        public string DataId { get; set; }
        public string AttributeName { get; set; }
        public int ArticleId { get; set; }
        public double? DoubleValue { get; set; }
        public int? IntegerValue { get; set; }
        public string StringValue { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public bool? BooleanValue { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public virtual MixArticleAttributeData Data { get; set; }
    }
}