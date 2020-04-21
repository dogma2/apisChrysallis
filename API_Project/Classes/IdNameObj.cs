using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Project.Classes
{
    public class IdNameObj
    {

        public int id { get; set; }
        public string nombre { get; set; }


        public IdNameObj(int id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }

        public static List<IdNameObj> Dele2IdNameObj(List<DELEGACIONES> _listOb)
        {
            List<IdNameObj> _retu = new List<IdNameObj>();
            foreach (DELEGACIONES o in _listOb)
            {
                _retu.Add(new IdNameObj(o.id, o.nombre));
            }
            return _retu;
        }

        public static List<IdNameObj> Dele2IdNameObj(List<PROVINCIAS> _listOb)
        {
            List<IdNameObj> _retu = new List<IdNameObj>();
            foreach (PROVINCIAS o in _listOb)
            {
                _retu.Add(new IdNameObj(o.id, o.nombre));
            }
            return _retu;
        }

        public static List<IdNameObj> Dele2IdNameObj(List<CCAA> _listOb)
        {
            List<IdNameObj> _retu = new List<IdNameObj>();
            foreach (CCAA o in _listOb)
            {
                _retu.Add(new IdNameObj(o.id, o.nombre));
            }
            return _retu;
        }

        public static List<IdNameObj> Events2IdNameObj(List<EVENTOS> _listOb)
        {
            List<IdNameObj> _retu = new List<IdNameObj>();
            foreach (EVENTOS o in _listOb)
            {
                _retu.Add(new IdNameObj(o.id, o.nombre));
            }
            return _retu;
        }

        public static List<IdNameObj> InterestData2IdNameObj(List<DATOSINTERES> _listOb)
        {
            List<IdNameObj> _retu = new List<IdNameObj>();
            foreach (DATOSINTERES o in _listOb)
            {
                _retu.Add(new IdNameObj(o.id, o.nombre));
            }
            return _retu;
        }

    }
}