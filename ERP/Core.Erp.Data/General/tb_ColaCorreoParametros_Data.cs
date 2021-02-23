using Core.Erp.Data;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.General
{
    public class tb_ColaCorreoParametros_Data
    {
        public tb_ColaCorreoParametros_Info GetInfo(int IdEmpresa)
        {
            try
            {
                tb_ColaCorreoParametros_Info info = new tb_ColaCorreoParametros_Info();

                using (Entities_general db = new Entities_general())
                {
                    var Entity = db.tb_ColaCorreoParametros.Where(q => q.IdEmpresa == IdEmpresa).FirstOrDefault();
                    if (Entity == null)
                        return null;

                    info = new tb_ColaCorreoParametros_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        Usuario = Entity.Usuario,
                        Contrasenia = Entity.Contrasenia,
                        Puerto = Entity.Puerto,
                        Host = Entity.Host,
                        PermitirSSL = Entity.PermitirSSL,
                        CorreoCopia = Entity.CorreoCopia
                    };
                }

                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GuardarDB(tb_ColaCorreoParametros_Info info)
        {
            try
            {
                using (Entities_general db = new Entities_general())
                {
                    var Entity = db.tb_ColaCorreoParametros.Where(q => q.IdEmpresa == info.IdEmpresa).FirstOrDefault();
                    if (Entity == null)
                    {
                        Entity = new tb_ColaCorreoParametros
                        {
                            IdEmpresa = info.IdEmpresa,
                            Usuario = info.Usuario,
                            Contrasenia = info.Contrasenia,
                            Puerto = info.Puerto,
                            Host = info.Host,
                            PermitirSSL = info.PermitirSSL,
                            CorreoCopia = info.CorreoCopia,
                            IdUsuarioCreacion = info.IdUsuarioCreacion,
                            FechaCreacion = DateTime.Now
                        };
                        db.tb_ColaCorreoParametros.Add(Entity);
                    }else
                    {
                        Entity.Usuario = info.Usuario;
                        Entity.Contrasenia = info.Contrasenia;
                        Entity.Puerto = info.Puerto;
                        Entity.Host = info.Host;
                        Entity.PermitirSSL = info.PermitirSSL;
                        Entity.CorreoCopia = info.CorreoCopia;
                        Entity.IdUsuarioModificacion = info.IdUsuarioCreacion;
                        Entity.FechaModificacion = DateTime.Now;
                    }
                    db.SaveChanges();
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
