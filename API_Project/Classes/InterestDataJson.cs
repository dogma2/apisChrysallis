using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Project.Classes
{
    public class InterestDataJson
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string cp { get; set; }
        public int idprovincia { get; set; }
        public int idccaa { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string contacto { get; set; }
        public int iddelegacion { get; set; }
    }
}