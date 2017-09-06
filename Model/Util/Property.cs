using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model.Util
{
    public struct Property
    {

        public string Name;

        public object Value;
        
        public Property(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
        
        public new string ToString()
        {
            return this.Name + "=" + this.Value;
        }
        
        public static string ToString(Property[] properties)
        {
            string propertiesString = "";

            foreach (Property property in properties)
            {
                propertiesString += " & " + property.ToString();
            }

            return propertiesString.Substring(3);
        }

    }
}
