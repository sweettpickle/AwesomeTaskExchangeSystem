﻿using Identity.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;

namespace Identity.Integration.Utils.Rest
{
    /// <summary>
    /// A base class for implementing a client to any restful API
    /// </summary>
    public abstract class BaseClient
    {
        protected readonly ILogger<BaseClient> _logger;
        protected readonly RestClient _client;

        protected BaseClient(Uri baseUrl, ILogger<BaseClient> logger, IWebProxy proxy = null, IAuthenticator authenticator = null)
        {
            _logger = logger;
            _client = new RestClient(new RestClientOptions(baseUrl)
            {
                Proxy = proxy,
                //ConfigureMessageHandler = handler =>
                //    new LoggingHandler(logger, handler),
                Authenticator = authenticator
            });
        }

        /// <summary>
        /// Invokes a request and returns a result
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="RemoteApiTimeout"></exception>
        /// <exception cref="RemoteApiCallFailed"></exception>
        public virtual async Task<TResponse> Invoke<TResponse>(BaseRequest<TResponse> request, CancellationToken token = default)
        {
            try
            {
                var response = await _client.ExecuteAsync<TResponse>(request, token);

                if (request.ErrorHandler != null)
                    request.ErrorHandler.Invoke(this, response);
                else
                    CommonErrorHandler(request, response);

                return response.Data;
            }
            catch (TimeoutException)
            {
                _logger.LogError("Call to '{uri}' has timed out.", BuildUri(request));
                throw;
            }
            catch (Exception ex) when (false == ex is ExceptionBase)
            {
                _logger.LogError(ex, "Failed to call {uri}.", BuildUri(request));
                throw;
            }
        }

        public Uri BuildUri(RestRequest request)
        {
            return _client.BuildUri(request);
        }

        /// <summary>
        /// Method for handling common errors
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <remarks>Some APIs are designed in such a way that they return the same error codes for the same situations, regardless of the method that called this situation. In such cases, it is possible to organize the processing of such errors in a single place - namely, in the implementation of this method.</remarks>
        public abstract void CommonErrorHandler<TResponse>(BaseRequest<TResponse> request, RestResponse<TResponse> response);
    }
}
