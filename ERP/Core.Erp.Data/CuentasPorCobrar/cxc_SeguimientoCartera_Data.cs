using Core.Erp.Data;
using Core.Erp.Info.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.CuentasPorCobrar
{
    public class cxc_SeguimientoCartera_Data
    {
        public List<cxc_SeguimientoCartera_Info> getList(int IdEmpresa, decimal IdCliente, bool MostrarAnulados, DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                decimal IdCliente_ini = IdCliente;
                decimal IdCliente_fin = IdCliente == 0 ? 999999 : IdCliente;
                List<cxc_SeguimientoCartera_Info> Lista = new List<cxc_SeguimientoCartera_Info>();

                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT s.IdEmpresa, s.IdSeguimiento, s.IdCliente, p.pe_nombreCompleto, s.CorreoEnviado, s.Fecha, s.Observacion, s.Estado, s.IdUsuarioCreacion "
                    +" FROM dbo.cxc_SeguimientoCartera AS s LEFT OUTER JOIN "
                    + " dbo.fa_cliente AS c ON s.IdEmpresa = c.IdEmpresa AND s.IdCliente = c.IdCliente LEFT OUTER JOIN "
                    + " dbo.tb_persona AS p ON c.IdPersona = p.IdPersona "
                    + " WHERE s.IdEmpresa =" + IdEmpresa.ToString() + " and s.IdCliente between " + IdCliente_ini.ToString() + " and " + IdCliente_fin.ToString()
                    + " and s.Fecha between DATEFROMPARTS(" + fecha_ini.Year.ToString() + "," + fecha_ini.Month.ToString() + "," + fecha_ini.Day.ToString() + ") and DATEFROMPARTS(" + fecha_fin.Year.ToString() + "," + fecha_fin.Month.ToString() + "," + fecha_fin.Day.ToString() + ")"
                    + (MostrarAnulados==true ? "" : " and s.Estado = 1");
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new cxc_SeguimientoCartera_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdSeguimiento = Convert.ToInt32(reader["IdSeguimiento"]),
                            IdCliente = Convert.ToDecimal(reader["IdCliente"]),
                            NombreCliente = string.IsNullOrEmpty(reader["pe_nombreCompleto"].ToString()) ? null : reader["pe_nombreCompleto"].ToString(),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            Observacion = string.IsNullOrEmpty(reader["Observacion"].ToString()) ? null : reader["Observacion"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            CorreoEnviado = Convert.ToBoolean(reader["CorreoEnviado"]),
                            IdUsuarioCreacion = string.IsNullOrEmpty(reader["IdUsuarioCreacion"].ToString()) ? null : reader["IdUsuarioCreacion"].ToString(),
                        });
                    }
                    reader.Close();
                    return Lista;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<cxc_SeguimientoCartera_Info> getList_x_Alumno(int IdEmpresa, decimal IdCliente)
        {
            try
            {
                List<cxc_SeguimientoCartera_Info> Lista = new List<cxc_SeguimientoCartera_Info>();

                using (Entities_cuentas_por_cobrar odata = new Entities_cuentas_por_cobrar())
                {
                    var lst = odata.cxc_SeguimientoCartera.Where(q => q.IdEmpresa == IdEmpresa && q.IdCliente == IdCliente).OrderByDescending(q => q.Fecha).ToList();

                    lst.ForEach(q =>
                    {
                        Lista.Add(new cxc_SeguimientoCartera_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdSeguimiento = q.IdSeguimiento,
                            IdCliente = q.IdCliente,
                            Fecha = q.Fecha,
                            Observacion = q.Observacion,
                            Estado = q.Estado,
                            CorreoEnviado = q.CorreoEnviado
                        });
                    });
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int getId(int IdEmpresa)
        {
            try
            {
                int ID = 1;

                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    var cont = Context.cxc_SeguimientoCartera.Where(q => q.IdEmpresa == IdEmpresa).Count();
                    if (cont > 0)
                        ID = Context.cxc_SeguimientoCartera.Where(q => q.IdEmpresa == IdEmpresa).Max(q => q.IdSeguimiento) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public cxc_SeguimientoCartera_Info get_info(int IdEmpresa, int IdSeguimiento)
        {
            try
            {
                cxc_SeguimientoCartera_Info info = new cxc_SeguimientoCartera_Info();
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    cxc_SeguimientoCartera Entity = Context.cxc_SeguimientoCartera.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdSeguimiento == IdSeguimiento);
                    if (Entity == null) return null;
                    info = new cxc_SeguimientoCartera_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdSeguimiento = Entity.IdSeguimiento,
                        Estado = Entity.Estado,
                        IdCliente = Entity.IdCliente,
                        Fecha = Entity.Fecha,
                        Observacion = Entity.Observacion
                    };
                }
                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool guardarDB(cxc_SeguimientoCartera_Info info)
        {
            try
            {
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    cxc_SeguimientoCartera Entity = new cxc_SeguimientoCartera
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdSeguimiento = info.IdSeguimiento = getId(info.IdEmpresa),
                        IdCliente = info.IdCliente,
                        CorreoEnviado = false,
                        Fecha = info.Fecha,
                        Observacion = info.Observacion,
                        Estado = true,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = info.FechaCreacion = DateTime.Now
                    };
                    Context.cxc_SeguimientoCartera.Add(Entity);

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool anularDB(cxc_SeguimientoCartera_Info info)
        {
            try
            {
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    cxc_SeguimientoCartera Entity = Context.cxc_SeguimientoCartera.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdSeguimiento == info.IdSeguimiento);
                    if (Entity == null)
                        return false;

                    Entity.Estado = info.Estado = false;
                    Entity.MotivoAnulacion = info.MotivoAnulacion;
                    Entity.IdUsuarioAnulacion = info.IdUsuarioAnulacion;
                    Entity.FechaAnulacion = info.FechaAnulacion = DateTime.Now;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool enviarcorreoDB(cxc_SeguimientoCartera_Info info)
        {
            try
            {
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    cxc_SeguimientoCartera Entity = Context.cxc_SeguimientoCartera.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdSeguimiento == info.IdSeguimiento);
                    if (Entity == null)
                        return false;

                    Entity.CorreoEnviado = info.CorreoEnviado = true;
                    Entity.IdUsuarioModificacion = info.IdUsuarioModificacion;
                    Entity.FechaModificacion = info.FechaModificacion = DateTime.Now;

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
