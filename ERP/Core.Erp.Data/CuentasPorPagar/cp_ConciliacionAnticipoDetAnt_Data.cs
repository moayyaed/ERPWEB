using Core.Erp.Info.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.CuentasPorPagar
{
    public class cp_ConciliacionAnticipoDetAnt_Data
    {
        public List<cp_ConciliacionAnticipoDetAnt_Info> getlist(int IdEmpresa, int IdConciliacion)
        {
            try
            {
                List<cp_ConciliacionAnticipoDetAnt_Info> Lista = new List<cp_ConciliacionAnticipoDetAnt_Info>();

                using (Entities_cuentas_por_pagar db = new Entities_cuentas_por_pagar())
                {
                    Lista = (from q in db.cp_ConciliacionAnticipoDetAnt
                             where q.IdEmpresa == IdEmpresa
                             && q.IdConciliacion == IdConciliacion
                             select new cp_ConciliacionAnticipoDetAnt_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdConciliacion = q.IdConciliacion,
                                 Secuencia = q.Secuencia,
                                 IdOrdenPago = q.IdOrdenPago,
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
