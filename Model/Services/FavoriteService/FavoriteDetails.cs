using System;

namespace Es.Udc.DotNet.WebMovies.Model.Services.FavoriteService
{
    [Serializable()]
    public class FavoriteDetails
    {

        #region Properties Region

        public long FavoriteId { get; set; }

        public long LinkId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        #endregion

        public FavoriteDetails(long favoriteId, long linkId, string name, string description, DateTime date)
        {
            this.FavoriteId = favoriteId;
            this.LinkId = linkId;
            this.Name = name;
            this.Description = description;
            this.Date = date;
        }
                
        public override bool Equals(object obj)
        {
            FavoriteDetails target = (FavoriteDetails) obj;

            return (this.FavoriteId == target.FavoriteId)
                   && (this.LinkId == target.LinkId)
                   && (this.Name == target.Name)
                   && (this.Description == target.Description)
                   && (this.Date == target.Date);
        }
        
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current UserProfile/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.FavoriteId.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            string strFavorite = "[ FavoriteId = " + this.FavoriteId + " | " +
                "LinkId = " + this.LinkId + " | " +
                "Name = " + this.Name + " | " +
                "Description = " + this.Description + " | " +
                "Date = " + this.Date + " ]";

            return strFavorite;
        }

    }
}
