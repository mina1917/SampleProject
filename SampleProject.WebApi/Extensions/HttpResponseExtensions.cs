using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using SampleProject.WebApi.Endpoint;
using SampleProject.Framework.Exceptions;
using SampleProject.Application.Exceptions;

namespace SampleProject.WebApi.Extensions
{
    internal static class HttpResponseExtensions
    {
        public static Task WriteValidationErrorsAsync(this HttpResponse response, ValidationException ex)
        {
            var errors = ex.Errors.Select(cur => cur.ErrorMessage)
                .Distinct()
                .ToList();

            var message = string.Concat(
                "Bad request",
                Environment.NewLine,
                string.Join(Environment.NewLine, errors));

            return response.WriteResponseAsync(HttpStatusCode.BadRequest, new ApiResult(false, message));
        }

        public static Task WriteApplicationErrorsAsync(this HttpResponse response, AppException ex)
        {
            return response.WriteResponseAsync(ex.HttpStatusCode, new ApiResult(false, ex.Message));
        }

        public static Task WriteBusinessErrorsAsync(this HttpResponse response, BusinessException ex)
        {
            return response.WriteResponseAsync(HttpStatusCode.BadRequest, new ApiResult(false, ex.Message));
        }

        public static Task WriteUnhandledExceptionsAsync(this HttpResponse response)
        {
            return response.WriteResponseAsync(HttpStatusCode.BadRequest, new ApiResult(false, "Internal server error"));
        }

        private static Task WriteResponseAsync(this HttpResponse response, HttpStatusCode statusCodes, ApiResult apiResult)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            response.StatusCode = (int)statusCodes;
            response.ContentType = "application/json";

            return response.WriteAsync(JsonConvert.SerializeObject(apiResult, settings));
        }
    }
}
