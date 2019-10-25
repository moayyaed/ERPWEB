using Core.Erp.Data.Reportes.Base;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Contabilidad
{
    public class CONTA_012_Data
    {
        public List<CONTA_012_Info> get_list(int IdEmpresa, int IdPeriodo)
        {
            try
            {
                List<CONTA_012_Info> Lista = new List<CONTA_012_Info>();
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.SPCONTA_012(IdEmpresa, IdPeriodo, "")
                             select new CONTA_012_Info
                             {
                                IdEmpresa = q.IdEmpresa,
                                dc_Valor = q.dc_Valor,
                                IdCta = q.IdCta,
                                IdTipo_Gasto = q.IdTipo_Gasto,
                                nivel = q.nivel,
                                nom_cuenta = q.nom_cuenta,
                                nom_grupo_CC = q.nom_grupo_CC,
                                nom_tipo_Gasto = q.nom_tipo_Gasto,
                                nom_tipo_Gasto_padre = q.nom_tipo_Gasto_padre,
                                orden = q.orden,
                                orden_tipo_gasto = q.orden_tipo_gasto
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
