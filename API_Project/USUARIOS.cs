//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API_Project
{
    using System;
    using System.Collections.Generic;
    
    public partial class USUARIOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USUARIOS()
        {
            this.ASISTENTES = new HashSet<ASISTENTES>();
        }
    
        public int id { get; set; }
        public Nullable<byte> estado { get; set; }
        public string cidapp { get; set; }
        public string idsocio { get; set; }
        public string email { get; set; }
        public string imei { get; set; }
        public Nullable<long> fechaestado { get; set; }
        public string notaestado { get; set; }
        public Nullable<int> iddelegacion { get; set; }
        public Nullable<int> iddsktuser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ASISTENTES> ASISTENTES { get; set; }
        public virtual DELEGACIONES DELEGACIONES { get; set; }
        public virtual DSKTUSERS DSKTUSERS { get; set; }
    }
}
