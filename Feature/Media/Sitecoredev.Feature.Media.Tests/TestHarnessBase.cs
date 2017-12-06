using Ploeh.AutoFixture;
using SimpleInjector;

namespace Sitecoredev.Feature.Media.Tests
{
    public abstract class TestHarnessBase : ITestHarness
    {
        protected Container _container = new Container();
        protected IFixture _fixture;
        public IFixture Fixture { get { return _fixture; } }
    }
}
