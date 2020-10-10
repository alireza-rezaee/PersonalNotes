using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MoreLinq.Extensions;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Models;

namespace Rezaee.Alireza.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/logs")]
    [Authorize]
    public class LogsController : Controller
    {
        private readonly LogsDbContext _context;

        public LogsController(LogsDbContext context)
        {
            _context = context;
        }

        [Route("latest")]
        public async Task<List<RequestResponse>> Latest(int take = 100) => await _context.RequestResponse.OrderByDescending(req => req.Time).Take(take).ToListAsync();

        [Route("search")]
        public async Task<List<RequestResponse>> Search(int? statusCode, string method, bool? hasHttps, string path, string ip, DateTime? date, int? hour, int? minute, int? seconds, string queryString, long? reponseTimeMoreThan, long? reponseTimeLessThan, bool? orderByDescending, int take = 100)
        {
            var logs = _context.RequestResponse.AsQueryable();

            //Let the user customize the search
            //<Conditions>
            if (statusCode is object) logs = logs.Where(req => req.StatusCode == statusCode);

            if (!string.IsNullOrEmpty(method)) logs = logs.Where(req => req.Method.Contains(method));

            if (hasHttps is object) logs = logs.Where(req => req.HasHttps == hasHttps);

            if (!string.IsNullOrEmpty(path)) logs = logs.Where(req => req.Path.Contains(path));

            if (!string.IsNullOrEmpty(ip)) logs = logs.Where(req => req.IP.Contains(ip));

            if (!string.IsNullOrEmpty(path)) logs = logs.Where(req => req.Path.Contains(path));

            if (date is object) logs = logs.Where(req => req.Time.Date == date);

            if (hour is object) logs = logs.Where(req => req.Time.Hour == hour);

            if (minute is object) logs = logs.Where(req => req.Time.Minute == minute);

            if (seconds is object) logs = logs.Where(req => req.Time.Second == seconds);

            if (!string.IsNullOrEmpty(queryString)) logs = logs.Where(req => req.QueryString.Contains(queryString));

            if (reponseTimeMoreThan is object) logs = logs.Where(req => req.ResponseTime >= (long)reponseTimeMoreThan);

            if (reponseTimeLessThan is object) logs = logs.Where(req => req.ResponseTime <= (long)reponseTimeLessThan);

            if (orderByDescending is object) logs = (bool)orderByDescending ? logs.OrderByDescending(req => req.Time) : logs.OrderBy(req => req.Time);

            logs = logs.Take(take);
            //</Conditions>

            return await logs.ToListAsync();
        }

        [Route("details")]
        public async Task<RequestResponse> Details(string requestId)
        {
            if (string.IsNullOrEmpty(requestId))
                return null;

            return await _context.RequestResponse.Include(req => req.Details).FirstOrDefaultAsync(req => req.RequestId == requestId);
        }

        [Route("response-body-preview")]
        public async Task<IActionResult> ResponseBodyPreview(string requestId)
        {
            if (string.IsNullOrEmpty(requestId))
                return NotFound();
            var a = (await Details(requestId)).Details.ResponseBody;
            return Content(content: a, contentType: "text/html", contentEncoding: Encoding.UTF8);
        }
    }
}
