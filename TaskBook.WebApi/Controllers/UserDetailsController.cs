using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TaskBook.DataAccessReader;
using TaskBook.DataAccessReader.ViewModels;

namespace TaskBook.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Users")]
    public class UserDetailsController: ApiController
    {
        private readonly ReaderRepository _readerRepository;

        public UserDetailsController(ReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        // GET api/Users/GetUserDetailsByUserName/{userName}
        [Route("GetUserDetailsByUserName/{userName}")]
        [ResponseType(typeof(TbUserVm))]
        public IHttpActionResult GetUserDetailsByUserName(string userName)
        {
            var user = _readerRepository.GetUserDetailsByUserName(userName).FirstOrDefault();

            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            _readerRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
