using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Http;
using OC.Core.Crypto;

namespace API_Project.Controllers
{
    public class APPCTIVEController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();
        /*
                public string GetAPPCTIVE()
                {
                    string retu = "";
                    // - - - - TEST de grabacion de log
                    string filename = "TEST";
                    retu += RegisterActionInLog("PRUEBA", filename);
                    retu += ReadingActionInLog(filename);

                    return retu;
                }
        */

        //[HttpGet]   // define a que solicitud responde, otra opcion es anteponer "Get/Post/Put/Delete/etc" al nombre del metodo
        //[Route("api/APPCTIVE/active/{email_imei}")] // define la ruta y parametros de llamada API
        //[ResponseType(string)]
        public string GetAPPCTIVE()
        {
            string email_imei = "grupo@dogma2.es|123456789012345";
            List<USUARIOS> _entidades = new List<USUARIOS>();

            bool isOK = true;
            RegisterActionInLog("GetAPPCTIVE", email_imei);

            string actionResult = "";

            // - - - - - getting data
            string _email = "";
            string _imei = "";
            if (email_imei != null) {
                string[] _data = email_imei.Split('|');
                if (_data.Count() > 1) {
                    _email = _data[0];
                    _imei = _data[1];
                }
            } else { isOK = false; actionResult += " | email_imei = null"; }

            // - - - - - control parametro
            if (isOK && _email != null)
            {
                _entidades = (from e in db.USUARIOS where e.email.Equals(_email) select e).ToList();

                // - - - - - control usuario existente
                if (_entidades[0] != null)
                {

                    // - - - - - control usuario activo
                    if (_entidades[0].estado != 0)
                    {
                        // - - - - - datos agrabar por activacion en socio
                        _entidades[0].imei = _imei;
                        _entidades[0].cidapp = getHashString(_imei);
                        _entidades[0].fechaestado = (long)DateTime.Now.Ticks;
                        _entidades[0].notaestado = "Activacion de app";

                        // - - - - - envía email
                        isOK = emailActivationAppCode(_entidades[0].email, _entidades[0].idsocio, _entidades[0].cidapp);

                        if (isOK)
                        {
                            // - - - - - graba activacion en socio
                            db.Entry(_entidades[0]).State = EntityState.Modified;
                            try { db.SaveChanges(); }
                            catch (DbUpdateConcurrencyException) { if (!USUARIOSExists(_entidades[0].id)) { isOK = false; actionResult += " | not USUARIOSExists"; } else { throw; } }
                        }
                        else { isOK = false; actionResult += " | emailing error"; }

                    } else { isOK = false; actionResult += " | _entidades[0].estado = 0"; }

                } else { isOK = false; actionResult += " | _entidades[0] = null"; }

            } else { isOK = false; actionResult += " | isOK = false o _email = null"; }

            RegisterActionInLog("GetAPPCTIVE", actionResult);

            if (isOK) { return _entidades[0].email; }
            else { return "Error: " + actionResult;  }

        }



        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - WORK LOG ->
        // - - - - - writting log
        public string RegisterActionInLog(string whoami, string whatsapp)
        {
            string retu = "";
            string filepath = "C:\\inetpub\\vhosts\\eevapp.es\\httpdocs\\logs\\";
            try
            {
                StreamWriter logfile = File.AppendText(filepath + whoami + ".log");
                WriteLog(whatsapp, logfile);
                logfile.Close();
            }
            catch(Exception e) { retu += e.ToString(); }
            return retu;
        }
        // - - - - - making content
        public void WriteLog(string logMessage, TextWriter w)
        {
            DateTime times = DateTime.Now;
            w.Write("- " + times.Year+"."+times.Month + "." +times.Day + "_" + times.Hour+":"+times.Minute+" -> "+logMessage + '\r' + '\n');
        }
        // - - - - - reading log
        public string ReadingActionInLog(string whoami)
        {
            string retu = "";
            string filepath = "C:\\inetpub\\vhosts\\eevapp.es\\httpdocs\\logs\\";
            try
            {
                StreamReader r = File.OpenText(filepath + whoami + ".log");
                retu = DumpLog(r);
                r.Close();
            }
            catch (Exception e) { retu += e.ToString(); }
            return whoami+": "+retu;
        }
        // - - - - - taking content
        public string DumpLog(StreamReader r)
        {
            string line;
            string content = "";
            while ((line = r.ReadLine()) != null) { content += line + '\r' + '\n'; }
            return content;
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - WORK LOG //




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

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

        protected override void Dispose(bool disposing)
        {
            if (disposing) { db.Dispose(); }
            base.Dispose(disposing);
        }

        private bool USUARIOSExists(int id)
        {
            return db.USUARIOS.Count(e => e.id == id) > 0;
        }










        // - - - - - retorna la clave en hash512
        public static String getHashString(String pass)
        {
            Hash encrypt = new Hash();
            return (String)encrypt.Sha512(pass);
        }

        protected bool emailActivationAppCode(string emailtarget, string nametarget, string imeicode)
        {
            bool isOK = true;
            Random random = new Random();
            int ops = random.Next(0, 120);
            string thecode = imeicode.Substring(ops, 6);

            try
            {
                // - - - - - servidor
                SmtpClient clienteSMTP = new SmtpClient();
                clienteSMTP.UseDefaultCredentials = false;
                clienteSMTP.Host = "81.169.196.120";
                clienteSMTP.Port = 25;
                clienteSMTP.Credentials = new System.Net.NetworkCredential("chrysallis@eevapp.es", "CeP$2020D!M2T");
                // - - - - - mensaje
                MailMessage mensaje = new MailMessage();
                mensaje.From = new MailAddress("chrysallis@eevapp.es", "Chrysallis.org");
                // email chrysallis@eevapp.es | CeP$2020D!M2T
                mensaje.To.Add( new MailAddress(emailtarget, nametarget) );
                mensaje.Subject = "Código de activacion Aplicación Chrysallis";
                mensaje.Body = "Para activar la aplicación debes introducir el siguiente código:"+'\n' + thecode + '\n';
                mensaje.IsBodyHtml = true;
                // - - - - - send
                clienteSMTP.Send(mensaje);
                mensaje.Dispose();
            }
            catch (Exception ep)
            {
                isOK = false;
                RegisterActionInLog("GetAPPCTIVE", "failed to send email with the following error:" + ep.ToString() );
            }
            return isOK;
        }
    }
}