using Core.Erp.Info.ActivoFijo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.ActivoFijo
{
    public class Af_Marca_Data
    {
        public List<Af_Marca_Info> get_list(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                List<Af_Marca_Info> Lista;
                using (Entities_activo_fijo Context = new Entities_activo_fijo())
                {
                    Lista = Context.Af_Marca.Where(q => q.IdEmpresa == IdEmpresa && q.Estado == (mostrar_anulados == true ? q.Estado : true)).Select(q => new Af_Marca_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdMarca = q.IdMarca,
                        ma_Descripcion = q.ma_Descripcion,
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

        public Af_Marca_Info get_info(int IdEmpresa, int IdMarca)
        {
            try
            {
                Af_Marca_Info info = new Af_Marca_Info();
                using (Entities_activo_fijo Context = new Entities_activo_fijo())
                {
                    Af_Marca Entity = Context.Af_Marca.FirstOrDefault(q => q.IdMarca == IdMarca);
                    if (Entity == null) return null;
                    info = new Af_Marca_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdMarca = Entity.IdMarca,
                        ma_Descripcion = Entity.ma_Descripcion,
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

        public bool guardarDB(Af_Marca_Info info)
        {
            try
            {
                using (Entities_activo_fijo Context = new Entities_activo_fijo())
                {
                    Af_Marca Entity = new Af_Marca
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdMarca = info.IdMarca = get_id(info.IdEmpresa),
                        ma_Descripcion = info.ma_Descripcion,
                        Estado = true,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = DateTime.Now
                    };

                    Context.Af_Marca.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool modificarDB(Af_Marca_Info info)
        {
            try
            {
                using (Entities_activo_fijo Context = new Entities_activo_fijo())
                {
                    Af_Marca Entity = Context.Af_Marca.FirstOrDefault(q => q.IdMarca == info.IdMarca);
                    if (Entity == null) return false;
                    Entity.ma_Descripcion = info.ma_Descripcion;

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool anularDB(Af_Marca_Info info)
        {
            try
            {
                using (Entities_activo_fijo Context = new Entities_activo_fijo())
                {
                    Af_Marca Entity = Context.Af_Marca.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdMarca == info.IdMarca);
                    if (Entity == null) return false;
                    Entity.Estado = false;

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
                using (Entities_activo_fijo Context = new Entities_activo_fijo())
                {
                    var lst = from q in Context.Af_Marca
                              where q.IdEmpresa == IdEmpresa
                              select q;
                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdMarca) + 1;
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
