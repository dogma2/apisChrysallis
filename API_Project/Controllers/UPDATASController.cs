using System.IO;
using System.Web.Http;
using API_Project.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API_Project.Controllers
{
    public class UPDATASController : ApiController
    {
        private static string FILE_PATH = "C:\\inetpub\\vhosts\\eevapp.es\\httpdocs\\dataXport_EEvApp\\";


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetUPCONFIG ->
        public AsociationDataes GetUPCONFIG()
        {
            AsociationDataes retu = ReadingJsonFile("datas");
            return retu;
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetUPCONFIG //
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - FILE READING ->
        public AsociationDataes ReadingJsonFile(string filename)
        {
            JObject jsonentidad = null;
            AsociationDataes entidad = null;
            if (File.Exists(FILE_PATH + filename + ".json"))
            {
                // Lee Archivo JSON
                jsonentidad = JObject.Parse(File.ReadAllText(FILE_PATH + filename + ".json"));
                entidad = jsonentidad.ToObject<AsociationDataes>();
            }
            return entidad;
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - FILE READING //


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - PostUPCONFIG ->
        public void PostUPCONFIG(AsociationDataes _entity)
        {
            SavingJsonFile("datas", _entity);
        }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - GetUPCONFIG //
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - FILE WRITING ->
        public void SavingJsonFile(string filename, AsociationDataes entidad)
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