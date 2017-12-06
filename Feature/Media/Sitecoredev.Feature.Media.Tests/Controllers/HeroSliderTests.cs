using FluentAssertions;
using Glass.Mapper.Sc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using SitecoreDev.Feature.Media.Controllers;
using SitecoreDev.Feature.Media.Models;
using SitecoreDev.Feature.Media.Services;
using SitecoreDev.Feature.Media.ViewModels;
using SitecoreDev.Foundation.Repository.Context;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sitecoredev.Feature.Media.Tests.Controllers
{
    [TestClass]
    public class HeroSliderTests
    {
        [TestMethod]
        public void HeroSliderSuccessful()
        {
            //Arrange
            var fixture = new Fixture();

            var content = fixture.Build<HeroSlider>()
                .With(x => x.Slides, fixture.CreateMany<HeroSliderSlide>().ToList())
                .Create();

            var contentService = new Mock<IMediaContentService>();
            contentService.Setup(x => x.GetHeroSliderContent(It.IsAny<string>()))
                .Returns(content)
                .Verifiable();

            var contextWrapper = new Mock<IContextWrapper>();
            contextWrapper.Setup(x => x.GetParameterValue(It.IsAny<string>()))
                .Returns("500")
                .Verifiable();
            contextWrapper.SetupGet(x => x.IsExperienceEditor)
                .Returns(true)
                .Verifiable();
            contextWrapper.SetupGet(x => x.Datasource)
                .Returns(Guid.NewGuid().ToString())
                .Verifiable();

            var glassHtml = new Mock<IGlassHtml>();
            glassHtml.Setup(x => x.Editable(
                    It.IsAny<IHeroSliderSlide>(),
                    It.IsAny<Expression<Func<IHeroSliderSlide, object>>>(),
                    It.IsAny<object>()))
                .Returns("test")
                .Verifiable();
            var controller = new MediaController(contextWrapper.Object, contentService.Object, glassHtml.Object);

            //Act
            var result = controller.HeroSlider();

            //Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<HeroSliderViewModel>();

            var viewModel = result.Model as HeroSliderViewModel;
            viewModel.Should().NotBeNull();
            viewModel.HasImages.Should().BeTrue();
            viewModel.ImageCount.Should().Be(content.Slides.Count());
            viewModel.SlideInterval.Should().Be(500);
            viewModel.IsSliderIntervalSet.Should().BeTrue();
            viewModel.IsInExperienceEditorMode.Should().BeTrue();
            viewModel.ParentGuid.Should().Be(content.Id.ToString());
        }

        [TestMethod]
        public void HeroSliderEmptyDatasource()
        {
            //Arrange
            var fixture = new Fixture();

            var contentService = new Mock<IMediaContentService>();

            var contextWrapper = new Mock<IContextWrapper>();
            contextWrapper.Setup(x => x.GetParameterValue(It.IsAny<string>()))
                .Returns("500")
                .Verifiable();
            contextWrapper.SetupGet(x => x.IsExperienceEditor)
                .Returns(true)
                .Verifiable();
            contextWrapper.SetupGet(x => x.Datasource)
                .Returns(string.Empty)
                .Verifiable();

            var glassHtml = new Mock<IGlassHtml>();

            var controller = new MediaController(contextWrapper.Object, contentService.Object, glassHtml.Object);

            //Act
            var result = controller.HeroSlider();

            //Assert
            result.Should().NotBeNull();
            result.Model.Should().BeOfType<HeroSliderViewModel>();
            var viewModel = result.Model as HeroSliderViewModel;
            viewModel.Should().NotBeNull();
            viewModel.HasImages.Should().BeFalse();
            viewModel.ImageCount.Should().Be(0);
            viewModel.SlideInterval.Should().Be(500);
            viewModel.IsSliderIntervalSet.Should().BeTrue();
            viewModel.IsInExperienceEditorMode.Should().BeTrue();
            viewModel.ParentGuid.Should().BeNullOrEmpty();
        }
    }
}
