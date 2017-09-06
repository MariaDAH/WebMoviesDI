using System;

namespace Es.Udc.DotNet.WebMovies.Model.Services.MovieService
{
    [Serializable()]
    public class MovieDetails
    {

        #region Properties Region

        public long MovieId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int LinkCount { get; set; }

        #endregion

        public MovieDetails(long movieId, string title, string description, double price, int linkCount)
        {
            this.MovieId = movieId;
            this.Title = title;
            this.Description = description;
            this.Price = price;
            this.LinkCount = linkCount;
        }

        public MovieDetails() { }

        public override bool Equals(object obj)
        {
            MovieDetails target = (MovieDetails)obj;

            return (this.MovieId == target.MovieId)
                   && (this.Title == target.Title)
                   && (this.Description == target.Description)
                   && (this.Price == target.Price)
                   && (this.LinkCount == target.LinkCount);
        }

        public override int GetHashCode()
        {
            return this.MovieId.GetHashCode();
        }

        public override String ToString()
        {
            String movieDetailsString = "[ MovieId = " + this.MovieId + " | " +
                "Title = " + this.Title + " | " +
                "Description = " + this.Description + " | " +
                "Price = " + this.Price + " | " +
                "LinkCount = " + this.LinkCount + " ]";

            return movieDetailsString;
        }

    }
}
