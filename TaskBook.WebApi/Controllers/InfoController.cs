using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using NLog.Mvc;

namespace TaskBook.WebApi.Controllers
{
    [RoutePrefix("api/Info")]
    public class InfoController : ApiController
    {
        private readonly ILogger _logger;

        public InfoController(ILogger logger)
        {
            _logger = logger;
        }

        // GET api/Info/GetAppVersion
        [Route("GetAppVersion")]
        [ResponseType(typeof(string[]))]
        public IHttpActionResult GetAppVersion()
        {
            string[] info = new string[2];
            Assembly assembly = Assembly.GetExecutingAssembly();
            info[0] = assembly.GetName().Version.ToString();
            var date = System.IO.File.GetLastWriteTime(assembly.Location).ToUniversalTime();
            info[1] = string.Format("{0: MMMM dd, yyyy HH:mm:ss} UTC", date);

            return Ok(info);
        }

    }
}
