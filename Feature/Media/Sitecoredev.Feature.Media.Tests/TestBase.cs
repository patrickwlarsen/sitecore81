using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sitecoredev.Feature.Media.Tests
{
    public abstract class TestBase<T> where T : ITestHarness, new()
    {
        protected T _testHarness;

        [TestInitialize]
        public void Setup()
        {
            _testHarness = new T();
        }
    }
}
