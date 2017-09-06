using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Es.Udc.DotNet.WebMovies.Model;
using Es.Udc.DotNet.WebMovies.Model.Services.FavoriteService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Test.Util;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es.Udc.DotNet.WebMovies.Test.Tests
{
    /// <summary>
    /// This is a test class for IFavoriteServiceTest and is intended to contain all IFavoriteServiceTest Unit Tests
    /// </summary>
    [TestClass()]
    public class IFavoriteTest
    {

        #region Fields and properties

        private TestUtil testUtil;

        private static IUnityContainer container;

        private static IFavoriteService favoriteService;

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

            favoriteService = container.Resolve<IFavoriteService>();
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
        public void HasInFavoritesTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            testUtil.CreateTestFavorite(user, link);

            /* Use case */
            bool actual = favoriteService.HasInFavorites(user.userId, link.linkId);

            /* Check */
            Assert.IsTrue(actual);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Link link2 = testUtil.CreateTestLinkTwo(user);

            /* Use case */
            actual = favoriteService.HasInFavorites(user.userId, link2.linkId);

            /* Check */
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void HasInFavoritesByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            favoriteService.HasInFavorites(testUtil.TestData.nonExistentUserId, link.linkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void HasInFavoritesByForExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            favoriteService.HasInFavorites(user.userId, testUtil.TestData.nonExistentLinkId);
        }

        [TestMethod()]
        public void GetFavoriteLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            Favorite favorite = testUtil.CreateTestFavorite(user, link);

            /* Use case */
            FavoriteDetails actual = favoriteService.GetFavorite(user.userId, link.linkId);

            /* Check */
            testUtil.AssertMatch(favorite, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void GetFavoriteLinkForNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            favoriteService.GetFavorite(testUtil.TestData.nonExistentUserId, link.linkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void GetFavoriteForNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            favoriteService.GetFavorite(user.userId, testUtil.TestData.nonExistentLinkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<FavoriteDetails>))]
        public void GetNonExistentFavoriteLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            FavoriteDetails actual = favoriteService.GetFavorite(user.userId, link.linkId);
        }

        [TestMethod()]
        public void GetFavoritesForUserTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            Favorite favorite = testUtil.CreateTestFavorite(user, link);

            /* Use case */
            List<FavoriteDetails> actual = favoriteService.GetFavoritesForUser(user.userId, 0, 2);

            /* Check */
            Assert.AreEqual(1, actual.Count);
            testUtil.AssertMatch(favorite, actual[0]);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Link link2 = testUtil.CreateTestLinkTwo(user);

            Favorite favorite2 = testUtil.CreateTestFavoriteTwo(user, link2);

            /* Use case */
            actual = favoriteService.GetFavoritesForUser(user.userId, 0, 3);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(favorite2, actual[0]);
            testUtil.AssertMatch(favorite, actual[1]);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            Link link3 = testUtil.CreateTestLinkThree(user);

            Favorite favorite3 = testUtil.CreateTestFavoriteThree(user, link3);

            /* Use case */
            actual = favoriteService.GetFavoritesForUser(user.userId, 1, 2);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(favorite2, actual[0]);
            testUtil.AssertMatch(favorite, actual[1]);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void GetFavoritesForNonExistentUserTest()
        {
            /* Use case */
            favoriteService.GetFavoritesForUser(testUtil.TestData.nonExistentUserId, 0, 2);
        }

        [TestMethod()]
        public void GetFavoritesForUserByRatingTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();
            UserProfile user3 = testUtil.CreateTestUserThree();

            Link link = testUtil.CreateTestLink(user);
            Link link2 = testUtil.CreateTestLinkTwo(user);
            Link link3 = testUtil.CreateTestLinkThree(user);

            Favorite favorite = testUtil.CreateTestFavorite(user, link);
            Favorite favorite2 = testUtil.CreateTestFavoriteTwo(user, link2);
            Favorite favorite3 = testUtil.CreateTestFavoriteThree(user, link3);

            testUtil.CreateTestRating(user2, link2, 1); // 0  1  0
            testUtil.CreateTestRating(user2, link3, 1); // 0  1  1
            testUtil.CreateTestRating(user3, link3, 1); // 0  1  2

            /* Use case */
            List<FavoriteDetails> actual = favoriteService.GetFavoritesForUserByRating(user.userId, 0, 2);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(favorite3.favoriteId, actual[0].FavoriteId);
            Assert.AreEqual(favorite2.favoriteId, actual[1].FavoriteId);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            UserProfile user4 = testUtil.CreateTestUserFour();

            testUtil.CreateTestRating(user3, link2, -1); // 0  0  2
            testUtil.CreateTestRating(user4, link, 1);   // 1  0  2
            testUtil.CreateTestRating(user4, link2, -1); // 1 -1  2

            /* Use case */
            actual = favoriteService.GetFavoritesForUserByRating(user.userId, 1, 4);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(favorite.favoriteId, actual[0].FavoriteId);
            Assert.AreEqual(favorite2.favoriteId, actual[1].FavoriteId);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void GetFavoritesForNonExistentUserByRatingTest()
        {
            /* Use case */
            favoriteService.GetFavoritesForUserByRating(testUtil.TestData.nonExistentUserId, 0, 2);
        }

        [TestMethod()]
        public void CountFavoritesForUserTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            Favorite favorite = testUtil.CreateTestFavorite(user, link);

            /* Use case */
            int actual = favoriteService.CountFavoritesForUser(user.userId);

            /* Check */
            Assert.AreEqual(1, actual);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Link link2 = testUtil.CreateTestLinkTwo(user);

            Favorite favorite2 = testUtil.CreateTestFavoriteTwo(user, link2);

            /* Use case */
            actual = favoriteService.CountFavoritesForUser(user.userId);

            /* Check */
            Assert.AreEqual(2, actual);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            Link link3 = testUtil.CreateTestLinkThree(user);

            Favorite favorite3 = testUtil.CreateTestFavoriteThree(user, link3);

            /* Use case */
            actual = favoriteService.CountFavoritesForUser(user.userId);

            /* Check */
            Assert.AreEqual(3, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void CountFavoritesForNonExistentUserTest()
        {
            /* Use case */
            favoriteService.CountFavoritesForUser(testUtil.TestData.nonExistentUserId);
        }

        [TestMethod()]
        public void AddToFavoritesTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            long favoriteId = favoriteService.AddToFavorites(user.userId, link.linkId, testUtil.TestData.favoriteName, testUtil.TestData.favoriteDescription);

            /* Check */
            Favorite actual = testUtil.FindFavorite(favoriteId);
            testUtil.AssertMatch(actual, favoriteId, user.userId, link.linkId, testUtil.TestData.favoriteName, testUtil.TestData.favoriteDescription);

            Assert.AreEqual(1, user.Favorites.Count());
            Assert.AreEqual(actual, user.Favorites.ElementAt(0));
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void AddToFavoritesByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            favoriteService.AddToFavorites(testUtil.TestData.nonExistentUserId, link.linkId, testUtil.TestData.favoriteName, testUtil.TestData.favoriteDescription);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void AddNonExistentLinkToFavoritesTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            favoriteService.AddToFavorites(user.userId, testUtil.TestData.nonExistentLinkId, testUtil.TestData.favoriteName, testUtil.TestData.favoriteDescription);
        }

        [TestMethod()]
        [ExpectedException(typeof(DuplicateInstanceException<FavoriteDetails>))]
        public void AddToFavoritesAgainTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            favoriteService.AddToFavorites(user.userId, link.linkId, testUtil.TestData.favoriteName, testUtil.TestData.favoriteDescription);

            /* Check */
            favoriteService.AddToFavorites(user.userId, link.linkId, testUtil.TestData.favoriteName, testUtil.TestData.favoriteDescription);
        }

        [TestMethod()]
        public void UpdateFavoriteTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            Favorite favorite = testUtil.CreateTestFavorite(user, link);
            long favoriteId = favorite.favoriteId;

            /* Use case */
            favoriteService.UpdateFavorite(user.userId, link.linkId, testUtil.TestData.favoriteName2, testUtil.TestData.favoriteDescription2);

            /* Check */
            Favorite actual = testUtil.FindFavorite(favoriteId);
            testUtil.AssertMatch(actual, favoriteId, user.userId, link.linkId, testUtil.TestData.favoriteName2, testUtil.TestData.favoriteDescription2);

            Assert.AreEqual(1, user.Favorites.Count);
            Assert.AreEqual(actual, link.Favorites.ElementAt(0));
            Assert.AreEqual(1, user.Favorites.Count);
            Assert.AreEqual(actual, link.Favorites.ElementAt(0));
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void UpdateFavoriteByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            testUtil.CreateTestFavorite(user, link);

            /* Use case */
            favoriteService.UpdateFavorite(testUtil.TestData.nonExistentUserId, link.linkId, testUtil.TestData.favoriteName2, testUtil.TestData.favoriteDescription2);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void UpdateFavoriteNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            testUtil.CreateTestFavorite(user, link);

            /* Use case */
            favoriteService.UpdateFavorite(user.userId, testUtil.TestData.nonExistentLinkId, testUtil.TestData.favoriteName2, testUtil.TestData.favoriteDescription2);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<FavoriteDetails>))]
        public void UpdateNonFavoriteLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            favoriteService.UpdateFavorite(user.userId, link.linkId, testUtil.TestData.favoriteName2, testUtil.TestData.favoriteDescription2);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<Favorite>))]
        public void RemoveFromFavoritesTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            Favorite favorite = testUtil.CreateTestFavorite(user, link);

            /* Use case */
            favoriteService.RemoveFromFavorites(user.userId, link.linkId);

            /* Check */
            Assert.AreEqual(0, testUtil.FindFavorites(user).Count);
            testUtil.FindFavorite(user, link);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void RemoveFromFavoritesByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            testUtil.CreateTestFavorite(user, link);

            /* Use case */
            favoriteService.RemoveFromFavorites(testUtil.TestData.nonExistentUserId, link.linkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void RemoveNonExistentLinkFromFavoritesTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            testUtil.CreateTestFavorite(user, link);

            /* Use case */
            favoriteService.RemoveFromFavorites(user.userId, testUtil.TestData.nonExistentLinkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<FavoriteDetails>))]
        public void RemoveNonFavoritedLinkFromFavoritesTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            favoriteService.RemoveFromFavorites(user.userId, link.linkId);
        }

    }
}
