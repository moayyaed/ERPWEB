using Core.Erp.Info.Inventario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Inventario
{
    public class in_linea_Data
    {
        public List<in_linea_Info> get_list(int IdEmpresa, string IdCategoria, bool mostrar_anulados)
        {
            try
            {
                List<in_linea_Info> Lista = new List<in_linea_Info>();

                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();
                    string query = "select IdEmpresa,IdCategoria,IdLinea,cod_linea,nom_linea,observacion,Estado, case when Estado = 'A' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END EstadoBool from in_linea where IdEmpresa = " + IdEmpresa.ToString() + " and IdCategoria = '"+IdCategoria+"'  and Estado = " + (mostrar_anulados ? "Estado" : "'A'");
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new in_linea_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdCategoria = Convert.ToString(reader["IdCategoria"]),
                            IdLinea = Convert.ToInt32(reader["IdLinea"]),
                            Estado = Convert.ToString(reader["Estado"]),
                            nom_linea = Convert.ToString(reader["nom_linea"]),
                            observacion = Convert.ToString(reader["observacion"]),
                            cod_linea = Convert.ToString(reader["cod_linea"]),
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

        public in_linea_Info get_info(int IdEmpresa, string IdCategoria, int IdLinea)
        {
            try
            {
                in_linea_Info info = new in_linea_Info();
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_linea Entity = Context.in_linea.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdCategoria == IdCategoria && q.IdLinea == IdLinea);
                    if (Entity == null) return null;
                    info = new in_linea_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdCategoria = Entity.IdCategoria,
                        IdLinea = Entity.IdLinea,
                        cod_linea = Entity.cod_linea,
                        nom_linea = Entity.nom_linea,
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

        public int get_id(int IdEmpresa, string IdCategoria)
        {
            try
            {
                int ID = 1;
                using (Entities_inventario Context = new Entities_inventario())
                {
                    var lst = from q in Context.in_linea
                              where q.IdEmpresa == IdEmpresa
                              && q.IdCategoria == IdCategoria
                              select q;
                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdLinea) + 1;
                }
                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(in_linea_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_linea Entity = new in_linea
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdCategoria = info.IdCategoria,
                        IdLinea = info.IdLinea=get_id(info.IdEmpresa, info.IdCategoria),
                        cod_linea = info.cod_linea,
                        nom_linea = info.nom_linea,
                        Estado = info.Estado="A",

                        IdUsuario = info.IdUsuario,
                        Fecha_Transac = DateTime.Now
                    };
                    Context.in_linea.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool modificarDB(in_linea_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_linea Entity = Context.in_linea.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa  && q.IdCategoria == info.IdCategoria && q.IdLinea == info.IdLinea);
                    if (Entity == null)
                        return false;
                    Entity.nom_linea = info.nom_linea;
                    Entity.cod_linea = info.cod_linea;

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
        public bool anularDB(in_linea_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_linea Entity = Context.in_linea.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdCategoria == info.IdCategoria && q.IdLinea == info.IdLinea);
                    if (Entity == null)
                        return false;
                    Entity.Estado = info.Estado="I";

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
