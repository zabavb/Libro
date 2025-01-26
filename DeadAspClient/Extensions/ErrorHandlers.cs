using DeadAspClient.Models;
using Microsoft.AspNetCore.Mvc;


namespace DeadAspClient.Extensions
{
    public static class ErrorHandlers
    {
        public static ViewResult HandleErrorResponse(HttpResponseMessage response)
        {
            var errorMessage = response.StatusCode switch
            {
                System.Net.HttpStatusCode.BadRequest => "Invalid request.",
                System.Net.HttpStatusCode.NotFound => "Resource not found.",
                System.Net.HttpStatusCode.InternalServerError => "Server encountered an error.",
                _ => "An unexpected error occurred."
            };
            // Replaces 'View' used in original function
            return new ViewResult
            {
                ViewName = "Error",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    Model = new ErrorViewModel { Message = errorMessage }
                }
            };
        }

        public static ViewResult HandleException(Exception ex, string defaultMessage)
        {
            return new ViewResult
            {
                ViewName = "Error",
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                                new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                                new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())
                {
                    Model = new ErrorViewModel
                    {
                        Message = defaultMessage,
                        Details = ex.Message
                    }
                }
            };
        }
    }
}
