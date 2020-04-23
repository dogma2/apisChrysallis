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

        public InterestDataJson(DATOSINTERES d)
        {
            this.id = d.id;
            this.nombre = d.nombre;
            this.descripcion = d.descripcion;
            this.direccion = d.direccion;
            this.ciudad = d.ciudad;
            this.cp = d.cp;
            this.idprovincia = (int)d.idprovincia;
            this.idccaa = (int)d.idccaa;
            this.telefono = d.telefono;
            this.email = d.email;
            this.contacto = d.contacto;
            this.iddelegacion = (int)d.iddelegacion;
        }

        public static List<InterestDataJson> Di2Idj(List<DATOSINTERES> datointeres)
        {
            List<InterestDataJson> retu = new List<InterestDataJson>();
            foreach (DATOSINTERES d in datointeres)
            {
                retu.Add(new InterestDataJson(d));
            }
            return retu;
        }

    }
}