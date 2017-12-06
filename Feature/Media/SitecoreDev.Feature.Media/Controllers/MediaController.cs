using System;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using SitecoreDev.Feature.Media.ViewModels;
using SitecoreDev.Foundation.Repository.Context;
using SitecoreDev.Feature.Media.Services;
using System.Linq;

namespace SitecoreDev.Feature.Media.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaContentService _mediaContentService;
        private readonly IContextWrapper _contextWrapper;

        public MediaController(IContextWrapper contextWrapper, IMediaContentService mediaContentService)
        {
            _contextWrapper = contextWrapper;
            _mediaContentService = mediaContentService;
        }

        public ViewResult HeroSlider()
        {
            var viewModel = new HeroSliderViewModel();

            if (!String.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                var contentItem = _mediaContentService.GetHeroSliderContent(RenderingContext.Current.Rendering.DataSource);

                foreach (var slide in contentItem?.Slides)
                {
                    viewModel.HeroImages.Add(new HeroSliderImageViewModel()
                    {
                        MediaUrl = slide.Image.Src,
                        AltText = slide.Image.Alt
                    });
                }
                var firstItem = viewModel.HeroImages.FirstOrDefault();
                firstItem.IsActive = true;
            }

            var parameterValue = _contextWrapper.GetParameterValue("Slide Interval in Milliseconds");
            int interval = 0;
            if (int.TryParse(parameterValue, out interval))
                viewModel.SlideInterval = interval;

            return View(viewModel);
        }
    }
}