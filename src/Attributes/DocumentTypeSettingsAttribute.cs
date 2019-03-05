﻿using System;

namespace EOls.UmbracoContentApi.Attributes
{
    public class DocumentTypeSettingsAttribute : Attribute
    {
        public bool Hide { get; set; }
        public string[] HideProperties { get; set; }
    }
}