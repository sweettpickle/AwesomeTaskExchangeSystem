namespace Identity.Web.Utils.Web.Middlewares.Tracing
{
    /// <summary>
    /// Instructs tracing middleware to skip logging response`s body.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DoNotTraceResponseBodyAttribute : Attribute
    {

    }
}