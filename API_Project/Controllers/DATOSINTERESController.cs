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
    public class DATOSINTERESController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

        // GET: api/DATOSINTERES
        public IQueryable<DATOSINTERES> GetDATOSINTERES()
        {
            return db.DATOSINTERES;
        }

        // GET: api/DATOSINTERES/5
        [ResponseType(typeof(DATOSINTERES))]
        public IHttpActionResult GetDATOSINTERES(int id)
        {
            DATOSINTERES dATOSINTERES = db.DATOSINTERES.Find(id);
            if (dATOSINTERES == null)
            {
                return NotFound();
            }

            return Ok(dATOSINTERES);
        }

        // PUT: api/DATOSINTERES/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDATOSINTERES(int id, DATOSINTERES dATOSINTERES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dATOSINTERES.id)
            {
                return BadRequest();
            }

            db.Entry(dATOSINTERES).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DATOSINTERESExists(id))
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

        // POST: api/DATOSINTERES
        [ResponseType(typeof(DATOSINTERES))]
        public IHttpActionResult PostDATOSINTERES(DATOSINTERES dATOSINTERES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DATOSINTERES.Add(dATOSINTERES);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dATOSINTERES.id }, dATOSINTERES);
        }

        // DELETE: api/DATOSINTERES/5
        [ResponseType(typeof(DATOSINTERES))]
        public IHttpActionResult DeleteDATOSINTERES(int id)
        {
            DATOSINTERES dATOSINTERES = db.DATOSINTERES.Find(id);
            if (dATOSINTERES == null)
            {
                return NotFound();
            }

            db.DATOSINTERES.Remove(dATOSINTERES);
            db.SaveChanges();

            return Ok(dATOSINTERES);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DATOSINTERESExists(int id)
        {
            return db.DATOSINTERES.Count(e => e.id == id) > 0;
        }
    }
}