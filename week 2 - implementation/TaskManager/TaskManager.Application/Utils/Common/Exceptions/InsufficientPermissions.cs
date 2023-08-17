using TaskManager.Shared.Exceptions;

namespace TaskManager.Application.Utils.Common.Exceptions
{
    public class InsufficientPermissions : ExceptionBase
    {
        public InsufficientPermissions(string principal, string resource, string permission)
            : base($"Principal '{principal}' lacks '{permission}' permission on resource '{resource}'")
        {

        }
    }
}