using System.IO;
using System.Web.Http;
using API_Project.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API_Project.Controllers
{
    public class UPCONFIGController : ApiController
    {
        private static string FILE_PATH = "C:\\inetpub\\vhosts\\eevapp.es\\httpdocs\\dataXport_EEvApp\\";


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetUPCONFIG ->
        public DeviceAppDataes GetUPCONFIG()
        {
            DeviceAppDataes retu = ReadingJsonFile("config");
            return retu;
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetUPCONFIG //
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - FILE READING ->
        public DeviceAppDataes ReadingJsonFile(string filename)
        {
            JObject jsonentidad = null;
            DeviceAppDataes entidad = null;
            if (File.Exists(FILE_PATH + filename + ".json"))
            {
                // Lee Archivo JSON
                jsonentidad = JObject.Parse(File.ReadAllText(FILE_PATH + filename + ".json"));
                entidad = jsonentidad.ToObject<DeviceAppDataes>();
            }
            return entidad;
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - FILE READING //


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - PostUPCONFIG ->
        public void PostUPCONFIG(DeviceAppDataes _entity)
        {
            SavingJsonFile("config", _entity);
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetUPCONFIG //
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - FILE WRITING ->
        public void SavingJsonFile(string filename, DeviceAppDataes entidad)
        {
            if (entidad != null)
            {

                JObject jsonentidad = (JObject)JToken.FromObject(entidad);
                StreamWriter fichero = File.CreateText(FILE_PATH + filename + ".json");
                JsonTextWriter jsonwriter = new JsonTextWriter(fichero);
                jsonentidad.WriteTo(jsonwriter);
                jsonwriter.Close();
            }
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - FILE WRITING //

    }
}