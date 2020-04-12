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
using API_Project;

namespace API_Project.Controllers
{
    public class EVENTOSController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

        // GET: api/EVENTOS
        public IQueryable<EVENTOS> GetEVENTOS()
        {
            return db.EVENTOS;
        }

        // GET: api/EVENTOS/5
        [ResponseType(typeof(EVENTOS))]
        public IHttpActionResult GetEVENTOS(int id)
        {
            EVENTOS eVENTOS = db.EVENTOS.Find(id);
            if (eVENTOS == null)
            {
                return NotFound();
            }

            return Ok(eVENTOS);
        }

        // PUT: api/EVENTOS/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEVENTOS(int id, EVENTOS eVENTOS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eVENTOS.id)
            {
                return BadRequest();
            }

            db.Entry(eVENTOS).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EVENTOSExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/EVENTOS
        [ResponseType(typeof(EVENTOS))]
        public IHttpActionResult PostEVENTOS(EVENTOS eVENTOS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EVENTOS.Add(eVENTOS);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = eVENTOS.id }, eVENTOS);
        }

        // DELETE: api/EVENTOS/5
        [ResponseType(typeof(EVENTOS))]
        public IHttpActionResult DeleteEVENTOS(int id)
        {
            EVENTOS eVENTOS = db.EVENTOS.Find(id);
            if (eVENTOS == null)
            {
                return NotFound();
            }

            db.EVENTOS.Remove(eVENTOS);
            db.SaveChanges();

            return Ok(eVENTOS);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EVENTOSExists(int id)
        {
            return db.EVENTOS.Count(e => e.id == id) > 0;
        }
    }
}