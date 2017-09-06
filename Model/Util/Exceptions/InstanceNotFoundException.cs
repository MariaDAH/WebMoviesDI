namespace Es.Udc.DotNet.WebMovies.Model.Util.Exceptions
{
    public class InstanceNotFoundException<T>
        : Es.Udc.DotNet.ModelUtil.Exceptions.InstanceNotFoundException
    {

        public Property[] Properties { get; private set; }

        public InstanceNotFoundException(Property[] properties)
            : base(Property.ToString(properties), typeof(T).FullName)
        {
            this.Properties = properties;
        }

        public InstanceNotFoundException(Property property)
            : this(new Property[] { property }) { }

        public InstanceNotFoundException(string property, object value)
            : this(new Property[] { new Property(property, value) }) { }

        public InstanceNotFoundException(string property, object value, string property2, object value2)
            : this(new Property[] { new Property(property, value), new Property(property2, value2) }) { }

        public InstanceNotFoundException(object id)
            : this(new Property[] { new Property(typeof(T).Name + "Id", id) }) { }

    }
}
