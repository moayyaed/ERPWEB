using Core.Erp.Info.Contabilidad;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Contabilidad
{
    public class ct_grupo_x_Tipo_Gasto_Data
    {
        public List<ct_grupo_x_Tipo_Gasto_Info> get_list(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                List<ct_grupo_x_Tipo_Gasto_Info> Lista;

                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    Lista = Context.ct_grupo_x_Tipo_Gasto.Where(q => q.IdEmpresa == IdEmpresa && q.estado == (mostrar_anulados == true ? q.estado : true)).Select(q => new ct_grupo_x_Tipo_Gasto_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdTipo_Gasto = q.IdTipo_Gasto,
                        IdTipo_Gasto_Padre = q.IdTipo_Gasto_Padre,
                        nom_tipo_Gasto = q.nom_tipo_Gasto,
                        estado = q.estado,
                        nivel =q.nivel,
                        orden =q.orden
                    }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ct_grupo_x_Tipo_Gasto_Info get_info(int IdEmpresa, int IdTipo_Gasto)
        {
            try
            {
                ct_grupo_x_Tipo_Gasto_Info info = new ct_grupo_x_Tipo_Gasto_Info();

                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_grupo_x_Tipo_Gasto Entity = Context.ct_grupo_x_Tipo_Gasto.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdTipo_Gasto == IdTipo_Gasto);
                    if (Entity == null) return null;

                    info = new ct_grupo_x_Tipo_Gasto_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdTipo_Gasto = Entity.IdTipo_Gasto,
                        IdTipo_Gasto_Padre = Entity.IdTipo_Gasto_Padre,
                        nom_tipo_Gasto = Entity.nom_tipo_Gasto,
                        estado = Entity.estado,
                        nivel = Entity.nivel,
                        orden = Entity.orden
                    };
                }

                return info;
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
                    var lst = from q in Context.ct_grupo_x_Tipo_Gasto
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdTipo_Gasto) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(ct_grupo_x_Tipo_Gasto_Info info)
        {
            try
            {
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_grupo_x_Tipo_Gasto Entity = new ct_grupo_x_Tipo_Gasto
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdTipo_Gasto = info.IdTipo_Gasto= get_id(info.IdEmpresa),
                        IdTipo_Gasto_Padre = info.IdTipo_Gasto_Padre,
                        nom_tipo_Gasto = info.nom_tipo_Gasto,
                        estado = true,
                        nivel = info.nivel,
                        orden = info.orden
                    };
                    Context.ct_grupo_x_Tipo_Gasto.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(ct_grupo_x_Tipo_Gasto_Info info)
        {
            try
            {
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_grupo_x_Tipo_Gasto Entity = Context.ct_grupo_x_Tipo_Gasto.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdTipo_Gasto == info.IdTipo_Gasto);
                    if (Entity == null)
                        return false;
                    Entity.nom_tipo_Gasto = info.nom_tipo_Gasto;
                    Entity.IdTipo_Gasto_Padre = info.IdTipo_Gasto_Padre;
                    Entity.nivel = info.nivel;
                    Entity.orden = info.orden;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB(ct_grupo_x_Tipo_Gasto_Info info)
        {
            try
            {
                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_grupo_x_Tipo_Gasto Entity = Context.ct_grupo_x_Tipo_Gasto.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdTipo_Gasto == info.IdTipo_Gasto);
                    if (Entity == null)
                        return false;
                    Entity.estado = false;

                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ct_grupo_x_Tipo_Gasto_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args, int IdEmpresa)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            List<ct_grupo_x_Tipo_Gasto_Info> Lista = new List<ct_grupo_x_Tipo_Gasto_Info>();
            Lista = get_list(IdEmpresa, skip, take, args.Filter);
            return Lista;
        }

        public List<ct_grupo_x_Tipo_Gasto_Info> get_list(int IdEmpresa, int skip, int take, string filter)
        {
            try
            {
                List<ct_grupo_x_Tipo_Gasto_Info> Lista = new List<ct_grupo_x_Tipo_Gasto_Info>();

                Entities_contabilidad context = new Entities_contabilidad();
                {
                    List<ct_grupo_x_Tipo_Gasto> lstg;
                    lstg = context.ct_grupo_x_Tipo_Gasto.Include("ct_grupo_x_Tipo_Gasto2").Where(q => q.IdEmpresa == IdEmpresa && q.estado == true && (q.IdTipo_Gasto + " " + q.nom_tipo_Gasto).Contains(filter)).OrderBy(q => q.IdTipo_Gasto).Skip(skip).Take(take).ToList();
                    foreach (var q in lstg)
                    {
                        Lista.Add(new ct_grupo_x_Tipo_Gasto_Info
                        {
                            IdTipo_Gasto = q.IdTipo_Gasto,
                            nom_tipo_Gasto = q.nom_tipo_Gasto,
                            IdTipo_Gasto_Padre = q.IdTipo_Gasto_Padre,
                            nom_tipo_Gasto_Padre = q.ct_grupo_x_Tipo_Gasto2 == null ? null : q.ct_grupo_x_Tipo_Gasto2.nom_tipo_Gasto
                        });
                    }
                }

                context.Dispose();
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ct_grupo_x_Tipo_Gasto_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args, int IdEmpresa)
        {
            //La variable args del devexpress ya trae el ID seleccionado en la propiedad Value, se pasa el IdEmpresa porque es un filtro que no tiene
            return get_info(IdEmpresa, args.Value == null ? 0 : Convert.ToInt32(args.Value));
        }

        public ct_grupo_x_Tipo_Gasto_Info get_info_nuevo(int IdEmpresa, int IdTipoGasto_padre)
        {
            try
            {
                ct_grupo_x_Tipo_Gasto_Info info = new ct_grupo_x_Tipo_Gasto_Info { nivel=0};
                int ID = IdTipoGasto_padre;

                using (Entities_contabilidad Context = new Entities_contabilidad())
                {
                    ct_grupo_x_Tipo_Gasto Entity_padre = Context.ct_grupo_x_Tipo_Gasto.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdTipo_Gasto == IdTipoGasto_padre);
                    if (Entity_padre == null) return info;

                    var lst = from q in Context.ct_grupo_x_Tipo_Gasto
                              where q.IdTipo_Gasto_Padre == IdTipoGasto_padre
                              && q.IdEmpresa == IdEmpresa
                              select q;

                    info = new ct_grupo_x_Tipo_Gasto_Info
                    {
                        IdTipo_Gasto = ID,
                        nom_tipo_Gasto = Entity_padre.nom_tipo_Gasto,
                        nivel = (Entity_padre.nivel == null ? 0 : Entity_padre.nivel+1)
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
