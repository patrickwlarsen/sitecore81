using SitecoreDev.Feature.Articles.Models;

namespace SitecoreDev.Feature.Articles.Repositories
{
    interface IArticlesRepository
    {
        IArticle GetArticleContent(string contentGuid);
    }
}
