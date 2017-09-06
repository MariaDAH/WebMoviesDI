using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Es.Udc.DotNet.WebMovies.Model;
using Es.Udc.DotNet.WebMovies.Model.Services.LabelService;
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
    /// This is a test class for IfavoriteServiceTest and is intended
    /// to contain all IfavoriteServiceTest Unit Tests
    /// </summary>
    [TestClass()]
    public class ILabelTest
    {

        #region Fields and properties

        private TestUtil testUtil;

        private static IUnityContainer container;

        private static ILabelService labelService;

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

            labelService = container.Resolve<ILabelService>();
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
        public void GetMostValuedLabelsTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Label label = testUtil.CreateTestLabel();
            Label label2 = testUtil.CreateTestLabelTwo();
            Label label3 = testUtil.CreateTestLabelThree();

            Link link = testUtil.CreateTestLink(user);
            testUtil.RegisterLabel(link, label);
            testUtil.RegisterLabel(link, label2);
            testUtil.RegisterLabel(link, label3);

            Link link2 = testUtil.CreateTestLinkTwo(user);
            testUtil.RegisterLabel(link2, label2);
            testUtil.RegisterLabel(link2, label3);

            Link link3 = testUtil.CreateTestLinkThree(user);
            testUtil.RegisterLabel(link3, label3);

            testUtil.CreateTestRating(user, link2, 1);
            testUtil.CreateTestRating(user, link3, 1);
            testUtil.CreateTestRating(user2, link3, 1);

            /* Use case */
            DictionaryBlock<string, long> actual = labelService.GetMostValuedLabels(0, 2);

            /* Check */
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(label3.text, actual.Keys.ElementAt(0));
            Assert.AreEqual(3, actual[label3.text]);
            Assert.AreEqual(label2.text, actual.Keys.ElementAt(1));
            Assert.AreEqual(2, actual[label2.text]);
        }

        [TestMethod()]
        public void GetLabelsForLinkTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            DictionaryBlock<string, long> actual = labelService.GetLabelsForLink(link.linkId, 0, 10);

            /* Check */
            Assert.AreEqual(0, actual.Count);

            /* *** Stage 2 ************************************************** */

            /* Initialization */
            Label label = testUtil.CreateTestLabel();
            testUtil.RegisterLabel(link, label);

            /* Use case */
            actual = labelService.GetLabelsForLink(link.linkId, 0, 10);

            /* Check */
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(label.text, actual.Keys.ElementAt(0));
            Assert.AreEqual(1, actual[label.text]);

            /* *** Stage 3 ************************************************** */

            /* Initialization */
            Link link2 = testUtil.CreateTestLinkTwo(user);

            Label label2 = testUtil.CreateTestLabelTwo();

            testUtil.RegisterLabel(link, label2);
            testUtil.RegisterLabel(link2, label2);
            testUtil.CreateTestRating(user, link2, 1);

            /* Use case */
            actual = labelService.GetLabelsForLink(link.linkId, 0, 10);

            /* Check */
            Assert.AreEqual(2, link.Labels.Count);
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(2, actual[label2.text]);
            Assert.AreEqual(1, actual[label.text]);

            /* *** Stage 4 ************************************************** */

            /* Initialization */
            Link link3 = testUtil.CreateTestLinkThree(user);

            Label label3 = testUtil.CreateTestLabelThree();

            testUtil.RegisterLabel(link, label3);
            testUtil.RegisterLabel(link2, label3);
            testUtil.RegisterLabel(link3, label3);
            testUtil.CreateTestRating(user, link3, 1);

            UserProfile user2 = testUtil.CreateTestUserTwo();

            testUtil.CreateTestRating(user2, link3, 1);

            /* Use case */
            actual = labelService.GetLabelsForLink(link.linkId, 0, 2);

            /* Check */
            Assert.AreEqual(3, link.Labels.Count);
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(3, actual[label3.text]);
            Assert.AreEqual(2, actual[label2.text]);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void GetLabelsForNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            DictionaryBlock<string, long> actual = labelService.GetLabelsForLink(testUtil.TestData.nonExistentLinkId, 0, 10);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<Label>))]
        public void SetLabelsForLinkTest()
        {
            /* *** Stage 1 ************************************************** */

            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            labelService.SetLabelsForLink(user.userId, link.linkId, new List<string>() { testUtil.TestData.labelText, testUtil.TestData.label2Text });

            /* Check */
            Assert.AreEqual(2, link.Labels.Count);
            Assert.AreEqual(testUtil.TestData.labelText, link.Labels.ElementAt(0).text);
            Assert.AreEqual(testUtil.TestData.label2Text, link.Labels.ElementAt(1).text);

            /* *** Stage 2 ************************************************** */

            /* Use case */
            labelService.SetLabelsForLink(user.userId, link.linkId, new List<string>() { testUtil.TestData.label2Text, testUtil.TestData.label3Text });

            /* Check */
            Assert.AreEqual(2, link.Labels.Count);
            Assert.AreEqual(testUtil.TestData.label3Text, link.Labels.ElementAt(0).text);
            Assert.AreEqual(testUtil.TestData.label2Text, link.Labels.ElementAt(1).text);
            testUtil.FindLabel(testUtil.TestData.labelText);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void SetLabelsByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            labelService.SetLabelsForLink(testUtil.TestData.nonExistentUserId, link.linkId, new List<string>() { testUtil.TestData.labelText, testUtil.TestData.label2Text });
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void SetLabelsForNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            labelService.SetLabelsForLink(user.userId, testUtil.TestData.nonExistentLinkId, new List<string>() { testUtil.TestData.labelText, testUtil.TestData.label2Text });
        }

        [TestMethod()]
        [ExpectedException(typeof(UserNotAuthorizedException<LinkDetails>))]
        public void SetLabelsForLinkByAnotherUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            labelService.SetLabelsForLink(user2.userId, link.linkId, new List<string>() { testUtil.TestData.labelText, testUtil.TestData.label2Text });
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<Label>))]
        public void RemoveLabelsForLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            Label label = testUtil.CreateTestLabel();
            Label label2 = testUtil.CreateTestLabelTwo();

            testUtil.RegisterLabel(link, label);
            testUtil.RegisterLabel(link, label2);

            /* Use case */
            labelService.RemoveLabelsForLink(user.userId, link.linkId);

            /* Check */
            Assert.AreEqual(0, link.Labels.Count);
            Assert.AreEqual(0, testUtil.FindAllLabels().Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<UserProfileDetails>))]
        public void RemoveLabelsByNonExistentUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            labelService.RemoveLabelsForLink(testUtil.TestData.nonExistentUserId, link.linkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException<LinkDetails>))]
        public void RemoveLabelsForNonExistentLinkTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();

            /* Use case */
            labelService.RemoveLabelsForLink(user.userId, testUtil.TestData.nonExistentLinkId);
        }

        [TestMethod()]
        [ExpectedException(typeof(UserNotAuthorizedException<LinkDetails>))]
        public void RemoveLabelsForLinkByAnotherUserTest()
        {
            /* Initialization */
            UserProfile user = testUtil.CreateTestUser();
            UserProfile user2 = testUtil.CreateTestUserTwo();

            Link link = testUtil.CreateTestLink(user);

            /* Use case */
            labelService.RemoveLabelsForLink(user2.userId, link.linkId);
        }

    }
}
