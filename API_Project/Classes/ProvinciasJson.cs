using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Project.Classes
{
    public class ProvinciasJson
    {

        public int id { get; set; }
        public string nombre { get; set; }
        public int idccaa { get; set; }

        public ProvinciasJson(int id, string nombre, int idccaa)
        {
            this.id = id;
            this.nombre = nombre;
            this.idccaa = idccaa;
        }

        public static List<ProvinciasJson> Prov2Pjs(List<PROVINCIAS> _listOb)
        {
            List<ProvinciasJson> _retu = new List<ProvinciasJson>();
            foreach (PROVINCIAS o in _listOb)
            {
                _retu.Add(new ProvinciasJson(o.id, o.nombre, (int)o.idccaa));
            }
            return _retu;
        }

    }
}