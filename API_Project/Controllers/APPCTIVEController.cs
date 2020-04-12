using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using API_Project;

/*
  
GET: http://api.eevapp.es/api/APPCTIVE/1
GET: http://api.eevapp.es/api/api/APPCTIVE/1
GET: http://localhost:64985/api/APPCTIVE/mgoncevatt.cep@gmail.com
GET: http://localhost:64985/api/APPCTIVE/1
POST: http://localhost:64985/api/APPCTIVE/ {"descripcion":"Alternativo"}
PUT: http://localhost:64985/api/APPCTIVE/1029 {"id_tema":1029, "descripcion":"Alternativo more"}
DEL http://localhost:64985/api/APPCTIVE/1029

         // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GET: api/ACTORES/5
        [HttpGet]   // define a que solicitud responde, otra opcion es anteponer "Get/Post/Put/Delete/etc" al nombre del metodo
        [Route("api/ACTORES/nombre/{nombre}")] // define la ruta y parametros de llamada API
        public IHttpActionResult ActoresByNombre(string nombre)
        {
            IHttpActionResult retu;
            db.Configuration.LazyLoadingEnabled = false; // no retorna entidades asociadas por PK
            List<ACTORES> _actores = (from a in db.ACTORES
                                  where a.nombre.Contains(nombre)
                                  select a).ToList();
            if (_actores == null) { retu = NotFound(); } else { retu = Ok(_actores); }
            return retu;
        }
     
     */

namespace API_Project.Controllers
{
    public class APPCTIVEController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

/*
        // GET: api/CCAA/5
        [ResponseType(typeof(USUARIOS))]
        public IHttpActionResult GetCCAA(int id)
        {
            USUARIOS usuario = db.USUARIOS.Find(id);
            if (usuario == null) { return NotFound(); }
            return Ok(usuario);
        }
*/



                [ResponseType(typeof(USUARIOS))]
                public IHttpActionResult GetUSUARIOS(int id)
                {
                    //            if (_email != null) { List<USUARIOS> _entidades = (from e in db.USUARIOS where e.email.Equals(_email) select e ).ToList();

                    USUARIOS uSUARIOS = db.USUARIOS.Find(id);
                    if (uSUARIOS == null) { return NotFound(); }
                    else
                    {
                        if (uSUARIOS.estado != 0) { emailActivationAppCode(uSUARIOS.email, uSUARIOS.idsocio); }
                    }

                    //if (_entidades.Count > 0) 
                    //    {
                    //        if (_entidades[0].estado != 0) { emailActivationAppCode(_entidades[0].email, _entidades[0].idsocio); }
                    //    }
                    //            }
                                return StatusCode(HttpStatusCode.NoContent); 


                }


        // PUT: api/APPCTIVE/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUSUARIOS(int id, USUARIOS uSUARIOS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uSUARIOS.id)
            {
                return BadRequest();
            }

            db.Entry(uSUARIOS).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!USUARIOSExists(id))
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

        // POST: api/APPCTIVE
        [ResponseType(typeof(USUARIOS))]
        public IHttpActionResult PostUSUARIOS(USUARIOS uSUARIOS)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.USUARIOS.Add(uSUARIOS);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (USUARIOSExists(uSUARIOS.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = uSUARIOS.id }, uSUARIOS);
        }

        // DELETE: api/APPCTIVE/5
        [ResponseType(typeof(USUARIOS))]
        public IHttpActionResult DeleteUSUARIOS(int id)
        {
            USUARIOS uSUARIOS = db.USUARIOS.Find(id);
            if (uSUARIOS == null)
            {
                return NotFound();
            }

            db.USUARIOS.Remove(uSUARIOS);
            db.SaveChanges();

            return Ok(uSUARIOS);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }

        private bool USUARIOSExists(int id)
        {
            return db.USUARIOS.Count(e => e.id == id) > 0;
        }





        /*

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GET: api/TEMAS/5
        [HttpGet]   // define a que solicitud responde, otra opcion es anteponer "Get/Post/Put/Delete/etc" al nombre del metodo
        [Route("api/TEMAS/descripcion/{descripcion}")] // define la ruta y parametros de llamada API
        public IHttpActionResult TemasByDescripcion(string descripcion)
        {
            IHttpActionResult retu;
            db.Configuration.LazyLoadingEnabled = false; // no retorna entidades asociadas por PK
            List<TEMAS> _temas = (from t in db.TEMAS
                                  where t.descripcion.Contains(descripcion)
                                  select t).ToList();
            if (_temas == null) { retu = NotFound(); } else { retu = Ok(_temas); }
            return retu;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GET: api/TEMAS
        public IQueryable<TEMAS> GetTEMAS()
        {
            db.Configuration.LazyLoadingEnabled = false; // no retorna entidades asociadas por PK
            return db.TEMAS;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GET: api/TEMAS/5
        [ResponseType(typeof(TEMAS))]
        public IHttpActionResult GetTEMAS(int id)
        {
            //TEMAS tEMAS = db.TEMAS.Find(id);  // busca por PK
            //if (tEMAS == null) { return NotFound(); }
            //return Ok(tEMAS);
            IHttpActionResult retu;
            db.Configuration.LazyLoadingEnabled = false; // no retorna entidades asociadas por PK
            TEMAS _tema = (from t in db.TEMAS.Include("PELICULAS")
                           where t.id_tema == id
                           select t).FirstOrDefault();
            if (_tema == null) { retu = NotFound(); } else { retu = Ok(_tema); }
            return retu;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - PUT: api/TEMAS/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTEMAS(int id, TEMAS tEMAS)
        {
            IHttpActionResult retu = StatusCode(HttpStatusCode.NoContent);
            if (!ModelState.IsValid) { retu = BadRequest(ModelState); }
            else if (id != tEMAS.id_tema) { retu = BadRequest(); }
            else
            {
                db.Entry(tEMAS).State = EntityState.Modified;
                try { db.SaveChanges(); }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TEMASExists(id)) { retu = NotFound(); }
                    else
                    {
                        SqlException sqlEx = (SqlException)ex.InnerException.InnerException;
                        retu = BadRequest(Utilidad.MensajeError(sqlEx));
                    }
                }
                catch (DbUpdateException ex)
                {
                    SqlException sqlEx = (SqlException)ex.InnerException.InnerException;
                    retu = BadRequest(Utilidad.MensajeError(sqlEx));
                }
            }
            return retu;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - POST: api/TEMAS
        [ResponseType(typeof(TEMAS))]
        public IHttpActionResult PostTEMAS(TEMAS tEMAS)
        {
            IHttpActionResult retu = CreatedAtRoute("DefaultApi", new { id = tEMAS.id_tema }, tEMAS);
            if (!ModelState.IsValid) { retu = BadRequest(ModelState); }
            else
            {
                db.TEMAS.Add(tEMAS);
                try { db.SaveChanges(); }
                catch (DbUpdateException ex)
                {
                    SqlException sqlEx = (SqlException)ex.InnerException.InnerException;
                    retu = BadRequest(Utilidad.MensajeError(sqlEx));
                }
            }
            return retu;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - DELETE: api/TEMAS/5
        [ResponseType(typeof(TEMAS))]
        public IHttpActionResult DeleteTEMAS(int id)
        {
            IHttpActionResult retu;
            TEMAS tEMAS = db.TEMAS.Find(id);
            if (tEMAS == null) { retu = NotFound(); }
            else
            {
                retu = Ok(tEMAS);
                db.TEMAS.Remove(tEMAS);
                try { db.SaveChanges(); }
                catch (DbUpdateException ex)
                {
                    SqlException sqlEx = (SqlException)ex.InnerException.InnerException;
                    retu = BadRequest(Utilidad.MensajeError(sqlEx));
                }
            }
            return retu;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }

        private bool TEMASExists(int id)
        {
            return db.TEMAS.Count(e => e.id_tema == id) > 0;
        }

    */










        //     Do not forget the using System.Net.Mail;
        private void emailActivationAppCode(string emailtarget, string nametarget)
        {
            MailAddress from = new MailAddress("eevapp@eevapp.com", "Chrysallis.org");
            MailAddress to = new MailAddress(emailtarget, nametarget);
            String subject = "Código de activacion Aplicación Chrysallis";
            String body = "Para activar la aplicación debes introducir el siguiente código:\n" + "03091969" + "\n";
            SendEmail( subject, body, from, to);
        }

        protected void SendEmail(string _subject, string _body, MailAddress _from, MailAddress _to)
        {
            try
            {
                // servidor
                SmtpClient clienteSMTP = new SmtpClient("eevapp.es", 465);
                clienteSMTP.UseDefaultCredentials = true;
                clienteSMTP.EnableSsl = true;
                // mensaje
                MailMessage mensaje = new MailMessage();
                mensaje.From = _from;
                mensaje.To.Add(_to);
                mensaje.Subject = _subject;
                mensaje.Body = _body;
                mensaje.IsBodyHtml = true;
                // send
                clienteSMTP.Send(mensaje);
                mensaje.Dispose();
            }
            catch (Exception ep) { Console.WriteLine("failed to send email with the following error:"); Console.WriteLine(ep.Message); }
        }
    }
}