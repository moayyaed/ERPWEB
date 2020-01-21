using Core.Erp.Data.Reportes.Base;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Contabilidad
{
    public class CONTA_013_Data
    {
        public List<CONTA_013_Info> get_list(int IdEmpresa, int IdPunto_cargo_grupo, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                int IdPunto_cargo_grupo_ini = IdPunto_cargo_grupo;
                int IdPunto_cargo_grupo_fin = (IdPunto_cargo_grupo==0 ? 999999 : IdPunto_cargo_grupo);
                List<CONTA_013_Info> Lista = new List<CONTA_013_Info>();
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWCONTA_013
                              where q.IdEmpresa == IdEmpresa
                              && q.IdPunto_cargo_grupo >= IdPunto_cargo_grupo_ini
                              && q.IdPunto_cargo_grupo <= IdPunto_cargo_grupo_fin
                              && q.cb_Fecha >= fechaIni
                              && q.cb_Fecha <= fechaFin
                              select new CONTA_013_Info
                             {
                                  cb_Fecha = q.cb_Fecha,
                                  IdEmpresa = q.IdEmpresa,
                                  cb_Observacion = q.cb_Observacion,
                                  dc_Valor = q.dc_Valor,
                                  IdCbteCble = q.IdCbteCble,
                                  IdCtaCble = q.IdCtaCble,
                                  IdPunto_cargo = q.IdPunto_cargo,
                                  IdPunto_cargo_grupo = q.IdPunto_cargo_grupo,
                                  IdTipoCbte = q.IdTipoCbte,
                                  nom_punto_cargo = q.nom_punto_cargo,
                                  nom_punto_cargo_grupo = q.nom_punto_cargo_grupo,
                                  nom_punto_cargo_grupoFiltro = q.nom_punto_cargo_grupoFiltro,
                                  pc_Cuenta = q.pc_Cuenta,
                                  secuencia = q.secuencia,
                                  tc_TipoCbte = q.tc_TipoCbte,
                                  TituloGrupo = q.TituloGrupo,
                                  TituloTotalGrupo = q.TituloTotalGrupo,
                                  TotalFinal = q.TotalFinal
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
