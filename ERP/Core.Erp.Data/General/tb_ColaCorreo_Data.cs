using Core.Erp.Data;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.General
{
    public class tb_ColaCorreo_Data
    {
        public List<tb_ColaCorreo_Info> GetList(int IdEmpresa, DateTime FechaIni, DateTime FechaFin)
        {
            try
            {
                FechaIni = FechaIni.Date;
                FechaFin = FechaFin.Date;
                List<tb_ColaCorreo_Info> Lista = new List<tb_ColaCorreo_Info>();

                using (Entities_general db = new Entities_general())
                {
                    var lst = db.tb_ColaCorreo.Where(q => q.IdEmpresa == IdEmpresa && FechaIni <= q.FechaCreacion && q.FechaCreacion <= FechaFin).ToList();
                    foreach (var item in lst)
                    {
                        Lista.Add(new tb_ColaCorreo_Info
                        {
                            IdEmpresa = item.IdEmpresa,
                            IdCorreo = item.IdCorreo,
                            Codigo = item.Codigo,
                            Destinatarios = item.Destinatarios,
                            Asunto = item.Asunto,
                            Cuerpo = item.Cuerpo,
                            Parametros = item.Parametros,
                            IdUsuarioCreacion = item.IdUsuarioCreacion,
                            Error = item.Error,
                            FechaCreacion = item.FechaCreacion,
                            FechaEnvio = Convert.ToDateTime(item.FechaEnvio)

                        });
                    }
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tb_ColaCorreo_Info GetInfoPendienteEnviar()
        {
            try
            {
                tb_ColaCorreo_Info info = new tb_ColaCorreo_Info();

                using (Entities_general db = new Entities_general())
                {
                    var Entity = db.tb_ColaCorreo.Where(q => q.FechaEnvio == null).OrderBy(q => q.IdCorreo).FirstOrDefault();
                    if (Entity == null)
                        return null;
                    else
                    {
                        Entity.FechaEnvio = DateTime.Now;
                        db.SaveChanges();

                        info = new tb_ColaCorreo_Info
                        {
                            IdEmpresa = Entity.IdEmpresa,
                            IdCorreo = Entity.IdCorreo,
                            Codigo = Entity.Codigo,
                            Destinatarios = Entity.Destinatarios,
                            Asunto = Entity.Asunto,
                            Cuerpo = Entity.Cuerpo,
                            Parametros = Entity.Parametros
                        };

                        var Parametro = db.tb_ColaCorreoParametros.Where(q => q.IdEmpresa == info.IdEmpresa).FirstOrDefault();
                        if (Parametro == null)
                        {
                            Entity.FechaEnvio = null;
                            Entity.Error = "Debe parametrizar las credenciales para esta empresa";
                            db.SaveChanges();
                            return null;
                        }else
                        {
                            info.ParametroInfo = new tb_ColaCorreoParametros_Info
                            {
                                IdEmpresa = Parametro.IdEmpresa,
                                Usuario = Parametro.Usuario,
                                Contrasenia = Parametro.Contrasenia,
                                Puerto = Parametro.Puerto,
                                Host = Parametro.Host,
                                PermitirSSL = Parametro.PermitirSSL,
                                CorreoCopia = Parametro.CorreoCopia
                            };
                        }
                    }
                    
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private decimal GetId(int IdEmpresa)
        {
            try
            {
                decimal ID = 1;

                using (Entities_general db = new Entities_general())
                {
                    var Cont = db.tb_ColaCorreo.Where(q => q.IdEmpresa == IdEmpresa).Count();
                    if (Cont > 0)
                        ID = db.tb_ColaCorreo.Where(q => q.IdEmpresa == IdEmpresa).Max(q => q.IdCorreo) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(tb_ColaCorreo_Info info)
        {
            try
            {
                using (Entities_general db = new Entities_general())
                {
                    tb_ColaCorreo Entity = new tb_ColaCorreo
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdCorreo = info.IdCorreo = GetId(info.IdEmpresa),
                        Codigo = info.Codigo,
                        Destinatarios = info.Destinatarios,
                        Asunto = info.Asunto,
                        Cuerpo = info.Cuerpo,
                        Parametros = info.Parametros,
                        IdUsuarioCreacion = info.IdUsuarioCreacion,
                        FechaCreacion = DateTime.Now
                    };
                    db.tb_ColaCorreo.Add(Entity);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarDB(tb_ColaCorreo_Info info)
        {
            try
            {
                using (Entities_general db = new Entities_general())
                {
                    var Entity = db.tb_ColaCorreo.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCorreo == info.IdCorreo).FirstOrDefault();
                    if (Entity == null)
                        return false;

                    Entity.FechaEnvio = DateTime.Now;
                    Entity.Error = info.Error;
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
