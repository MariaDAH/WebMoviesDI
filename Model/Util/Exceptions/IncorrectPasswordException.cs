using System;
using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.WebMovies.Model.Util.Exceptions
{
    /// <summary>
    /// Public <c>ModelException</c> which captures the error with the passwords of the users.
    /// </summary>
    public class IncorrectPasswordException
        : ModelException
    {

        /// <summary>
        /// Stores the User login name of the exception
        /// </summary>
        /// <value>The name of the login.</value>
        public String LoginName { get; private set; }


        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IncorrectPasswordException"/> class.
        /// </summary>
        /// <param name="loginName"><c>loginName</c> that causes the error.</param>
        public IncorrectPasswordException(String loginName)
            : base("Incorrect password exception for user \"" + loginName + "\"")
        {
            this.LoginName = loginName;
        }

    }
}
