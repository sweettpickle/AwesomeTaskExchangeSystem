namespace Accounting.Web.Utils.Web.Middlewares.Tracing
{
    /// <summary>
    /// Instructs tracing middleware to skip logging request`s body. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DoNotTraceRequestBodyAttribute : Attribute
    {

    }
}