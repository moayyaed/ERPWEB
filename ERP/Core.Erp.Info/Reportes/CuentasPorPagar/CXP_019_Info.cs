using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.CuentasPorPagar
{
    public class CXP_019_Info
    {
        public long IdRow { get; set; }
        public int IdEmpresa { get; set; }
        public decimal IdProveedor { get; set; }
        public string nom_proveedor { get; set; }
        public Nullable<double> Valor_a_pagar { get; set; }
        public Nullable<double> Valor_a_pagar1 { get; set; }
        public Nullable<double> MontoAplicado { get; set; }
        public Nullable<double> Saldo { get; set; }
        public string Ruc_Proveedor { get; set; }
        public string representante_legal { get; set; }
        public Nullable<double> x_Vencer { get; set; }
        public Nullable<double> Vencido { get; set; }
        public Nullable<double> Vencido_1_30 { get; set; }
        public Nullable<double> Vencido_31_60 { get; set; }
        public Nullable<double> Vencido_60_90 { get; set; }
        public Nullable<double> Vencido_mayor_90 { get; set; }
        public string Su_Descripcion { get; set; }
        public int IdClaseProveedor { get; set; }
        public string descripcion_clas_prove { get; set; }
        public string EsRelacionado { get; set; }
    }
}
