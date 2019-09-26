using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.Contabilidad
{
    public class CONTA_010_Data
    {
        public List<CONTA_010_Info> get_list(int IdEmpresa, string IdGrupoCble, string IdCtaCble, DateTime fechaini, DateTime fechafin)
        {
            try
            {
                List<CONTA_010_Info> Lista;

                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWCONTA_010
                             where q.IdEmpresa == IdEmpresa
                             && q.IdGrupoCble == IdGrupoCble
                             && q.IdCtaCble == (IdCtaCble=="" ? q.IdCtaCble: IdCtaCble)
                             && q.cb_Fecha >= fechaini
                             && q.cb_Fecha <= fechafin
                             select new CONTA_010_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdTipoCbte = q.IdTipoCbte,
                                 IdCbteCble = q.IdCbteCble,
                                 secuencia = q.secuencia,
                                 cb_Fecha = q.cb_Fecha,
                                 cb_Observacion = q.cb_Observacion,
                                 IdCtaCble = q.IdCtaCble,
                                 pc_Cuenta = q.pc_Cuenta,
                                 gc_GrupoCble = q.gc_GrupoCble,
                                 cc_Descripcion = q.cc_Descripcion,
                                 Debe = q.Debe,
                                 Haber = q.Haber,
                                 tc_TipoCbte = q.tc_TipoCbte,
                                 IdCentroCosto = q.IdCentroCosto,
                                 IdGrupoCble =q.IdGrupoCble
                                 
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
