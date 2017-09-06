using System.Collections.Generic;
using System.Transactions;
using Es.Udc.DotNet.WebMovies.Model.Services.LocalizationService;
using Es.Udc.DotNet.WebMovies.Model.Util;
using Es.Udc.DotNet.WebMovies.Test.Util;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es.Udc.DotNet.WebMovies.Test.Tests
{
    /// <summary>
    /// This is a test class for ILocalizationServiceTest and is intended to contain all Unit Tests
    /// </summary>
    [TestClass]
    public class ILocalizationTest
    {

        #region Fields and properties

        private TestUtil testUtil;

        private static IUnityContainer container;

        private static ILocalizationService localizationService;

        TransactionScope transactionScope;

        private TestContext testContext;

        /// <summary>
        /// Gets or sets the test context which provides information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return testContext;
            }
            set
            {
                testContext = value;
            }
        }

        #endregion

        #region Initializations and cleanups

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            container = TestManager.ConfigureUnityContainer("unity");

            localizationService = container.Resolve<ILocalizationService>();
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            TestManager.ClearUnityContainer(container);
        }

        // Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            transactionScope = new TransactionScope();
            testUtil = new TestUtil(container);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            transactionScope.Dispose();
        }

        #endregion

        [TestMethod()]
        public void GetAvailableLanguagesTest()
        {
            /* Use case */
            Dictionary<string, string> languages = localizationService.getAvailableLanguages();

            /* Check */
            Assert.AreEqual("Español", languages["es"]);
            Assert.AreEqual("Galego", languages["gl"]);
            Assert.AreEqual("English", languages["en"]);
            Assert.AreEqual("Français", languages["fr"]);
        }

        [TestMethod()]
        public void GetAvailableCountriesTest()
        {
            /* Use case */
            Dictionary<string, string> countries = localizationService.getAvailableCountries();

            /* Check */
            Assert.AreEqual("España", countries["ES"]);
            Assert.AreEqual("United Kingdom", countries["GB"]);
            Assert.AreEqual("United States of America", countries["US"]);
            Assert.AreEqual("France", countries["FR"]);
        }

        [TestMethod()]
        public void GetAvailableLocalesTest()
        {
            /* Use case */
            List<Locale> locales = localizationService.getAvailableLocales();

            /* Check */
            Assert.AreEqual(5, locales.Count);
            Assert.IsTrue(locales.Contains(new Locale("es", "ES")));
            Assert.IsTrue(locales.Contains(new Locale("gl", "ES")));
            Assert.IsTrue(locales.Contains(new Locale("en", "GB")));
            Assert.IsTrue(locales.Contains(new Locale("en", "US")));
            Assert.IsTrue(locales.Contains(new Locale("fr", "FR")));
        }

    }
}
