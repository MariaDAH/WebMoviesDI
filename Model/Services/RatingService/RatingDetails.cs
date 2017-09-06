using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model.Services.RatingService
{
    /// <summary>
    /// This class is a stub and is not intended to be used.
    /// Its main purpose is to serve as a friendly representation of the Rating
    /// entity for the UserNotAuthorizedException exception.
    /// </summary>
    [Serializable()]
    public class RatingDetails
    {

        #region Properties Region

        /// <summary>
        /// ID for the rating.
        /// </summary>
        /// <value>The rating id.</value>
        public long RatingId { get; private set; }

        /// <summary>
        /// ID of the user who added the link.
        /// </summary>
        /// <value>The user ID.</value>
        public long UserId { get; private set; }

        /// <summary>
        /// ID for the link.
        /// </summary>
        /// <value>The link id.</value>
        public long LinkId { get; private set; }

        /// <summary>
        /// Date of the addition of the link.
        /// </summary>
        /// <value>The date of addition.</value>
        public int Value { get; private set; }

        #endregion

        public RatingDetails(long ratingId, long linkId, long userId, string userName, int value)
        {
            this.RatingId = ratingId;
            this.LinkId = linkId;
            this.UserId = userId;
            this.Value = value;
        }

    }
}
