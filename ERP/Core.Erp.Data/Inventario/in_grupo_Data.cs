using Core.Erp.Info.Inventario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Inventario
{
    public class in_grupo_Data
    {
        public List<in_grupo_Info> get_list(int IdEmpresa, string IdCategoria, int IdLinea, bool mostrar_anulados)
        {
            try
            {
                List<in_grupo_Info> Lista = new List<in_grupo_Info>();
                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();
                    string query = "select IdEmpresa,IdCategoria,IdLinea,IdGrupo,nom_grupo,observacion,Estado, case when Estado = 'A' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END EstadoBool from in_grupo where IdEmpresa = " + IdEmpresa.ToString() + " and IdCategoria = '" + IdCategoria + "' and IdLinea = "+IdLinea.ToString()+"  and Estado = " + (mostrar_anulados ? "Estado" : "'A'");
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new in_grupo_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdCategoria = Convert.ToString(reader["IdCategoria"]),
                            IdLinea = Convert.ToInt32(reader["IdLinea"]),
                            IdGrupo = Convert.ToInt32(reader["IdGrupo"]),
                            Estado = Convert.ToString(reader["Estado"]),
                            nom_grupo = Convert.ToString(reader["nom_grupo"]),
                            observacion = Convert.ToString(reader["observacion"]),
                            EstadoBool = Convert.ToBoolean(reader["EstadoBool"]),
                        });
                    }
                    reader.Close();
                }
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public in_grupo_Info get_info(int IdEmpresa, string IdCategoria, int IdLinea, int IdGrupo)
        {
            try
            {
                in_grupo_Info info = new in_grupo_Info();
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_grupo Entity = Context.in_grupo.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdCategoria == IdCategoria && q.IdLinea == IdLinea && q.IdGrupo == IdGrupo);
                    if (Entity == null) return null;
                    info = new in_grupo_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdCategoria = Entity.IdCategoria,
                        IdLinea = Entity.IdLinea,
                        IdGrupo = Entity.IdGrupo,
                        cod_grupo = Entity.cod_grupo,
                        nom_grupo = Entity.nom_grupo,
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

        private int get_id(int IdEmpresa, string IdCategoria, int IdLinea)
        {

            try
            {
                int ID = 1;
                using (Entities_inventario Context = new Entities_inventario())
                {
                    var lst = from q in Context.in_grupo
                              where q.IdEmpresa == IdEmpresa
                              && q.IdCategoria == IdCategoria
                              && q.IdLinea == IdLinea
                              select q;
                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdGrupo) + 1;
                }
                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(in_grupo_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_grupo Entity = new in_grupo
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdCategoria = info.IdCategoria,
                        IdLinea = info.IdLinea,
                        IdGrupo = info.IdGrupo = get_id(info.IdEmpresa, info.IdCategoria, info.IdLinea),
                        cod_grupo = info.cod_grupo,
                        nom_grupo = info.nom_grupo,
                        Estado = info.Estado = "A",

                        IdUsuario = info.IdUsuario,
                        Fecha_Transac = DateTime.Now

                    };
                    Context.in_grupo.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool modificarDB(in_grupo_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_grupo Entity = Context.in_grupo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdCategoria == info.IdCategoria && q.IdLinea == info.IdLinea && q.IdGrupo== info.IdGrupo);
                    if (Entity == null)
                        return false;
                    Entity.cod_grupo = info.cod_grupo;
                    Entity.nom_grupo = info.nom_grupo;

                    Entity.IdUsuarioUltMod = info.IdUsuarioUltMod;
                    Entity.Fecha_UltMod = DateTime.Now;
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool anularDB(in_grupo_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_grupo Entity = Context.in_grupo.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdCategoria == info.IdCategoria && q.IdLinea == info.IdLinea && q.IdGrupo == info.IdGrupo);
                    if (Entity == null)
                        return false;
                    Entity.Estado = info.Estado = "I";

                    Entity.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                    Entity.Fecha_UltAnu = DateTime.Now;
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
