using Core.Erp.Info.Inventario;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Inventario
{
    public class in_UnidadMedida_Data
    {
        public List<in_UnidadMedida_Info> get_list(bool mostrar_anulados)
        {
            try
            {
                List<in_UnidadMedida_Info> Lista = new List<in_UnidadMedida_Info>();

                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();
                    string query = "select IdUnidadMedida, cod_alterno, Descripcion, Estado from in_UnidadMedida where Estado = "+(mostrar_anulados ? "Estado" : "'A'");
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new in_UnidadMedida_Info
                        {
                            IdUnidadMedida = Convert.ToString(reader["IdUnidadMedida"]),
                            cod_alterno = Convert.ToString(reader["cod_alterno"]),
                            Descripcion = Convert.ToString(reader["Descripcion"]),
                            Estado = Convert.ToString(reader["Estado"])
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

        public in_UnidadMedida_Info get_info(string IdUnidadMedida)
        {
            try
            {
                in_UnidadMedida_Info info = new in_UnidadMedida_Info();

                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_UnidadMedida Entity = Context.in_UnidadMedida.FirstOrDefault(q => q.IdUnidadMedida == IdUnidadMedida);
                    if (Entity == null) return null;
                    info = new in_UnidadMedida_Info
                    {
                        IdUnidadMedida = Entity.IdUnidadMedida,
                        cod_alterno = Entity.cod_alterno,
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

        public bool validar_existe_IdUnidadMedida(string IdUnidadMedida)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    var lst = from q in Context.in_UnidadMedida
                              where q.IdUnidadMedida == IdUnidadMedida
                              select q;

                    if (lst.Count() > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(in_UnidadMedida_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_UnidadMedida Entity = new in_UnidadMedida
                    {
                        IdUnidadMedida = info.IdUnidadMedida,
                        cod_alterno = info.cod_alterno,
                        Descripcion = info.Descripcion,
                        Estado = info.Estado = "A"
                    };
                    Context.in_UnidadMedida.Add(Entity);
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(in_UnidadMedida_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_UnidadMedida Entity = Context.in_UnidadMedida.FirstOrDefault(q => q.IdUnidadMedida == info.IdUnidadMedida);
                    if (Entity == null) return false;
                    Entity.cod_alterno = info.cod_alterno;
                    Entity.Descripcion = info.Descripcion;

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

        public bool anularDB(in_UnidadMedida_Info info)
        {
            try
            {
                using (Entities_inventario Context = new Entities_inventario())
                {
                    in_UnidadMedida Entity = Context.in_UnidadMedida.FirstOrDefault(q => q.IdUnidadMedida == info.IdUnidadMedida);
                    if (Entity == null) return false;
                    Entity.Estado = "I";

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
