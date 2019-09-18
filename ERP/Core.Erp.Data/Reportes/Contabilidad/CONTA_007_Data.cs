using Core.Erp.Data.Reportes.Base;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Contabilidad
{
    public class CONTA_007_Data
    {
        public List<CONTA_007_Info> GetList(int IdEmpresa, string IdUsuario, DateTime fecha_ini, DateTime fecha_fin, bool MostarAcumulado, bool MostarDetalle)
        {
            try
            {
                List<CONTA_007_Info> Lista = null;
                using (Entities_reportes Context = new Entities_reportes())
                {

                    Lista = (from q in Context.SPCONTA_007(IdEmpresa, IdUsuario, fecha_ini, fecha_fin, MostarAcumulado, MostarDetalle)
                             select new CONTA_007_Info
                             {
                                IdEmpresa = q.IdEmpresa,
                                Clasificacion = q.Clasificacion,
                                ClasificacionOrden = q.ClasificacionOrden,
                                IdCtaCble = q.IdCtaCble,
                                pc_cuenta= q.pc_cuenta,
                                Secuencia = q.Secuencia,
                                Tipo = q.Tipo,
                                TipoOrden = q.TipoOrden,
                                Valor = q.Valor,
                                IdUsuario= q.IdUsuario

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
