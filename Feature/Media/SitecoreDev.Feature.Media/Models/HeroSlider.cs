using System.Collections.Generic;
using SitecoreDev.Foundation.Model;
using Glass.Mapper.Sc;

namespace SitecoreDev.Feature.Media.Models
{
    public class HeroSlider : CmsEntity, IHeroSlider
    {
        public IEnumerable<IHeroSliderSlide> Slides { get; set; }
    }
}