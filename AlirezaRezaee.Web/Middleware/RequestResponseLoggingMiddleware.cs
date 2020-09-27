using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Rezaee.Alireza.Web.Helpers;
using Rezaee.Alireza.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public RequestResponseLoggingMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            //Copy a pointer to the original response body stream
            //Stream originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            //using var responseBody = new MemoryStream();
            //...and use that for the temporary response body
            //context.Response.Body = responseBody;

            //Continue down the Middleware pipeline, eventually returning to this class
            //TODO: ExceptionHandler in ~/Home/Error
            await _next(context);

            //Save log to chosen datastore
            try
            {
                var persianDate = PersianDateTime.Now;
                var requestResponse = FormatRequestAndResponse(request: context.Request, response: context.Response);
                requestResponse.Summary.RequestId = Activity.Current?.Id ?? context.TraceIdentifier;
                requestResponse.Summary.Time = persianDate.ToString("yyyy/MM/dd HH:mm:ss");

                //Create Directory (If it is not exist) & generate file names
                var logFileName = $"{requestResponse.Summary.RequestMethod}({requestResponse.Summary.RequestStatusCode})-{DateTime.Now:HHmmssfffffff}";
                var absolutePath = Path.GetFullPath($"..\\Logs\\RequestResponds\\{persianDate.ToString("yyyy-MM-dd")}\\{logFileName}", _env.WebRootPath);
                string
                    baseFileAbsolutePath = $"{absolutePath}.json",
                    requestBodyFilePath = $"{absolutePath}-reqBody.txt",
                    responseBodyFilePath = $"{absolutePath}-resBody.html";
                Directory.CreateDirectory(Path.GetDirectoryName(baseFileAbsolutePath));

                //Create Base File (Not included request/response bodies)
                using FileStream requestResponseFileStream = System.IO.File.Create(baseFileAbsolutePath);
                await JsonSerializer.SerializeAsync(requestResponseFileStream, requestResponse);

                //Create bodies
                //Create request body
                UnicodeEncoding uniencoding = new UnicodeEncoding();
                if (context.Request.HasFormContentType)
                {
                    requestBodyFilePath = $"{absolutePath}-reqFormData.json";
                    using FileStream requestBodyFileStream = System.IO.File.Create(requestBodyFilePath);
                    await JsonSerializer.SerializeAsync(requestBodyFileStream, GetFormDataFromRequest(context.Request));
                }
                else if ((context.Request.ContentLength ?? 0) != 0)
                {
                    byte[] requestBodyBytes = uniencoding.GetBytes(await FormatRequestBody(context.Request));
                    using FileStream requestBodyFileStream = System.IO.File.Open(requestBodyFilePath, FileMode.OpenOrCreate);
                    requestBodyFileStream.Seek(0, SeekOrigin.End);
                    await requestBodyFileStream.WriteAsync(requestBodyBytes, 0, requestBodyBytes.Length);
                }
                ////Create response body
                //byte[] responseBodyBytes = uniencoding.GetBytes(await FormatResponseBody(context.Response));
                //using FileStream resposeBodyFileStream = System.IO.File.Open(responseBodyFilePath, FileMode.OpenOrCreate);
                //resposeBodyFileStream.Seek(0, SeekOrigin.End);
                //await resposeBodyFileStream.WriteAsync(responseBodyBytes, 0, responseBodyBytes.Length);
            }
            catch (Exception)
            {
                //
            }

            //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            //await responseBody.CopyToAsync(originalBodyStream);
        }

        private RequestResponseLog FormatRequestAndResponse(HttpRequest request, HttpResponse response)
        {
            return new RequestResponseLog
            {
                RequestHeaders = request.Headers.Select(header => new StringCouple { Key = header.Key.ToString(), Value = header.Value.ToString() }).ToList(),
                ResponseHeaders = response.Headers.Select(header => new StringCouple { Key = header.Key.ToString(), Value = header.Value.ToString() }).ToList(),
                Summary = new SummaryLog
                {
                    RequestMethod = request.Method,
                    RequestRoute = new RouteLog { Scheme = request.Scheme, Host = request.Host.ToString(), Path = request.Path, QueryString = request.QueryString.ToString() },
                    RequestStatusCode = response.StatusCode,
                    RequestProtocol = request.Protocol,
                    ResponseContentLength = response.ContentLength,
                    RequestReferrer = request.Headers["Referer"],
                    RequestIpAddress = request.HttpContext.Connection.RemoteIpAddress.ToString()
                }
            };
        }

        private async Task<string> FormatRequestBody(HttpRequest request)
        {
            var body = request.Body;

            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body = body;

            return bodyAsText;
        }

        public static RequestFormLog GetFormDataFromRequest(HttpRequest request)
        {
            RequestFormLog formModel = null;

            if (request.Form.Any())
            {
                formModel = new RequestFormLog { FormField = new List<StringCouple>() };
                foreach (var formField in request.Form)
                    formModel.FormField.Add(new StringCouple { Key = formField.Key, Value = formField.Value });

                if (request.Form.Files.Any())
                {
                    formModel.FormFile = new List<FileDetails>();
                    foreach (var file in request.Form.Files)
                        formModel.FormFile.Add(new FileDetails
                        {
                            Name = file.FileName,
                            ContentType = file.ContentType,
                            Length = file.Length
                        });
                }
            }

            return formModel;
        }

        private async Task<string> FormatResponseBody(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return text;
        }
    }
}
