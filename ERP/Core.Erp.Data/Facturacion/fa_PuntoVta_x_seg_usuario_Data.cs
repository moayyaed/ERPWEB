using Core.Erp.Data.Facturacion.Base;
using Core.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Facturacion
{
    public class fa_PuntoVta_x_seg_usuario_Data
    {
        public List<fa_PuntoVta_x_seg_usuario_Info> get_list(int IdEmpresa, int IdPuntoVta)
        {
            try
            {
                var Lista= new List<fa_PuntoVta_x_seg_usuario_Info>();

                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    var lst = Context.fa_PuntoVta_x_seg_usuario.Where(q=> q.IdEmpresa == IdEmpresa && q.IdPuntoVta == IdPuntoVta).ToList();

                    lst.ForEach(q =>
                    {
                        Lista.Add(new fa_PuntoVta_x_seg_usuario_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdSucursal = q.IdSucursal,
                            Secuencia = q.Secuencia,
                            IdPuntoVta = q.IdPuntoVta,
                            IdUsuario = q.IdUsuario
                        });
                    });
                    return Lista;
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public fa_PuntoVta_x_seg_usuario_Info get_info(int IdEmpresa, int IdPuntoVta, int Secuencia)
        {
            try
            {
                fa_PuntoVta_x_seg_usuario_Info info = new fa_PuntoVta_x_seg_usuario_Info();

                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    fa_PuntoVta_x_seg_usuario Entity = Context.fa_PuntoVta_x_seg_usuario.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdPuntoVta == IdPuntoVta && q.Secuencia == Secuencia);
                    if (Entity == null) return null;
                    info = new fa_PuntoVta_x_seg_usuario_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdPuntoVta = Entity.IdPuntoVta,
                        IdSucursal = Entity.IdSucursal,
                        Secuencia = Entity.Secuencia,
                        IdUsuario = Entity.IdUsuario,
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int get_secuencia(int IdEmpresa, decimal IdPuntoVta)
        {
            try
            {
                int Secuencia = 1;

                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    var lst = from q in Context.fa_PuntoVta_x_seg_usuario
                              where q.IdEmpresa == IdEmpresa
                              && q.IdPuntoVta == IdPuntoVta
                              select q;

                    if (lst.Count() > 0)
                        Secuencia = lst.Max(q => q.Secuencia) + 1;
                }

                return Secuencia;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(fa_PuntoVta_x_seg_usuario_Info info)
        {
            try
            {
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    fa_PuntoVta_x_seg_usuario Entity = new fa_PuntoVta_x_seg_usuario
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdSucursal = info.IdSucursal,
                        IdPuntoVta = info.IdPuntoVta,
                        Secuencia = info.Secuencia,
                        IdUsuario = info.IdUsuario
                    };
                    Context.fa_PuntoVta_x_seg_usuario.Add(Entity);
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
