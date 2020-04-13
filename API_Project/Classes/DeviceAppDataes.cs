using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Project.Classes
{
    public class DeviceAppDataes
    {
        public int m_GPS { get; set; }
        public int m_Sound { get; set; }
        public int m_Lang { get; set; }
        public int m_Update { get; set; }
        public int m_LongAlertAct { get; set; }
        public int m_LongAlert { get; set; }
        public int m_ShortAlertAct { get; set; }
        public int m_ShortAlert { get; set; }
        public int m_AlertFromHH { get; set; }
        public int m_AlertFromMM { get; set; }
        public int m_AlertToHH { get; set; }
        public int m_AlertToMM { get; set; }
        public int[] m_Delegaciones { get; set; }
        public byte[] m_idCCAAs { get; set; }
        public byte[] m_idProvincias { get; set; }
    }
}