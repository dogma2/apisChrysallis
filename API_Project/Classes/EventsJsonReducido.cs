using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Project.Classes
{
    public class EventsJsonReducido
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string intro { get; set; }
        public string fechainicio { get; set; }
        public string horainicio { get; set; }
        public int asist { get; set; }

        public EventsJsonReducido(EVENTOS evento)
        {
            this.id = evento.id;
            this.titulo = evento.titulo;
            this.intro = evento.intro;
            this.fechainicio = evento.fechainicio.ToString();
            this.horainicio = evento.horainicio.ToString();
            this.asist = 0;
        }

        public static List<EventsJsonReducido> Ev2EJR(List<EVENTOS> eventos)
        {
            List<EventsJsonReducido> retu = new List<EventsJsonReducido>();
            foreach (EVENTOS e in eventos)
            {
                retu.Add(new EventsJsonReducido(e));
            }
            return retu;
        }

    }
}