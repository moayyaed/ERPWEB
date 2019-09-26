using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Contabilidad
{
    public class CONTA_010_Info
    {
        public int IdEmpresa { get; set; }
        public int IdTipoCbte { get; set; }
        public decimal IdCbteCble { get; set; }
        public int secuencia { get; set; }
        public string IdCtaCble { get; set; }
        public System.DateTime cb_Fecha { get; set; }
        public string cb_Observacion { get; set; }
        public string IdCentroCosto { get; set; }
        public string cc_Descripcion { get; set; }
        public string pc_Cuenta { get; set; }
        public string IdGrupoCble { get; set; }
        public string gc_GrupoCble { get; set; }
        public string tc_TipoCbte { get; set; }
        public double Debe { get; set; }
        public Nullable<double> Haber { get; set; }
    }
}
