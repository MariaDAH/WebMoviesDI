using System;

namespace Es.Udc.DotNet.WebMovies.Model.Util.Exceptions
{
    class XmlErrorException
        : InternalErrorException
    {

        public XmlErrorException(String message)
            : base("Failure processing XML: " + message) { }

    }
}
