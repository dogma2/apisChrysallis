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
    public class CCAAController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

        // GET: api/CCAA
        public IQueryable<CCAA> GetCCAA()
        {
            return db.CCAA;
        }

        // GET: api/CCAA/5
        [ResponseType(typeof(CCAA))]
        public IHttpActionResult GetCCAA(byte id)
        {
            CCAA cCAA = db.CCAA.Find(id);
            if (cCAA == null)
            {
                return NotFound();
            }

            return Ok(cCAA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CCAAExists(byte id)
        {
            return db.CCAA.Count(e => e.id == id) > 0;
        }
    }
}