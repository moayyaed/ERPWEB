using Core.Erp.Info.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.CuentasPorPagar
{
    public class cp_ConciliacionAnticipoDetCXP_Data
    {
        public List<cp_ConciliacionAnticipoDetCXP_Info> getlist(int IdEmpresa, int IdConciliacion)
        {
            try
            {
                List<cp_ConciliacionAnticipoDetCXP_Info> Lista = new List<cp_ConciliacionAnticipoDetCXP_Info>();

                using (Entities_cuentas_por_pagar db = new Entities_cuentas_por_pagar())
                {
                    Lista = (from q in db.cp_ConciliacionAnticipoDetCXP
                             where q.IdEmpresa == IdEmpresa
                             && q.IdConciliacion == IdConciliacion
                             select new cp_ConciliacionAnticipoDetCXP_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdConciliacion = q.IdConciliacion,
                                 Secuencia = q.Secuencia,
                                 IdOrdenPago = q.IdOrdenPago,
                                 IdEmpresa_cxp = q.IdEmpresa_cxp,
                                 IdTipoCbte_cxp = q.IdTipoCbte_cxp,
                                 IdCbteCble_cxp = q.IdCbteCble_cxp,
                                 MontoAplicado = q.MontoAplicado
                             }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
