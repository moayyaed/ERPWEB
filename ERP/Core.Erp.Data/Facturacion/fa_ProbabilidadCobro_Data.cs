using Core.Erp.Data.Facturacion.Base;
using Core.Erp.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Facturacion
{
    public class fa_ProbabilidadCobro_Data
    {
        public List<fa_ProbabilidadCobro_Info> get_list(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                List<fa_ProbabilidadCobro_Info> Lista;

                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    Lista = Context.fa_ProbabilidadCobro.Where(q => q.IdEmpresa == IdEmpresa && q.Estado ==(MostrarAnulados==true ? q.Estado : true)).Select(q => new fa_ProbabilidadCobro_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdProbabilidad = q.IdProbabilidad,
                        Descripcion = q.Descripcion,
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
        public fa_ProbabilidadCobro_Info get_info(int IdEmpresa, int IdProbabilidad)
        {
            try
            {
                fa_ProbabilidadCobro_Info info = new fa_ProbabilidadCobro_Info();

                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    fa_ProbabilidadCobro Entity = Context.fa_ProbabilidadCobro.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdProbabilidad == IdProbabilidad);
                    if (Entity == null) return null;

                    info = new fa_ProbabilidadCobro_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdProbabilidad = Entity.IdProbabilidad,
                        Descripcion = Entity.Descripcion,
                        Estado = Entity.Estado
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(fa_ProbabilidadCobro_Info info)
        {
            try
            {
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    fa_ProbabilidadCobro Entity = new fa_ProbabilidadCobro
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdProbabilidad = info.IdProbabilidad = get_id(info.IdEmpresa),
                        Estado = true,
                        FechaCreacion = DateTime.Now,
                        IdUsuarioCreacion = info.IdUsuarioCreacion
                    };
                    Context.fa_ProbabilidadCobro.Add(Entity);

                    foreach (var item in info.lst_detalle)
                    {
                        fa_ProbabilidadCobroDet Entity_det = new fa_ProbabilidadCobroDet
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdProbabilidad = info.IdProbabilidad,
                            Secuencia = item.Secuencia,
                            IdSucursal = item.IdSucursal,
                            IdBodega = item.IdBodega,
                            IdCbteVta= item.IdCbteVta,
                            vt_tipoDoc = item.vt_tipoDoc
                        };
                        Context.fa_ProbabilidadCobroDet.Add(Entity_det);
                    }
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool modificarDB(fa_ProbabilidadCobro_Info info)
        {
            try
            {
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    fa_ProbabilidadCobro Entity = Context.fa_ProbabilidadCobro.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdProbabilidad == info.IdProbabilidad);
                    if (Entity == null)
                        return false;
                    Entity.Descripcion = info.Descripcion;
                    Entity.FechaModificacion = DateTime.Now;
                    Entity.IdUsuarioModificacion = info.IdUsuarioModificacion;

                    var select = Context.fa_ProbabilidadCobroDet.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdProbabilidad == info.IdProbabilidad);
                    Context.fa_ProbabilidadCobroDet.RemoveRange(select);
                    foreach (var item in info.lst_detalle)
                    {
                        fa_ProbabilidadCobroDet Entity_det = new fa_ProbabilidadCobroDet
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdProbabilidad = info.IdProbabilidad,
                            Secuencia = item.Secuencia,
                            IdSucursal = item.IdSucursal,
                            IdBodega = item.IdBodega,
                            IdCbteVta = item.IdCbteVta,
                            vt_tipoDoc = item.vt_tipoDoc
                        };
                        Context.fa_ProbabilidadCobroDet.Add(Entity_det);
                    }
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB(fa_ProbabilidadCobro_Info info)
        {
            try
            {
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    fa_ProbabilidadCobro Entity = Context.fa_ProbabilidadCobro.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdProbabilidad == info.IdProbabilidad);
                    if (Entity == null)
                        return false;
                    Entity.Estado = false;
                    Entity.FechaAnulacion = DateTime.Now;
                    Entity.IdUsuarioAnulacion = info.IdUsuarioAnulacion;

                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int get_id(int IdEmpresa)
        {
            try
            {
                int ID = 1;
                using (Entities_facturacion Context = new Entities_facturacion())
                {
                    var lst = from q in Context.fa_ProbabilidadCobro
                              where q.IdEmpresa == IdEmpresa
                              select q;
                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdProbabilidad) + 1;
                }
                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
