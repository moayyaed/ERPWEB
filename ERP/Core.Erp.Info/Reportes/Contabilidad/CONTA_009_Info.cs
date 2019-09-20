using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Contabilidad
{
    public class CONTA_009_Info
    {
        public string IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public string IdCtaCble { get; set; }
        public string IdCentroCosto { get; set; }
        public string pc_Cuenta { get; set; }
        public string IdCtaCblePadre { get; set; }
        public bool EsCtaUtilidad { get; set; }
        public int IdNivelCta { get; set; }
        public string IdGrupoCble { get; set; }
        public string gc_GrupoCble { get; set; }
        public string gc_estado_financiero { get; set; }
        public int gc_Orden { get; set; }
        public decimal SaldoFinal { get; set; }
        public decimal SaldoFinalNaturaleza { get; set; }
        public bool EsCuentaMovimiento { get; set; }
        public string Naturaleza { get; set; }
        public string Su_Descripcion { get; set; }
        public string cc_Descripcion { get; set; }
        public string pc_cuenta_padre { get; set; }
        public string FiltroCC { get; set; }
    }
}
