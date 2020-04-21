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
        public System.DateTime fechainicio { get; set; }
        public System.TimeSpan horainicio { get; set; }
        public int asist { get; set; }
    }
}