using System;

namespace Es.Udc.DotNet.WebMovies.Model.Util
{
    [Serializable()]
    public struct Locale
    {

        private string country;
        private string language;

        #region Properties region

        public string Language
        {
            get { return language; }
            set { language = value; }
        }


        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        #endregion

        public Locale(string language, string country)
            : this()
        {
            this.language = language;
            this.country = country;
        }

        public override bool Equals(object obj)
        {
            Locale target = (Locale)obj;

            return (this.language == target.language)
                   && (this.country == target.country);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + this.language.GetHashCode();
            hash = hash * 23 + this.country.GetHashCode();
            return hash;
        }

    }
}
