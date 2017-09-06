using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.WebMovies.Model.Util.Exceptions
{
    class NetworkException
        : InternalErrorException
    {

        public NetworkException(Exception encapsulatedException)
            : base(encapsulatedException) { }

    }
}
