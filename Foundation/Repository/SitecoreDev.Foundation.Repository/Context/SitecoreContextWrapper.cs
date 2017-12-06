﻿using System.Collections.Specialized;
using Sitecore.Mvc.Presentation;

namespace SitecoreDev.Foundation.Repository.Context
{
    public class SitecoreContextWrapper : IContextWrapper
    {
        public bool IsExperienceEditor { get { return Sitecore.Context.PageMode.IsExperienceEditor; } }

        public string GetParameterValue(string key)
        {
            var value = string.Empty;
            var parameters = RenderingContext.Current.Rendering.Parameters;
            if (parameters != null /*&& parameters.Count() > 0*/)
                value = parameters[key];
            return value;
        }
    }
}