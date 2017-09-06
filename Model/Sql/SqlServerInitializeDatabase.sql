/* 
 * SQL Server Script
 * 
 * This script can be directly executed to configure the test database from
 * PCs located at CECAFI Lab. The database and the corresponding users are 
 * already created in the sql server, so it will create the tables needed 
 * in the samples. 
 * 
 * In a local environment (for example, with the SQLServerExpress instance 
 * included in the VStudio installation) it will be necessary to create the 
 * database and the user required by the connection string. So, the following
 * steps are needed:
 *
 *   1) Uncomment lines between [BEGIN] and [END] Local Configuration 
 *      (remove -- characters before each line)
 *   2) Configure within the CREATE DATABASE sql-sentence the path where 
 *      database and log files will be created  
 *
 * This script can be executed from MS Sql Server Management Studio Express,
 * but also it is possible to use a command Line syntax:
 *
 *    > sqlcmd.exe -U [user] -P [password] -I -i SqlServerCreateTables.sql
 *
 */

/* Local Configuration ****************************************************** */

USE [master]
GO

/* Drop database if already exists */
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'PracticaIS')
DROP DATABASE PracticaIS
GO

USE [master]
GO

/* DataBase Creation */
CREATE DATABASE PracticaIS ON PRIMARY 
	(NAME = 'PracticaIS', FILENAME = 'S:\Database\PracticaIS.mdf') 
LOG ON 
	(NAME = 'PracticaIS_log', FILENAME = 'S:\Database\PracticaIS_log.ldf')
GO

/* Create LoginUser */
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'movieMaster')
CREATE LOGIN movieMaster
	WITH PASSWORD='movieRecord',
	DEFAULT_DATABASE=PracticaIS,
	DEFAULT_LANGUAGE=Español,
	CHECK_EXPIRATION=OFF,
	CHECK_POLICY=OFF
GO

/* Set user as database dbo */
USE PracticaIS
GO

SP_CHANGEDBOWNER 'movieMaster'
GO


/* Deletion of users approachs explained */

/* As this is an educative practice we'll be covering the case when an user is
   removed from the dabase physically. On a real environment, we'd recommend
   having an state property on the users where we would set it as "active" or
   "inactive" (or any other different states we'd consider) instead of removing
   them physically from the data base. This would allow not only reactivating an
   user, but having the option to recover usefull data for statistical purposes
   or integrity such as having a "Removed user" message on comments or ratings
   but knowing these are different from one user to another. Just add that we'd
   agree this with the user through some kind of policy document for legal
   reasons, and really allow the user to remove them physically if requested. */

/* Alternatives considered:

   1) Have a "state" column on UserProfile and set it to inactive on "removal"
   but never do it physically. This allows to keep track of the data and to
   reenable an user. although, it requires to take care on any query to check if
   the user is active or not. This is a dangerous approach since future options
   might "resurrect" disabled users, such as happened to Facebook. To face this
   would be possible to have a view over the table where only active users where
   shown and would be more optimal and safe.

   2) Move removed users and their data to side tables. This solution is fine
   but risks that the entries that should be kept for all those deletions are
   overtaken. Anyway this approach is discouraged as there are better options
   for this using the database manager and using time driven databases instead
   of controlling it manually.

   3) Set user to NULL in comments and ratings. This solution has lacks and is
   unable to restore data on undeletion, which is not this case but would be
   nice in a real application. Anyway SQL Server, unlike standard SQL, does
   not allow to have several entries set to NULL on UNIQUEs or PRIMERY KEYs.
   Let's suppose we have users A, B and C, and both rate +1 on Link1; we'll
   have
       (A, Link1, 1) (B, Link1, 1) (C, Link1, 1) -> global rating = 3
   If A and B are removed, on standard SQL we would have
       (null, Link1, 1) (null, Link1, 1) (C, Link1, 1) -> global rating = 3
   but SQL Server would fusion both nulls having
       (null, Link1, 1) (C, Link1, 1) -> global rating = 2
   and thus the global rating would be 2 instead of the desired 3.
   http://msdn.microsoft.com/en-us/library/ms191166.aspx

   4) Delete everything related to user. This is the most simple approach and
   the most appropiate for this aducational environment, specially when the
   removal of users is not anything considered in this practice. On deletion of
   an user, anything related to him or her is removed and so comments and
   ratings are lost. The only interesting thing to be kept as null are the
   links (which could be removed too) but as we consider the publisher of that
   link not a highly relevant datum to be kept, we can have it to null.
   
   There are more alternatives based on database patterns but these are the
   basic ones. */

/* Alternative chosen:
   On first stage we're going for the 4) option as it is the most simple and
   covers the basics requested for this scenario. If we're able we'll turn this
   to option 1) on later development having into account that this system won't
   grow further and only future risks are observed in that case. */

USE PracticaIS

/* Table dropping (if exists) *********************************************** */
/* NOTE: Before dropping a table (when re-executing the script), the tables
   having columns acting as foreign keys of the table to be dropped must be
   dropped first (otherwise, the corresponding checks on those tables could not
   be done). */

/* LinkLabel */
IF EXISTS (SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID('[LinkLabel]') AND type in ('U'))
DROP TABLE LinkLabel
GO

/* Label */
IF EXISTS (SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID('[Label]') AND type in ('U'))
DROP TABLE Label
GO

/* Rating */
IF EXISTS (SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID('[Rating]') AND type in ('U'))
DROP TABLE Rating
GO

/* Favorite */
IF EXISTS (SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID('[Favorite]') AND type in ('U'))
DROP TABLE Favorite
GO

/* Comment */
IF  EXISTS (SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID('[Comment]') AND type in ('U'))
DROP TABLE Comment
GO

/* Link */
IF EXISTS (SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID('[Link]') AND type in ('U'))
DROP TABLE Link
GO

/* UserProfile */
IF EXISTS (SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID('[UserProfile]') AND type in ('U'))
DROP TABLE UserProfile
GO

/* LanguageCountry */
IF EXISTS (SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID('[LanguageCountry]') AND type in ('U'))
DROP TABLE LanguageCountry
GO

/* Language */
IF EXISTS (SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID('[Language]') AND type in ('U'))
DROP TABLE [Language]
GO

/* Country */
IF EXISTS (SELECT *
	FROM sys.objects
	WHERE object_id = OBJECT_ID('[Country]') AND type in ('U'))
DROP TABLE Country
GO

/* Table creation *********************************************************** */

/* Language */
CREATE TABLE [Language] (
	languageCode NCHAR(2) NOT NULL,
	languageName VARCHAR(50) NOT NULL,

	CONSTRAINT PK_Language PRIMARY KEY (languageCode))
GO     


/* Country */
CREATE TABLE Country (
    countryCode NCHAR(2) NOT NULL,
	countryName VARCHAR(50) NOT NULL,

    CONSTRAINT PK_Country PRIMARY KEY (countryCode))
GO

/* CountryLanguage */
CREATE TABLE LanguageCountry (
	languageCode NCHAR(2) NOT NULL,
    countryCode NCHAR(2) NOT NULL,

    CONSTRAINT PK_LanguageCountry PRIMARY KEY (languageCode, countryCode),
    CONSTRAINT FK_LanguageCountry_LanguageCode FOREIGN KEY (languageCode)
			REFERENCES [Language] (languageCode) ON DELETE CASCADE,
	CONSTRAINT FK_LanguageCountry_CountryCode FOREIGN KEY (countryCode)
			REFERENCES Country (countryCode) ON DELETE CASCADE)
GO


/* UserState */
--CREATE TABLE UserState (
--	stateCode INTEGER NOT NULL,
--	stateName VARCHAR(8) NOT NULL,
--
--    CONSTRAINT PK_UserState PRIMARY KEY (stateCode))
--
--INSERT INTO UserState (stateCode, stateName) VALUES (0, 'Disabled')
--INSERT INTO UserState (stateCode, stateName) VALUES (1, 'Pending')
--INSERT INTO UserState (stateCode, stateName) VALUES (2, 'Active')
--GO


/* UserProfile */
CREATE TABLE UserProfile (
	usrId BIGINT IDENTITY (1, 1) NOT NULL,
	usrlogin VARCHAR(30) NOT NULL,
	enPassword VARCHAR(50) NOT NULL,
	firstName VARCHAR(30) NOT NULL,
	lastName VARCHAR(60) NOT NULL,
	email VARCHAR(120) NOT NULL,
	[language] NCHAR(2),
	country NCHAR(2),
--	[state] INTEGER NOT NULL DEFAULT 0, -- Posible state for the alternative solution

    CONSTRAINT PK_UserProfile PRIMARY KEY (usrId),
	CONSTRAINT FK_UserProfile_Language FOREIGN KEY ([language]) 
			REFERENCES [Language] (languageCode) ON DELETE SET NULL,
	CONSTRAINT FK_UserProfile_Country FOREIGN KEY (country) 
			REFERENCES Country (countryCode) ON DELETE SET NULL,
--	CONSTRAINT FK_UserProfile_State FOREIGN KEY (state)
--			REFERENCES UserState (stateCode) ON DELETE SET DEFAULT, -- Enable when using alternative 1) for user deletion
--	CONSTRAINT FK_UserProfile_LanguageCountry FOREIGN KEY ([language], country)
--			REFERENCES LanguageCountry (languageCode, countryCode) ON DELETE SET NULL, -- This is a proper restriction but it prevents the "many to many" relationship between country and language to be automatically generated. So it's removed and controlled in model
    CONSTRAINT UK_UserProfile_UsrLogin UNIQUE (usrLogin))

CREATE NONCLUSTERED INDEX IX_UserProfileIndexByUsrLogin
	ON UserProfile (usrLogin ASC)
CREATE NONCLUSTERED INDEX IX_UserProfileIndexByUsrId
	ON UserProfile (usrId ASC)
GO


/* Label */
CREATE TABLE Label (
	labelId BIGINT IDENTITY (1, 1) NOT NULL,
	labelText VARCHAR(50) NOT NULL,

    CONSTRAINT PK_Label PRIMARY KEY (labelId),
	CONSTRAINT UK_Label_LabelText UNIQUE (labelText))

CREATE NONCLUSTERED INDEX IX_LabelIndexByLabelId
	ON Label (labelId ASC)
CREATE NONCLUSTERED INDEX IX_LabelIndexByLabelText
	ON Label (labelText ASC)
GO


/* Link */
CREATE TABLE Link (
	linkId BIGINT IDENTITY (1, 1) NOT NULL,
    usrId BIGINT, -- Might be null if the user is removed
	movieId BIGINT NOT NULL,
--	movieTitle VARCHAR(150) NOT NULL, -- Not necessary to "cache" this for optimization as every time this entity is retrieved we'll have the movie title avaialble already
	name VARCHAR(50) NOT NULL,
	url VARCHAR(150) NOT NULL,
	[description] VARCHAR(300), -- Can be null if the user adds no description, we could use "" as default and make it null though
    [date] DATETIME2 NOT NULL,
--	globalRating BIGINT, -- This could be persisted for optimization but it'd be required to be updated on every entry of a new rating or rating modification
    reportRead BIT,

    CONSTRAINT PK_Link PRIMARY KEY (linkId ASC),
    CONSTRAINT FK_Link_UsrId FOREIGN KEY (usrId)
		REFERENCES UserProfile (usrId) ON DELETE SET NULL, -- If the user is removed, the link is kept but no user will be displayed
	CONSTRAINT UK_Link_MovieName UNIQUE (movieId, name)) -- The name of a link must be unique for each movie
--	CONSTRAINT UK_Link_Url UNIQUE (url), -- An URL might be unique, but we decided not in order to make a link available for different movies, e.g. a critic of a trilogy we might link on each of the individual movies

CREATE NONCLUSTERED INDEX IX_LinkIndexById
	ON Link (linkId);
CREATE NONCLUSTERED INDEX IX_LinkIndexByIdAndDate
	ON Link (linkId, date);
CREATE NONCLUSTERED INDEX IX_FK_LinkIndexByUser
	ON Link (usrId ASC);
GO


/* LinkLabel */
CREATE TABLE LinkLabel (
	linkId BIGINT NOT NULL,
	labelId BIGINT NOT NULL,

    CONSTRAINT PK_LinkLabel PRIMARY KEY (linkId, labelId),
	CONSTRAINT FK_LinkLabel_LinkId FOREIGN KEY (linkId)
        REFERENCES Link (linkId) ON DELETE CASCADE, -- If the link is removed, thois association is lost
	CONSTRAINT FK_LinkLabel_LabelId FOREIGN KEY (labelId)
        REFERENCES Label (labelId) ON DELETE CASCADE) -- If the label is removed, this association is lost

CREATE NONCLUSTERED INDEX IX_LinkLabelIndexByLinkId
	ON LinkLabel (linkId ASC);
CREATE NONCLUSTERED INDEX IX_LinkLabelIndexByLabelId
	ON LinkLabel (labelId ASC);
GO


/* Rating */
CREATE TABLE Rating (
	ratingId BIGINT IDENTITY (1, 1) NOT NULL,
	usrId BIGINT NOT NULL, -- For this solution we allow only non null users
--	usrId BIGINT, -- Might be null if the user is removed, on some solutions
	linkId BIGINT NOT NULL,
	value INTEGER NOT NULL,
	[date] DATETIME2 NOT NULL,

	CONSTRAINT PK_Rating PRIMARY KEY (ratingId),
	CONSTRAINT FK_Rating_LinkId FOREIGN KEY (linkId)
		REFERENCES Link (linkId) ON DELETE CASCADE,
	CONSTRAINT FK_Rating_UsrId FOREIGN KEY (usrId)
--		REFERENCES UserProfile (usrId) ON DELETE SET NULL, -- If we allow null users, this should be set to null on removal
		REFERENCES UserProfile (usrId) ON DELETE CASCADE, -- On this simple solution, we only consider removing all user info on reomval
	CONSTRAINT UK_Rating_UserLink UNIQUE (usrId, linkId))
--	CONSTRAINT ValidRating CHECK ((value = (-1)) OR (value = (1)))) -- This would be needed if we wanted to make this strict. We leave it open so a pro user could have more value on his or her ratings

CREATE NONCLUSTERED INDEX IX_RatingIndexRatingId
	ON Rating (ratingId ASC)
CREATE NONCLUSTERED INDEX IX_RatingIndexLinkId
	ON Rating (linkId ASC)
GO


/* Comment */
CREATE TABLE Comment (
	commentId BIGINT IDENTITY (1, 1) NOT NULL,
	usrId BIGINT NOT NULL, -- For this solution we allow only non null users
--	usrId BIGINT, -- Might be null if the user is removed, on some solutions
	linkId BIGINT NOT NULL,
--	movieTitle VARCHAR(150) NOT NULL, -- This could be good for optimization if we wanted to have the title available when displaying a comment alone. This is not the case but is left for future purposes
	commentText varchar(300) NOT NULL,
	[date] DATETIME2 NOT NULL,

    CONSTRAINT PK_Comment PRIMARY KEY (commentId),
	CONSTRAINT FK_Comment_UsrId FOREIGN KEY (usrId) 
--		REFERENCES UserProfile (usrId) ON DELETE SET NULL, -- If we allow null users, this should be set to null on removal
		REFERENCES UserProfile (usrId) ON DELETE CASCADE, -- On this simple solution, we only consider removing all user info on reomval
	CONSTRAINT FK_Comment_LinkId FOREIGN KEY (linkId) 
		REFERENCES Link(linkId) ON DELETE CASCADE)

--CREATE NONCLUSTERED INDEX IX_CommentIndexByMovieId
--	ON Comment (movieId ASC) -- If we added the movieId, this would be desirable
CREATE NONCLUSTERED INDEX IX_CommentIndexByUserId
	ON Comment (usrId ASC)
CREATE NONCLUSTERED INDEX IX_CommentIndexByDate 
	ON Comment (commentId, [date]);
GO


/* Favorite */
CREATE TABLE Favorite (
	favoriteId bigint IDENTITY (1, 1) NOT NULL,
	usrId bigint NOT NULL,
	linkId BIGINT NOT NULL,
	name VARCHAR(120),
--	movieTitle VARCHAR(150) NOT NULL, -- Not necessary to "cache" this for optimization as every time this entity is retrieved we'll have the movie title avaialble already. This would be persisted for optimization but is not a proper part of the entity itself. The favorite will be shown without having the reference to the movie and thus instead of retrieving it again we'll save the movie title for display purposes. Actually this is not needed as the titles are cached globally in the web application
	[description] VARCHAR(300), -- Can be null if the user adds no description, we could use "" as default and make it null though
	[date] DATETIME2 NOT NULL,

	CONSTRAINT PK_Favorite PRIMARY KEY (favoriteId),
	CONSTRAINT FK_Favorite_UsrId FOREIGN KEY(usrId)
		REFERENCES UserProfile(usrId) ON DELETE CASCADE, -- This is private data, it only affects data shown to the own user, so on removal, we consider this data to be removed. If, for instance, we considered showing "This link has been favorited X times", this would be public and thus we should treat it setting user reference to null or whaever the aproximation for public data is
	CONSTRAINT FK_Favorite_LinkId FOREIGN KEY (linkId)
		REFERENCES Link (linkId) ON DELETE CASCADE,
	CONSTRAINT UK_Favorite_UserIdLinkId UNIQUE (usrId, linkId))

CREATE NONCLUSTERED INDEX IX_FavoriteIndexByFavoriteId
	ON Favorite (favoriteId ASC)
CREATE NONCLUSTERED INDEX IX_CommentIndexByUsrId
	ON Favorite (usrId ASC)
GO

/* Table population ********************************************************* */

/* Language */
INSERT INTO [Language] (languageCode, languageName) VALUES ('es', 'Español')
INSERT INTO [Language] (languageCode, languageName) VALUES ('gl', 'Galego')
INSERT INTO [Language] (languageCode, languageName) VALUES ('en', 'English')
INSERT INTO [Language] (languageCode, languageName) VALUES ('fr', 'Français')
GO     


/* Country */
INSERT INTO Country (countryCode, countryName) VALUES ('ES', 'España')
INSERT INTO Country (countryCode, countryName) VALUES ('GB', 'United Kingdom')
INSERT INTO Country (countryCode, countryName) VALUES ('US', 'United States of America')
INSERT INTO Country (countryCode, countryName) VALUES ('FR', 'France')
GO


/* CountryLanguage */
INSERT INTO LanguageCountry (languageCode, countryCode) VALUES ('es', 'ES')
INSERT INTO LanguageCountry (languageCode, countryCode) VALUES ('gl', 'ES')
INSERT INTO LanguageCountry (languageCode, countryCode) VALUES ('en', 'US')
INSERT INTO LanguageCountry (languageCode, countryCode) VALUES ('en', 'GB')
INSERT INTO LanguageCountry (languageCode, countryCode) VALUES ('fr', 'FR')
GO


/* UserState */
--INSERT INTO UserState (stateCode, stateName) VALUES (0, 'Disabled')
--INSERT INTO UserState (stateCode, stateName) VALUES (1, 'Pending')
--INSERT INTO UserState (stateCode, stateName) VALUES (2, 'Active')
--GO
