using System;
using System.Transactions;
using Es.Udc.DotNet.WebMovies.Model;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;
using Es.Udc.DotNet.WebMovies.Model.Util.Collections;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Test.Util;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es.Udc.DotNet.WebMovies.Test.Tests
{
    /// <summary>
    /// This is a test class for ILinkServiceTest and is intended to contain all Unit Tests
    /// </summary>
    [TestClass()]
    public class ILinkServiceTest
    {

        #region Fields and properties

        private TestUtil testUtil;

        private static IUnityContainer container;

        private static ILinkService linkService;

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

            linkService = container.Resolve<ILinkService>();
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
        public void GetLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();
            UserProfile user3 = testUtil.CreateTestUserThree();
            UserProfile user4 = testUtil.CreateTestUserFour();

            Link link = testUtil.CreateTestLink(user);

            testUtil.CreateTestRating(user2, link, 1);
            testUtil.CreateTestRating(user3, link, 1);
            testUtil.CreateTestRating(user4, link, -1);

            /* Use case */
            LinkDetails actual = linkService.GetLink(link.linkId);

            /* Check */
            testUtil.AssertMatch(link, actual); // Double checked
            testUtil.AssertMatch(actual, link.linkId, user.userId, user.userLogin, link.movieId, testUtil.TestData.linkName, testUtil.TestData.linkDescription, testUtil.TestData.linkUrl, 1 + 1 - 1);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void GetNonExistentLinkTest()
        {
            linkService.GetLink(testUtil.TestData.nonExistentLinkId);
        }

        [TestMethod()]
        public void GetLinksForMovieTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);
            Link link2 = testUtil.CreateTestLinkTwo(user, testUtil.TestData.movie1Id);

            /* Use case */
            ListBlock<LinkDetails> actual = linkService.GetLinksForMovie(testUtil.TestData.movie1Id, 0, 10);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(link2, actual[0]);
            testUtil.AssertMatch(link, actual[1]);
            Assert.IsTrue(actual[0].Date.CompareTo(actual[1].Date) > 0); // First element [0] is newer than second [1], if comparison > 0

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Link link3 = testUtil.CreateTestLinkThree(user, testUtil.TestData.movie1Id);

            /* Use case */
            actual = linkService.GetLinksForMovie(testUtil.TestData.movie1Id, 0, 2);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(link3, actual[0]);
            testUtil.AssertMatch(link2, actual[1]);
            Assert.IsTrue(actual[0].Date.CompareTo(actual[1].Date) > 0); // First element [0] is newer than second [1], if [0] - [1] > 0
        }

        [TestMethod()]
        public void GetMostValuedLinksForMovieTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);
            Link link2 = testUtil.CreateTestLinkTwo(user, testUtil.TestData.movie1Id);

            testUtil.CreateTestRating(user2, link, -1);
            testUtil.CreateTestRating(user2, link2, 1);

            /* Use case */
            ListBlock<LinkDetails> actual = linkService.GetMostValuedLinksForMovie(testUtil.TestData.movie1Id, 0, 10);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(link2, actual[0]);
            testUtil.AssertMatch(link, actual[1]);

            Assert.IsTrue(actual[0].Rating.CompareTo(actual[1].Rating) > 0); // First element [0] is greater than second [1], if [0] - [1] > 0

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            UserProfile user3 = testUtil.CreateTestUserThree();

            Link link3 = testUtil.CreateTestLinkThree(user, testUtil.TestData.movie1Id);

            testUtil.CreateTestRating(user3, link, 1);
            testUtil.CreateTestRating(user3, link2, 1);
            testUtil.CreateTestRating(user3, link3, 1);

            /* Use case */
            actual = linkService.GetMostValuedLinksForMovie(testUtil.TestData.movie1Id, 1, 2);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(link3, actual[0]);
            testUtil.AssertMatch(link, actual[1]);

            Assert.IsTrue(actual[0].Rating.CompareTo(actual[1].Rating) > 0); // First element [0] is greater than second [1], if [0] - [1] > 0
        }

        [TestMethod()]
        public void CountLinksForMovieTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Use case */
            int actual = linkService.CountLinksForMovie(testUtil.TestData.movie1Id);

            /* Check */
            Assert.AreEqual(0, actual);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);
            Link link2 = testUtil.CreateTestLinkTwo(user, testUtil.TestData.movie1Id);

            /* Use case */
            actual = linkService.CountLinksForMovie(testUtil.TestData.movie1Id);

            /* Check */
            Assert.AreEqual(2, actual);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            UserProfile user3 = testUtil.CreateTestUserThree();

            Link link3 = testUtil.CreateTestLinkThree(user, testUtil.TestData.movie1Id);

            /* Use case */
            actual = linkService.CountLinksForMovie(testUtil.TestData.movie1Id);

            /* Check */
            Assert.AreEqual(3, actual);
        }

        [TestMethod()]
        public void GetLinksForUserTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);
            Link link2 = testUtil.CreateTestLinkTwo(user, testUtil.TestData.movie1Id);

            /* Use case */
            ListBlock<LinkDetails> actual = linkService.GetLinksForUser(user.userId, 0, 10);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(link2, actual[0]);
            testUtil.AssertMatch(link, actual[1]);
            Assert.IsTrue(actual[0].Date.CompareTo(actual[1].Date) > 0); // First element [0] is newer than second [1], if comparison > 0

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Link link3 = testUtil.CreateTestLinkThree(user, testUtil.TestData.movie1Id);

            /* Use case */
            actual = linkService.GetLinksForUser(user.userId, 0, 2);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(link3, actual[0]);
            testUtil.AssertMatch(link2, actual[1]);
            Assert.IsTrue(actual[0].Date.CompareTo(actual[1].Date) > 0); // First element [0] is newer than second [1], if [0] - [1] > 0
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void GetLinksForNonExistentUserTest()
        {
            linkService.GetLinksForUser(testUtil.TestData.nonExistentUserId, 0, 10);
        }

        [TestMethod()]
        public void GetMostValuedLinksForUserTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);
            Link link2 = testUtil.CreateTestLinkTwo(user, testUtil.TestData.movie1Id);

            testUtil.CreateTestRating(user2, link, -1);
            testUtil.CreateTestRating(user2, link2, 1);

            /* Use case */
            ListBlock<LinkDetails> actual = linkService.GetMostValuedLinksForUser(user.userId, 0, 10);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(link2, actual[0]);
            testUtil.AssertMatch(link, actual[1]);

            Assert.IsTrue(actual[0].Rating.CompareTo(actual[1].Rating) > 0); // First element [0] is greater than second [1], if [0] - [1] > 0

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            UserProfile user3 = testUtil.CreateTestUserThree();

            Link link3 = testUtil.CreateTestLinkThree(user, testUtil.TestData.movie1Id);

            testUtil.CreateTestRating(user3, link, 1);
            testUtil.CreateTestRating(user3, link2, 1);
            testUtil.CreateTestRating(user3, link3, 1);

            /* Use case */
            actual = linkService.GetMostValuedLinksForUser(user.userId, 1, 2);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(link3, actual[0]);
            testUtil.AssertMatch(link, actual[1]);

            Assert.IsTrue(actual[0].Rating.CompareTo(actual[1].Rating) > 0); // First element [0] is greater than second [1], if [0] - [1] > 0
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void GetMostValuedLinksForNonExistentUserTest()
        {
            linkService.GetMostValuedLinksForUser(testUtil.TestData.nonExistentUserId, 0, 10);
        }

        [TestMethod()]
        public void CountLinksForUserTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            int actual = linkService.CountLinksForUser(user.userId);

            /* Check */
            Assert.AreEqual(0, actual);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);
            Link link2 = testUtil.CreateTestLinkTwo(user, testUtil.TestData.movie1Id);

            /* Use case */
            actual = linkService.CountLinksForUser(user.userId);

            /* Check */
            Assert.AreEqual(2, actual);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            UserProfile user3 = testUtil.CreateTestUserThree();

            Link link3 = testUtil.CreateTestLinkThree(user, testUtil.TestData.movie1Id);

            /* Use case */
            actual = linkService.CountLinksForUser(user.userId);

            /* Check */
            Assert.AreEqual(3, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void CountLinksForNonExistentUserTest()
        {
            linkService.CountLinksForUser(testUtil.TestData.nonExistentUserId);
        }

        [TestMethod()]
        public void GetReportedLinksForUserTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            ListBlock<LinkDetails> actual = linkService.GetReportedLinksForUser(user.userId, testUtil.TestData.linkReportThreshold, 0, 10);

            /* Check */
            Assert.AreEqual(0, actual.Count);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);
            Link link2 = testUtil.CreateTestLinkTwo(user, testUtil.TestData.movie1Id);

            /* Use case */
            actual = linkService.GetReportedLinksForUser(user.userId, testUtil.TestData.linkReportThreshold, 0, 10);

            /* Check */
            Assert.AreEqual(0, actual.Count);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            UserProfile user2 = testUtil.CreateTestUserTwo();

            testUtil.CreateTestRating(user2, link2, -1);

            /* Use case */
            actual = linkService.GetReportedLinksForUser(user.userId, testUtil.TestData.linkReportThreshold, 0, 10);

            /* Check */
            Assert.AreEqual(1, actual.Count);
            testUtil.AssertMatch(link2, actual[0]);

            /* *** Stage 4 ************************************************** */

            /* Initialization */
            Link link3 = testUtil.CreateTestLinkThree(user, testUtil.TestData.movie1Id);

            testUtil.CreateTestRating(user2, link3, -1);

            /* Use case */
            actual = linkService.GetReportedLinksForUser(user.userId, testUtil.TestData.linkReportThreshold, 0, 10);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(link2, actual[0]);
            testUtil.AssertMatch(link3, actual[1]);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void GetReportedLinksForNonExistentUserTest()
        {
            linkService.GetReportedLinksForUser(testUtil.TestData.nonExistentUserId, testUtil.TestData.linkReportThreshold, 0, 10);
        }

        [TestMethod()]
        public void CountReportedLinksForUserTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            int actual = linkService.CountReportedLinksForUser(user.userId, testUtil.TestData.linkReportThreshold);

            /* Check */
            Assert.AreEqual(0, actual);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);
            Link link2 = testUtil.CreateTestLinkTwo(user, testUtil.TestData.movie1Id);

            /* Use case */
            actual = linkService.CountReportedLinksForUser(user.userId, testUtil.TestData.linkReportThreshold);

            /* Check */
            Assert.AreEqual(0, actual);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            UserProfile user2 = testUtil.CreateTestUserTwo();

            testUtil.CreateTestRating(user2, link2, -1);

            /* Use case */
            actual = linkService.CountReportedLinksForUser(user.userId, testUtil.TestData.linkReportThreshold);

            /* Check */
            Assert.AreEqual(1, actual);

            /* *** Stage 4 ************************************************** */

            /* Initialization */
            Link link3 = testUtil.CreateTestLinkThree(user, testUtil.TestData.movie1Id);

            testUtil.CreateTestRating(user2, link3, -1);

            /* Use case */
            actual = linkService.CountReportedLinksForUser(user.userId, testUtil.TestData.linkReportThreshold);

            /* Check */
            Assert.AreEqual(2, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void CountReportedLinksForNonExistentUserTest()
        {
            linkService.CountReportedLinksForUser(testUtil.TestData.nonExistentUserId, testUtil.TestData.linkReportThreshold);
        }

        [TestMethod()]
        public void SetReportedLinkAsReadTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);
            Link link2 = testUtil.CreateTestLinkTwo(user, testUtil.TestData.movie1Id);

            testUtil.CreateTestRating(user2, link, -1);
            testUtil.CreateTestRating(user2, link2, -1);

            /* Check */
            ListBlock<Link> reportedLinks = testUtil.LinkDao.ListForUserReported(user.userId, testUtil.TestData.linkReportThreshold, 0, 10);
            Assert.AreNotEqual(true, link.reportRead);
            Assert.AreEqual(2, reportedLinks.Count);
            Assert.AreEqual(link, reportedLinks[0]);
            Assert.AreEqual(link2, reportedLinks[1]);

            /* *** Stage 2 ************************************************** */

            /* Use case */
            linkService.SetReportedLinkAsRead(user.userId, link.linkId);

            /* Check */
            reportedLinks = testUtil.LinkDao.ListForUserReported(user.userId, testUtil.TestData.linkReportThreshold, 0, 10);
            Assert.AreEqual(true, link.reportRead);
            Assert.AreEqual(1, reportedLinks.Count);
            Assert.AreEqual(link2, reportedLinks[0]);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void SetReportedLinkAsReadByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);

            /* Use case */
            linkService.SetReportedLinkAsRead(testUtil.TestData.nonExistentUserId, link.linkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void SetReportedNonExistentLinkAsReadTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            linkService.SetReportedLinkAsRead(user.userId, testUtil.TestData.nonExistentLinkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(UserNotAuthorizedException<LinkDetails>))]
        public void SetReportedLinkAsReadByAnotherUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);

            /* Use case */
            linkService.SetReportedLinkAsRead(user2.userId, link.linkId);
        }

        [TestMethod()]
        public void GetLinksForLabelTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);
            Link link2 = testUtil.CreateTestLinkTwo(user);

            /* Use case */
            ListBlock<LinkDetails> actual = linkService.GetLinksForLabel(testUtil.TestData.labelText, 0, 10);

            /* Check */
            Assert.AreEqual(0, actual.Count);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Label label = testUtil.CreateTestLabel();

            testUtil.RegisterLabel(link, label);

            /* Use case */
            actual = linkService.GetLinksForLabel(label.text, 0, 10);

            /* Check */
            Assert.AreEqual(1, actual.Count);
            testUtil.AssertMatch(link, actual[0]);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            Link link3 = testUtil.CreateTestLinkThree(user);

            Label label2 = testUtil.CreateTestLabelTwo();

            testUtil.RegisterLabel(link, label2);
            testUtil.RegisterLabel(link3, label2);

            /* Use case */
            actual = linkService.GetLinksForLabel(label2.text, 0, 10);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(link3, actual[0]);
            testUtil.AssertMatch(link, actual[1]);

            /* *** Stage 4 ************************************************** */

            /* Use case */
            actual = linkService.GetLinksForLabel(label.text, 0, 10);

            /* Check */
            Assert.AreEqual(1, actual.Count);
            testUtil.AssertMatch(link, actual[0]);
        }

        [TestMethod()]
        public void GetMostValuedLinksForLabelTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);
            Link link2 = testUtil.CreateTestLinkTwo(user);

            /* Use case */
            ListBlock<LinkDetails> actual = linkService.GetMostValuedLinksForLabel(testUtil.TestData.labelText, 0, 10);

            /* Check */
            Assert.AreEqual(0, actual.Count);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Label label = testUtil.CreateTestLabel();

            testUtil.RegisterLabel(link, label);

            /* Use case */
            actual = linkService.GetMostValuedLinksForLabel(label.text, 0, 10);

            /* Check */
            Assert.AreEqual(1, actual.Count);
            testUtil.AssertMatch(link, actual[0]);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            UserProfile user2 = testUtil.CreateTestUserTwo();
            UserProfile user3 = testUtil.CreateTestUserThree();

            Link link3 = testUtil.CreateTestLinkThree(user);

            testUtil.RegisterLabel(link2, label);
            testUtil.RegisterLabel(link3, label);

            testUtil.CreateTestRating(user2, link, -1);
            testUtil.CreateTestRating(user3, link, 1);
            testUtil.CreateTestRating(user2, link2, -1);
            testUtil.CreateTestRating(user3, link2, -1);
            testUtil.CreateTestRating(user2, link3, 1);
            testUtil.CreateTestRating(user3, link3, 1);

            /* Use case */
            actual = linkService.GetMostValuedLinksForLabel(label.text, 1, 2);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(link, actual[0]);
            testUtil.AssertMatch(link2, actual[1]);
        }

        [TestMethod()]
        public void CountLinksForLabelTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);
            Link link2 = testUtil.CreateTestLinkTwo(user);

            /* Use case */
            int actual = linkService.CountLinksForLabel(testUtil.TestData.labelText);

            /* Check */
            Assert.AreEqual(0, actual);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Label label = testUtil.CreateTestLabel();

            testUtil.RegisterLabel(link, label);

            /* Use case */
            actual = linkService.CountLinksForLabel(label.text);

            /* Check */
            Assert.AreEqual(1, actual);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            UserProfile user2 = testUtil.CreateTestUserTwo();
            UserProfile user3 = testUtil.CreateTestUserThree();

            Link link3 = testUtil.CreateTestLinkThree(user);

            testUtil.RegisterLabel(link2, label);
            testUtil.RegisterLabel(link3, label);

            /* Use case */
            actual = linkService.CountLinksForLabel(label.text);

            /* Check */
            Assert.AreEqual(3, actual);
        }

        [TestMethod()]
        public void AddLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            long linkId = linkService.AddLink(user.userId, testUtil.TestData.movie1Id, testUtil.TestData.linkName, testUtil.TestData.linkDescription, testUtil.TestData.linkUrl);

            /* Check */
            Link actual = testUtil.FindLink(linkId);
            testUtil.AssertMatch(actual, linkId, testUtil.TestData.movie1Id, user.userId, testUtil.TestData.linkName, testUtil.TestData.linkDescription, testUtil.TestData.linkUrl, 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void AddLinkByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            linkService.AddLink(testUtil.TestData.nonExistentUserId, testUtil.TestData.movie1Id, testUtil.TestData.linkName, testUtil.TestData.linkDescription, testUtil.TestData.linkUrl);
        }

        [TestMethod()]
        [ExpectedException(typeof(DuplicateInstanceException<LinkDetails>))]
        public void AddDuplicatedLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            linkService.AddLink(user.userId, testUtil.TestData.movie1Id, testUtil.TestData.linkName, testUtil.TestData.linkDescription, testUtil.TestData.linkUrl);

            /* Check */
            linkService.AddLink(user.userId, testUtil.TestData.movie1Id, testUtil.TestData.linkName, testUtil.TestData.linkDescription, testUtil.TestData.linkUrl);
        }

        [TestMethod()]
        public void UpdateLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);
            long linkId = link.linkId;
            DateTime linkDate = link.date;

            testUtil.CreateTestRating(user2, link, 1);

            /* Use case */
            linkService.UpdateLink(user.userId, link.linkId, testUtil.TestData.linkName2, testUtil.TestData.linkDescription2);

            /* Check */
            Link actual = testUtil.FindLink(linkId);
            testUtil.AssertMatch(actual, linkId, testUtil.TestData.movie1Id, user.userId, testUtil.TestData.linkName2, testUtil.TestData.linkDescription2, testUtil.TestData.linkUrl, 1, linkDate);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void UpdateLinkByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);

            testUtil.CreateTestRating(user2, link, 1);

            /* Use case */
            linkService.UpdateLink(testUtil.TestData.nonExistentUserId, link.linkId, testUtil.TestData.linkName2, testUtil.TestData.linkDescription2);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void UpdateLinkForNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            linkService.UpdateLink(user.userId, testUtil.TestData.nonExistentLinkId, testUtil.TestData.linkName2, testUtil.TestData.linkDescription2);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<Link>))]
        public void RemoveLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);
            long linkId = link.linkId;

            /* Use case */
            linkService.RemoveLink(user.userId, linkId);

            /* Check */
            int userLinkCount;
            try
            {
                userLinkCount = testUtil.ListLinks(user).Count;
            }
            catch (InstanceNotFoundException<Link>)
            {
                userLinkCount = 0;
            }
            Assert.AreEqual(0, userLinkCount);
            testUtil.FindLink(linkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void RemoveLinkByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);

            /* Use case */
            linkService.RemoveLink(testUtil.TestData.nonExistentUserId, link.linkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void RemoveNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            linkService.RemoveLink(user.userId, testUtil.TestData.nonExistentLinkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(UserNotAuthorizedException<LinkDetails>))]
        public void RemoveLinkByAnotherUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Link link = testUtil.CreateTestLink(user, testUtil.TestData.movie1Id);

            /* Use case */
            linkService.RemoveLink(user2.userId, link.linkId);
        }

    }
}
