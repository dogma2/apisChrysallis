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
    public class PROVINCIASController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

        // GET: api/PROVINCIAS
        public IQueryable<PROVINCIAS> GetPROVINCIAS()
        {
            return db.PROVINCIAS;
        }

        // GET: api/PROVINCIAS/5
        [ResponseType(typeof(PROVINCIAS))]
        public IHttpActionResult GetPROVINCIAS(byte id)
        {
            PROVINCIAS pROVINCIAS = db.PROVINCIAS.Find(id);
            if (pROVINCIAS == null)
            {
                return NotFound();
            }

            return Ok(pROVINCIAS);
        }

        // PUT: api/PROVINCIAS/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPROVINCIAS(byte id, PROVINCIAS pROVINCIAS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pROVINCIAS.id)
            {
                return BadRequest();
            }

            db.Entry(pROVINCIAS).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PROVINCIASExists(id))
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

        // POST: api/PROVINCIAS
        [ResponseType(typeof(PROVINCIAS))]
        public IHttpActionResult PostPROVINCIAS(PROVINCIAS pROVINCIAS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PROVINCIAS.Add(pROVINCIAS);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PROVINCIASExists(pROVINCIAS.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = pROVINCIAS.id }, pROVINCIAS);
        }

        // DELETE: api/PROVINCIAS/5
        [ResponseType(typeof(PROVINCIAS))]
        public IHttpActionResult DeletePROVINCIAS(byte id)
        {
            PROVINCIAS pROVINCIAS = db.PROVINCIAS.Find(id);
            if (pROVINCIAS == null)
            {
                return NotFound();
            }

            db.PROVINCIAS.Remove(pROVINCIAS);
            db.SaveChanges();

            return Ok(pROVINCIAS);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PROVINCIASExists(byte id)
        {
            return db.PROVINCIAS.Count(e => e.id == id) > 0;
        }
    }
}