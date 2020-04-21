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
        public class APPDATEController : ApiController
        {
            private EEvAppEntities db = new EEvAppEntities();

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetAPPDATE ->
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetAPPDATE ->
        // http://api.eevapp.es/api/APPDATE/update/timestamp/123456789012345/1$2_1_1
            public HttpResponseMessage GetAPPDATE([FromUri] int _date, string _imei, string _search)
            {

                db.Configuration.LazyLoadingEnabled = false;
                

                RegisterActionInLog("GetAPPDATE", "101 - " + _date + " | " + _imei);

                bool isOK = true;
                string thecode = "";
                string actionResult = "";
                string filezip = "";
                USUARIOS _entidad = new USUARIOS();

                // - - - - - getting data
                if (_email == null || _imei == null) { isOK = false; actionResult += " | email_imei = null"; }


                // - - - - - control parametro
                if (isOK && _imei != null)
                {

                    try { _entidad = (from e in db.USUARIOS where e.imei.Equals(_imei) select e).First(); }
                    catch (Exception e) { isOK = false; }

                    // - - - - - control parametro
                    if (isOK && _entidad != null)
                    {
                        // - - - - - control usuario activo
                        if (_entidad.estado == 0)
                        {
                        isOK = false; actionResult += " | _entidad[0].estado = 0";
                        }
                    }
                    else { isOK = false; actionResult += " | _entidad[0] = null"; }

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
                    string zipFiles = basePath + "zipFiles\\";
                    string dirMaster = activatePath + "EEvaApp\\"; // - - - - - dirMaster
                    string dirConfig = dirMaster + "Config\\"; // - - - - - dirConfig
                    string dirXtras = dirMaster + "Xtras\\"; // - - - - - dirXtras
                    string dirDatas = dirMaster + "Datas\\"; // - - - - - dirDatas --> Eventos.json y datosinteres.json
                    string dirImages = dirDatas + "Images\\"; // - - - - - dirImages --> iMÁGENES EVENTOS
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

                        RegisterActionInLog("GetAPPDATE", "194 - Directory.Exists(activatePath)");

                    }
                    else
                    {
                        // crea directorio
                        try { Directory.CreateDirectory(dirMaster); } catch (Exception e) { RegisterActionInLog("GetAPPDATE", "200 - Error !"); }
                        try { Directory.CreateDirectory(dirConfig); } catch (Exception e) { RegisterActionInLog("GetAPPDATE", "201 - Error !"); }
                        try { Directory.CreateDirectory(dirXtras); } catch (Exception e) { RegisterActionInLog("GetAPPDATE", "202 - Error !"); }
                        try { Directory.CreateDirectory(dirDatas); } catch (Exception e) { RegisterActionInLog("GetAPPDATE", "203 - Error !"); }
                        try { Directory.CreateDirectory(dirImages); } catch (Exception e) { RegisterActionInLog("GetAPPDATE", "204 - Error !"); }
                        try { Directory.CreateDirectory(dirWork); } catch (Exception e) { RegisterActionInLog("GetAPPDATE", "205 - Error !"); }
                        try { Directory.CreateDirectory(dirZipped); } catch (Exception e) { RegisterActionInLog("GetAPPDATE", "206 - Error !"); }
                        try { Directory.CreateDirectory(dirUnzipped); } catch (Exception e) { RegisterActionInLog("GetAPPDATE", "207 - Error !"); }

                        RegisterActionInLog("GetAPPDATE", "209 - CREA DIR");

                    }
                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - Elimina y regenera directorios //

                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera JSons y almacena ->
                    StreamWriter fichero;
                    JsonTextWriter jsonwriter;
                    List<PairIdName> pairs = new List<PairIdName>();
                // timestamp --> (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                
                    try
                    {
                    /* Elemento final(_search), tiene que ser 3 array de ints, posible null:
                    * Usados para hacer las consultas --> Delegación, comunidad, provincia
                    */
                    string[] locationData = _search.Split('_');
                    string[] delgaciones = locationData[0].Split('$');
                    // - - - - - directory "EEvaApp/Datas"
                    List<EVENTOS> _event = (from e in db.EVENTOS orderby e.fechainicio where e.estado == 1 && e.comunidad == locationData[1] && e.provincia == locationData[2] select e).ToList(); //Formato LinQ
                    List<EventsJson> _olist = IdNameObj.Dele2IdNameObj(_event); //De cada evento, se genera un json
                    //Trabajo sobre listas
                    List<EventsJsonReducido> _rlist = new List<EventsJsonReducido>();
                    EventsJsonReducido r;
                    foreach (EventsJson e in _olist) {
                        r = new EventsJsonReducido();
                        e.reduceData(r);
                        _rlist.Add(r);
                    }
                    // - - - - - graba datos completos         
                    var jsonData;
                    foreach (EventsJson e in _olist)
                    {
                        jsonData = JToken.FromObject(e);
                        fichero = File.CreateText(dirDatas + "Evento" + e.id + ".json");
                        jsonwriter = new JsonTextWriter(fichero);
                        jsonData.WriteTo(jsonwriter);
                        jsonwriter.Close();
                    }
                    //var json_data02 = JToken.FromObject(_olist);
                    //fichero = File.CreateText(dirDatas + "Eventos" + ".json");
                    //jsonwriter = new JsonTextWriter(fichero);
                    //json_data02.WriteTo(jsonwriter);
                    //jsonwriter.Close();
                    // - - - - - graba json datos reducidos
                    json_data02 = JToken.FromObject(_rlist);
                    fichero = File.CreateText(dirDatas + "DatosEventos" + ".json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_data02.WriteTo(jsonwriter);
                    jsonwriter.Close();
                }
                    catch (Exception e) { RegisterActionInLog("GetAPPDATE", "253 - Error !" + e.ToString()); }


                    // - - - - - directory EEvaApp/Data: app data (events & interestdata)
                    string json_events = "{}";


                    // - - - - - graba imagenes
                    if (File.Exists(imagespath + "appimage" + expo.Id + ".jpg"))
                    {
                        File.Copy(imagespath + "appimage" + expo.Id + ".jpg", xprtelements + @"\" + "appimage" + ".jpg");
                    }

                    string json_interestdata = "{}";

                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera JSons y almacena //

                    List<DATOSINTERES> _data = (from e in db.DATOSINTERES orderby e.id select e).ToList(); //Formato LinQ
                    List<InterestDataJson> _dlist = IdNameObj.Dele2IdNameObj(_data);

                    var json_interesdata = JToken.FromObject(_dlist);
                    fichero = File.CreateText(dirDatas + "DatosInteres" + ".json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_interesdata.WriteTo(jsonwriter);
                    jsonwriter.Close();

                    RegisterActionInLog("GetAPPDATE", "294 - CREA ZIP");

                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera ZIP ->
                    filezip = zipFiles + _entidad.cidapp + ".zip";
                    if (File.Exists(filezip)) { File.Delete(filezip); }

                    ZipFile.CreateFromDirectory(activatePath, filezip);

                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera ZIP //
                }

                if (!isOK)
                {
                    RegisterActionInLog("GetAPPDATE", "309 - SALE ERROR");

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

                    RegisterActionInLog("GetAPPDATE", "324 - RETORNA ARCHIVO");

                    return response;

                    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - RETURN FILE //
                }

            }
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetAPPDATE //
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GettAPPDATE //


            

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
                if (r != null) { while ((line = r.ReadLine()) != null) { content += line + '\r' + '\n'; } }
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
                    RegisterActionInLog("GetAPPDATE", "failed to send email with the following error:" + ep.ToString());
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