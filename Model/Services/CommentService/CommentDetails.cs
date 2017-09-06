using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model.Services.CommentService
{
    [Serializable()]
    public class CommentDetails
    {

        #region Properties Region

        public long CommentId { get; private set; }

        public string Text { get; private set; }

        public long AuthorId { get; private set; }

        public string AuthorName { get; private set; }

        public long LinkId { get; private set; }

        public DateTime Date { get; private set; }
        
        #endregion

        public CommentDetails(long commentId, string text, long authorId, string authorName, long linkId, DateTime date)
        {
            this.CommentId = commentId;
            this.Text = text;
            this.AuthorId = authorId;
            this.AuthorName = authorName;
            this.LinkId = linkId;
            this.Date = date;
        }
        
        public override bool Equals(object obj)
        {
            CommentDetails target = (CommentDetails)obj;

            return (this.CommentId == target.CommentId)
                   && (this.Text == target.Text)
                   && (this.AuthorId == target.AuthorId)
                   && (this.AuthorName == target.AuthorName)
                   && (this.LinkId == target.LinkId)
                   && (this.Date == this.Date);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current UserProfile/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.CommentId.GetHashCode();
        }
        
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            string strComment = "[ CommentId = " + this.CommentId + " | " +
                "Text = " + this.Text + " | " +
                "AuthorId = " + this.AuthorId + " | " +
                "AuthorName = " + this.AuthorName + " | " +
                "LinkId = " + this.LinkId + " | " +
                "Date = " + this.Date + " ]";

            return strComment;
        }

    }
}
