using Core.Erp.Info.ActivoFijo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.ActivoFijo
{
    public class Af_Modelo_Data
    {
        public List<Af_Modelo_Info> get_list(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                List<Af_Modelo_Info> Lista;
                using (Entities_activo_fijo Context = new Entities_activo_fijo())
                {
                    Lista = Context.Af_Modelo.Where(q=> q.IdEmpresa == IdEmpresa && q.Estado == (mostrar_anulados == true ? q.Estado : true)).Select(q=> new Af_Modelo_Info {
                        IdEmpresa = q.IdEmpresa,
                        IdModelo = q.IdModelo,
                        mo_Descripcion = q.mo_Descripcion,
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

        public Af_Modelo_Info get_info(int IdEmpresa, int IdModelo)
        {
            try
            {
                Af_Modelo_Info info = new Af_Modelo_Info();
                using (Entities_activo_fijo Context = new Entities_activo_fijo())
                {
                    Af_Modelo Entity = Context.Af_Modelo.FirstOrDefault(q => q.IdModelo == IdModelo);
                    if (Entity == null) return null;
                    info = new Af_Modelo_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdModelo = Entity.IdModelo,
                        mo_Descripcion = Entity.mo_Descripcion,
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

        public bool guardarDB(Af_Modelo_Info info)
        {
            try
            {
                using (Entities_activo_fijo Context = new Entities_activo_fijo())
                {
                    Af_Modelo Entity = new Af_Modelo
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdModelo = info.IdModelo = get_id(info.IdEmpresa),
                        mo_Descripcion = info.mo_Descripcion,
                        Estado = true,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = DateTime.Now
                    };

                    Context.Af_Modelo.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool modificarDB(Af_Modelo_Info info)
        {
            try
            {
                using (Entities_activo_fijo Context = new Entities_activo_fijo())
                {
                    Af_Modelo Entity = Context.Af_Modelo.FirstOrDefault(q => q.IdModelo == info.IdModelo);
                    if (Entity == null) return false;
                    Entity.mo_Descripcion = info.mo_Descripcion;
                    
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool anularDB(Af_Modelo_Info info)
        {
            try
            {
                using (Entities_activo_fijo Context = new Entities_activo_fijo())
                {
                    Af_Modelo Entity = Context.Af_Modelo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdModelo == info.IdModelo);
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
                    var lst = from q in Context.Af_Modelo
                              where q.IdEmpresa == IdEmpresa
                              select q;
                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdModelo) + 1;
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
