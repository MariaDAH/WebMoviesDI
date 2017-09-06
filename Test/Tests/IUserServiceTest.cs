using System.Transactions;
using Es.Udc.DotNet.WebMovies.Model;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;
using Es.Udc.DotNet.WebMovies.Model.Util.Exceptions;
using Es.Udc.DotNet.WebMovies.Test.Util;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Es.Udc.DotNet.WebMovies.Test.Tests
{
    /// <summary>
    /// This is a test class for IUserServiceTest and is intended
    /// to contain all IUserServiceTest Unit Tests
    /// </summary>
    [TestClass()]
    public class IUserServiceTest
    {

        #region Fields and properties

        private TestUtil testUtil;

        private static IUnityContainer container;

        private static IUserService userService;

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

            userService = container.Resolve<IUserService>();
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
        public void RegisterUserTest()
        {
            /* Use case */
            long userId = userService.Register(testUtil.TestData.userLogin, testUtil.TestData.userPassword, testUtil.TestData.userName, testUtil.TestData.userLastName, testUtil.TestData.userEmail, testUtil.TestData.userLanguage, testUtil.TestData.userCountry);

            /* Check */
            UserProfile actual = testUtil.FindUser(userId);
            testUtil.AssertMatch(actual, userId, testUtil.TestData.userLogin, testUtil.TestData.userPasswordEncrypted, testUtil.TestData.userName, testUtil.TestData.userLastName, testUtil.TestData.userEmail, testUtil.TestData.userLanguage, testUtil.TestData.userCountry);
        }

        [TestMethod()]
        [ExpectedException(typeof(DuplicateInstanceException<UserProfileDetails>))]
        public void RegisterDuplicatedUserTest()
        {
            /* Initialization */
            testUtil.CreateTestUser();

            /* Use case */
            userService.Register(testUtil.TestData.userLogin, testUtil.TestData.userPassword, testUtil.TestData.userName, testUtil.TestData.userLastName, testUtil.TestData.userEmail, testUtil.TestData.userLanguage, testUtil.TestData.userCountry);
        }

        [TestMethod()]
        public void LoginWithClearPasswordTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            LoginResult actual = userService.Login(testUtil.TestData.userLogin, testUtil.TestData.userPassword, false);

            /* Check */
            testUtil.AssertMatch(user, actual);
        }

        [TestMethod()]
        public void LoginWithEncryptedPasswordTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            LoginResult actual = userService.Login(testUtil.TestData.userLogin, testUtil.TestData.userPasswordEncrypted, true);

            /* Check */
            testUtil.AssertMatch(user, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(IncorrectPasswordException))]
        public void LoginWithIncorrectPasswordTest()
        {
            /* Initialization */
            testUtil.CreateTestUser();

            /* Use case */
            userService.Login(testUtil.TestData.userLogin, testUtil.TestData.userPassword2, false);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void LoginNonExistentUserTest()
        {
            /* Use case */
            userService.Login(testUtil.TestData.userLogin, testUtil.TestData.userPassword, false);
        }

        [TestMethod()]
        public void GetUserProfileTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            UserProfileDetails actual = userService.GetUserProfile(user.userId);

            /* Check */
            testUtil.AssertMatch(user, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void GetNonExistentUserProfileTest()
        {
            /* Use case */
            userService.GetUserProfile(testUtil.TestData.nonExistentUserId);
        }

        [TestMethod()]
        public void UpdateUserProfileTest()
        {
            UserProfile user = testUtil.CreateTestUser();

            userService.UpdateUserProfile(user.userId, testUtil.TestData.userName2, testUtil.TestData.userLastName2, testUtil.TestData.userEmail2, testUtil.TestData.userLanguage2, testUtil.TestData.userCountry2);

            UserProfileDetails actual = userService.GetUserProfile(user.userId);
            testUtil.AssertMatch(actual, testUtil.TestData.userName2, testUtil.TestData.userLastName2, testUtil.TestData.userEmail2, testUtil.TestData.userLanguage2, testUtil.TestData.userCountry2);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void UpdateNonExistentUserProfileTest()
        {
            /* Use case */
            userService.UpdateUserProfile(testUtil.TestData.nonExistentUserId, testUtil.TestData.userName, testUtil.TestData.userLastName, testUtil.TestData.userEmail, testUtil.TestData.userLanguage, testUtil.TestData.userCountry);
        }

        [TestMethod()]
        public void ChangePasswordTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            userService.ChangePassword(user.userId, testUtil.TestData.userPassword, testUtil.TestData.userPassword2);

            /* Check */
            string actual = testUtil.FindUser(user.userId).password;
            Assert.AreEqual(testUtil.TestData.userPassword2Encrypted, actual);
            Assert.AreEqual(testUtil.TestData.userPassword2Encrypted, user.password);
            userService.Login(user.userLogin, testUtil.TestData.userPassword2, false);
        }

        [TestMethod()]
        [ExpectedException(typeof(IncorrectPasswordException))]
        public void ChangePasswordIncorrectlyTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            userService.ChangePassword(user.userId, testUtil.TestData.user2Password, testUtil.TestData.userPassword2);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void ChangePasswordForNonExistentUserTest()
        {
            /* Use case */
            userService.ChangePassword(testUtil.TestData.nonExistentUserId, testUtil.TestData.userPassword, testUtil.TestData.userPassword2);
        }

    }
}
