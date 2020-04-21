using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Project.Classes
{
    public class EventsJson
    {
        public int id { get; set; }
        public string cidevento { get; set; }
        public byte estado { get; set; }
        public string titulo { get; set; }
        public string intro { get; set; }
        public string descripcion { get; set; }
        public System.DateTime fechainicio { get; set; }
        public System.TimeSpan horainicio { get; set; }
        public System.DateTime fechafin { get; set; }
        public System.TimeSpan horafin { get; set; }
        public byte[] imagen { get; set; }
        public string notasevento { get; set; }
        public string notastransporte { get; set; }
        public byte idccaa { get; set; }
        public byte idprovincia { get; set; }
        public string ciudad { get; set; }
        public string coordgps { get; set; }
        public byte ctrlglobal { get; set; }
        public int iddelegacion { get; set; }
        public int iddsktuser { get; set; }
        public int asist { get; set; }

        public void reduceData(EventsJsonReducido evento)
        {
            evento.id = this.id;
            evento.titulo = this.titulo;
            evento.intro = this.intro;
            evento.fechainicio = this.fechainicio;
            evento.horainicio = this.horainicio;
            evento.asist = this.asist;
    }
    }
}