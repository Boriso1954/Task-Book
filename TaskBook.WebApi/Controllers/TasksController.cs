using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TaskBook.DomainModel;
using TaskBook.DataAccessLayer;

namespace TaskBook.WebApi.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Tasks")]
    public class TasksController : ApiController
    {
        private readonly TaskBookDbContext _db;

        public TasksController(TaskBookDbContext database)
        {
            _db = database;
        }


        // GET api/Tasks
        [Route("")]
        public IQueryable<TbTask> GetTasks()
        {
            return _db.Tasks;
        }

        // GET api/Tasks/{id}
        [Route("{id:long}")]
        [ResponseType(typeof(TbTask))]
        public IHttpActionResult GetTask(long id)
        {
            TbTask tbtask = _db.Tasks.Find(id);
            if(tbtask == null)
            {
                return NotFound();
            }

            return Ok(tbtask);
        }

        //// PUT api/Tasks/5
        //public IHttpActionResult PutTbTask(long id, TbTask tbtask)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != tbtask.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(tbtask).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TbTaskExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST api/Tasks
        //[ResponseType(typeof(TbTask))]
        //public IHttpActionResult PostTbTask(TbTask tbtask)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Tasks.Add(tbtask);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = tbtask.Id }, tbtask);
        //}

        //// DELETE api/Tasks/5
        //[ResponseType(typeof(TbTask))]
        //public IHttpActionResult DeleteTbTask(long id)
        //{
        //    TbTask tbtask = db.Tasks.Find(id);
        //    if (tbtask == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Tasks.Remove(tbtask);
        //    db.SaveChanges();

        //    return Ok(tbtask);
        //}

        //private bool TbTaskExists(long id)
        //{
        //    return db.Tasks.Count(e => e.Id == id) > 0;
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}