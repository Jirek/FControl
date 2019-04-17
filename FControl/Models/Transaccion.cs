//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FControl.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaccion
    {
        public int TransaccionID { get; set; }
        public Nullable<int> CuentaID { get; set; }
        public Nullable<int> DivisaID { get; set; }
        public Nullable<double> Importe { get; set; }
        public string Concepto { get; set; }
        public Nullable<System.DateTime> FechaTransaccion { get; set; }
        public Nullable<int> BenefeciarioID { get; set; }
        public Nullable<int> UsuarioID { get; set; }
        public Nullable<double> ImporteOtraDivisa { get; set; }
        public Nullable<int> TipoTransaccion { get; set; }
        public Nullable<int> TraspasoID { get; set; }
    
        public virtual Beneficiario Beneficiario { get; set; }
        public virtual Cuenta Cuenta { get; set; }
        public virtual Divisa Divisa { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Cuenta Cuenta1 { get; set; }
    }
}
