using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        public RequestResponseLoggingMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext httpContext, LogsDbContext dbContext)
        {
            await _next(httpContext);

            //Save log to chosen datastore
            try
            {
                //<Config>
                var saveLog = true;
                if (!saveLog) return;

                var saveRequestBodyFile = true;
                var saveRequestHeadersFile = true;
                var saveResponseBodyFile = true;
                var saveResponseHeadersFile = true;
                //</Config>

                var log = new Requestlogs
                {
                    //Request
                    RequestId = Activity.Current?.Id ?? httpContext.TraceIdentifier,
                    IP = httpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Referrer = httpContext.Request.Headers["Referer"],
                    Time = DateTime.Now,
                    Method = httpContext.Request.Method,
                    Protocol = httpContext.Request.Protocol,
                    Scheme = httpContext.Request.Scheme,
                    Host = httpContext.Request.Host.ToString(),
                    Path = httpContext.Request.Path,
                    QueryString = httpContext.Request.QueryString.ToString(),
                    //Response
                    StatusCode = httpContext.Response.StatusCode,
                    ResponseContentLength = httpContext.Response.ContentLength

                    //RequestHeadersFilePathPostfix = string.Empty,
                    //RequestBodyFilePathPostfix = string.Empty,
                    //ResponseHeadersFilePath = string.Empty,
                    //ResponseBodyFilePath = string.Empty
                };

                if (saveRequestHeadersFile || saveRequestBodyFile || saveResponseHeadersFile || saveResponseBodyFile)
                {
                    var persianDate = PersianDateTime.Now;
                    var absolutePath = Path.GetFullPath($"..\\Logs\\RequestResponds\\{persianDate.ToString("yyyy-MM-dd")}\\log-{DateTime.Now:HHmmssfffffff}{Guid.NewGuid().ToString().Replace("-", string.Empty)}", _env.WebRootPath);
                    Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));
                    log.FilesPath = absolutePath;

                    if (saveRequestHeadersFile)
                    {
                        var filePathPostfix = "-request-headers.json";
                        await SaveLogToFile(JsonSerializer.Serialize(RetrieveRequestHeaders(httpContext.Request)), absolutePath + filePathPostfix);
                        log.RequestHeadersFilePathPostfix = filePathPostfix;
                    }

                    if (saveRequestBodyFile)
                    {
                        if (!httpContext.Request.HasFormContentType)
                        {
                            var filePathPostfix = "-request-body.html";
                            await SaveLogToFile(await RetrieveRequestBody(httpContext.Request), absolutePath + filePathPostfix);
                            log.RequestBodyFilePathPostfix = filePathPostfix;
                        }
                        else
                        {
                            var filePathPostfix = "-request-formdata.txt";
                            await SaveLogToFile(JsonSerializer.Serialize(RetrieveRequestFormData(httpContext.Request)), absolutePath + filePathPostfix);
                            log.RequestBodyFilePathPostfix = filePathPostfix;
                        }
                    }

                    if (saveResponseHeadersFile)
                    {
                        var filePathPostfix = "-response-headers.json";
                        await SaveLogToFile(JsonSerializer.Serialize(RetrieveResponseHeaders(httpContext.Response)), absolutePath + filePathPostfix);
                        log.ResponseHeadersFilePathPostfix = filePathPostfix;
                    }

                    if (saveResponseBodyFile)
                    {
                        var filePathPostfix = "-response-body.html";
                        await SaveLogToFile(await RetrieveResponseBody(httpContext.Response), absolutePath + filePathPostfix);
                        log.ResponseBodyFilePathPostfix = filePathPostfix;
                    }
                }

                await dbContext.AddAsync(log);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            //await responseBody.CopyToAsync(originalBodyStream);
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

        private async Task SaveLogToFile(string content, string path)
        {
            try
            {
                UnicodeEncoding uniencoding = new UnicodeEncoding();
                byte[] contentBytes = uniencoding.GetBytes(content);
                using FileStream fileStream = System.IO.File.Open(path, FileMode.OpenOrCreate);
                fileStream.Seek(0, SeekOrigin.End);
                await fileStream.WriteAsync(contentBytes, 0, contentBytes.Length);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
