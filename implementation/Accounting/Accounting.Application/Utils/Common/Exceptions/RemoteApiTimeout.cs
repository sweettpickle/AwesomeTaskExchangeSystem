
using Accounting.Shared.Exceptions;

namespace Accounting.Application.Utils.Common.Exceptions
{
    /// <summary>
    /// Should be thrown in case we didn`t get response from remote API.
    /// </summary>
    public class RemoteApiTimeout : ExceptionBase
    {
        public RemoteApiTimeout(Uri uri)
            : base($"Call to '{uri}' has timed out.")
        {
        }
    }
}