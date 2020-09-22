using Core.Erp.Info.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.CuentasPorPagar
{
    public class cp_codigo_SRI_x_CtaCble_Data
    {
        public cp_codigo_SRI_x_CtaCble_Info get_info(int idCodigo_SRI, int IdEmpresa)
        {
            try
            {
                cp_codigo_SRI_x_CtaCble_Info info = new cp_codigo_SRI_x_CtaCble_Info();
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_codigo_SRI_x_CtaCble Entity = Context.cp_codigo_SRI_x_CtaCble.FirstOrDefault(q => q.idCodigo_SRI == idCodigo_SRI && q.IdEmpresa == IdEmpresa);
                    if (Entity == null) return null;
                    info = new cp_codigo_SRI_x_CtaCble_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        idCodigo_SRI = Entity.idCodigo_SRI,
                        IdCtaCble = Entity.IdCtaCble
                        
                    };
                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(cp_codigo_SRI_x_CtaCble_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_codigo_SRI_x_CtaCble Entity = new cp_codigo_SRI_x_CtaCble
                    {
                        idCodigo_SRI = info.idCodigo_SRI,
                        IdEmpresa = info.IdEmpresa,
                        IdCtaCble = info.IdCtaCble, fecha_UltMod = DateTime.Now                     
                    };
                    Context.cp_codigo_SRI_x_CtaCble.Add(Entity);
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool eliminarDB(int idCodigo_SRI, int IdEmpresa)
        {
            try
            {
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    Context.Database.ExecuteSqlCommand("delete cp_codigo_SRI_x_CtaCble where idCodigo_SRI = "+idCodigo_SRI+"  and IdEmpresa =" +IdEmpresa);
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<cp_codigo_SRI_Info> get_list(int IdEmpresa)
        {
            try
            {

                List<cp_codigo_SRI_Info> lista = new List<cp_codigo_SRI_Info>();

                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();
                    string query = "select a.IdCodigo_SRI, a.codigoSRI, a.co_codigoBase,'['+cast(a.IdCodigo_SRI as varchar)+'] '+ a.co_descripcion AS co_descripcion, a.co_porRetencion, a.co_f_valides_desde,"
                                + " a.co_f_valides_hasta, a.co_estado, a.IdTipoSRI"
                                +" from cp_codigo_SRI as a left joIN"
                                +" [dbo].[cp_codigo_SRI_x_CtaCble] as b on a.IdCodigo_SRI = b.IdCodigo_SRI"
                                +" where a.co_estado = 'A' and a.IdTipoSRI = 'COD_IDCREDITO' "
                                + " ORDER BY codigoSRI desc";
                    SqlCommand command = new SqlCommand(query,connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(new cp_codigo_SRI_Info
                        {
                            IdCodigo_SRI = Convert.ToInt32(reader["IdCodigo_SRI"]),
                            codigoSRI = Convert.ToString(reader["codigoSRI"]),
                            co_codigoBase = Convert.ToString(reader["co_codigoBase"]),
                            co_descripcion = Convert.ToString(reader["co_descripcion"]),
                            co_porRetencion = Convert.ToInt32(reader["co_porRetencion"]),
                            co_f_valides_desde = Convert.ToDateTime(reader["co_f_valides_desde"]),
                            co_f_valides_hasta = Convert.ToDateTime(reader["co_f_valides_hasta"]),
                            co_estado = Convert.ToString(reader["co_estado"]),
                            IdTipoSRI = Convert.ToString(reader["IdTipoSRI"])
                        });
                    }
                    reader.Close();
                }
              
                return (lista);
            }
            catch (Exception ex)
            {
               
                throw new Exception(ex.InnerException.ToString());
            }
        }
    }
}
