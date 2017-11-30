﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace SitecoreDev.Feature.Media.Repositories
{
    interface IMediaRepository
    {
        Item GetItem(string contentGuid);
    }
}
