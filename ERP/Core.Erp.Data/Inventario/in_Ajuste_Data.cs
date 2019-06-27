using Core.Erp.Info.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Inventario
{
    public class in_Ajuste_Data
    {
        public List<in_Ajuste_Info> get_list(int IdEmpresa, int IdSucursal, bool mostrar_anulados, DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                List<in_Ajuste_Info> Lista = new List<in_Ajuste_Info>();

                using (Entities_inventario db = new Entities_inventario())
                {
                    if (mostrar_anulados == false)
                    {
                        Lista = db.vwin_Ajuste.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.Estado == true && q.Fecha >= fecha_ini && q.Fecha <= fecha_fin).Select(q => new in_Ajuste_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdSucursal = q.IdSucursal,
                            IdAjuste = q.IdAjuste,
                            IdBodega = q.IdBodega,
                            IdMovi_inven_tipo_ing = q.IdMovi_inven_tipo_ing,
                            IdMovi_inven_tipo_egr = q.IdMovi_inven_tipo_egr,
                            IdNumMovi_ing = q.IdNumMovi_ing,
                            IdNumMovi_egr = q.IdNumMovi_egr,
                            IdCatalogo_Estado = q.IdCatalogo_Estado,
                            Fecha = q.Fecha,
                            NombreEstado = q.NombreEstado,
                            Su_Descripcion = q.Su_Descripcion,
                            bo_Descripcion = q.bo_Descripcion,
                            Observacion = q.Observacion,
                            Estado = q.Estado
                        }).ToList();
                    }
                    else
                    {
                        Lista = db.vwin_Ajuste.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.Fecha >= fecha_ini && q.Fecha <= fecha_fin).Select(q => new in_Ajuste_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdSucursal = q.IdSucursal,
                            IdAjuste = q.IdAjuste,
                            IdBodega = q.IdBodega,
                            IdMovi_inven_tipo_ing = q.IdMovi_inven_tipo_ing,
                            IdMovi_inven_tipo_egr = q.IdMovi_inven_tipo_egr,
                            IdNumMovi_ing = q.IdNumMovi_ing,
                            IdNumMovi_egr = q.IdNumMovi_egr,
                            IdCatalogo_Estado = q.IdCatalogo_Estado,
                            Fecha = q.Fecha,
                            NombreEstado = q.NombreEstado,
                            Su_Descripcion = q.Su_Descripcion,
                            bo_Descripcion = q.bo_Descripcion,
                            Observacion = q.Observacion,
                            Estado = q.Estado
                        }).ToList();
                    }
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public in_Ajuste_Info get_info(int IdEmpresa, decimal IdAjuste)
        {
            try
            {
                in_Ajuste_Info info = new in_Ajuste_Info();
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_Ajuste Entity = Context.in_Ajuste.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdAjuste == IdAjuste);
                    if (Entity == null) return null;
                    info = new in_Ajuste_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdSucursal = Entity.IdSucursal,
                        IdAjuste = Entity.IdAjuste,
                        IdBodega = Entity.IdBodega,
                        IdMovi_inven_tipo_ing = Entity.IdMovi_inven_tipo_ing,
                        IdMovi_inven_tipo_egr = Entity.IdMovi_inven_tipo_egr,
                        IdNumMovi_ing = Entity.IdNumMovi_ing,
                        IdNumMovi_egr = Entity.IdNumMovi_egr,
                        IdCatalogo_Estado = Entity.IdCatalogo_Estado,
                        Fecha = Entity.Fecha,
                        Observacion = Entity.Observacion,
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

        private decimal get_id(int IdEmpresa)
        {

            try
            {
                decimal ID = 1;

                using (Entities_inventario Context = new Entities_inventario())
                {
                    var lst = from q in Context.in_Ajuste
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdAjuste) + 1;

                    return ID;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(in_Ajuste_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_Ajuste Entity = new in_Ajuste
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdSucursal = info.IdSucursal,
                        IdAjuste = info.IdAjuste,
                        IdBodega = info.IdBodega,
                        IdMovi_inven_tipo_ing = info.IdMovi_inven_tipo_ing,
                        IdMovi_inven_tipo_egr = info.IdMovi_inven_tipo_egr,
                        IdNumMovi_ing = info.IdNumMovi_ing,
                        IdNumMovi_egr = info.IdNumMovi_egr,
                        IdCatalogo_Estado = info.IdCatalogo_Estado,
                        Fecha = info.Fecha,
                        Observacion = info.Observacion,
                        Estado = info.Estado,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = DateTime.Now
                        
                    };
                    Context.in_Ajuste.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool modificarDB(in_Ajuste_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_Ajuste Entity = Context.in_Ajuste.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdAjuste == info.IdAjuste);
                    if (Entity == null)
                        return false;

                    Entity.IdEmpresa = info.IdEmpresa;
                    Entity.IdSucursal = info.IdSucursal;
                    Entity.IdAjuste = info.IdAjuste;
                    Entity.IdBodega = info.IdBodega;
                    Entity.IdMovi_inven_tipo_ing = info.IdMovi_inven_tipo_ing;
                    Entity.IdMovi_inven_tipo_egr = info.IdMovi_inven_tipo_egr;
                    Entity.IdNumMovi_ing = info.IdNumMovi_ing;
                    Entity.IdNumMovi_egr = info.IdNumMovi_egr;
                    Entity.IdCatalogo_Estado = info.IdCatalogo_Estado;
                    Entity.Fecha = info.Fecha;
                    Entity.Observacion = info.Observacion;
                    Entity.Estado = info.Estado;
                    Entity.IdUsuarioModificacion = info.IdUsuarioModificacion;
                    Entity.FechaModificacion = DateTime.Now;

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool anularDB(in_Ajuste_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_Ajuste Entity = Context.in_Ajuste.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdAjuste == info.IdAjuste);
                    if (Entity == null)
                        return false;

                    Entity.Estado = info.Estado;

                    Entity.IdUsuarioAnulacion = info.IdUsuarioAnulacion;
                    Entity.FechaAnulacion = DateTime.Now;
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
