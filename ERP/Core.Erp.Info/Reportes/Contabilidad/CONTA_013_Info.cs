using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Contabilidad
{
    public class CONTA_013_Info
    {
        public int IdEmpresa { get; set; }
        public int IdTipoCbte { get; set; }
        public decimal IdCbteCble { get; set; }
        public int secuencia { get; set; }
        public string IdCtaCble { get; set; }
        public string pc_Cuenta { get; set; }
        public double dc_Valor { get; set; }
        public string cb_Observacion { get; set; }
        public Nullable<int> IdPunto_cargo_grupo { get; set; }
        public Nullable<int> IdPunto_cargo { get; set; }
        public string nom_punto_cargo { get; set; }
        public string nom_punto_cargo_grupo { get; set; }
        public System.DateTime cb_Fecha { get; set; }
        public string tc_TipoCbte { get; set; }
        public string TituloGrupo { get; set; }
        public string TituloTotalGrupo { get; set; }
        public string nom_punto_cargo_grupoFiltro { get; set; }
        public string TotalFinal { get; set; }
    }
}
