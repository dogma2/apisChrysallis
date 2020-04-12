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
    public class DELEGACIONESController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

        // GET: api/DELEGACIONES
        public IQueryable<DELEGACIONES> GetDELEGACIONES()
        {
            return db.DELEGACIONES;
        }

        // GET: api/DELEGACIONES/5
        [ResponseType(typeof(DELEGACIONES))]
        public IHttpActionResult GetDELEGACIONES(int id)
        {
            DELEGACIONES dELEGACIONES = db.DELEGACIONES.Find(id);
            if (dELEGACIONES == null)
            {
                return NotFound();
            }

            return Ok(dELEGACIONES);
        }

        // PUT: api/DELEGACIONES/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDELEGACIONES(int id, DELEGACIONES dELEGACIONES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dELEGACIONES.id)
            {
                return BadRequest();
            }

            db.Entry(dELEGACIONES).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DELEGACIONESExists(id))
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

        // POST: api/DELEGACIONES
        [ResponseType(typeof(DELEGACIONES))]
        public IHttpActionResult PostDELEGACIONES(DELEGACIONES dELEGACIONES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DELEGACIONES.Add(dELEGACIONES);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dELEGACIONES.id }, dELEGACIONES);
        }

        // DELETE: api/DELEGACIONES/5
        [ResponseType(typeof(DELEGACIONES))]
        public IHttpActionResult DeleteDELEGACIONES(int id)
        {
            DELEGACIONES dELEGACIONES = db.DELEGACIONES.Find(id);
            if (dELEGACIONES == null)
            {
                return NotFound();
            }

            db.DELEGACIONES.Remove(dELEGACIONES);
            db.SaveChanges();

            return Ok(dELEGACIONES);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DELEGACIONESExists(int id)
        {
            return db.DELEGACIONES.Count(e => e.id == id) > 0;
        }
    }
}