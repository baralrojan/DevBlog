using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
namespace DevBlog.Library.Tests
{
    [TestClass]
    public class TagServicesIntegrationTests
    {
        private IWebDriver _webDriver;
        [TestMethod]

        public void TestMethod1()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            _webDriver = new ChromeDriver();

            _webDriver.Navigate().GoToUrl("https://localhost:7024/");
            Assert.IsTrue(_webDriver.Title.Contains("Home Page"));

            _webDriver.Quit();

        }
    }
}