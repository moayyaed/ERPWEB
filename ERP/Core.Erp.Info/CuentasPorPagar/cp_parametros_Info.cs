using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.CuentasPorPagar
{
   public class cp_parametros_Info
    {
        public int IdEmpresa { get; set; }
        public Nullable<int> pa_TipoCbte_OG { get; set; }
        public string pa_ctacble_iva { get; set; }
        public Nullable<int> pa_IdTipoCbte_x_Retencion { get; set; }
        public string IdUsuario { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> FechaUltMod { get; set; }
        public Nullable<int> pa_TipoCbte_NC { get; set; }
        public Nullable<int> pa_TipoCbte_ND { get; set; }
        public Nullable<int> pa_TipoCbte_para_conci_x_antcipo { get; set; }
        public int DiasTransaccionesAFuturo { get; set; }
        public bool? SeValidaCtaGasto { get; set; }
    }
}
