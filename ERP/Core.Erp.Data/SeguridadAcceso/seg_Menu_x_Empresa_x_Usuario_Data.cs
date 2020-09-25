using Core.Erp.Info.SeguridadAcceso;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.SeguridadAcceso
{
    public class seg_Menu_x_Empresa_x_Usuario_Data
    {
        public List<seg_Menu_x_Empresa_x_Usuario_Info> get_list(int IdEmpresa, string IdUsuario, bool MostrarTodo)
        {
            try
            {
                List<seg_Menu_x_Empresa_x_Usuario_Info> Lista = new List<seg_Menu_x_Empresa_x_Usuario_Info>();

                using (SqlConnection connection = new SqlConnection(ConexionesERP.GetConnectionString()))
                {
                    connection.Open();

                    #region Menu asignado
                    string query = "select b.IdEmpresa, b.IdMenu, b.Nuevo, b.Modificar, b.Anular, a.DescripcionMenu, a.Habilitado, a.IdMenuPadre, a.PosicionMenu, a.web_nom_Action, a.web_nom_Area, a.web_nom_Controller"
                                + " from seg_Menu as a inner join"
                                + " seg_Menu_x_Empresa_x_Usuario as b on a.IdMenu = b.IdMenu inner join"
                                + " seg_Menu_x_Empresa as c on c.IdMenu = b.IdMenu and c.IdEmpresa = b.IdEmpresa"
                                + " where b.IdEmpresa = " + IdEmpresa.ToString() + " and b.IdUsuario = '" + IdUsuario + "' and a.Habilitado = 1"
                                + " order by a.PosicionMenu";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new seg_Menu_x_Empresa_x_Usuario_Info
                        {
                            seleccionado = true,
                            IdEmpresa = IdEmpresa,
                            IdUsuario = IdUsuario,
                            IdMenu = Convert.ToInt32(reader["IdMenu"]),
                            Nuevo = Convert.ToBoolean(reader["Nuevo"]),
                            Modificar = Convert.ToBoolean(reader["Modificar"]),
                            Anular = Convert.ToBoolean(reader["Anular"]),
                            IdMenuPadre = string.IsNullOrEmpty(reader["IdMenuPadre"].ToString()) ? null : (int?)reader["IdMenuPadre"],
                            DescripcionMenu = Convert.ToString(reader["DescripcionMenu"]),
                            info_menu = new seg_Menu_Info
                            {
                                IdMenu = Convert.ToInt32(reader["IdMenu"]),
                                DescripcionMenu = Convert.ToString(reader["DescripcionMenu"]),
                                IdMenuPadre = string.IsNullOrEmpty(reader["IdMenuPadre"].ToString()) ? null : (int?)reader["IdMenuPadre"],
                                PosicionMenu = Convert.ToInt32(reader["PosicionMenu"]),
                                web_nom_Action = Convert.ToString(reader["web_nom_Action"]),
                                web_nom_Area = Convert.ToString(reader["web_nom_Area"]),
                                web_nom_Controller = Convert.ToString(reader["web_nom_Controller"])
                            }
                        });
                    }
                    reader.Close();
                    #endregion

                    #region Menu no asignado
                    if (MostrarTodo)
                    {
                        string queryAsignados = "select c.IdEmpresa, c.IdMenu,  a.DescripcionMenu, a.Habilitado, a.IdMenuPadre, a.PosicionMenu, a.web_nom_Action, a.web_nom_Area, a.web_nom_Controller"
                                            + " from seg_Menu as a inner join"
                                            + " seg_Menu_x_Empresa as c on c.IdMenu = a.IdMenu"
                                            + " where c.IdEmpresa = " + IdEmpresa.ToString() + " and a.Habilitado = 1 and not exists("
                                            + " select f.IdEmpresa from seg_Menu_x_Empresa_x_Usuario as f where c.IdEmpresa = f.IdEmpresa and c.IdMenu = f.IdMenu and f.IdUsuario = '" + IdUsuario + "') "
                                            + " order by a.PosicionMenu";

                        SqlCommand commandNoAsignado = new SqlCommand(queryAsignados, connection);
                        SqlDataReader readerNA = commandNoAsignado.ExecuteReader();
                        while (readerNA.Read())
                        {
                            Lista.Add(new seg_Menu_x_Empresa_x_Usuario_Info
                            {
                                seleccionado = false,
                                IdEmpresa = IdEmpresa,
                                IdUsuario = IdUsuario,
                                IdMenu = Convert.ToInt32(readerNA["IdMenu"]),
                                Nuevo = true,
                                Modificar = true,
                                Anular = true,
                                IdMenuPadre = string.IsNullOrEmpty(readerNA["IdMenuPadre"].ToString()) ? null : (int?)readerNA["IdMenuPadre"],
                                DescripcionMenu = Convert.ToString(readerNA["DescripcionMenu"]),
                                info_menu = new seg_Menu_Info
                                {
                                    IdMenu = Convert.ToInt32(readerNA["IdMenu"]),
                                    DescripcionMenu = Convert.ToString(readerNA["DescripcionMenu"]),
                                    IdMenuPadre = string.IsNullOrEmpty(readerNA["IdMenuPadre"].ToString()) ? null : (int?)readerNA["IdMenuPadre"],
                                    PosicionMenu = Convert.ToInt32(readerNA["PosicionMenu"]),
                                    web_nom_Action = Convert.ToString(readerNA["web_nom_Action"]),
                                    web_nom_Area = Convert.ToString(readerNA["web_nom_Area"]),
                                    web_nom_Controller = Convert.ToString(readerNA["web_nom_Controller"])
                                }
                            });
                        }
                        readerNA.Close();
                    }
                    #endregion
                }
                
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<seg_Menu_x_Empresa_x_Usuario_Info> get_list(int IdEmpresa, string IdUsuario, int IdMenuPadre)
        {
            try
            {
                List<seg_Menu_x_Empresa_x_Usuario_Info> Lista;

                using (Entities_seguridad_acceso Context = new Entities_seguridad_acceso())
                {
                    Lista = (from m in Context.seg_Menu
                             join me in Context.seg_Menu_x_Empresa
                             on m.IdMenu equals me.IdMenu
                             join meu in Context.seg_Menu_x_Empresa_x_Usuario
                             on new { me.IdEmpresa, me.IdMenu } equals new { meu.IdEmpresa, meu.IdMenu }
                             where m.Habilitado == true && meu.IdEmpresa == IdEmpresa
                             && meu.IdUsuario == IdUsuario && m.IdMenuPadre == IdMenuPadre
                             && m.es_web == true
                             orderby m.PosicionMenu
                             select new seg_Menu_x_Empresa_x_Usuario_Info
                             {
                                 seleccionado = true,
                                 IdEmpresa = meu.IdEmpresa,
                                 IdUsuario = meu.IdUsuario,
                                 IdMenu = meu.IdMenu,
                                 Nuevo = meu.Nuevo,
                                 Modificar = meu.Modificar,
                                 Anular = meu.Anular,
                                 info_menu = new seg_Menu_Info
                                 {
                                     IdMenu = m.IdMenu,
                                     DescripcionMenu = m.DescripcionMenu,
                                     IdMenuPadre = m.IdMenuPadre,
                                     PosicionMenu = m.PosicionMenu,
                                     web_nom_Action = m.web_nom_Action,
                                     web_nom_Area = m.web_nom_Area,
                                     web_nom_Controller = m.web_nom_Controller
                                 }
                             }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public seg_Menu_x_Empresa_x_Usuario_Info getInfo(int IdEmpresa, string IdUsuario, int IdMenu)
        {
            try
            {
                seg_Menu_x_Empresa_x_Usuario_Info info;
                using (Entities_seguridad_acceso Context = new Entities_seguridad_acceso())
                {
                    var Entity = Context.seg_Menu_x_Empresa_x_Usuario.Where(q => q.IdEmpresa == IdEmpresa && q.IdUsuario == IdUsuario && q.IdMenu == IdMenu).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    info = new seg_Menu_x_Empresa_x_Usuario_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdMenu = Entity.IdMenu,
                        IdUsuario = Entity.IdUsuario,
                        Nuevo = Entity.Nuevo,
                        Modificar = Entity.Modificar,
                        Anular = Entity.Anular,
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool eliminarDB(int IdEmpresa, string IdUsuario)
        {
            try
            {
                using (Entities_seguridad_acceso Context = new Entities_seguridad_acceso())
                {
                    Context.Database.ExecuteSqlCommand("DELETE seg_Menu_x_Empresa_x_Usuario WHERE IdEmpresa = "+IdEmpresa+" AND IdUsuario = '"+IdUsuario+"'");
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(List<seg_Menu_x_Empresa_x_Usuario_Info> Lista, int IdEmpresa, string IdUsuario)
        {
            try
            {
                using (Entities_seguridad_acceso Context = new Entities_seguridad_acceso())
                {
                    foreach (var item in Lista)
                    {
                        seg_Menu_x_Empresa_x_Usuario Entity = new seg_Menu_x_Empresa_x_Usuario
                        {
                            IdEmpresa = item.IdEmpresa,
                            IdUsuario = item.IdUsuario,
                            IdMenu = item.IdMenu,
                            Nuevo = item.Nuevo,
                            Modificar = item.Modificar,
                            Anular = item.Anular
                        };
                        Context.seg_Menu_x_Empresa_x_Usuario.Add(Entity);
                    }

                    Context.SaveChanges();
                    string sql = "exec spseg_corregir_menu '" + IdEmpresa + "','"+ IdUsuario + "'";
                    Context.Database.ExecuteSqlCommand(sql);

                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(seg_Menu_x_Empresa_x_Usuario_Info info)
        {
            try
            {
                using (Entities_seguridad_acceso Context = new Entities_seguridad_acceso())
                {
                    seg_Menu_x_Empresa_x_Usuario Entity = new seg_Menu_x_Empresa_x_Usuario
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdMenu = info.IdMenu,
                        IdUsuario = info.IdUsuario,
                        Nuevo = info.Nuevo,
                        Modificar = info.Modificar,
                        Anular = info.Anular,
                    };
                    Context.seg_Menu_x_Empresa_x_Usuario.Add(Entity);

                    Context.SaveChanges();
                    string sql = "exec spseg_corregir_menu '" + info.IdEmpresa + "','" + info.IdUsuario + "'";
                    Context.Database.ExecuteSqlCommand(sql);
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(seg_Menu_x_Empresa_x_Usuario_Info info)
        {
            try
            {
                using (Entities_seguridad_acceso Context = new Entities_seguridad_acceso())
                {
                    seg_Menu_x_Empresa_x_Usuario Entity = Context.seg_Menu_x_Empresa_x_Usuario.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdMenu == info.IdMenu && q.IdUsuario == info.IdUsuario);
                    if (Entity == null)
                        return false;

                    Entity.Nuevo = info.Nuevo;
                    Entity.Modificar = info.Modificar;
                    Entity.Anular = info.Anular;

                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public seg_Menu_x_Empresa_x_Usuario_Info get_list_menu_accion(int IdEmpresa, string IdUsuario, string Area, string NomControlador, string Accion)
        {
            try
            {
                seg_Menu_x_Empresa_x_Usuario_Info info = new seg_Menu_x_Empresa_x_Usuario_Info();

                using (Entities_seguridad_acceso odata = new Entities_seguridad_acceso())
                {
                    var Entity = odata.seg_Menu_x_Empresa_x_Usuario.Include("seg_Menu").Where(q => q.IdEmpresa == IdEmpresa && q.IdUsuario == IdUsuario && (q.seg_Menu == null ? "" : q.seg_Menu.web_nom_Controller) == NomControlador && (q.seg_Menu == null ? "" : q.seg_Menu.web_nom_Area) == Area && (q.seg_Menu == null ? "" : q.seg_Menu.web_nom_Action) == Accion).FirstOrDefault();
                    if (Entity == null)
                        return new seg_Menu_x_Empresa_x_Usuario_Info();

                    info = new seg_Menu_x_Empresa_x_Usuario_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdUsuario = Entity.IdUsuario,
                        IdMenu = Entity.IdMenu,
                        Nuevo = Entity.Nuevo,
                        Modificar = Entity.Modificar,
                        Anular = Entity.Anular,
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
