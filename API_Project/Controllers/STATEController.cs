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
using API_Project.Classes;

namespace API_Project.Controllers
{
    public class STATEController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetSTATE ->
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetSTATE ->
        // http://api.eevapp.es/api/STATE/action/123456789012345
        public IHttpActionResult GetSTATE([FromUri] string _imei)
        {

            db.Configuration.LazyLoadingEnabled = false;

            bool isOK = true;
            USUARIOS _entidad = new USUARIOS();
            IdNameObj action = new IdNameObj(0, "ko");

            // - - - - - getting data
            if (_imei != null)
            { 

                try { _entidad = (from e in db.USUARIOS where e.imei.Equals(_imei) select e).First(); }
                catch (Exception e) { isOK = false; }

                // - - - - - control parametro
                if (isOK && _entidad != null)
                {
                    if (_entidad.estado == 1)
                    {
                        action = new IdNameObj(1, "Ok");
                    }
                }
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - RETURN DATA ->

            return Ok(action);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - RETURN FILE //

        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetSTATE //
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetSTATE //

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool USUARIOSExists(int id)
        {
            return db.USUARIOS.Count(e => e.id == id) > 0;
        }
    }
}