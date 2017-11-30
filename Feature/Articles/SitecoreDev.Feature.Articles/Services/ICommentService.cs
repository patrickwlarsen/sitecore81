using System.Collections.Generic;
using SitecoreDev.Feature.Articles.Models;

namespace SitecoreDev.Feature.Articles.Services
{
    interface ICommentService
    {
        IEnumerable<IComment> GetComments(string blogId);
    }
}
