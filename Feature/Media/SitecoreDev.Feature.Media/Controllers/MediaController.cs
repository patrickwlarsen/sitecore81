using System;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using SitecoreDev.Feature.Media.ViewModels;
using SitecoreDev.Foundation.Repository.Context;
using SitecoreDev.Feature.Media.Services;
using System.Linq;
using Glass.Mapper.Sc;
using System.Web;
using SitecoreDev.Feature.Media.Models;

namespace SitecoreDev.Feature.Media.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaContentService _mediaContentService;
        private readonly IContextWrapper _contextWrapper;
        private readonly IGlassHtml _glassHtml;

        public MediaController(IContextWrapper contextWrapper, IMediaContentService mediaContentService, IGlassHtml glassHtml)
        {
            _contextWrapper = contextWrapper;
            _mediaContentService = mediaContentService;
            _glassHtml = glassHtml;
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
                        Image = new HtmlString(_glassHtml.Editable<IHeroSliderSlide>(
                            slide, i => i.Image))
                    });
                }
                var firstItem = viewModel.HeroImages.FirstOrDefault();
                firstItem.IsActive = true;
                viewModel.ParentGuid = contentItem.Id.ToString();
            }

            var parameterValue = _contextWrapper.GetParameterValue("Slide Interval in Milliseconds");
            int interval = 0;
            if (int.TryParse(parameterValue, out interval))
                viewModel.SlideInterval = interval;

            viewModel.IsInExperienceEditorMode = _contextWrapper.IsExperienceEditor;

            return View(viewModel);
        }
    }
}