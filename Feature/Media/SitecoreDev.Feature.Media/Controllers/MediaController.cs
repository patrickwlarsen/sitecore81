﻿using System;
using System.Web.Mvc;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using SitecoreDev.Feature.Media.Repositories;
using SitecoreDev.Feature.Media.ViewModels;
using SitecoreDev.Foundation.Repository.Context;

namespace SitecoreDev.Feature.Media.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaRepository _repository;
        private readonly IContextWrapper _contextWrapper;

        public MediaController(IContextWrapper contextWrapper)
        {
            _repository = new SitecoreMediaRepository();
            _contextWrapper = contextWrapper;
        }

        public ViewResult HeroSlider()
        {
            var viewModel = new HeroSliderViewModel();

            if(!String.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                var contentItem = _repository.GetItem(RenderingContext.Current.Rendering.DataSource);
                if(contentItem != null)
                {
                    var heroImagesField = new MultilistField(contentItem.Fields["Hero Images"]);
                    if(heroImagesField != null)
                    {
                        var items = heroImagesField.GetItems();
                        var itemCounter = 0;
                        foreach(var item in items)
                        {
                            var mediaItem = (MediaItem)item;
                            viewModel.HeroImages.Add(new HeroSliderImageViewModel()
                            {
                                MediaUrl = MediaManager.GetMediaUrl(mediaItem),
                                AltText = mediaItem.Alt,
                                IsActive = itemCounter == 0
                            });
                            itemCounter++;
                        }
                    }
                }
            }

            var parameterValue = _contextWrapper.GetParameterValue("Slide Interval in Milliseconds");
            int interval = 0;
            if (int.TryParse(parameterValue, out interval))
                viewModel.SlideInterval = interval;
            return View(viewModel);
        }
    }
}