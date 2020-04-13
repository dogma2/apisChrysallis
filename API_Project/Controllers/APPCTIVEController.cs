using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Http;
using Newtonsoft.Json;
using OC.Core.Crypto;

namespace API_Project.Controllers
{
    public class APPCTIVEController : ApiController
    {
        private EEvAppEntities db = new EEvAppEntities();

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


        public string PostAPPCTIVE(String email_imei)
        {
            bool isOK = true;
            string actionResult = "";
            List<USUARIOS> _entidades = new List<USUARIOS>();
            RegisterActionInLog("GetAPPCTIVE", email_imei);

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

                    }
                    else { isOK = false; actionResult += " | _entidades[0].estado = 0"; }

                }
                else { isOK = false; actionResult += " | _entidades[0] = null"; }

            }
            else { isOK = false; actionResult += " | isOK = false o _email = null"; }

            RegisterActionInLog("GetAPPCTIVE", actionResult);

            if (isOK) { 
                // - - - - - directory "config"

/*            
            
            public bool m_set_GPS { get; set; }
        public bool m_set_Sound { get; set; }
        public int m_set_Lang { get; set; }
        public int m_set_Update { get; set; }
        public bool m_set_LongAlertAct { get; set; }
        public int m_set_LongAlert { get; set; }
        public bool m_set_ShortAlertAct { get; set; }
        public int m_set_ShortAlert { get; set; }
        "AlertFromHH":  { get; set; }
        "AlertFromMM" { get; set; }
        "AlertToHH" { get; set; }
        "AlertToMM" { get; set; }

    \"delegaciones\":[{"+_entidades[0].iddelegacion+"}], \"ccaas\":[{}], \"provincias\":[{}]

        // - - - - - directory "state"
        string json_state = "{\"cidapp\":\""+_entidades[0].cidapp+ "\",\"state\":0,\"date\":\"\",\"updated\":\"\",\"result\":\"\"}";
                // - - - - - directory "Xtra"
                List<DELEGACIONES> _dele = (from e in db.DELEGACIONES orderby e.nombre where e.estado.Equals(1) select e).ToList();
                string json_dele = JsonConvert.SerializeObject(_dele);
                List<IDIOMAS> _lang = (from e in db.IDIOMAS orderby e.nombre select e).ToList();
                string json_lang = JsonConvert.SerializeObject(_dele);
                List<CCAA> _ccaa = (from e in db.CCAA orderby e.nombre select e).ToList();
                string json_ccaa = JsonConvert.SerializeObject(_dele);
                List<PROVINCIAS> _prov = (from e in db.PROVINCIAS orderby e.nombre select e).ToList();
                string json_prov = JsonConvert.SerializeObject(_dele);

                
                string json_events = "{}";
                string json_interestdata = "{}";




































                string fullpath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;

                string dataespath = fullpath + @"\Data\";
                string imagespath = fullpath + @"\Images\";
                // estructura de exportacion
                string targetpath = fullpath + @"\Export\" + expo.Id;
                string xprtbase = targetpath + @"\EEvApp";

                string xprtdata = database + @"\Data"; // json:  de aqui va el exposicion.json
                string xprtimagenes = xprtdata + @"\Imagenes";

                /*                // ----- prepara carpeta export
                                if (Directory.Exists(targetpath))
                                {
                                    System.IO.DirectoryInfo dirinfo;
                                    // elimina archivos contenidos para generar nuevos
                                    dirinfo = new System.IO.DirectoryInfo(xprtdata);
                                    foreach (System.IO.FileInfo file in dirinfo.GetFiles()) { file.Delete(); }
                                    // elimina archivos contenidos para generar nuevos
                                    dirinfo = new System.IO.DirectoryInfo(xprtelements);
                                    foreach (System.IO.FileInfo file in dirinfo.GetFiles()) { file.Delete(); }
                                    // elimina archivos contenidos para generar nuevos
                                    dirinfo = new System.IO.DirectoryInfo(xprtpregunts);
                                    foreach (System.IO.FileInfo file in dirinfo.GetFiles()) { file.Delete(); }
                                }
                                else
                                {
                                    // crea directorio
                                    Directory.CreateDirectory(targetpath);
                                    Directory.CreateDirectory(xprtbase);
                                    Directory.CreateDirectory(xprtdata);
                                    Directory.CreateDirectory(xprtimagenes);
                                    Directory.CreateDirectory(xprtelements);
                                    // Directory.CreateDirectory(xprtpregunts);
                                }
                                // ----- carga literales de app en json de exposicion
                                if (expo.ExposicionIdiomas == null) { expo.ExposicionIdiomas = new Dictionary<Idioma, ExposicionIdioma>(); }
                                if (File.Exists(dataespath + "literalesE" + expo.Id + ".json"))
                                {
                                    //fullpath + @"Files\ + "literales" + "E" + nExpoId + ".json"
                                    JArray jsonlites = JArray.Parse(File.ReadAllText(dataespath + "literalesE" + expo.Id + ".json"));
                                    List<ExposicionIdioma> literales = jsonlites.ToObject<List<ExposicionIdioma>>();

                                    Dictionary<Idioma, ExposicionIdioma> exposicionIdiomas = new Dictionary<Idioma, ExposicionIdioma>();

                                    foreach (ExposicionIdioma exposicionIdioma in literales)
                                    {
                                        foreach (Idioma idioma in expo.Idiomas)
                                        {
                                            if (idioma.Id == exposicionIdioma.LangId)
                                            {
                                                exposicionIdiomas.Add(idioma, exposicionIdioma);
                                            }
                                        }
                                    }

                                    expo.ExposicionIdiomas = exposicionIdiomas;

                                    foreach (Idioma lang in expo.Idiomas)
                                    {
                                        foreach (ExposicionIdioma langlite in literales)
                                        {
                                            if (lang.Id.Equals(langlite.LangId))
                                            {
                                                if (expo.ExposicionIdiomas == null) { expo.ExposicionIdiomas.Add(lang, langlite); }
                                            }
                                        }
                                    }
                                }
                                // ----- graba json en carpeta de exportacion
                                using (StreamWriter fichero = File.CreateText(xprtdata + @"\" + "exposicion.json"))
                                {
                                    JsonSerializerSettings settings = new JsonSerializerSettings
                                    {
                                        ContractResolver = new DictionaryAsArrayResolver()
                                    };

                                    String json = JsonConvert.SerializeObject(expo, settings);

                                    fichero.Write(json);
                                }
                                // ----- graba imagenes
                                // appimage
                                if (File.Exists(imagespath + "appimage" + expo.Id + ".jpg"))
                                {
                                    File.Copy(imagespath + "appimage" + expo.Id + ".jpg", xprtelements + @"\" + "appimage" + ".jpg");
                                }
                                // sumimage
                                if (File.Exists(imagespath + "sumimage" + expo.Id + ".jpg"))
                                {
                                    File.Copy(imagespath + "sumimage" + expo.Id + ".jpg", xprtelements + @"\" + "sumimage" + ".jpg");
                                }
                                // banderas
                                foreach (Idioma lang in expo.Idiomas)
                                {
                                    if (File.Exists(imagespath + lang.Image)) { File.Copy(imagespath + lang.Image, xprtelements + @"\" + lang.Image); }
                                }
                                // personajes
                                foreach (Personaje charac in expo.Personaje)
                                {
                                    if (File.Exists(imagespath + charac.Image)) { File.Copy(imagespath + charac.Image, xprtelements + @"\" + charac.Image); }
                                }
                                // Preguntas
                                foreach (Nivel nivel in expo.Niveles)
                                {
                                    foreach (Pregunta pregunta in expo.Preguntas[nivel])
                                    {
                                        if (File.Exists(imagespath + pregunta.ImagenPregunta)) { File.Copy(imagespath + pregunta.ImagenPregunta, xprtpregunts + @"\" + pregunta.ImagenPregunta); }
                                        if (File.Exists(imagespath + pregunta.ImagenRespuestaCorrecta)) { File.Copy(imagespath + pregunta.ImagenRespuestaCorrecta, xprtpregunts + @"\" + pregunta.ImagenRespuestaCorrecta); }
                                        if (File.Exists(imagespath + pregunta.ImagenRespuestaIncorrecta1)) { File.Copy(imagespath + pregunta.ImagenRespuestaIncorrecta1, xprtpregunts + @"\" + pregunta.ImagenRespuestaIncorrecta1); }
                                        if (File.Exists(imagespath + pregunta.ImagenRespuestaIncorrecta2)) { File.Copy(imagespath + pregunta.ImagenRespuestaIncorrecta2, xprtpregunts + @"\" + pregunta.ImagenRespuestaIncorrecta2); }
                                        if (File.Exists(imagespath + pregunta.ImagenRespuestaIncorrecta3)) { File.Copy(imagespath + pregunta.ImagenRespuestaIncorrecta3, xprtpregunts + @"\" + pregunta.ImagenRespuestaIncorrecta3); }
                                    }
                                }

                                // ----- selecciona carpeta de exportacion de archivo zip
                                string zipPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Export";
                                using (FolderBrowserDialog folderdiag = new FolderBrowserDialog())
                                {
                                    if (folderdiag.ShowDialog() == DialogResult.OK)
                                    {
                                        if (folderdiag.SelectedPath.Length > 0) { zipPath = folderdiag.SelectedPath; }
                                    }
                                }
                                if (File.Exists(zipPath + @"\E" + expo.Id + ".zip"))
                                {
                                    File.Delete(zipPath + @"\E" + expo.Id + ".zip");
                                }
                                ZipFile.CreateFromDirectory(targetpath, zipPath + @"\E" + expo.Id + ".zip");
                                MessageBox.Show("Se han exportado los datos de la Exposicion \"" + expo.Nombre + "\"", "INFORMACION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // - - - - - - - - - - - - - - - - - - - - - - - -/MAGOMO
                            }

                */

























                return _entidades[0].email;





            }
            else { return "Error: " + actionResult; }



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

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - EMAILING ->
        // - - - - - retorna string en hash512
        public static String getHashString(String pass)
        {
            Hash encrypt = new Hash();
            return (String)encrypt.Sha512(pass);
        }
        // - - - - - send email
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