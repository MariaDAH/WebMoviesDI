using System;
using Es.Udc.DotNet.WebMovies.Model;
using Es.Udc.DotNet.WebMovies.Model.Daos.CommentDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.FavoriteDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.LabelDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.LinkDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.RatingDao;
using Es.Udc.DotNet.WebMovies.Model.Daos.UserProfileDao;
using Es.Udc.DotNet.WebMovies.Model.Services.CommentService;
using Es.Udc.DotNet.WebMovies.Model.Services.FavoriteService;
using Es.Udc.DotNet.WebMovies.Model.Services.LinkService;
using Es.Udc.DotNet.WebMovies.Model.Services.MovieService;
using Es.Udc.DotNet.WebMovies.Model.Services.UserService;
using Es.Udc.DotNet.WebMovies.Model.Util;
using Es.Udc.DotNet.WebMovies.Test.Properties;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Es.Udc.DotNet.WebMovies.Test.Util
{
    /// <summary>
    /// Most the methods in this class might be static. We're leaving them as
    /// object methods because we want to rest assure that there is no kind of
    /// interaction between unit test instances as static instances are shared
    /// among all the objects executed at the same time. Also those methods that
    /// can properly be static with no risk are made object methods for an
    /// homogeneous access.
    /// </summary>
    class TestUtil
    {

        #region Fields & Properties

        private IUnityContainer container;

        private IUserProfileDao userDao;
        private ILinkDao linkDao;
        private ILabelDao labelDao;
        private IRatingDao ratingDao;
        private ICommentDao commentDao;
        private IFavoriteDao favoriteDao;

        public Settings TestData { get; private set; }

        public IUserProfileDao UserDao
        {
            get
            {
                if (userDao == null)
                {
                    userDao = container.Resolve<IUserProfileDao>();
                }

                return userDao;
            }
            private set
            {
                userDao = value;
            }
        }

        public ILinkDao LinkDao
        {
            get
            {
                if (linkDao == null)
                {
                    linkDao = container.Resolve<ILinkDao>();
                }

                return linkDao;
            }
            private set
            {
                linkDao = value;
            }
        }

        public ILabelDao LabelDao
        {
            get
            {
                if (labelDao == null)
                {
                    labelDao = container.Resolve<ILabelDao>();
                }

                return labelDao;
            }
            private set
            {
                labelDao = value;
            }
        }

        public IRatingDao RatingDao
        {
            get
            {
                if (ratingDao == null)
                {
                    ratingDao = container.Resolve<IRatingDao>();
                }

                return ratingDao;
            }
            private set
            {
                ratingDao = value;
            }
        }

        public ICommentDao CommentDao
        {
            get
            {
                if (commentDao == null)
                {
                    commentDao = container.Resolve<ICommentDao>();
                }

                return commentDao;
            }
            private set
            {
                commentDao = value;
            }
        }

        public IFavoriteDao FavoriteDao
        {
            get
            {
                if (favoriteDao == null)
                {
                    favoriteDao = container.Resolve<IFavoriteDao>();
                }

                return favoriteDao;
            }
            private set
            {
                favoriteDao = value;
            }
        }

        #endregion

        public TestUtil(IUnityContainer container)
        {
            this.container = container;

            TestData = Properties.Settings.Default;
        }

        private void ClearDaos()
        {
            UserDao = null;
            LinkDao = null;
            LabelDao = null;
            RatingDao = null;
            CommentDao = null;
            FavoriteDao = null;
        }

        public void Dispose()
        {
            ClearDaos();
        }

        public UserProfile FindUser(long userId)
        {
            return UserDao.Find(userId);
        }

        public Link FindLink(long linkId)
        {
            return LinkDao.Find(linkId);
        }
        public List<Link> ListLinks(UserProfile user)
        {
            return LinkDao.ListForUser(user.userId, 0, 1000);
        }

        public Label FindLabel(long labelId)
        {
            return LabelDao.Find(labelId);
        }
        public Label FindLabel(string text)
        {
            return LabelDao.FindByText(text);
        }
        public List<Label> FindAllLabels()
        {
            return LabelDao.ListAllRated(0, 1000);
        }

        public Rating FindRating(long ratingId)
        {
            return RatingDao.Find(ratingId);
        }
        public Rating FindRating(UserProfile user, Link link)
        {
            return RatingDao.FindForUserAndLink(user.userId, link.linkId);
        }
        public int CalculateRating(UserProfile user)
        {
            return RatingDao.CalculateValueForUser(user.userId);
        }
        public int CalculateRating(Link link)
        {
            return RatingDao.CalculateValueForLink(link.linkId);
        }
        public int CalculateRating(Label label)
        {
            return RatingDao.CalculateValueForLabel(label.text);
        }

        public Comment FindComment(long commentId)
        {
            return CommentDao.Find(commentId);
        }
        public Comment FindComment(UserProfile user, Comment comment)
        {
            return CommentDao.FindForUserAndComment(user.userId, comment.commentId);
        }
        public int CountComments(Link link)
        {
            return CommentDao.CountForLink(link.linkId);
        }

        public Favorite FindFavorite(long favoriteId)
        {
            return FavoriteDao.Find(favoriteId);
        }
        public Favorite FindFavorite(UserProfile user, Link link)
        {
            return FavoriteDao.FindForUserAndLink(user.userId, link.linkId);
        }
        public List<Favorite> FindFavorites(UserProfile user)
        {
            return FavoriteDao.ListForUser(user.userId, 0, 1000);
        }

        public bool ExistsUSer(long userId)
        {
            return UserDao.Exists(userId);
        }

        public bool ExistsLink(long linkId)
        {
            return LinkDao.Exists(linkId);
        }

        public bool ExistsLabel(long labelId)
        {
            return LabelDao.Exists(labelId);
        }

        public bool ExistsRating(long ratingId)
        {
            return RatingDao.Exists(ratingId);
        }

        public bool ExistsComment(long commentId)
        {
            return CommentDao.Exists(commentId);
        }

        public bool ExistsFavorite(long favoriteId)
        {
            return FavoriteDao.Exists(favoriteId);
        }

        private UserProfile CreateTestUser(string login, string password, string name, string lastName, string email, string language, string country)
        {
            UserProfile user = UserProfile.CreateUserProfile(
                TestData.nonExistentUserId,
                login,
                PasswordEncrypter.Crypt(password),
                name,
                lastName,
                email,
                language,
                country);
            UserDao.Create(user);

            return user;
        }
        public UserProfile CreateTestUser()
        {
            return CreateTestUser(TestData.userLogin, TestData.userPassword, TestData.userName, TestData.userLastName, TestData.userEmail, TestData.userLanguage, TestData.userCountry);
        }
        public UserProfile CreateTestUserTwo()
        {
            return CreateTestUser(TestData.user2Login, TestData.user2Password, TestData.user2Name, TestData.user2LastName, TestData.user2Email, TestData.user2Language, TestData.user2Country);
        }
        public UserProfile CreateTestUserThree()
        {
            return CreateTestUser(TestData.user3Login, TestData.user3Password, TestData.user3Name, TestData.user3LastName, TestData.user3Email, TestData.user3Language, TestData.user3Country);
        }
        public UserProfile CreateTestUserFour()
        {
            return CreateTestUser(TestData.user4Login, TestData.user4Password, TestData.user4Name, TestData.user4LastName, TestData.user4Email, TestData.user4Language, TestData.user4Country);
        }

        private Link CreateTestLink(UserProfile user, long movieId, string linkName, string linkDescription, string linkUrl, DateTime time)
        {
            Link link = Link.CreateLink(
                TestData.nonExistentLinkId,
                movieId,
                user.userId,
                linkName,
                linkDescription,
                linkUrl,
                Trunk(time));
            LinkDao.Create(link);

            return link;
        }
        public Link CreateTestLink(UserProfile user)
        {
            return CreateTestLink(user, TestData.movie1Id, TestData.linkName, TestData.linkDescription, TestData.linkUrl, DateTime.Now);
        }
        public Link CreateTestLink(UserProfile user, long movieId)
        {
            return CreateTestLink(user, movieId, TestData.linkName, TestData.linkDescription, TestData.linkUrl, DateTime.Now);
        }
        public Link CreateTestLinkTwo(UserProfile user)
        {
            return CreateTestLink(user, TestData.movie2Id, TestData.link2Name, TestData.link2Description, TestData.link2Url, DateTime.Now.AddDays(1));
        }
        public Link CreateTestLinkTwo(UserProfile user, long movieId)
        {
            return CreateTestLink(user, movieId, TestData.link2Name, TestData.link2Description, TestData.link2Url, DateTime.Now.AddDays(1));
        }
        public Link CreateTestLinkThree(UserProfile user)
        {
            return CreateTestLink(user, TestData.movie3Id, TestData.link3Name, TestData.link3Description, TestData.link3Url, DateTime.Now.AddDays(2));
        }
        public Link CreateTestLinkThree(UserProfile user, long movieId)
        {
            return CreateTestLink(user, movieId, TestData.link3Name, TestData.link3Description, TestData.link3Url, DateTime.Now.AddDays(2));
        }

        public Label CreateTestLabel(string text)
        {
            Label label = Label.CreateLabel(
                TestData.nonExistentLabelId,
                text);
            LabelDao.Create(label);

            return label;
        }
        public Label CreateTestLabel()
        {
            return CreateTestLabel(TestData.labelText);
        }
        public Label CreateTestLabelTwo()
        {
            return CreateTestLabel(TestData.label2Text);
        }
        public Label CreateTestLabelThree()
        {
            return CreateTestLabel(TestData.label3Text);
        }

        public Rating CreateTestRating(UserProfile user, Link link, int value)
        {
            Rating rating = Rating.CreateRating(
                TestData.nonExistentRatingId,
                user.userId,
                link.linkId,
                value,
                Trunk(DateTime.Now));
            RatingDao.Create(rating);

            return rating;
        }
        public void RegisterLabel(Link link, Label label)
        {
            link.Labels.Add(label);
            LinkDao.Update(link);
        }

        private Comment CreateTestComment(UserProfile user, Link link, string text, DateTime time)
        {
            Comment comment = Comment.CreateComment(
                TestData.nonExistentCommentId,
                user.userId,
                link.linkId,
                text,
                Trunk(time));
            CommentDao.Create(comment);

            return comment;
        }
        public Comment CreateTestCommentZero(UserProfile user, Link link)
        {
            return CreateTestComment(user, link, TestData.comment0Text, DateTime.Now.AddDays(-1));
        }
        public Comment CreateTestComment(UserProfile user, Link link)
        {
            return CreateTestComment(user, link, TestData.commentText, DateTime.Now);
        }
        public Comment CreateTestCommentTwo(UserProfile user, Link link)
        {
            return CreateTestComment(user, link, TestData.comment2Text, DateTime.Now.AddDays(1));
        }
        public Comment CreateTestCommentThree(UserProfile user, Link link)
        {
            return CreateTestComment(user, link, TestData.comment3Text, DateTime.Now.AddDays(2));
        }

        private Favorite CreateTestFavorite(UserProfile user, Link link, string favoriteName, string favoriteDescription, DateTime time)
        {
            Favorite favorite = Favorite.CreateFavorite(
                TestData.nonExistentFavoriteId,
                user.userId,
                link.linkId,
                favoriteName,
                favoriteDescription,
                Trunk(time));
            FavoriteDao.Create(favorite);

            return favorite;
        }
        public Favorite CreateTestFavorite(UserProfile user, Link link)
        {
            return CreateTestFavorite(user, link, TestData.favoriteName, TestData.favoriteDescription, DateTime.Now);
        }
        public Favorite CreateTestFavoriteTwo(UserProfile user, Link link)
        {
            return CreateTestFavorite(user, link, TestData.favorite2Name, TestData.favorite2Description, DateTime.Now.AddDays(1));
        }
        public Favorite CreateTestFavoriteThree(UserProfile user, Link link)
        {
            return CreateTestFavorite(user, link, TestData.favorite3Name, TestData.favorite3Description, DateTime.Now.AddDays(2));
        }

        public bool AssertMatch(MovieDetails movieDetails, long movieId, string title, string description, double price)
        {
            Assert.AreEqual(movieId, movieDetails.MovieId);
            Assert.AreEqual(title, movieDetails.Title);
            Assert.AreEqual(description, movieDetails.Description);
            Assert.AreEqual(price, movieDetails.Price);

            return true;
        }
        public bool AssertMatch(MovieDetails movieDetails, long movieId, string title, string description, double price, int linkCount)
        {
            Assert.AreEqual(linkCount, movieDetails.LinkCount);

            return AssertMatch(movieDetails, movieId, title, description, price) && true;
        }

        public bool AssertMatch(UserProfile user, long userId, string login, string encryptedPassword, string firstName, string lastName, string email, string language, string country)
        {
            Assert.AreEqual(user.userId, userId);
            Assert.AreEqual(user.userLogin, login);
            Assert.AreEqual(user.password, encryptedPassword);
            Assert.AreEqual(user.firstName, firstName);
            Assert.AreEqual(user.lastName, lastName);
            Assert.AreEqual(user.email, email);
            Assert.AreEqual(user.languageCode, language);
            Assert.AreEqual(user.countryCode, country);

            return true;
        }
        public bool AssertMatch(UserProfileDetails userDetails, string firstName, string lastName, string email, string language, string country)
        {
            Assert.AreEqual(userDetails.FirstName, firstName);
            Assert.AreEqual(userDetails.LastName, lastName);
            Assert.AreEqual(userDetails.Email, email);
            Assert.AreEqual(userDetails.LanguageCode, language);
            Assert.AreEqual(userDetails.CountryCode, country);

            return true;
        }
        public bool AssertMatch(UserProfile user, UserProfileDetails userDetails)
        {
            AssertMatch(userDetails, user.firstName, user.lastName, user.email, user.languageCode, user.countryCode);

            return true;
        }
        public bool AssertMatch(LoginResult loginResult, long userId, string encryptedPassword, string firstName, string languageCode, string countryCode)
        {
            Assert.AreEqual(loginResult.UserId, userId);
            Assert.AreEqual(loginResult.EncryptedPassword, encryptedPassword);
            Assert.AreEqual(loginResult.FirstName, firstName);
            Assert.AreEqual(loginResult.Language, languageCode);
            Assert.AreEqual(loginResult.Country, countryCode);

            return true;
        }
        public bool AssertMatch(UserProfile user, LoginResult loginResult)
        {
            AssertMatch(loginResult, user.userId, user.password, user.firstName, user.languageCode, user.countryCode);

            return true;
        }

        public bool AssertMatch(Link link, long linkId, long movieId, long userId, string name, string description, string url, bool reportRead, long rating, DateTime date)
        {
            Assert.AreEqual(reportRead, link.reportRead);
            Assert.AreEqual(date, link.date);
            Assert.AreEqual(rating, link.Rating);

            return AssertMatch(link, linkId, movieId, userId, name, description, url) && true;
        }
        public bool AssertMatch(Link link, long linkId, long movieId, long userId, string name, string description, string url, bool reportRead, DateTime date)
        {
            Assert.AreEqual(reportRead, link.reportRead);
            Assert.AreEqual(date, link.date);

            return AssertMatch(link, linkId, movieId, userId, name, description, url) && true;
        }
        public bool AssertMatch(Link link, long linkId, long movieId, long userId, string name, string description, string url, bool reportRead, long rating)
        {
            Assert.AreEqual(reportRead, link.reportRead);
            Assert.AreEqual(rating, link.Rating);

            return AssertMatch(link, linkId, movieId, userId, name, description, url) && true;
        }
        public bool AssertMatch(Link link, long linkId, long movieId, long userId, string name, string description, string url, long rating, DateTime date)
        {
            Assert.AreEqual(date, link.date);
            Assert.AreEqual(rating, link.Rating);

            return AssertMatch(link, linkId, movieId, userId, name, description, url) && true;
        }
        public bool AssertMatch(Link link, long linkId, long movieId, long userId, string name, string description, string url, bool reportRead)
        {
            Assert.AreEqual(reportRead, link.reportRead);

            return AssertMatch(link, linkId, movieId, userId, name, description, url) && true;
        }
        public bool AssertMatch(Link link, long linkId, long movieId, long userId, string name, string description, string url, long rating)
        {
            Assert.AreEqual(rating, link.Rating);

            return AssertMatch(link, linkId, movieId, userId, name, description, url) && true;
        }
        public bool AssertMatch(Link link, long linkId, long movieId, long userId, string name, string description, string url, DateTime date)
        {
            Assert.AreEqual(date, link.date);

            return AssertMatch(link, linkId, movieId, userId, name, description, url) && true;
        }
        public bool AssertMatch(Link link, long linkId, long movieId, long userId, string name, string description, string url)
        {
            Assert.AreEqual(linkId, link.linkId);
            Assert.AreEqual(movieId, link.movieId);
            Assert.AreEqual(userId, link.userId);
            Assert.AreEqual(name, link.name);
            Assert.AreEqual(description, link.description);
            Assert.AreEqual(url, link.url);

            return true;
        }
        public bool AssertMatch(LinkDetails linkDetails, long linkId, long userId, string userName, long movieId, string name, string description, string url, long rating, DateTime date)
        {
            Assert.AreEqual(date, linkDetails.Date);
            
            return AssertMatch(linkDetails, linkId, userId, userName, movieId, name, description, url, rating) && true;
        }
        public bool AssertMatch(LinkDetails linkDetails, long linkId, long userId, string userName, long movieId, string name, string description, string url, long rating)
        {
            Assert.AreEqual(linkId, linkDetails.LinkId);
            Assert.AreEqual(userId, linkDetails.UserId);
            Assert.AreEqual(userName, linkDetails.UserName);
            Assert.AreEqual(movieId, linkDetails.MovieId);
            Assert.AreEqual(name, linkDetails.Name);
            Assert.AreEqual(description, linkDetails.Description);
            Assert.AreEqual(url, linkDetails.Url);
            Assert.AreEqual(rating, linkDetails.Rating);

            return true;
        }
        public bool AssertMatch(Link link, LinkDetails linkDetails)
        {
            return AssertMatch(linkDetails, link.linkId, link.userId, link.UserProfile.userLogin, link.movieId, link.name, link.description, link.url, link.Rating);
        }

        public bool AssertMatch(Comment comment, long commentId, long userId, long linkId, string text, DateTime date)
        {
            Assert.AreEqual(date, comment.date);

            return AssertMatch(comment, commentId, userId, linkId, text) && true;
        }
        public bool AssertMatch(Comment comment, long commentId, long userId, long linkId, string text)
        {
            Assert.AreEqual(commentId, comment.commentId);
            Assert.AreEqual(userId, comment.userId);
            Assert.AreEqual(linkId, comment.linkId);
            Assert.AreEqual(text, comment.text);

            return true;
        }
        public bool AssertMatch(CommentDetails commentDetails, long commentId, string text, long authorId, string authorName, DateTime date)
        {
            Assert.AreEqual(date, commentDetails.Date);

            return AssertMatch(commentDetails, commentId, text, authorId, authorName) && true;
        }
        public bool AssertMatch(CommentDetails commentDetails, long commentId, string text, long authorId, string authorName)
        {
            Assert.AreEqual(commentId, commentDetails.CommentId);
            Assert.AreEqual(text, commentDetails.Text);
            Assert.AreEqual(authorId, commentDetails.AuthorId);
            Assert.AreEqual(authorName, commentDetails.AuthorName);

            return true;
        }
        public bool AssertMatch(Comment comment, CommentDetails commentDetails)
        {
            return AssertMatch(commentDetails, comment.commentId, comment.text, comment.UserProfile.userId, comment.UserProfile.userLogin, comment.date);
        }

        public bool AssertMatch(Favorite favorite, long favoriteId, long userId, long linkId, string name, string description, DateTime date)
        {
            Assert.AreEqual(date, favorite.date);

            return AssertMatch(favorite, favoriteId, userId, linkId, name, description) && true;
        }
        public bool AssertMatch(Favorite favorite, long favoriteId, long userId, long linkId, string name, string description)
        {
            Assert.AreEqual(favoriteId, favorite.favoriteId);
            Assert.AreEqual(userId, favorite.userId);
            Assert.AreEqual(linkId, favorite.linkId);
            Assert.AreEqual(name, favorite.name);
            Assert.AreEqual(description, favorite.description);

            return true;
        }
        public bool AssertMatch(FavoriteDetails favoriteDetails, long favoriteId, long linkId, string name, string description, DateTime date)
        {
            Assert.AreEqual(date, favoriteDetails.Date);

            return AssertMatch(favoriteDetails, favoriteId, linkId, name, description) && true;
        }
        public bool AssertMatch(FavoriteDetails favoriteDetails, long favoriteId, long linkId, string name, string description)
        {
            Assert.AreEqual(favoriteId, favoriteDetails.FavoriteId);
            Assert.AreEqual(linkId, favoriteDetails.LinkId);
            Assert.AreEqual(name, favoriteDetails.Name);
            Assert.AreEqual(description, favoriteDetails.Description);

            return true;
        }
        public bool AssertMatch(Favorite favorite, FavoriteDetails favoriteDetails)
        {
            return AssertMatch(favoriteDetails, favorite.favoriteId, favorite.linkId, favorite.name, favorite.description, favorite.date);
        }

        public bool AssertMatch(Rating rating, long ratingId, long userId, long linkId, int value, DateTime date)
        {
            Assert.AreEqual(date, rating.date);

            return AssertMatch(rating, ratingId, userId, linkId, value) && true;
        }
        public bool AssertMatch(Rating rating, long ratingId, long userId, long linkId, int value)
        {
            Assert.AreEqual(ratingId, rating.ratingId);
            Assert.AreEqual(userId, rating.userId);
            Assert.AreEqual(linkId, rating.linkId);
            Assert.AreEqual(value, rating.value);

            return true;
        }

        public DateTime Trunk(DateTime time)
        {
            return new DateTime((long)(time.Ticks / TimeSpan.TicksPerSecond));
        }
        public DateTime Now()
        {
            return Trunk(DateTime.Now);
        }

    }
}
