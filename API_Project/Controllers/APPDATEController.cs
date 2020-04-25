using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using API_Project;
using API_Project.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OC.Core.Crypto;

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


            RegisterActionInLog("GetAPPDATE", "35 - " + _date + " | " + _imei);

            bool isOK = true;
            string actionResult = "";
            string filezip = "";
            USUARIOS _entidad = new USUARIOS();

            // - - - - - getting data
            if (_imei == null) { isOK = false; actionResult += " | _imei = null"; }


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

            RegisterActionInLog("GetAPPDATE", "67 - " + actionResult);

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera directorios ->

            // - - - - - Estructura de carpetas
            string basePath = "C:\\inetpub\\vhosts\\eevapp.es\\httpdocs\\dataXport_EEvApp\\";
            string zipFiles = basePath + "zipFiles\\";
            string updatePath = basePath + "UpdateWork\\";
            string dirMaster = updatePath + "EEvaApp\\"; // - - - - - dirMaster
            string dirXtras = dirMaster + "Xtras\\"; // - - - - - dirXtras
            string dirDatas = dirMaster + "Datas\\"; // - - - - - dirDatas --> Eventos.json y datosinteres.json
            string dirImages = dirDatas + "Images\\"; // - - - - - dirImages --> iMÁGENES EVENTOS

            // - - - - - prepara carpeta export
            DirectoryInfo workdir;

            // zipFiles
            if (!Directory.Exists(zipFiles)) { try { Directory.CreateDirectory(zipFiles); } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "create zipFiles - Error !"); } }
            // dirMaster
            if (!Directory.Exists(dirMaster)) { try { Directory.CreateDirectory(dirMaster); } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "create dirMaster - Error !"); } }
            // dirXtras
            if (!Directory.Exists(dirXtras)) { try { Directory.CreateDirectory(dirXtras); } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "create dirXtras - Error !"); } }
            else { workdir = new DirectoryInfo(dirXtras); foreach (FileInfo file in workdir.GetFiles()) { file.Delete(); } }
            // dirDatas
            if (!Directory.Exists(dirDatas)) { try { Directory.CreateDirectory(dirDatas); } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "create dirDatas - Error !"); } }
            else { workdir = new DirectoryInfo(dirDatas); foreach (FileInfo file in workdir.GetFiles()) { file.Delete(); } }
            // dirImages
            if (!Directory.Exists(dirImages)) { try { Directory.CreateDirectory(dirImages); } catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "create dirImages - Error !"); } }
            else { workdir = new DirectoryInfo(dirImages); foreach (FileInfo file in workdir.GetFiles()) { file.Delete(); } }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera directorios //

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera JSons y almacena ->
            StreamWriter fichero;
            JsonTextWriter jsonwriter;
            List<PairIdName> pairs = new List<PairIdName>();
            JToken json_data;

            if (isOK)
            {
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - JSON EVENTOS ->

                // Elemento final(_search), tiene que ser 3 array de ints, posible null:  Usados para hacer las consultas --> Delegación, comunidad, provincia ->
                int tmpint;
                var linQsEv = from e in db.EVENTOS orderby e.fechainicio where e.estado == 1 select e;
                var linQsDI = from d in db.DATOSINTERES orderby d.nombre where d.estado == 1 select d;
                if (isOK && _search != null)
                {
                    string[] filtro = _search.Split('_');
                    for (int i = 0; i < filtro.Length; i++)
                    {
                        string[] lista = filtro[0].Split('$');
                        for (int k = 0; k < lista.Length; k++)
                        {
                            if (int.TryParse(lista[k], out tmpint))
                            {
                                //if (i == 0) {  /*Delegacion*/ linQsEv = linQsEv.Where(e => e.iddelegacion == tmpint); linQsDI = linQsDI.Where(d => d.iddelegacion == tmpint); }
                                //else if (i == 1) { /*Comunidad*/ linQsEv = linQsEv.Where(e => e.idccaa == tmpint); linQsDI = linQsDI.Where(d => d.idccaa == tmpint); }
                                //else if (i == 2) { /*Provincia*/ linQsEv = linQsEv.Where(e => e.idprovincia == tmpint); linQsDI = linQsDI.Where(d => d.idprovincia == tmpint); }
                            }
                        }
                    }
                }
                // RegisterActionInLog("GetAPPDATE", "135 - linq" + linQsEv.ToString());
                // Elemento final(_search), tiene que ser 3 array de ints, posible null:  Usados para hacer las consultas --> Delegación, comunidad, provincia //

                try
                {
                    // - - - - - directory "EEvaApp/Datas"
                    List<EVENTOS> _event = (linQsEv).ToList(); //Formato LinQ
                    EventsJson _fullEv = null;
                    Image image = null;
                    RegisterActionInLog("GetAPPDATE", "144 - _event.Count: " + _event.Count);
                    foreach (EVENTOS e in _event)
                    {
                        _fullEv = new EventsJson(e);
                        // - - - - - graba json datos reducidos
                        json_data = JToken.FromObject(_fullEv);
                        fichero = File.CreateText(dirDatas + "Evento" + e.id + ".json");
                        jsonwriter = new JsonTextWriter(fichero);
                        json_data.WriteTo(jsonwriter);
                        jsonwriter.Close();

                        RegisterActionInLog("GetAPPDATE", "156 - fichero: " + dirDatas + "Evento" + _fullEv.id + ".json");

                        // - - - - - graba imagenes
                        if (File.Exists(dirImages + "Evento" + e.id + ".jpg")) { File.Delete(dirImages + "Evento" + e.id + ".jpg"); }
                        if (e.imagen != null)
                        {
                            using (MemoryStream ms = new MemoryStream(e.imagen))
                            {

                                image = Image.FromStream(ms);

                            }
                            var bmi = new Bitmap(image);
                            bmi.Save(dirImages + "Evento" + e.id + ".jpg", ImageFormat.Jpeg);

                            RegisterActionInLog("GetAPPDATE", "156 - imagen: " + dirImages + "Evento" + e.id + ".jpg");
                        }
                    }
                    List<EventsJsonReducido> _olist = EventsJsonReducido.Ev2EJR(_event);
                    // - - - - - graba datos completos
                    json_data = JToken.FromObject(_olist);
                    fichero = File.CreateText(dirDatas + "Eventos.json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_data.WriteTo(jsonwriter);
                    jsonwriter.Close();

                }
                catch (Exception e) { RegisterActionInLog("GetAPPDATE", "166 - Error !" + e.ToString()); }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - JSON EVENTOS //

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - JSON DATOSINTERES ->
                try
                {
                    // - - - - - directory "EEvaApp/Datas"
                    List<DATOSINTERES> _dain = (linQsDI).ToList(); //Formato LinQ
                    List<InterestDataJson> _olist = InterestDataJson.Di2Idj(_dain);
                    // - - - - - graba json en carpeta                
                    json_data = JToken.FromObject(_olist);
                    fichero = File.CreateText(dirDatas + "DatosInteres" + ".json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_data.WriteTo(jsonwriter);
                    jsonwriter.Close();
                }
                catch (Exception e) { RegisterActionInLog("GetAPPDATE", "253 - Error !" + e.ToString()); }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - JSON DATOSINTERES //

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - DELEGACIONES ->
                try
                {
                    // - - - - - directory "EEvaApp/Xtras"
                    List<DELEGACIONES> _dele = (from e in db.DELEGACIONES orderby e.nombre where e.estado == 1 select e).ToList();
                    List<IdNameObj> _olist = IdNameObj.Dele2IdNameObj(_dele);
                    //List<DELEGACIONES> _dele = (from e in db.DELEGACIONES orderby e.nombre where e.estado == 1 select e).ToList();
                    // - - - - - graba json en carpeta                
                    var json_data02 = JToken.FromObject(_olist);
                    fichero = File.CreateText(dirXtras + "Delegaciones" + ".json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_data02.WriteTo(jsonwriter);
                    jsonwriter.Close();
                }
                catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "253 - Error !" + e.ToString()); }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - DELEGACIONES //

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - IDIOMAS ->
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
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - IDIOMAS ->

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - CCAA ->
                try
                {
                    List<CCAA> _ccaa = (from e in db.CCAA orderby e.nombre select e).ToList();
                    List<IdNameObj> _olist = IdNameObj.Dele2IdNameObj(_ccaa);
                    //List<CCAA> _ccaa = (from e in db.CCAA orderby e.nombre select e).ToList();
                    // - - - - - graba json en carpeta                
                    var json_data04 = JToken.FromObject(_olist, new JsonSerializer() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None });
                    fichero = File.CreateText(dirXtras + "Comunidades" + ".json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_data04.WriteTo(jsonwriter);
                    jsonwriter.Close();
                }
                catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "273 - Error !" + e.ToString()); }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - CCAA ->

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - PROVINCIAS ->
                try
                {
                    List<PROVINCIAS> _prov = (from e in db.PROVINCIAS orderby e.idccaa,e.nombre select e).ToList();
                    List<ProvinciasJson> _olist = ProvinciasJson.Prov2Pjs(_prov);
                    //List<PROVINCIAS> _prov = (from e in db.PROVINCIAS orderby e.nombre select e).ToList();
                    // - - - - - graba json en carpeta         
                    var json_data05 = JToken.FromObject(_olist, new JsonSerializer() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None });
                    fichero = File.CreateText(dirXtras + "Provincias" + ".json");
                    jsonwriter = new JsonTextWriter(fichero);
                    json_data05.WriteTo(jsonwriter);
                    jsonwriter.Close();
                }
                catch (Exception e) { RegisterActionInLog("GetAPPCTIVE", "284 - Error !" + e.ToString()); }
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - PROVINCIAS ->

            }

            if (!_imei.Equals(""))
            {

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - JSON ESTADO ->
                IdNameObj action = null;
                if (isOK) { action = new IdNameObj(1, "Ok"); } else { action = new IdNameObj(0, "ko"); }
                json_data = JToken.FromObject(action);
                fichero = File.CreateText(dirDatas + "Action" + ".json");
                jsonwriter = new JsonTextWriter(fichero);
                json_data.WriteTo(jsonwriter);
                jsonwriter.Close();
                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - JSON ESSTADO //

                RegisterActionInLog("GetAPPDATE", "181 - CREA ZIP");

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera ZIP ->
                filezip = zipFiles + _imei + ".zip";
                if (File.Exists(filezip)) { File.Delete(filezip); }

                ZipFile.CreateFromDirectory(updatePath, filezip);

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - genera ZIP //

                RegisterActionInLog("GetAPPDATE", "191 - RETORNA ARCHIVO");

                // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - RETURN FILE ->

                FileInfo infozip = new FileInfo(filezip);
                // localFilePath = getFileFromID(id, out infozip.Name, out infozip.Length);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new FileStream(filezip, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = infozip.Name;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");

                RegisterActionInLog("GetAPPDATE", "203 - RETORNA ARCHIVO");

                return response;

            }
            else
            {
                RegisterActionInLog("GetAPPCTIVE", "245 - SALE IMEI VACIO");

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }



            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - RETURN FILE //

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



    }
}