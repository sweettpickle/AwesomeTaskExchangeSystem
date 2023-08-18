using RestSharp;

namespace Identity.Integration.Utils.Rest
{
    public abstract class BaseRequest<TResponse> : RestRequest
    {
        protected BaseRequest(string resource, Method method) : base(resource, method)
        {
        }

        /// <summary>
        /// A method for implementing per-request error handling logic
        /// </summary>
        /// <remarks>some APIs are designed in such a way that the same error codes in the responses of different endpoints mean completely different situations. In such cases, it becomes necessary to implement a separate processing logic for each endpoint. This method is used for exactly this purpose.</remarks>
        public Action<BaseClient, RestResponse<TResponse>> ErrorHandler { get; protected set; }
    }
}
