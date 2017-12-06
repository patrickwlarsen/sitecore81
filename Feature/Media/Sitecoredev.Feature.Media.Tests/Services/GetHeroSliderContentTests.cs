using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SitecoreDev.Foundation.Repository.Content;
using FluentAssertions;
using Ploeh.AutoFixture;
using SitecoreDev.Feature.Media.Models;
using SitecoreDev.Feature.Media.Services;


namespace Sitecoredev.Feature.Media.Tests.Services
{
    [TestClass]
    public class GetHeroSliderContentTests
    {
        [TestMethod]
        public void GetHeroSliderContentSuccessful()
        {
            //Arrange
            var fixture = new Fixture();

            var heroSlide = fixture
                .Build<HeroSlider>()
                .Without(x => x.Slides)
                .Create();
            var children = fixture
                .CreateMany<HeroSliderSlide>()
                .ToList();

            var repository = new Mock<IContentRepository>();
            repository
                .Setup(x => x.GetContentItem<IHeroSlider>(It.IsAny<string>()))
                .Returns(heroSlide)
                .Verifiable();
            repository
                .Setup(x => x.GetChildren<IHeroSliderSlide>(It.IsAny<string>()))
                .Returns(children)
                .Verifiable();

            var service = new SitecoreMediaContentService(repository.Object);

            //Act
            var result = service.GetHeroSliderContent("123");

            //Assert
            repository.Verify();
            result.Should().NotBeNull();
            result.Slides.Should().NotBeNullOrEmpty();
            result.Slides.Count().Should().Be(children.Count());
            var slides = result.Slides.ToList();
            
            foreach (var slide in slides)
                slide.Image.Should().NotBeNull();
        }

        [TestMethod]
        public void GetHeroSliderContentEmptyContentGuid()
        {
            //Arrange
            var contentGuidNullException = new ArgumentNullException("contentGuid");
            var parentGuidNullException = new ArgumentNullException("parentGuid");

            var repository = new Mock<IContentRepository>();
            repository
                .Setup(x => x.GetContentItem<IHeroSlider>(String.Empty))
                .Throws(contentGuidNullException);
            repository
                .Setup(x => x.GetChildren<IHeroSliderSlide>(String.Empty))
                .Throws(parentGuidNullException);

            var service = new SitecoreMediaContentService(repository.Object);

            //Act
            var result = service.GetHeroSliderContent(String.Empty);

            //Assert
            result.Should().BeNull();
        }
    }
}
