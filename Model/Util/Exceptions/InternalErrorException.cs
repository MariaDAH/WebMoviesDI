using System;

namespace Es.Udc.DotNet.WebMovies.Model.Util.Exceptions
{
    /// <summary>
    /// This exception is never intended to happen. If it is detected it is due
    /// to an error in the application and not because of its normal execution.
    /// It is not intended to be managed and thus is never added in
    /// documentation.
    /// </summary>
    class InternalErrorException
        : Es.Udc.DotNet.ModelUtil.Exceptions.InternalErrorException
    {

        public InternalErrorException(String message)
            : base(message) { }

        public InternalErrorException(Exception encapsulatedException)
            : base(encapsulatedException) { }

    }
}
