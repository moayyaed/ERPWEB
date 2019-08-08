using Core.Erp.Info.Reportes.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.Banco
{
    public class BAN_002_cancelaciones_Data
    {
        public List<BAN_002_cancelaciones_Info> get_list(int mba_IdEmpresa, int mba_IdTipocbte, decimal mba_IdCbteCble)
        {
            try
            {
                List<BAN_002_cancelaciones_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWBAN_002_cancelaciones
                             where q.mba_IdEmpresa == mba_IdEmpresa
                             && q.mba_IdTipocbte == mba_IdTipocbte
                             && q.mba_IdCbteCble == mba_IdCbteCble
                             select new BAN_002_cancelaciones_Info
                             {
                                 mba_IdTipocbte = q.mba_IdTipocbte,
                                 mba_IdCbteCble = q.mba_IdCbteCble,
                                 mba_IdEmpresa = q.mba_IdEmpresa,
                                 mcj_IdEmpresa = q.mcj_IdEmpresa,
                                 cm_observacion = q.cm_observacion,
                                 mcj_IdCbteCble = q.mcj_IdCbteCble,
                                 mcj_IdTipocbte = q.mcj_IdTipocbte,
                                 cm_fecha = q.cm_fecha,
                                 cm_valor = q.cm_valor,
                                 cbr_IdCobro = q.cbr_IdCobro,
                                 ct_IdCbteCble = q.ct_IdCbteCble
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
