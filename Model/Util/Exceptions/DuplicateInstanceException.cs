namespace Es.Udc.DotNet.WebMovies.Model.Util.Exceptions
{
    public class DuplicateInstanceException<T>
        : Es.Udc.DotNet.ModelUtil.Exceptions.DuplicateInstanceException
    {

        public Property[] Properties { get; private set; }

        public DuplicateInstanceException(Property[] properties)
            : base(Property.ToString(properties), typeof(T).FullName)
        {
            this.Properties = properties;
        }

        public DuplicateInstanceException(Property property)
            : this(new Property[] { property }) { }

        public DuplicateInstanceException(string property, object value)
            : this(new Property[] { new Property(property, value) }) { }

        public DuplicateInstanceException(string property, object value, string property2, object value2)
            : this(new Property[] { new Property(property, value), new Property(property2, value2) }) { }

        public DuplicateInstanceException(object id)
            : this(new Property[] { new Property(typeof(T).Name + "Id", id) }) { }

    }
}
