
using Accounting.Shared.Exceptions;

namespace Accounting.Application.Utils.Common.Exceptions
{
    /// <summary>
    /// Indicates error occured while calling remote api.
    /// </summary>
    public class RemoteApiCallFailed : ExceptionBase
    {
        public Uri Uri { get; }
        public RemoteApiCallFailed(Uri uri) : base($"Failed to call {uri}.")
        {
            Uri = uri;
        }

        public RemoteApiCallFailed(Uri uri, Exception inner) : base($"Failed to call {uri}.", inner)
        {
            Uri = uri;
        }
    }
}