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
    
    public partial class PROVINCIAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROVINCIAS()
        {
            this.DATOSINTERES = new HashSet<DATOSINTERES>();
            this.EVENTOS = new HashSet<EVENTOS>();
        }
    
        public byte id { get; set; }
        public string nombre { get; set; }
        public Nullable<byte> idccaa { get; set; }
    
        public virtual CCAA CCAA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DATOSINTERES> DATOSINTERES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVENTOS> EVENTOS { get; set; }
    }
}
