using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.CuentasPorCobrar
{
    public class CXC_003_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public int IdBodega { get; set; }
        public decimal IdCbteVta { get; set; }
        public string vt_NumFactura { get; set; }
        public string pe_nombreCompleto { get; set; }
        public string pe_cedulaRuc { get; set; }
        public Nullable<System.DateTime> cr_fecha { get; set; }
        public string cr_NumDocumento { get; set; }
        public string IdCobro_tipo { get; set; }
        public string IdMotivo_tipo_cobro { get; set; }
        public Nullable<double> PorcentajeRet { get; set; }
        public decimal Base { get; set; }
        public string ESRetenIVA { get; set; }
        public string ESRetenFTE { get; set; }
        public string cr_EsElectronico { get; set; }
        public string tc_descripcion { get; set; }
        public string TipoRetencion { get; set; }
        public decimal IdCliente { get; set; }
        public System.DateTime vt_fecha { get; set; }
        public double? dc_ValorPago { get; set; }
    }
}
