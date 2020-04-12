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
    public class DOCUMENTOSController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

        // GET: api/DOCUMENTOS
        public IQueryable<DOCUMENTOS> GetDOCUMENTOS()
        {
            return db.DOCUMENTOS;
        }

        // GET: api/DOCUMENTOS/5
        [ResponseType(typeof(DOCUMENTOS))]
        public IHttpActionResult GetDOCUMENTOS(int id)
        {
            DOCUMENTOS dOCUMENTOS = db.DOCUMENTOS.Find(id);
            if (dOCUMENTOS == null)
            {
                return NotFound();
            }

            return Ok(dOCUMENTOS);
        }

        // PUT: api/DOCUMENTOS/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDOCUMENTOS(int id, DOCUMENTOS dOCUMENTOS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dOCUMENTOS.id)
            {
                return BadRequest();
            }

            db.Entry(dOCUMENTOS).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DOCUMENTOSExists(id))
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

        // POST: api/DOCUMENTOS
        [ResponseType(typeof(DOCUMENTOS))]
        public IHttpActionResult PostDOCUMENTOS(DOCUMENTOS dOCUMENTOS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DOCUMENTOS.Add(dOCUMENTOS);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dOCUMENTOS.id }, dOCUMENTOS);
        }

        // DELETE: api/DOCUMENTOS/5
        [ResponseType(typeof(DOCUMENTOS))]
        public IHttpActionResult DeleteDOCUMENTOS(int id)
        {
            DOCUMENTOS dOCUMENTOS = db.DOCUMENTOS.Find(id);
            if (dOCUMENTOS == null)
            {
                return NotFound();
            }

            db.DOCUMENTOS.Remove(dOCUMENTOS);
            db.SaveChanges();

            return Ok(dOCUMENTOS);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DOCUMENTOSExists(int id)
        {
            return db.DOCUMENTOS.Count(e => e.id == id) > 0;
        }
    }
}