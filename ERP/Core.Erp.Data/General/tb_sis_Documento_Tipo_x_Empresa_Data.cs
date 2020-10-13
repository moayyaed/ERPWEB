using Core.Erp.Info.General;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.General
{
    public class tb_sis_Documento_Tipo_x_Empresa_Data
    {
        public List<tb_sis_Documento_Tipo_x_Empresa_Info> GetList(int IdEmpresa, bool MostrarNoAsignado)
        {
            try
            {
                List<tb_sis_Documento_Tipo_x_Empresa_Info> Lista = new List<tb_sis_Documento_Tipo_x_Empresa_Info>();
                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();
                    string queryAsignados = "SELECT a.IdEmpresa, a.codDocumentoTipo, a.Posicion, b.descripcion"
                                + " FROM tb_sis_Documento_Tipo_x_Empresa as a inner join"
                                + " tb_sis_Documento_Tipo as b on a.CodDocumentoTipo = b.CodDocumentoTipo"
                                + " WHERE a.IdEmpresa = " + IdEmpresa.ToString()
                                + " order by a.Posicion";
                    SqlCommand commandAsignados = new SqlCommand(queryAsignados,connection);
                    SqlDataReader readerAsignados = commandAsignados.ExecuteReader();
                    while (readerAsignados.Read())
                    {
                        Lista.Add(new tb_sis_Documento_Tipo_x_Empresa_Info
                        {
                            IdEmpresa = Convert.ToInt32(readerAsignados["IdEmpresa"]),
                            codDocumentoTipo = Convert.ToString(readerAsignados["codDocumentoTipo"]),
                            Posicion = Convert.ToInt32(readerAsignados["Posicion"]),
                            Descripcion = Convert.ToString(readerAsignados["descripcion"]),
                            Seleccionado = true
                        });
                    }
                    readerAsignados.Close();

                    if (MostrarNoAsignado)
                    {
                        string queryNoAsignados = "select a.codDocumentoTipo, a.descripcion, a.Posicion"
                                            + " from tb_sis_Documento_Tipo as a"
                                            + " where not exists("
                                            + " SELECT IdEmpresa"
                                            + " FROM tb_sis_Documento_Tipo_x_Empresa as b"
                                            + " where a.codDocumentoTipo = b.codDocumentoTipo"
                                            + " and a.IdEmpresa = " + IdEmpresa.ToString()
                                            + " ) ";
                        SqlCommand commandNoAsignados = new SqlCommand(queryNoAsignados, connection);
                        SqlDataReader readerNoAsignados = commandNoAsignados.ExecuteReader();
                        while (readerNoAsignados.Read())
                        {
                            Lista.Add(new tb_sis_Documento_Tipo_x_Empresa_Info
                            {
                                IdEmpresa = IdEmpresa,
                                codDocumentoTipo = Convert.ToString(readerAsignados["codDocumentoTipo"]),
                                Posicion = Convert.ToInt32(readerAsignados["Posicion"]),
                                Descripcion = Convert.ToString(readerAsignados["descripcion"]),
                                Seleccionado = false
                            });
                        }
                        readerNoAsignados.Close();
                    }
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(tb_sis_Documento_Tipo_x_Empresa_Info info)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();

                    string query = "SELECT * FROM [dbo].[tb_sis_Documento_Tipo_x_Empresa] WHERE IdEmpresa = "+info.IdEmpresa.ToString()+ " and codDocumentoTipo = '"+info.codDocumentoTipo+"'";
                    SqlCommand command = new SqlCommand(query,connection);
                    var ValidateResult = command.ExecuteScalar();
                    if (ValidateResult != null)
                        return true;

                    query = "INSERT INTO [dbo].[tb_sis_Documento_Tipo_x_Empresa]([IdEmpresa],[codDocumentoTipo],[Posicion])"
                                    +" VALUES("+info.IdEmpresa.ToString()+", '"+info.codDocumentoTipo+"', "+info.Posicion.ToString()+")";
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool EliminarDB(int IdEmpresa)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();

                    string query = "DELETE [dbo].[tb_sis_Documento_Tipo_x_Empresa] WHERE IdEmpresa = " + IdEmpresa.ToString();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();                    
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
