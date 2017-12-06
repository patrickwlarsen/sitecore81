using SitecoreDev.Foundation.Model;
using System.Collections.Generic;

namespace SitecoreDev.Foundation.Repository.Content
{
    public interface IContentRepository
    {
        T GetContentItem<T>(string contentGuid) where T : class, ICmsEntity;
        IEnumerable<T> GetChildren<T>(string parentGuid) where T : class, ICmsEntity;
    }
}
