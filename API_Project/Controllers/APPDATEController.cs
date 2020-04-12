﻿using System;
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
    public class APPDATEController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

        // GET: api/APPDATE
        public IQueryable<EVENTOS> GetEVENTOS()
        {
            return db.EVENTOS;
        }

        // GET: api/APPDATE/5
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


        /* RETURN FILE

        public HttpResponseMessage GetFile(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            string fileName;
            string localFilePath;
            int fileSize;

            localFilePath = getFileFromID(id, out fileName, out fileSize);

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;
        }
        */


        /* RETURN IMAGE

        public HttpResponseMessage Get(string imageName, int width, int height)
        {
            Image img = GetImage(imageName, width, height);
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(ms.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                return result;
            }
        }

    */





        // PUT: api/APPDATE/5
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

        // POST: api/APPDATE
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

        // DELETE: api/APPDATE/5
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