using SitecoreDev.Feature.Articles.Models;

namespace SitecoreDev.Feature.Articles.Services
{
    interface IContentService
    {
        IArticle GetArticleContent(string contentGuid);
    }
}
