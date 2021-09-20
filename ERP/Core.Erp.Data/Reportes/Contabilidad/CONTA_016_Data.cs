using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Contabilidad
{
    public class CONTA_016_Data
    {
        public List<CONTA_016_Info> GetList(int IdEmpresa, DateTime FechaDesde, DateTime FechaHasta)
        {
            List<CONTA_016_Info> Lista = new List<CONTA_016_Info>();

            using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                #region Query
                command.CommandText = "declare @IdEmpresa int = " + IdEmpresa.ToString();
                #endregion

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Lista.Add(new CONTA_016_Info
                    {
                        IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                    });
                }
                reader.Close();
            }

            return Lista;
        }
    }
}
