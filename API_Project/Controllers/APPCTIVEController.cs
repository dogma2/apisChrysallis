using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Web.Http;
using API_Project.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OC.Core.Crypto;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace API_Project.Controllers
{

    public class PairIdName
    {
        private int id;
        private string nombre;

        public PairIdName(int id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }

        public int m_id { get; set; }
        public string m_name { get; set; }
    }

    public class APPCTIVEController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - PostAPPCTIVE ->
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - PostAPPCTIVE ->
        // http://api.eevapp.es/api/APPCTIVE/active/mgoncevatt.cep@gmail.com/123456789012345
        public HttpResponseMessage GetAPPCTIVE([FromUri] string _email, string _imei )
        {

            db.Configuration.LazyLoadingEnabled = false;

            RegisterActionInLog("GetAPPCTIVE", "49 - " + _email + " | " + _imei);

            bool isOK = true;
            string thecode = "";
            string actionResult = "";
            string filezip = "";
            USUARIOS _entidad = new USUARIOS();

            // - - - - - getting data
            if (_email == null || _imei == null) { isOK = false; actionResult += " | email_imei = null"; }


            // - - - - - control parametro
            if (isOK && _email != null)
            {

                try { _entidad = (from e in db.USUARIOS where e.email.Equals(_email) select e).First(); }
                catch (Exception e) { isOK = false; }

                // - - - - - control parametro
                if (isOK && _entidad != null)
                    {

                    // - - - - - control usuario activo
                    if (_entidad.estado != 0)
                    {
                        // - - - - - datos agrabar por activacion en socio
                        _entidad.imei = _imei;
                        _entidad.cidapp = getHashString(_imei);
                        _entidad.fechaestado = (long)DateTime.Now.Ticks;
                        _entidad.notaestado = "Activacion de app";

                        Random random = new Random();
                        int ops = random.Next(0, 120);
                        thecode = _entidad.cidapp.Substring(ops, 6);

                        // - - - - - envía email
                        isOK = emailActivationAppCode(_entidad.email, _entidad.idsocio, thecode);

                        if (isOK)
                        {
                            // - - - - - graba activacion en socio
                            db.Entry(_entidad).State = EntityState.Modified;
                            try { db.SaveChanges(); }
                            catch (DbUpdateConcurrencyException) { if (!USUARIOSExists(_entidad.id)) { isOK = false; actionResult += " | not USUARIOSExists"; } else { throw; } }
                        }
                        else { isOK = false; actionResult += " | emailing error"; }

                    }
                    else { isOK = false; actionResult += " | _entidad[0].estado = 0"; }

                }
                else { isOK = false; actionResult += " | _entidad[0] = null"; }

            }
            else { isOK = false; actionResult += " | isOK = false o _email = null"; }

            RegisterActionInLog("GetAPPCTIVE", "167 - " + actionResult);

            if (isOK)
            {

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera directorios ->

                // - - - - - Estructura de carpetas
                string basePath = "C:\\inetpub\\vhosts\\eevapp.es\\httpdocs\\dataXport_EEvApp\\";
                string zipFiles = basePath + "zipFiles\\";
                string activatePath = basePath + "ActivateWork\\";
                string dirMaster = activatePath + "EEvaApp\\"; // - - - - - dirMaster
                string dirConfig = dirMaster + "Config\\"; // - - - - - dirConfig

                // - - - - - prepara carpeta export
                DirectoryInfo workdir;

                // zipFiles
                if (!Directory.Exists(zipFiles)) { try { Directory.CreateDirectory(zipFiles); } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "create zipFiles - Error !"); } }
                // dirMaster
                if (!Directory.Exists(dirMaster)) { try { Directory.CreateDirectory(dirMaster); } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "create dirMaster - Error !"); } }
                // dirConfig
                if (!Directory.Exists(dirConfig)) { try { Directory.CreateDirectory(dirConfig); } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "create dirConfig - Error !"); } }
                else { workdir = new DirectoryInfo(dirConfig); foreach (FileInfo file in workdir.GetFiles()) { file.Delete(); } }

                RegisterActionInLog("GetAPPCTIVE", "133 - CREA DIR");

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera directorios //

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera JSons y almacena ->
                StreamWriter fichero;
                JsonTextWriter jsonwriter;
                List<PairIdName> pairs = new List<PairIdName>();

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - APPDATA ->
                try
                {
                    // - - - - - directory EEvaApp/Config: configuration data
                    UserDataes _appdata = new UserDataes();
                    _appdata.m_cidapp = _entidad.cidapp;
                    _appdata.m_code = thecode;
                    _appdata.m_state = 0;
                    _appdata.m_date = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    _appdata.m_updated = _appdata.m_date;
                    _appdata.m_result = 0;
                    _appdata.m_next = _appdata.m_date;
                    // - - - - - graba json en carpeta                
                    var json_data01 = JToken.FromObject(_appdata);
                    fichero = File.CreateText(dirConfig + "appData" + ".json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_data01.WriteTo(jsonwriter);
                    jsonwriter.Close();
                }
                catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "UserDataes - Error !"); }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - APPDATA //

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - CONFIG ->
                try
                {
                    if (File.Exists(basePath + "config.json"))
                    {
                        if (File.Exists(dirConfig + "config.json")) { File.Delete(dirConfig + "config.json"); }
                        File.Copy(basePath + "config.json", dirConfig + "config.json");
                    }
                }
                catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "config - Error !" + e.ToString()); }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - CONFIG //

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - DATAS ->
                try
                {
                    if (File.Exists(basePath + "datas.json"))
                    {
                        if (File.Exists(dirConfig + "datas.json")) { File.Delete(dirConfig + "datas.json"); }
                        File.Copy(basePath + "datas.json", dirConfig + "datas.json");
                    }
                }
                catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "datas - Error !" + e.ToString()); }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - DATAS //

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera JSons y almacena //

                RegisterActionInLog("GetAPPCTIVE", "294 - CREA ZIP");

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera ZIP ->
                filezip = zipFiles + _entidad.cidapp + ".zip";
                if (File.Exists(filezip)) { File.Delete(filezip); }

                ZipFile.CreateFromDirectory(activatePath, filezip);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera ZIP ->
            }

            if (!isOK)
            {
                RegisterActionInLog("GetAPPCTIVE", "309 - SALE ERROR");

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - RETURN FILE ->

                FileInfo infozip = new FileInfo(filezip);
                // localFilePath = getFileFromID(id, out infozip.Name, out infozip.Length);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new FileStream(filezip, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = infozip.Name;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");

                RegisterActionInLog("GetAPPCTIVE", "324 - RETORNA ARCHIVO");

                return response;

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - RETURN FILE //
            }

        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetAPPCTIVE //
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetAPPCTIVE //

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - retorna string en hash512 ->
        public static String getHashString(String pass)
        {
            Hash encrypt = new Hash();
            return (String)encrypt.Sha512(pass);
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - retorna string en hash512 //

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
            catch (Exception e) { retu += e.ToString(); }
            return retu;
        }
        // - - - - - making content
        public void WriteLog(string logMessage, TextWriter w)
        {
            DateTime times = DateTime.Now;
            w.Write("- " + times.Year + "." + times.Month + "." + times.Day + "_" + times.Hour + ":" + times.Minute + " -> " + logMessage + '\r' + '\n');
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
            return whoami + ": " + retu;
        }
        // - - - - - taking content
        public string DumpLog(StreamReader r)
        {
            string line;
            string content = "";
            if (r != null) {  while ((line = r.ReadLine()) != null) { content += line + '\r' + '\n'; } }
            return content;
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - WORK LOG //

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - EMAILING ->


        // - - - - - send email
        protected bool emailActivationAppCode(string emailtarget, string nametarget, string thecode)
        {
            bool isOK = true;
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
                mensaje.To.Add(new MailAddress(emailtarget, nametarget));
                mensaje.Subject = "Código de activacion Aplicación Chrysallis";
                mensaje.Body = "Para activar la aplicación debes introducir el siguiente código:" + '\n' + thecode + '\n';
                mensaje.IsBodyHtml = true;
                // - - - - - send
                clienteSMTP.Send(mensaje);
                mensaje.Dispose();
            }
            catch (Exception ep)
            {
                isOK = false;
                RegisterActionInLog("GetAPPCTIVE", "failed to send email with the following error:" + ep.ToString());
            }
            return isOK;
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - EMAILING //

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

    }
}


