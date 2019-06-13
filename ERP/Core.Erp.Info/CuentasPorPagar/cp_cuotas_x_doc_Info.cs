using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.CuentasPorPagar
{
   public class cp_cuotas_x_doc_Info
    {
        public int IdEmpresa { get; set; }
        public decimal IdCuota { get; set; }
        public Nullable<int> IdEmpresa_ct { get; set; }
        public Nullable<int> IdTipoCbte { get; set; }
        public Nullable<decimal> IdCbteCble { get; set; }
        public double Total_a_pagar { get; set; }
        public int Num_cuotas { get; set; }
        public int Dias_plazo { get; set; }
        public System.DateTime Fecha_inicio { get; set; }
        public bool Estado { get; set; }
        public string Observacion { get; set; }

        //public List<cp_cuotas_x_doc_det_Info> lst_cuotas_det { get; set; }

        //public  cp_cuotas_x_doc_Info()
        //{
        //    lst_cuotas_det = new List<cp_cuotas_x_doc_det_Info>();
        //}
    }
}
