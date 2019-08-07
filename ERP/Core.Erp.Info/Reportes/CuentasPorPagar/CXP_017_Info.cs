using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.CuentasPorPagar
{
    public class CXP_017_Info
    {
        public int IdEmpresa { get; set; }
        public decimal IdOrdenPago { get; set; }
        public string IdTipo_op { get; set; }
        public string IdTipo_Persona { get; set; }
        public decimal IdPersona { get; set; }
        public decimal IdEntidad { get; set; }
        public System.DateTime Fecha { get; set; }
        public string IdEstadoAprobacion { get; set; }
        public string IdFormaPago { get; set; }
        public string Estado { get; set; }
        public string pe_nombreCompleto { get; set; }
        public Nullable<double> Total_OP { get; set; }
        public double Total_cancelado { get; set; }
        public Nullable<double> Saldo { get; set; }
        public string Observacion { get; set; }
        public Nullable<int> IdTipoFlujo { get; set; }
        public string nom_tipoFlujo { get; set; }
        public string EstadoCancelacion { get; set; }
        public string Descripcion { get; set; }
        public int IdSucursal { get; set; }
        public string Su_Descripcion { get; set; }
    }
}
