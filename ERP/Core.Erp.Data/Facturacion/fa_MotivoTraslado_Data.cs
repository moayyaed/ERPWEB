using Core.Erp.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Facturacion
{
    public class fa_MotivoTraslado_Data
    {
        public List<fa_MotivoTraslado_Info> get_list(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                List<fa_MotivoTraslado_Info> Lista;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    if (mostrar_anulados)
                        Lista = (from q in Context.fa_MotivoTraslado
                                 where q.IdEmpresa == IdEmpresa
                                 select new fa_MotivoTraslado_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdMotivoTraslado = q.IdMotivoTraslado,
                                     tr_Descripcion = q.tr_Descripcion,
                                     Estado = q.Estado
                                 }).ToList();
                    else
                        Lista = (from q in Context.fa_MotivoTraslado
                                 where q.IdEmpresa == IdEmpresa
                                 && q.Estado == true
                                 select new fa_MotivoTraslado_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdMotivoTraslado = q.IdMotivoTraslado,
                                     tr_Descripcion = q.tr_Descripcion,
                                     Estado = q.Estado
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
