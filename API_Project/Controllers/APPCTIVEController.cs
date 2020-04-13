using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System;
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
    public class APPCTIVEController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

        /*
        public string GetAPPCTIVE()
        {
            string email_imei = "marcelo@sys-mas.com|123456789012345";
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
            else { return "Error: " + actionResult; }

        }
        */

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - PostAPPCTIVE ->
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - PostAPPCTIVE ->
//        public HttpResponseMessage PostAPPCTIVE(string email_imei)
//        {
        public HttpResponseMessage GetAPPCTIVE()
        {
            string email_imei = "mgoncevatt.cep@gmail.com|123456789012345";


            RegisterActionInLog("GetAPPCTIVE", "101 - " + email_imei);


            bool isOK = true;
            string thecode = "";
            string actionResult = "";
            string filezip = "";
            List<USUARIOS> _entidades = new List<USUARIOS>();

            // - - - - - getting data
            string _email = "";
            string _imei = "";
            if (email_imei != null)
            {
                string[] _data = email_imei.Split('|');
                if (_data.Count() > 1)
                {
                    _email = _data[0];
                    _imei = _data[1];
                }
            }
            else { isOK = false; actionResult += " | email_imei = null"; }

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

                        Random random = new Random();
                        int ops = random.Next(0, 120);
                        thecode = _entidades[0].cidapp.Substring(ops, 6);

                        // - - - - - envía email
                        isOK = emailActivationAppCode(_entidades[0].email, _entidades[0].idsocio, thecode);

                        if (isOK)
                        {
                            // - - - - - graba activacion en socio
                            db.Entry(_entidades[0]).State = EntityState.Modified;
                            try { db.SaveChanges(); }
                            catch (DbUpdateConcurrencyException) { if (!USUARIOSExists(_entidades[0].id)) { isOK = false; actionResult += " | not USUARIOSExists"; } else { throw; } }
                        }
                        else { isOK = false; actionResult += " | emailing error"; }

                    }
                    else { isOK = false; actionResult += " | _entidades[0].estado = 0"; }

                }
                else { isOK = false; actionResult += " | _entidades[0] = null"; }

            }
            else { isOK = false; actionResult += " | isOK = false o _email = null"; }

RegisterActionInLog("GetAPPCTIVE", "167 - " + actionResult);

            if (isOK)
            {

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - Elimina y regenera directorios ->
                // - - - - - Estructura de carpetas
                string basePath = "C:\\inetpub\\vhosts\\eevapp.es\\httpdocs\\dataXport_EEvApp\\";
                string activatePath = basePath + "ActivateWork\\";
                string updatePath = basePath + "UpdateWork\\";
                string dirMaster = activatePath + "EEvaApp\\"; // - - - - - dirMaster
                string dirConfig = dirMaster + "Config\\"; // - - - - - dirConfig
                string dirXtras = dirMaster + "Xtras\\"; // - - - - - dirXtras
                string dirDatas = dirMaster + "Datas\\"; // - - - - - dirDatas
                string dirImages = dirDatas + "Images\\"; // - - - - - dirImages
                string dirWork = dirMaster + "Work\\"; // - - - - - dirWork
                string dirZipped = dirMaster + "Zipped\\"; // - - - - - dirZipped
                string dirUnzipped = dirMaster + "Unzipped\\"; // - - - - - dirUnzipped
                // - - - - - prepara carpeta export
                if (Directory.Exists(dirMaster))
                {
                    System.IO.DirectoryInfo dirinfo;
                    // elimina archivos contenidos para generar nuevos
                    dirinfo = new System.IO.DirectoryInfo(activatePath);
                    foreach (System.IO.FileInfo file in dirinfo.GetFiles()) { file.Delete(); }


RegisterActionInLog("GetAPPCTIVE", "194 - Directory.Exists(activatePath)");

                }
                else
                {
                    // crea directorio
                    try { Directory.CreateDirectory(dirMaster); } catch ( Exception e ) { RegisterActionInLog("GetAPPCTIVE", "200 - Error !"); }
                    try { Directory.CreateDirectory(dirConfig); } catch ( Exception e ) { RegisterActionInLog("GetAPPCTIVE", "201 - Error !"); }
                    try { Directory.CreateDirectory(dirXtras); } catch ( Exception e ) { RegisterActionInLog("GetAPPCTIVE", "202 - Error !"); }
                    try { Directory.CreateDirectory(dirDatas); } catch ( Exception e ) { RegisterActionInLog("GetAPPCTIVE", "203 - Error !"); }
                    try { Directory.CreateDirectory(dirImages); } catch ( Exception e ) { RegisterActionInLog("GetAPPCTIVE", "204 - Error !"); }
                    try { Directory.CreateDirectory(dirWork); } catch ( Exception e ) { RegisterActionInLog("GetAPPCTIVE", "205 - Error !"); }
                    try { Directory.CreateDirectory(dirZipped); } catch ( Exception e ) { RegisterActionInLog("GetAPPCTIVE", "206 - Error !"); }
                    try { Directory.CreateDirectory(dirUnzipped); } catch ( Exception e ) { RegisterActionInLog("GetAPPCTIVE", "207 - Error !"); }

RegisterActionInLog("GetAPPCTIVE", "209 - CREA DIR");

                }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - Elimina y regenera directorios //

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera JSons y almacena ->
                StreamWriter fichero;
                JsonTextWriter jsonwriter;

                try {
                    // - - - - - directory EEvaApp/Config: configuration data
                    UserDataes _appdata = new UserDataes();
                    _appdata.m_cidapp = _entidades[0].cidapp;
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
                } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "UserDataes - Error !"); }

                try {
                    if (File.Exists(basePath + "config.json"))
                    {
                        if (File.Exists(dirConfig + "config.json")) { File.Delete(dirConfig + "config.json"); }
                        File.Copy(basePath + "config.json", dirConfig + "config.json");
                    }
                } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "config - Error !" + e.ToString()); }

                try {
                    if (File.Exists(basePath + "datas.json"))
                    {
                        if (File.Exists(dirConfig + "datas.json")) { File.Delete(dirConfig + "datas.json"); }
                        File.Copy(basePath + "datas.json", dirConfig + "datas.json");
                    }
                } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "datas - Error !" + e.ToString()); }

                try
                {
                    // - - - - - directory "EEvaApp/Xtras"
                    List<DELEGACIONES> _dele = (from e in db.DELEGACIONES orderby e.nombre where e.estado == 1 select e).ToList();
                    // - - - - - graba json en carpeta                
                    var json_data02 = JToken.FromObject(_dele, new JsonSerializer() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    fichero = File.CreateText(dirXtras + "Delegaciones" + ".json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_data02.WriteTo(jsonwriter);
                    jsonwriter.Close();
                } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "253 - Error !" + e.ToString()); }

                try
                {
                    List<IDIOMAS> _lang = (from e in db.IDIOMAS orderby e.nombre select e).ToList();
                    // - - - - - graba json en carpeta                
                    var json_data03 = JToken.FromObject(_lang);
                    fichero = File.CreateText(dirXtras + "Idiomas" + ".json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_data03.WriteTo(jsonwriter);
                    jsonwriter.Close();
                }
                catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "261 - Error !" + e.ToString()); }

                try
                {
                    List<CCAA> _ccaa = (from e in db.CCAA orderby e.nombre select e).ToList();
                    // - - - - - graba json en carpeta                
                    var json_data04 = JToken.FromObject(_ccaa, new JsonSerializer() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    fichero = File.CreateText(dirXtras + "Comunidades" + ".json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_data04.WriteTo(jsonwriter);
                    jsonwriter.Close();
                }
                catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "273 - Error !" + e.ToString()); }

                try
                {
                    List<PROVINCIAS> _prov = (from e in db.PROVINCIAS orderby e.nombre select e).ToList();
                    // - - - - - graba json en carpeta         
                    var json_data05 = JToken.FromObject(_prov, new JsonSerializer() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    fichero = File.CreateText(dirXtras + "Provincias" + ".json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_data05.WriteTo(jsonwriter);
                    jsonwriter.Close();
                }
                catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "284 - Error !" + e.ToString()); }


                // - - - - - directory EEvaApp/Data: app data (events & interestdata)
                //    string json_events = "{}";


                // - - - - - graba imagenes
                //if (File.Exists(imagespath + "appimage" + expo.Id + ".jpg"))
                //{
                //    File.Copy(imagespath + "appimage" + expo.Id + ".jpg", xprtelements + @"\" + "appimage" + ".jpg");
                //}

                // string json_interestdata = "{}";

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera JSons y almacena //

            RegisterActionInLog("GetAPPCTIVE", "294 - CREA ZIP");

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera ZIP ->
                filezip = activatePath + _entidades[0].cidapp + ".zip";
                if (File.Exists(filezip)) { File.Delete(filezip); }

                ZipFile.CreateFromDirectory(dirMaster, filezip);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera ZIP ->
            }



            if (!isOK) { return Request.CreateResponse(HttpStatusCode.BadRequest);

RegisterActionInLog("GetAPPCTIVE", "309 - SALE ERROR");

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
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - PostAPPCTIVE //
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - PostAPPCTIVE //



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
            while ((line = r.ReadLine()) != null) { content += line + '\r' + '\n'; }
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


