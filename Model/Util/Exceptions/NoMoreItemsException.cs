using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.WebMovies.Model.Util.Exceptions
{
    public class NoMoreItemsException<T>
        : ModelException
    {

        public Property[] Properties { get; private set; }

        public NoMoreItemsException(Property[] properties)
            : base("No " + typeof(T).FullName + " item found that matches: " + Property.ToString(properties))
        {
            this.Properties = properties;
        }

        public NoMoreItemsException(Property property)
            : this(new Property[] { property }) { }

        public NoMoreItemsException(string property, object value)
            : this(new Property[] { new Property(property, value) }) { }

        public NoMoreItemsException(string property, object value, string property2, object value2)
            : this(new Property[] { new Property(property, value), new Property(property2, value2) }) { }

        public NoMoreItemsException(object id)
            : this(new Property[] { new Property(typeof(T).Name + "Id", id) }) { }

    }
}
