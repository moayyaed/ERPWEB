using Core.Erp.Info.SeguridadAcceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.SeguridadAcceso
{
 public class seg_usuario_x_tb_sucursal_Data
    {
        public List<seg_usuario_x_tb_sucursal_Info> GetList(string IdUsuario)
        {
            try
            {
                List<seg_usuario_x_tb_sucursal_Info> Lista;
                using (Entities_seguridad_acceso Context = new Entities_seguridad_acceso())
                {
                    Lista = Context.vwseg_usuario_x_tb_sucursal.Where(q => q.IdUsuario == IdUsuario).Select(q => new seg_usuario_x_tb_sucursal_Info
                    {
                        IdUsuario  = q.IdUsuario,
                        IdEmpresa = q.IdEmpresa,
                        IdSucursal = q.IdSucursal,
                        Su_Descripcion = q.Su_Descripcion,
                        em_nombre = q.em_nombre
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
