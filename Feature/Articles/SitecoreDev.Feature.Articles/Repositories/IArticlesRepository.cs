using Sitecore.Data.Items;

namespace SitecoreDev.Feature.Articles.Repositories
{
    interface IArticlesRepository
    {
        Item GetArticleContent(string contentGuid);
    }
}
