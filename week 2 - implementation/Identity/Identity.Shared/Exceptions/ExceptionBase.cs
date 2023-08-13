using System.Runtime.Serialization;

namespace Identity.Shared.Exceptions
{
    /// <summary>
    /// A base class for any exception thrown in current solution
    /// </summary>
    public abstract class ExceptionBase : ApplicationException
    {
        protected ExceptionBase() : base()
        {
        }

        protected ExceptionBase(string message) : base(message)
        {
        }

        protected ExceptionBase(string message, Exception inner) : base(message, inner)
        {
        }

        protected ExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}