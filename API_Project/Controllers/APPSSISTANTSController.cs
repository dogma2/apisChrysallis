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
    public class APPSSISTANTSController : ApiController
    {
        private EEVAPPEntities db = new EEVAPPEntities();

        // GET: api/APPSSISTANTS
        public IQueryable<ASISTENTES> GetASISTENTES()
        {
            return db.ASISTENTES;
        }

        // GET: api/APPSSISTANTS/5
        [ResponseType(typeof(ASISTENTES))]
        public IHttpActionResult GetASISTENTES(int id)
        {
            ASISTENTES aSISTENTES = db.ASISTENTES.Find(id);
            if (aSISTENTES == null)
            {
                return NotFound();
            }

            return Ok(aSISTENTES);
        }

        // PUT: api/APPSSISTANTS/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutASISTENTES(int id, ASISTENTES aSISTENTES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aSISTENTES.ideevento)
            {
                return BadRequest();
            }

            db.Entry(aSISTENTES).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ASISTENTESExists(id))
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

        // POST: api/APPSSISTANTS
        [ResponseType(typeof(ASISTENTES))]
        public IHttpActionResult PostASISTENTES(ASISTENTES aSISTENTES)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ASISTENTES.Add(aSISTENTES);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ASISTENTESExists(aSISTENTES.ideevento))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aSISTENTES.ideevento }, aSISTENTES);
        }

        // DELETE: api/APPSSISTANTS/5
        [ResponseType(typeof(ASISTENTES))]
        public IHttpActionResult DeleteASISTENTES(int id)
        {
            ASISTENTES aSISTENTES = db.ASISTENTES.Find(id);
            if (aSISTENTES == null)
            {
                return NotFound();
            }

            db.ASISTENTES.Remove(aSISTENTES);
            db.SaveChanges();

            return Ok(aSISTENTES);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ASISTENTESExists(int id)
        {
            return db.ASISTENTES.Count(e => e.ideevento == id) > 0;
        }
    }
}