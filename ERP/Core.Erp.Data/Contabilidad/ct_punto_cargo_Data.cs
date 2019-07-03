using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.Contabilidad;
namespace Core.Erp.Data.Contabilidad
{
   public class ct_punto_cargo_Data
    {
        public List<ct_punto_cargo_Info> get_list(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                List<ct_punto_cargo_Info> Lista;
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    if (mostrar_anulados == true)
                        Lista = (from q in Context.ct_punto_cargo
                                 where q.IdEmpresa == IdEmpresa
                                 select new ct_punto_cargo_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdPunto_cargo = q.IdPunto_cargo,
                                     IdPunto_cargo_grupo = q.IdPunto_cargo_grupo,
                                     Estado = q.Estado
                                 }).ToList();
                    else
                        Lista = (from q in Context.ct_punto_cargo
                                 where q.IdEmpresa == IdEmpresa
                                 && q.Estado == true
                                 select new ct_punto_cargo_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdPunto_cargo = q.IdPunto_cargo,
                                     IdPunto_cargo_grupo = q.IdPunto_cargo_grupo,
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

        public ct_punto_cargo_Info get_info(int Idempresa, int IdPunto_cargo)
        {
            try
            {
                ct_punto_cargo_Info info = new ct_punto_cargo_Info();
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_punto_cargo Entity = Context.ct_punto_cargo.FirstOrDefault(q => q.IdEmpresa == Idempresa && q.IdPunto_cargo== IdPunto_cargo);
                    if (Entity == null) return null;
                    info = new ct_punto_cargo_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdPunto_cargo_grupo = Entity.IdPunto_cargo_grupo,
                        IdPunto_cargo = Entity.IdPunto_cargo,
                        nom_punto_cargo = Entity.nom_punto_cargo,
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

        public bool guardarDB(ct_punto_cargo_Info info)
        {
            try
            {
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_punto_cargo Entity = new ct_punto_cargo
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdPunto_cargo = info.IdPunto_cargo = get_id(info.IdEmpresa),
                        IdPunto_cargo_grupo = info.IdPunto_cargo_grupo,
                        nom_punto_cargo = info.nom_punto_cargo,
                        Estado = info.Estado = true
                    };
                    Context.ct_punto_cargo.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool modificarDB(ct_punto_cargo_Info info)
        {
            try
            {
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_punto_cargo Entity = Context.ct_punto_cargo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdPunto_cargo == info.IdPunto_cargo);
                    if (Entity == null)
                        return false;
                    Entity.IdPunto_cargo_grupo = info.IdPunto_cargo_grupo;
                    Entity.nom_punto_cargo = info.nom_punto_cargo;

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool anularDB(ct_punto_cargo_Info info)
        {
            try
            {
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_punto_cargo Entity = Context.ct_punto_cargo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdPunto_cargo == info.IdPunto_cargo);
                    if (Entity == null)
                        return false;
                    Entity.Estado = info.Estado = false;

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int get_id(int IdEmpresa)
        {
            try
            {
                int ID = 1;
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    var lst = from q in Context.ct_punto_cargo
                              where q.IdEmpresa == IdEmpresa
                              select q;
                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdPunto_cargo) + 1;
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
