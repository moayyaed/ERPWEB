//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Core.Erp.Data.Reportes.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class VWFAC_012
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public int IdBodega { get; set; }
        public decimal IdCambio { get; set; }
        public int Secuencia { get; set; }
        public decimal IdCbteVta { get; set; }
        public int SecuenciaFact { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public bool Estado { get; set; }
        public Nullable<int> IdMovi_inven_tipo { get; set; }
        public Nullable<decimal> IdNumMovi { get; set; }
        public string pr_descripcionFact { get; set; }
        public string pr_descripcionCambio { get; set; }
        public double CantidadFact { get; set; }
        public double CantidadCambio { get; set; }
        public string vt_NumFactura { get; set; }
        public string NombreCliente { get; set; }
        public string Su_Descripcion { get; set; }
        public string bo_Descripcion { get; set; }
    }
}
