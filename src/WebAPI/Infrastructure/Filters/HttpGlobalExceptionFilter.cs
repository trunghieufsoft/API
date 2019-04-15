using Serilog;
using System.Net;
using Common.Core.Models;
using Common.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            System.Exception exception = context.Exception;
            ResponseModel responseModel = new ResponseModel();
            ErrorModel errorModel = new ErrorModel();
            int code = (int)HttpStatusCode.BadRequest;
            switch (exception)
            {
                case NotFound _:
                    code = (int)HttpStatusCode.NotFound;
                    errorModel.ErrorCode = (int)HttpStatusCode.NotFound;
                    errorModel.Message = "The requested resource is not found. Please try another one.";
                    Log.Error("DataNotFound exception has occurred. Details: {Exception}", exception.Message);
                    break;

                case DefinedException userError:
                    errorModel = userError.Error;
                    code = (int)HttpStatusCode.OK;
                    Log.Error("Authentication exception has occurred. Details: {Exception}", exception.Message);
                    break;

                case BadData _:
                    errorModel.ErrorCode = (int)HttpStatusCode.BadRequest;
                    errorModel.Message = exception.Message;
                    Log.Error("Validation exception has occurred. Details: {Exception}", exception.Message);
                    break;

                case Dupplication _:
                    code = (int)HttpStatusCode.Conflict;
                    errorModel.ErrorCode = (int)HttpStatusCode.Conflict;
                    errorModel.Message = "Record Existed.";
                    Log.Error("Dupplication exception has occurred. Details: {Exception}", exception.Message);
                    break;

                case SqlException _:
                case System.Data.SqlClient.SqlException _:
                    errorModel.ErrorCode = (int)HttpStatusCode.BadRequest;
                    errorModel.Message = "Data exception has occurred.";
                    Log.Error("SQL exception has occurred. Details: {Exception}", exception.Message);
                    break;

                default:
                    errorModel.ErrorCode = (int)HttpStatusCode.BadRequest;
                    errorModel.Message = "Internal Server Error";
                    code = (int)HttpStatusCode.InternalServerError;
                    Log.Error("Internal server error has occurred. Details: {Exception}", exception);
                    break;
            }

            responseModel.Error = errorModel;
            responseModel.Success = false;
            context.Result = new JsonResult(responseModel);

            context.HttpContext.Response.StatusCode = code;
            context.ExceptionHandled = true;
        }
    }
}