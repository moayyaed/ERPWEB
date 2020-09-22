using Core.Erp.Info.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.CuentasPorPagar
{
    public class cp_proveedor_microempresa_Data
    {
        public cp_proveedor_microempresa_Info GetInfo(string Ruc)
        {
            try
            {
                cp_proveedor_microempresa_Info info = new cp_proveedor_microempresa_Info();

                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();
                    string query = "select Ruc, Nombre from cp_proveedor_microempresa where Ruc ='"+Ruc+"'";
                    SqlCommand command = new SqlCommand(query,connection);
                    var ValidateValue = command.ExecuteScalar();
                    if (ValidateValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        info = new cp_proveedor_microempresa_Info
                        {
                            Ruc = reader["Ruc"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                        };
                    }
                    reader.Close();
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
