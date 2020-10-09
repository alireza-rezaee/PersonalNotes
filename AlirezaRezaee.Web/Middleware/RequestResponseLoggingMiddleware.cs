using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Logging;
using MoreLinq;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Helpers;
using Rezaee.Alireza.Web.Models;
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
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            _next = next;
            _env = env;
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext, LogsDbContext dbContext)
        {
            //<Config>
            var saveLog = true;
            if (!saveLog) return;

            var saveRequestBodyFile = true;
            var saveResponseBodyFile = true;
            //</Config>

            var sw = new Stopwatch();

            try
            {
                var log = new RequestResponse
                {
                    RequestId = Activity.Current?.Id ?? httpContext.TraceIdentifier,
                    IP = httpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Time = DateTime.Now,
                    Method = httpContext.Request.Method,
                    HasHttps = (httpContext.Request.Scheme.ToLower()) switch
                    {
                        "https" => true,
                        "http" => false,
                        _ => null,
                    },
                    Path = httpContext.Request.Path,
                    QueryString = httpContext.Request.QueryString.ToString(),
                    Details = new RequestResponseDetails
                    {
                        Protocol = httpContext.Request.Protocol,
                        Host = httpContext.Request.Host.ToString(),
                        Referrer = httpContext.Request.Headers["Referer"]
                    }
                };

                if (saveResponseBodyFile)
                {
                    Stream originalBody = httpContext.Response.Body;
                    try
                    {
                        using var memStream = new MemoryStream();
                        httpContext.Response.Body = memStream;

                        try
                        {
                            sw.Start();
                            await _next(httpContext);
                            sw.Stop();
                        }
                        catch (Exception e)
                        {
                            log.Details.Exception = e.ToString();
                        }

                        memStream.Position = 0;
                        string responseBody = new StreamReader(memStream).ReadToEnd();
                        log.Details.ResponseBody = responseBody;

                        memStream.Position = 0;
                        await memStream.CopyToAsync(originalBody);
                    }
                    finally
                    {
                        httpContext.Response.Body = originalBody;
                    }
                }
                else
                {
                    try
                    {
                        sw.Start();
                        await _next(httpContext);
                        sw.Stop();
                    }
                    catch (Exception e)
                    {
                        log.Details.Exception = e.ToString();
                    }
                }

                if (saveRequestBodyFile)
                {
                    if (!httpContext.Request.HasFormContentType)
                    {
                        log.Details.RequestBody = await RetrieveRequestBody(httpContext.Request);
                    }
                    else
                    {
                        var formData = RetrieveRequestFormData(httpContext.Request);
                        StringBuilder requestFormDataBuilder = new StringBuilder(string.Concat("=".Repeat(10)));
                        requestFormDataBuilder.AppendLine("=== Form Data: ===");
                        foreach (var item in formData.FormFields)
                            requestFormDataBuilder.AppendLine($"{item.Key}: {item.Value}");
                        requestFormDataBuilder.AppendLine("=== Form Files: ===");
                        foreach (var item in formData.FormFiles)
                            requestFormDataBuilder.AppendLine($"File name: {item.Name}\nFile content-type: {item.ContentType}\nFile size: {item.Length} bytes\n");
                        log.Details.RequestBody = requestFormDataBuilder.ToString();
                    }
                }

                StringBuilder requestHeadersBuilder = new StringBuilder();
                foreach (var item in RetrieveRequestHeaders(httpContext.Request))
                    requestHeadersBuilder.AppendLine($"{item.Key}: {item.Value}");
                log.Details.RequestHeaders = requestHeadersBuilder.ToString();

                //Important: Response properties must be set after _next(httpContext)
                StringBuilder responseHeadersBuilder = new StringBuilder();
                foreach (var item in RetrieveResponseHeaders(httpContext.Response))
                    responseHeadersBuilder.AppendLine($"{item.Key}: {item.Value}");
                log.Details.ResponseHeaders = responseHeadersBuilder.ToString();

                //Important: Response properties must be set after _next(httpContext)
                log.ResponseTime = sw.ElapsedMilliseconds;
                log.StatusCode = httpContext.Response.StatusCode;
                await dbContext.AddAsync(log);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                _logger.LogInformation(
                    "Request {method} {url} => {statusCode}",
                    httpContext.Request?.Method,
                    httpContext.Request?.Path.Value,
                    httpContext.Response?.StatusCode);
            }
        }

        private List<StringCouple> RetrieveRequestHeaders(HttpRequest request) => request.Headers.Select(header => new StringCouple { Key = header.Key.ToString(), Value = header.Value.ToString() }).ToList();

        private List<StringCouple> RetrieveResponseHeaders(HttpResponse response) => response.Headers.Select(header => new StringCouple { Key = header.Key.ToString(), Value = header.Value.ToString() }).ToList();

        private async Task<string> RetrieveRequestBody(HttpRequest request)
        {
            try
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
            catch (Exception e)
            {
                throw e;
            }
        }

        private RequestFormLog RetrieveRequestFormData(HttpRequest request)
        {
            try
            {
                RequestFormLog formModel = null;

                if (request.Form.Any())
                {
                    formModel = new RequestFormLog { FormFields = new List<StringCouple>() };
                    foreach (var formField in request.Form)
                        formModel.FormFields.Add(new StringCouple { Key = formField.Key, Value = formField.Value });

                    if (request.Form.Files.Any())
                    {
                        formModel.FormFiles = new List<FileDetails>();
                        foreach (var file in request.Form.Files)
                            formModel.FormFiles.Add(new FileDetails
                            {
                                Name = file.FileName,
                                ContentType = file.ContentType,
                                Length = file.Length
                            });
                    }
                }

                return formModel;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private async Task<string> RetrieveResponseBody(HttpResponse response)
        {
            try
            {
                var original = response.Body;
                var stream = new MemoryStream();
                response.Body = stream;

                stream.Seek(0, SeekOrigin.Begin);
                var body = new StreamReader(stream).ReadToEnd();
                //var url = UriHelper.GetDisplayUrl(context.Request);
                //log.Debug("Response Url '{url}' Response Body '{responsebody}'", url, body);
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(original);

                return body;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
