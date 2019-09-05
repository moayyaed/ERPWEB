using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Contabilidad
{
    public class Dinardap_Info
    {
        public string CodEntidad { get; set; }
        public string FechaDatos { get; set; }
        public string TipoIden { get; set; }
        public string Identificacion { get; set; }
        public string Nom_apellido { get; set; }
        public string clase_suje { get; set; }
        public string Provincia { get; set; }
        public string canton { get; set; }
        public string parroquia { get; set; }
        public string sexo { get; set; }
        public string estado_civil { get; set; }
        public string Origen_Ing { get; set; }
        public string num_operacion { get; set; }
        public decimal valor_ope { get; set; }
        public decimal saldo_ope { get; set; }
        public string fecha_conse { get; set; }
        public string fecha_vct { get; set; }
        public string fecha_exigi { get; set; }
        public decimal Plazo_op { get; set; }
        public decimal Periodicidad_pago { get; set; }
        public decimal dias_morosidad { get; set; }
        public decimal monto_morosidad { get; set; }
        public decimal monto_inte_mora { get; set; }
        public decimal valor_x_vencer_1_30 { get; set; }
        public decimal valor_x_vencer_31_90 { get; set; }
        public decimal valor_x_vencer_91_180 { get; set; }
        public decimal valor_x_vencer_181_360 { get; set; }
        public decimal valor_x_vencer_mas_360 { get; set; }
        public decimal valor_vencido_1_30 { get; set; }
        public decimal valor_vencido_31_90 { get; set; }
        public decimal valor_vencido_91_180 { get; set; }
        public decimal valor_vencido_181_360 { get; set; }
        public decimal valor_vencido_mas_360 { get; set; }
        public decimal valor_en_demand_judi { get; set; }
        public decimal cartera_castigada { get; set; }
        public decimal couta_credito { get; set; }
        public string fecha_cancela { get; set; }
        public string forma_cance { get; set; }
    }
}
