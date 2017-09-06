using System.Transactions;
using Es.Udc.DotNet.WebMovies.Model;
using Es.Udc.DotNet.WebMovies.Model.Services.CommentService;
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
    /// This is a test class for ICommentService and is intended to contain all Unit Tests
    /// </summary>
    [TestClass()]
    public class ICommentServiceTest
    {

        #region Fields and properties

        private TestUtil testUtil;

        private static IUnityContainer container;

        private static ICommentService commentService;

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

            commentService = container.Resolve<ICommentService>();
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
        public void GetCommentTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            Comment comment = testUtil.CreateTestComment(user, link);

            /* Use case */
            CommentDetails actual = commentService.GetComment(comment.commentId);

            /* Check */
            testUtil.AssertMatch(comment, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<CommentDetails>))]
        public void GetNonExistentCommentTest()
        {
            /* Use case */
            commentService.GetComment(testUtil.TestData.nonExistentCommentId);
        }

        [TestMethod()]
        public void GetCommentsForLinkTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            ListBlock<CommentDetails> actual = commentService.GetCommentsForLink(link.linkId, 0, 10);

            /* Check */
            Assert.AreEqual(0, actual.Count);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Comment comment = testUtil.CreateTestComment(user, link);
            Comment comment2 = testUtil.CreateTestCommentTwo(user, link);

            /* Use case */
            actual = commentService.GetCommentsForLink(link.linkId, 0, 10);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(actual[0], comment2.commentId, testUtil.TestData.comment2Text, user.userId, user.userLogin, comment2.date);
            testUtil.AssertMatch(actual[1], comment.commentId, testUtil.TestData.commentText, user.userId, user.userLogin, comment.date);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            Comment comment0 = testUtil.CreateTestCommentZero(user, link);
            Comment comment3 = testUtil.CreateTestCommentThree(user, link);

            /* Use case */
            actual = commentService.GetCommentsForLink(link.linkId, 1, 2);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(actual[0], comment2.commentId, testUtil.TestData.comment2Text, user.userId, user.userLogin, comment2.date);
            testUtil.AssertMatch(actual[1], comment.commentId, testUtil.TestData.commentText, user.userId, user.userLogin, comment.date);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void GetCommentsForNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            commentService.GetCommentsForLink(testUtil.TestData.nonExistentLinkId, 0, 10);
        }

        [TestMethod()]
        public void GetCommentsForUserTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            ListBlock<CommentDetails> actual = commentService.GetCommentsForUser(user.userId, 0, 10);

            /* Check */
            Assert.AreEqual(0, actual.Count);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Comment comment = testUtil.CreateTestComment(user, link);
            Comment comment2 = testUtil.CreateTestCommentTwo(user, link);

            /* Use case */
            actual = commentService.GetCommentsForUser(user.userId, 0, 10);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(actual[0], comment2.commentId, testUtil.TestData.comment2Text, user.userId, user.userLogin, comment2.date);
            testUtil.AssertMatch(actual[1], comment.commentId, testUtil.TestData.commentText, user.userId, user.userLogin, comment.date);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            Comment comment0 = testUtil.CreateTestCommentZero(user, link);
            Comment comment3 = testUtil.CreateTestCommentThree(user, link);

            /* Use case */
            actual = commentService.GetCommentsForUser(user.userId, 1, 2);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            testUtil.AssertMatch(actual[0], comment2.commentId, testUtil.TestData.comment2Text, user.userId, user.userLogin, comment2.date);
            testUtil.AssertMatch(actual[1], comment.commentId, testUtil.TestData.commentText, user.userId, user.userLogin, comment.date);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void GetCommentsForNonExistentUserTest()
        {
            /* Use case */
            commentService.GetCommentsForUser(testUtil.TestData.nonExistentUserId, 0, 10);
        }

        [TestMethod()]
        public void CountCommentsForLinkTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            int actual = commentService.CountCommentsForLink(link.linkId);

            /* Check */
            Assert.AreEqual(0, actual);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Comment comment = testUtil.CreateTestComment(user, link);
            Comment comment2 = testUtil.CreateTestCommentTwo(user, link);

            /* Use case */
            actual = commentService.CountCommentsForLink(link.linkId);

            /* Check */
            Assert.AreEqual(2, actual);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            Comment comment0 = testUtil.CreateTestCommentZero(user, link);
            Comment comment3 = testUtil.CreateTestCommentThree(user, link);

            /* Use case */
            actual = commentService.CountCommentsForLink(link.linkId);

            /* Check */
            Assert.AreEqual(4, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void CountCommentsForNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            commentService.CountCommentsForLink(testUtil.TestData.nonExistentLinkId);
        }

        [TestMethod()]
        public void CountCommentsForUserTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            int actual = commentService.CountCommentsForUser(user.userId);

            /* Check */
            Assert.AreEqual(0, actual);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Comment comment = testUtil.CreateTestComment(user, link);
            Comment comment2 = testUtil.CreateTestCommentTwo(user, link);

            /* Use case */
            actual = commentService.CountCommentsForUser(user.userId);

            /* Check */
            Assert.AreEqual(2, actual);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            Comment comment0 = testUtil.CreateTestCommentZero(user, link);
            Comment comment3 = testUtil.CreateTestCommentThree(user, link);

            /* Use case */
            actual = commentService.CountCommentsForUser(user.userId);

            /* Check */
            Assert.AreEqual(4, actual);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void CountCommentsForNonExistentUserTest()
        {
            /* Use case */
            commentService.CountCommentsForUser(testUtil.TestData.nonExistentUserId);
        }

        [TestMethod()]
        public void AddCommentTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            long commentId = commentService.AddComment(user.userId, link.linkId, testUtil.TestData.commentText);

            /* Check */
            Comment actual = testUtil.FindComment(commentId);
            testUtil.AssertMatch(actual, commentId, user.userId, link.linkId, testUtil.TestData.commentText);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void AddCommentByNonExistentUserTest()
        {
            /* Use case */
            long commentId = commentService.AddComment(testUtil.TestData.nonExistentUserId, testUtil.TestData.nonExistentLinkId, testUtil.TestData.commentText);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void AddCommentToNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            long commentId = commentService.AddComment(user.userId, testUtil.TestData.nonExistentLinkId, testUtil.TestData.commentText);
        }

        [TestMethod()]
        public void UpdateCommentTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            Comment comment = testUtil.CreateTestComment(user, link);

            /* Use case */
            commentService.UpdateComment(user.userId, comment.commentId, testUtil.TestData.commentText2);

            /* Check */
            Comment actual = testUtil.FindComment(comment.commentId);
            testUtil.AssertMatch(actual, comment.commentId, user.userId, link.linkId, testUtil.TestData.commentText2, comment.date);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<CommentDetails>))]
        public void UpdateNonExistentCommentTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            commentService.UpdateComment(user.userId, testUtil.TestData.nonExistentCommentId, testUtil.TestData.commentText2);
        }

        [TestMethod()]
        [ExpectedException(typeof(UserNotAuthorizedException<CommentDetails>))]
        public void UpdateCommentByAnotherUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Link link = testUtil.CreateTestLink(user);

            Comment comment = testUtil.CreateTestComment(user, link);

            /* Use case */
            commentService.UpdateComment(user2.userId, comment.commentId, testUtil.TestData.commentText2);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void UpdateCommentByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            Comment comment = testUtil.CreateTestComment(user, link);

            /* Use case */
            commentService.UpdateComment(testUtil.TestData.nonExistentUserId, comment.commentId, testUtil.TestData.commentText2);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<Comment>))]
        public void RemoveCommentTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            Comment comment = testUtil.CreateTestComment(user, link);

            /* Use case */
            commentService.RemoveComment(user.userId, comment.commentId);

            /* Check */
            Assert.IsFalse(testUtil.ExistsComment(comment.commentId));
            Assert.AreEqual(0, testUtil.CountComments(link));
            testUtil.FindComment(user, comment);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<Comment>))]
        public void RemoveCommentByLinkAuthorTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Link link = testUtil.CreateTestLink(user);

            Comment comment = testUtil.CreateTestComment(user2, link);

            /* Use case */
            commentService.RemoveComment(user.userId, comment.commentId);

            /* Check */
            Assert.IsFalse(testUtil.ExistsComment(comment.commentId));
            Assert.AreEqual(0, testUtil.CountComments(link));
            testUtil.FindComment(user, comment);
        }

        [TestMethod()]
        [ExpectedException(typeof(UserNotAuthorizedException<CommentDetails>))]
        public void RemoveCommentByAnotherUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();
            UserProfile user3 = testUtil.CreateTestUserThree();

            Link link = testUtil.CreateTestLink(user);

            Comment comment = testUtil.CreateTestComment(user2, link);

            /* Use case */
            commentService.RemoveComment(user3.userId, comment.commentId);

            /* Check */
            Assert.IsFalse(testUtil.ExistsComment(comment.commentId));
            Assert.AreEqual(0, testUtil.CountComments(link));
            testUtil.FindComment(user, comment);
        }

    }
}
