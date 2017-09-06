using System.Transactions;
using Es.Udc.DotNet.WebMovies.Model;
using Es.Udc.DotNet.WebMovies.Model.Services.RatingService;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Test.Util;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;

namespace Es.Udc.DotNet.WebMovies.Test.Tests
{
    /// <summary>
    /// This is a test class for IRatingServiceTest and is intended to contain all Unit Tests
    /// </summary>
    [TestClass()]
    public class IRatingServiceTest
    {

        #region Fields and properties

        private TestUtil testUtil;

        private static IUnityContainer container;

        private static IRatingService ratingService;

        TransactionScope transaction;

        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides information about and functionality for the current test run.
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

            ratingService = container.Resolve<IRatingService>();
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
        public void GetRatingTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            int actual = ratingService.GetRating(user.userId, link.linkId);

            /* Check */
            Assert.AreEqual(0, actual);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            testUtil.CreateTestRating(user, link, 1);

            /* Use case */
            actual = ratingService.GetRating(user.userId, link.linkId);

            /* Check */
            Assert.AreEqual(1, actual);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            UserProfile user2 = testUtil.CreateTestUserTwo();

            testUtil.CreateTestRating(user2, link, -1);

            /* Use case */
            actual = ratingService.GetRating(user2.userId, link.linkId);

            /* Check */
            Assert.AreEqual(-1, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void GetRatingByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            ratingService.GetRating(testUtil.TestData.nonExistentUserId, link.linkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void GetRatingForNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            ratingService.GetRating(user.userId, testUtil.TestData.nonExistentLinkId);
        }

        [TestMethod()]
        public void RateTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Check */
            Assert.AreEqual(0, user.Rating);
            Assert.AreEqual(0, testUtil.CalculateRating(user));

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Link link = testUtil.CreateTestLink(user);

            Label label = testUtil.CreateTestLabel();

            /* Check */
            Assert.AreEqual(1, user.Rating); // 1 from publishing
            Assert.AreEqual(0, link.Rating);
            Assert.AreEqual(0, label.Rating);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            testUtil.RegisterLabel(link, label);

            /* Check */
            Assert.AreEqual(1, user.Rating); // 1 from publishing
            Assert.AreEqual(0, link.Rating);
            Assert.AreEqual(1, label.Rating); // 1 from link

            /* *** Stage 4 ************************************************** */

            /* Initialization */
            UserProfile user2 = testUtil.CreateTestUserTwo();

            /* Use case */
            long ratingId = ratingService.Rate(user2.userId, link.linkId, 1);

            /* Check */
            Rating actual = testUtil.FindRating(ratingId);
            testUtil.AssertMatch(actual, ratingId, user2.userId, link.linkId, 1);
            Assert.AreEqual(2, user.Rating); // 1 from publishing + 1 from users' rating
            Assert.AreEqual(1, link.Rating); // 1 from users' rating
            Assert.AreEqual(1, label.Rating); // 1 from link

            /* *** Stage 5 ************************************************** */

            /* Initialization */
            UserProfile user3 = testUtil.CreateTestUserThree();

            Link link2 = testUtil.CreateTestLinkTwo(user);

            Label label2 = testUtil.CreateTestLabelTwo();

            testUtil.RegisterLabel(link, label2);
            testUtil.RegisterLabel(link2, label2);

            /* Use case */
            ratingId = ratingService.Rate(user3.userId, link.linkId, 1);

            /* Check */
            actual = testUtil.FindRating(ratingId);
            testUtil.AssertMatch(actual, ratingId, user3.userId, link.linkId, 1);
            Assert.AreEqual(4, user.Rating); // 2 from publishing + 2 from users' rating
            Assert.AreEqual(2, link.Rating); // 2 from users' rating
            Assert.AreEqual(0, link2.Rating);
            Assert.AreEqual(1, label.Rating); // 1 from publishing
            Assert.AreEqual(2, label2.Rating); // 2 from publishing

            /* *** Stage 6 ************************************************** */

            /* Initialization */
            UserProfile user4 = testUtil.CreateTestUserFour();

            /* Use case */
            ratingId = ratingService.Rate(user4.userId, link.linkId, -1);

            /* Check */
            actual = testUtil.FindRating(ratingId);
            testUtil.AssertMatch(actual, ratingId, user4.userId, link.linkId, -1);
            Assert.AreEqual(3, user.Rating); // 2 from publishing + 2-1 from users' rating
            Assert.AreEqual(1, link.Rating); // 2-1 from users' rating
            Assert.AreEqual(0, link2.Rating);
            Assert.AreEqual(1, label.Rating); // 1 from publishing
            Assert.AreEqual(2, label2.Rating); // 2 from publishing
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void RateByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            ratingService.Rate(testUtil.TestData.nonExistentUserId, link.linkId, 1);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void RateNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            ratingService.Rate(user.userId, testUtil.TestData.nonExistentLinkId, 1);
        }

        [TestMethod()]
        [ExpectedException(typeof(UserNotAuthorizedException<RatingDetails>))]
        public void RateOwnLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            ratingService.Rate(user.userId, link.linkId, 1);
        }

    }
}
