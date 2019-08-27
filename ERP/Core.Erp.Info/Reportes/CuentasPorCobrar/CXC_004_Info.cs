using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.CuentasPorCobrar
{
    public class CXC_004_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public int IdBodega { get; set; }
        public decimal IdCbteVta { get; set; }
        public string vt_NumFactura { get; set; }
        public string vt_tipoDoc { get; set; }
        public decimal IdCliente { get; set; }
        public System.DateTime vt_fecha { get; set; }
        public System.DateTime vt_fech_venc { get; set; }
        public string vt_Observacion { get; set; }
        public string Estado { get; set; }
        public string pe_nombreCompleto { get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<double> Debe { get; set; }
        public double Haber { get; set; }
        public string Referencia { get; set; }
        public int Orden { get; set; }
        public string Tipo { get; set; }
        public System.DateTime FechaReferencia { get; set; }
        public decimal IdReferencia { get; set; }
        public int SecuenciaReferencia { get; set; }
        public Nullable<int> Dias { get; set; }
        public string Su_Descripcion { get; set; }
        public Nullable<double> Saldo { get; set; }
    }
}
