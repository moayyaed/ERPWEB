using Core.Erp.Info.SeguridadAcceso;
using System;
using System.Collections.Generic;
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
                List<seg_Menu_x_Empresa_x_Usuario_Info> Lista;

                using (Entities_seguridad_acceso Context = new Entities_seguridad_acceso())
                {
                    Lista = (from m in Context.seg_Menu
                             join me in Context.seg_Menu_x_Empresa
                             on m.IdMenu equals me.IdMenu
                             join meu in Context.seg_Menu_x_Empresa_x_Usuario
                             on new { me.IdEmpresa, me.IdMenu } equals new { meu.IdEmpresa, meu.IdMenu }
                             where m.Habilitado == true && meu.IdEmpresa == IdEmpresa
                             && meu.IdUsuario == IdUsuario
                             orderby m.PosicionMenu
                             select new seg_Menu_x_Empresa_x_Usuario_Info
                             {
                                 seleccionado = true,
                                 IdEmpresa = meu.IdEmpresa,
                                 IdUsuario = meu.IdUsuario,
                                 IdMenu = meu.IdMenu,
                                 Lectura = meu.Lectura,
                                 Escritura = meu.Escritura,
                                 Eliminacion = meu.Eliminacion,
                                 IdMenuPadre = m.IdMenuPadre,
                                 DescripcionMenu = m.DescripcionMenu,
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

                    if(MostrarTodo)
                    Lista.AddRange((from q in Context.seg_Menu
                                    join me in Context.seg_Menu_x_Empresa
                                    on q.IdMenu equals me.IdMenu
                                    where q.Habilitado == true && me.IdEmpresa  == IdEmpresa
                                    && !Context.seg_Menu_x_Empresa_x_Usuario.Any(meu => meu.IdMenu == q.IdMenu && meu.IdEmpresa == IdEmpresa && meu.IdUsuario == IdUsuario)
                                    select new seg_Menu_x_Empresa_x_Usuario_Info
                                    {
                                        seleccionado = false,
                                        IdEmpresa = IdEmpresa,
                                        IdUsuario = IdUsuario,
                                        IdMenu = q.IdMenu,
                                        IdMenuPadre = q.IdMenuPadre,
                                        DescripcionMenu = q.DescripcionMenu,
                                        Lectura = true,
                                        Escritura = true,
                                        Eliminacion = true,
                                        info_menu = new seg_Menu_Info
                                        {
                                            IdMenu = q.IdMenu,
                                            DescripcionMenu = q.DescripcionMenu,
                                            IdMenuPadre = q.IdMenuPadre,
                                            PosicionMenu = q.PosicionMenu
                                        }

                                    }).ToList());
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
                                 Lectura = meu.Lectura,
                                 Escritura = meu.Escritura,
                                 Eliminacion = meu.Eliminacion,
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
                        Lectura = Entity.Lectura,
                        Escritura = Entity.Escritura,
                        Eliminacion = Entity.Eliminacion,
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
                            Lectura = item.Lectura,
                            Escritura = item.Escritura,
                            Eliminacion = item.Eliminacion
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
                        Lectura = info.Lectura,
                        Escritura = info.Escritura,
                        Eliminacion = info.Eliminacion,
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

                    Entity.Lectura = info.Lectura;
                    Entity.Escritura = info.Escritura;
                    Entity.Eliminacion = info.Eliminacion;

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
