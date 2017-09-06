using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.WebMovies.Model.Util.Exceptions
{
    /// <summary>
    /// Public <c>ModelException</c> which captures the error 
    /// with the owner of a link.
    /// </summary>
    public class UserNotAuthorizedException<T>
        : ModelException
    {

        /// <summary>
        /// Stores the id of the user
        /// </summary>
        /// <value>The id of the user.</value>
        public long UserId { get; private set; }

        /// <summary>
        /// Stores the properties of the target that selects the proper entities
        /// </summary>
        /// <value>The id of the target.</value>
        public Property[] Properties { get; private set; }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="NotOwnerException"/> class.
        /// </summary>
        /// <param name="userId"><c>userId</c> that causes the error.</param>
        /// <param name="linkId"><c>linkId</c> that causes the error.</param>
        public UserNotAuthorizedException(long userId, Property[] properties)
            : base("No " + typeof(T).FullName + " item found that matches: " + Property.ToString(properties))
        {
            this.UserId = userId;
            this.Properties = properties;
        }

        public UserNotAuthorizedException(long userId, Property property)
            : this(userId, new Property[] { property }) { }

        public UserNotAuthorizedException(long userId, string property, object value)
            : this(userId, new Property[] { new Property(property, value) }) { }

        public UserNotAuthorizedException(long userId, string property, object value, string property2, object value2)
            : this(userId, new Property[] { new Property(property, value), new Property(property2, value2) }) { }

        public UserNotAuthorizedException(long userId, object id)
            : this(userId, new Property[] { new Property(typeof(T).Name + "Id", id) }) { }

    }
}
