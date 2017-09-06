using System;
using System.Transactions;
using System.Xml;
using Es.Udc.DotNet.WebMovies.Model.Services.MovieService;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Test.Util;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es.Udc.DotNet.WebMovies.Test.Tests
{
    [TestClass]
    public class IMovieServiceTest
    {

        #region Fields and properties

        private TestUtil testUtil;

        private static IUnityContainer container;

        private static IMovieService movieService;

        TransactionScope transaction;

        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        #region Initializations and cleanups

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            container = TestManager.ConfigureUnityContainer("unity");

            movieService = container.Resolve<IMovieService>();
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
            transaction = new TransactionScope();
            testUtil = new TestUtil(container);
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            transaction.Dispose();
        }

        #endregion

        [TestMethod()]
        public void GetMovieTest()
        {
            /* Use case */
            MovieDetails actual = movieService.GetMovie(testUtil.TestData.movie1Id);

            /* Check */
            testUtil.AssertMatch(actual, testUtil.TestData.movie1Id, testUtil.TestData.movie1Title, testUtil.TestData.movie1Description, testUtil.TestData.movie1Price);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<MovieDetails>))]
        public void GetNonExistentMovieTest()
        {
            MovieDetails actual = movieService.GetMovie(testUtil.TestData.nonExistentMovieId);
        }

        [TestMethod()]
        [DeploymentItem(@"Test/XmlDocuments/DetailProductTheArtist.xml", "XmlDocuments")]
        public void GetMovieXmlTest()
        {
            XmlDocument actual = movieService.GetMovieXml(testUtil.TestData.movie1Id);

            XmlDocument expected = new XmlDocument();
            expected.Load(@"XmlDocuments/DetailProductTheArtist.xml");
            Assert.AreEqual(expected.InnerXml, actual.InnerXml);
        }

        [TestMethod()]
        [DeploymentItem(@"Test/XmlDocuments/DetailProductError.xml", "XmlDocuments")]
        public void GetNonExistentMovieXmlTest()
        {
            XmlDocument actual = movieService.GetMovieXml(testUtil.TestData.nonExistentMovieId);

            XmlDocument expected = new XmlDocument();
            expected.Load(@"XmlDocuments/DetailProductError.xml");
            Assert.AreEqual(expected.InnerXml, actual.InnerXml);
        }

        [TestMethod()]
        public void SearchMoviesTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Use case */
            ListBlock<MovieDetails> actual = movieService.SearchMovies(testUtil.TestData.movie1Title, 0, 10);

            /* Checked */
            Assert.AreEqual(1, actual.Count);
            Assert.IsFalse(actual.HasMore);
            testUtil.AssertMatch(actual[0], testUtil.TestData.movie1Id, testUtil.TestData.movie1Title, null, testUtil.TestData.movie1Price);

            /* *** Stage 2 ************************************************** */

            /* Use case */
            actual = movieService.SearchMovies(testUtil.TestData.movie1And2CommonKeyword, 0, 10);

            /* Checked */
            Assert.AreEqual(2, actual.Count);
            Assert.IsFalse(actual.HasMore);
            testUtil.AssertMatch(actual[0], testUtil.TestData.movie1Id, testUtil.TestData.movie1Title, null, testUtil.TestData.movie1Price);
            testUtil.AssertMatch(actual[1], testUtil.TestData.movie2Id, testUtil.TestData.movie2Title, null, testUtil.TestData.movie2Price);

            /* *** Stage 3 ************************************************** */

            /* Use case */
            actual = movieService.SearchMovies("", 0, 1000);

            /* Checked */
            Assert.AreEqual(6, actual.Count);
            Assert.IsFalse(actual.HasMore);
            testUtil.AssertMatch(actual[0], testUtil.TestData.movie1Id, testUtil.TestData.movie1Title, null, testUtil.TestData.movie1Price);
            testUtil.AssertMatch(actual[1], testUtil.TestData.movie2Id, testUtil.TestData.movie2Title, null, testUtil.TestData.movie2Price);
            testUtil.AssertMatch(actual[2], testUtil.TestData.movie3Id, testUtil.TestData.movie3Title, null, testUtil.TestData.movie3Price);
            testUtil.AssertMatch(actual[3], testUtil.TestData.movie4Id, testUtil.TestData.movie4Title, null, testUtil.TestData.movie4Price);
            testUtil.AssertMatch(actual[4], testUtil.TestData.movie5Id, testUtil.TestData.movie5Title, null, testUtil.TestData.movie5Price);
            testUtil.AssertMatch(actual[5], testUtil.TestData.movie6Id, testUtil.TestData.movie6Title, null, testUtil.TestData.movie6Price);

            /* *** Stage 4 ************************************************** */

            /* Use case */
            actual = movieService.SearchMovies("", 0, 4);

            /* Checked */
            Assert.AreEqual(4, actual.Count);
            Assert.IsTrue(actual.HasMore);
            testUtil.AssertMatch(actual[0], testUtil.TestData.movie1Id, testUtil.TestData.movie1Title, null, testUtil.TestData.movie1Price);
            testUtil.AssertMatch(actual[1], testUtil.TestData.movie2Id, testUtil.TestData.movie2Title, null, testUtil.TestData.movie2Price);
            testUtil.AssertMatch(actual[2], testUtil.TestData.movie3Id, testUtil.TestData.movie3Title, null, testUtil.TestData.movie3Price);
            testUtil.AssertMatch(actual[3], testUtil.TestData.movie4Id, testUtil.TestData.movie4Title, null, testUtil.TestData.movie4Price);

            /* *** Stage 5 ************************************************** */

            /* Use case */
            actual = movieService.SearchMovies("", 4, 4);

            /* Checked */
            Assert.AreEqual(2, actual.Count);
            Assert.IsFalse(actual.HasMore);
            testUtil.AssertMatch(actual[0], testUtil.TestData.movie5Id, testUtil.TestData.movie5Title, null, testUtil.TestData.movie5Price);
            testUtil.AssertMatch(actual[1], testUtil.TestData.movie6Id, testUtil.TestData.movie6Title, null, testUtil.TestData.movie6Price);

            /* *** Stage 6 ************************************************** */

            /* Use case */
            actual = movieService.SearchMovies(testUtil.TestData.nonExistentMovieTitle, 1, 1000);

            /* Checked */
            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.HasMore);
        }

        [TestMethod()]
        [DeploymentItem(@"Test/XmlDocuments/FindProductsXmlSearchTheArtist.xml", "XmlDocuments")]
        public void SearchMoviesXmlForFullTitleTest()
        {
            XmlDocument actual = movieService.SearchMoviesXml(testUtil.TestData.movie1Title, 0, 10);

            XmlDocument expected = new XmlDocument();
            expected.Load(@"XmlDocuments/FindProductsXmlSearchTheArtist.xml");
            Console.WriteLine(expected.InnerXml);
            Assert.AreEqual(expected.InnerXml, actual.InnerXml);
        }

        [TestMethod()]
        [DeploymentItem(@"Test/XmlDocuments/FindProductsXmlSearchThe.xml", "XmlDocuments")]
        public void SearchMoviesXmlForCommonKeywordTest()
        {
            XmlDocument actual = movieService.SearchMoviesXml(testUtil.TestData.movie1And2CommonKeyword, 0, 10);

            XmlDocument expected = new XmlDocument();
            expected.Load(@"XmlDocuments/FindProductsXmlSearchThe.xml");
            Console.WriteLine(expected.InnerXml);
            Assert.AreEqual(expected.InnerXml, actual.InnerXml);
        }

        [TestMethod()]
        [DeploymentItem(@"Test/XmlDocuments/FindProductsXmlAll.xml", "XmlDocuments")]
        public void SearchMoviesXmlForAllTest()
        {
            XmlDocument actual = movieService.SearchMoviesXml("", 0, 1000);

            XmlDocument expected = new XmlDocument();
            expected.Load(@"XmlDocuments/FindProductsXmlAll.xml");
            Console.WriteLine(expected.InnerXml);
            Assert.AreEqual(expected.InnerXml, actual.InnerXml);
        }

        [TestMethod()]
        [DeploymentItem(@"Test/XmlDocuments/FindProductsXmlAll1To4.xml", "XmlDocuments")]
        public void SearchMoviesXmlForAllEach4Step1Test()
        {
            XmlDocument actual = movieService.SearchMoviesXml("", 0, 4);

            XmlDocument expected = new XmlDocument();
            expected.Load(@"XmlDocuments/FindProductsXmlAll1To4.xml");
            Console.WriteLine(expected.InnerXml);
            Assert.AreEqual(expected.InnerXml, actual.InnerXml);
        }

        [TestMethod()]
        [DeploymentItem(@"Test/XmlDocuments/FindProductsXmlAll5To8.xml", "XmlDocuments")]
        public void SearchMoviesXmlForAllEach4Step2Test()
        {
            XmlDocument actual = movieService.SearchMoviesXml("", 4, 4);

            XmlDocument expected = new XmlDocument();
            expected.Load(@"XmlDocuments/FindProductsXmlAll5To8.xml");
            Console.WriteLine(expected.InnerXml);
            Assert.AreEqual(expected.InnerXml, actual.InnerXml);
        }

        [TestMethod()]
        [DeploymentItem(@"Test/XmlDocuments/FindProductsXmlError.xml", "XmlDocuments")]
        public void SearchMoviesXmlEmptyTest()
        {
            XmlDocument actual = movieService.SearchMoviesXml(testUtil.TestData.nonExistentMovieTitle, 1, 1000);

            XmlDocument expected = new XmlDocument();
            expected.Load(@"XmlDocuments/FindProductsXmlError.xml");
            Console.WriteLine(expected.InnerXml);
            Assert.AreEqual(expected.InnerXml, actual.InnerXml);
        }

    }
}
