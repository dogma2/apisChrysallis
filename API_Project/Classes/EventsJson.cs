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
        public int estado { get; set; }
        public string titulo { get; set; }
        public string intro { get; set; }
        public string descripcion { get; set; }
        public string fechainicio { get; set; }
        public string horainicio { get; set; }
        public string fechafin { get; set; }
        public string horafin { get; set; }
        public string notasevento { get; set; }
        public string notastransporte { get; set; }
        public int idccaa { get; set; }
        public int idprovincia { get; set; }
        public string ciudad { get; set; }
        public string coordgps { get; set; }
        public int ctrlglobal { get; set; }
        public int iddelegacion { get; set; }
        public int asist { get; set; }

        public EventsJson(EVENTOS evento)
        {
            this.id = evento.id;
            this.cidevento = evento.cidevento;
            this.estado = (int)evento.estado;
            this.titulo = evento.titulo;
            this.intro = evento.intro;
            this.descripcion = evento.descripcion;
            this.fechainicio = evento.fechainicio.ToString();
            this.horainicio = evento.horainicio.ToString();
            this.fechafin = evento.fechafin.ToString();
            this.horafin = evento.horafin.ToString();
            this.notasevento = evento.notasevento;
            this.notastransporte = evento.notastransporte;
            this.idccaa = (int)evento.idccaa;
            this.idprovincia = (int)evento.idprovincia;
            this.ciudad = evento.ciudad;
            this.coordgps = evento.coordgps;
            this.ctrlglobal = (int)evento.ctrlglobal;
            this.iddelegacion = (int)evento.iddelegacion;
            this.asist = 0;
        }

    }
}