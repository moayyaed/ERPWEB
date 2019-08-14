using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.Contabilidad;
using DevExpress.Web;

namespace Core.Erp.Data.Contabilidad
{
   public class ct_punto_cargo_Data
    {
        public List<ct_punto_cargo_Info> GetList(int IdEmpresa,int IdPunto_cargo_grupo, bool mostrar_anulados, bool NoMostrarTodos)
        {
            try
            {
                int IdPunto_cargo_grupo_ini = IdPunto_cargo_grupo;
                int IdPunto_cargo_grupo_fin = IdPunto_cargo_grupo == 0 ? (NoMostrarTodos ? 0 : 999999) : IdPunto_cargo_grupo;

                List<ct_punto_cargo_Info> Lista;
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                        Lista = (from q in Context.ct_punto_cargo
                                 where q.IdEmpresa == IdEmpresa
                                 && q.IdPunto_cargo_grupo >= IdPunto_cargo_grupo_ini
                                 && q.IdPunto_cargo_grupo <= IdPunto_cargo_grupo_fin
                                 &&  q.Estado == (mostrar_anulados ? q.Estado : true)
                                 select new ct_punto_cargo_Info
                                 {
                                     IdEmpresa = q.IdEmpresa,
                                     IdPunto_cargo = q.IdPunto_cargo,
                                     IdPunto_cargo_grupo = q.IdPunto_cargo_grupo,
                                     cod_punto_cargo = q.cod_punto_cargo,
                                     Estado = q.Estado,
                                     nom_punto_cargo = q.nom_punto_cargo
                                 }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ct_punto_cargo_Info GetInfo(int IdEmpresa, int IdPunto_cargo)
        {
            try
            {
                ct_punto_cargo_Info info = new ct_punto_cargo_Info();
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_punto_cargo Entity = Context.ct_punto_cargo.Where(q => q.IdEmpresa == IdEmpresa && q.IdPunto_cargo== IdPunto_cargo).FirstOrDefault();
                    if (Entity == null) return null;
                    info = new ct_punto_cargo_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdPunto_cargo_grupo = Entity.IdPunto_cargo_grupo,
                        cod_punto_cargo = Entity.cod_punto_cargo,
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

        public int GetId(int IdEmpresa)
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
        
        public bool GuardarDB(ct_punto_cargo_Info info)
        {
            try
            {
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    Context.ct_punto_cargo.Add(new ct_punto_cargo
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdPunto_cargo = info.IdPunto_cargo = GetId(info.IdEmpresa),
                        IdPunto_cargo_grupo = info.IdPunto_cargo_grupo,
                        cod_punto_cargo = info.cod_punto_cargo,
                        nom_punto_cargo = info.nom_punto_cargo,
                        Estado = true,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = DateTime.Now
                    });
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ModificarDB(ct_punto_cargo_Info info)
        {
            try
            {
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_punto_cargo Entity = Context.ct_punto_cargo.Where(q => q.IdEmpresa == info.IdEmpresa&& q.IdPunto_cargo_grupo == info.IdPunto_cargo_grupo && q.IdPunto_cargo == info.IdPunto_cargo).FirstOrDefault();
                    if (Entity == null)
                        return false;
                    Entity.nom_punto_cargo = info.nom_punto_cargo;
                    Entity.cod_punto_cargo = info.cod_punto_cargo;
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

        public bool AnularDB(ct_punto_cargo_Info info)
        {
            try
            {
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_punto_cargo Entity = Context.ct_punto_cargo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdPunto_cargo == info.IdPunto_cargo);
                    if (Entity == null)
                        return false;
                    Entity.Estado = info.Estado = false;
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

        public List<ct_punto_cargo_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args, int IdEmpresa, int IdPuntoCargoGrupo)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            List<ct_punto_cargo_Info> Lista = new List<ct_punto_cargo_Info>();
            Lista = get_list(IdEmpresa, skip, take, args.Filter, IdPuntoCargoGrupo);
            return Lista;
        }

        public List<ct_punto_cargo_Info> get_list(int IdEmpresa, int skip, int take, string filter, int IdPuntoCargoGrupo)
        {
            try
            {
                List<ct_punto_cargo_Info> Lista;
                using (Entities_contabilidad context_g = new Entities_contabilidad())
                {
                    Lista = context_g.ct_punto_cargo.Where(q => q.IdEmpresa == IdEmpresa && q.Estado == true && (q.IdPunto_cargo + " " + q.nom_punto_cargo).Contains(filter)).OrderBy(q => q.IdPunto_cargo).Skip(skip).Take(take).Select(q=> new ct_punto_cargo_Info
                    {
                        IdPunto_cargo = q.IdPunto_cargo,
                        IdPunto_cargo_grupo = q.IdPunto_cargo_grupo,
                        nom_punto_cargo = q.nom_punto_cargo,
                    }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ct_punto_cargo_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args, int IdEmpresa)
        {
            //La variable args del devexpress ya trae el ID seleccionado en la propiedad Value, se pasa el IdEmpresa porque es un filtro que no tiene
            return GetInfo(IdEmpresa, args.Value == null ? 0 : Convert.ToInt32(args.Value.ToString()) );
        }
    }
}
