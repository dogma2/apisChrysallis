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
    
    public partial class DSKTUSERS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DSKTUSERS()
        {
            this.DATOSINTERES = new HashSet<DATOSINTERES>();
            this.DOCUMENTOS = new HashSet<DOCUMENTOS>();
            this.EVENTOS = new HashSet<EVENTOS>();
        }
    
        public int id { get; set; }
        public Nullable<byte> estado { get; set; }
        public string nickname { get; set; }
        public string password { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public Nullable<byte> idprovincia { get; set; }
        public Nullable<byte> idccaa { get; set; }
        public int iddelegacion { get; set; }
        public Nullable<byte> ctrlmaster { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DATOSINTERES> DATOSINTERES { get; set; }
        public virtual DELEGACIONES DELEGACIONES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTOS> DOCUMENTOS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVENTOS> EVENTOS { get; set; }
    }
}
