using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Project.Classes
{
    public class AsociationDataes
    {
        public string m_Nombre { get; set; }
        public string m_CIF { get; set; }
        public string m_Telefono { get; set; }
        public string m_Direccion { get; set; }
        public string m_Ciudad { get; set; }
        public string m_CodigoPostal { get; set; }
        public byte m_IdProvincia { get; set; }
        public byte m_IdComunidad { get; set; }
        public string m_Email { get; set; }
        public string m_Web { get; set; }
        public string m_RGPD { get; set; }
        public string m_UserModif { get; set; }
        public int[] m_LastModif { get; set; }
    }
}