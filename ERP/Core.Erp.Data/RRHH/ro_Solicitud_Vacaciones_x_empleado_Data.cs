using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.RRHH;
using Core.Erp.Data.General;
using Core.Erp.Info.General;

namespace Core.Erp.Data.RRHH
{
    public class ro_Solicitud_Vacaciones_x_empleado_Data
    {
        public List<ro_Solicitud_Vacaciones_x_empleado_Info> get_list(int IdEmpresa, DateTime fechaInicio, DateTime FechaFin)
        {
            try
            {
                List<ro_Solicitud_Vacaciones_x_empleado_Info> Lista = new List<ro_Solicitud_Vacaciones_x_empleado_Info>();
                DateTime fi = fechaInicio.Date;
                DateTime ff = FechaFin.Date;
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = Context.vwRo_Solicitud_Vacaciones.Where(q => q.IdEmpresa == IdEmpresa && q.Fecha_Hasta >= fi && q.Fecha_Hasta <= ff).Select(q => new ro_Solicitud_Vacaciones_x_empleado_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdEmpleado = q.IdEmpleado,
                        IdSolicitud = q.IdSolicitud,
                        IdEstadoAprobacion = q.IdEstadoAprobacion,
                        Fecha = q.Fecha,
                       
                        Fecha_Desde = q.Fecha_Desde,
                        Fecha_Hasta = q.Fecha_Hasta,
                        Fecha_Retorno = q.Fecha_Retorno,
                        Observacion = q.Observacion,
                        Gozadas = q.Gozadas,
                        em_codigo = q.em_codigo,
                        pe_cedulaRuc = q.pe_cedulaRuc,
                        pe_nombre_completo = q.pe_apellido + " " + q.pe_nombre,
                        Estado = q.Estado,
                        Estado_liquidacion = q.Estado_liquidacion,
                        EstadoBool = q.Estado == "A" ? true : false
                    }).ToList();
                }

                return Lista;
            }
            catch (Exception )
            {

                throw;
            }
        }
        public ro_Solicitud_Vacaciones_x_empleado_Info get_info(int IdEmpresa,decimal IdEmpleado, decimal IdSolicitud)
        {
            try
            {
                ro_Solicitud_Vacaciones_x_empleado_Info info = new ro_Solicitud_Vacaciones_x_empleado_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_Solicitud_Vacaciones_x_empleado Entity = Context.ro_Solicitud_Vacaciones_x_empleado.FirstOrDefault(q => q.IdEmpresa == IdEmpresa 
                    && q.IdEmpleado==IdEmpleado
                    && q.IdSolicitud == IdSolicitud);
                    if (Entity == null) return null;

                    info = new ro_Solicitud_Vacaciones_x_empleado_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdEmpleado = Entity.IdEmpleado,
                        IdSolicitud = Entity.IdSolicitud,
                        IdEstadoAprobacion = Entity.IdEstadoAprobacion,
                        Fecha = Entity.Fecha,
                       
                        Fecha_Desde = Entity.Fecha_Desde,
                        Fecha_Hasta = Entity.Fecha_Hasta,
                        Fecha_Retorno = Entity.Fecha_Retorno,
                        Observacion = Entity.Observacion,
                        Gozadas = Entity.Gozadas,
                        Estado = Entity.Estado,
                    };

                  info.lst_vacaciones = Context.ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado.Where(q => q.IdEmpresa == IdEmpresa && q.IdSolicitud == q.IdSolicitud ).Select(q => new ro_Solicitud_Vacaciones_x_empleado_det_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdEmpleado = q.IdEmpleado,
                        IdSolicitud = q.IdSolicitud,
                         IdPeriodo_Fin=q.IdPeriodo_Fin,
                        IdPeriodo_Inicio=q.IdPeriodo_Inicio,
                        Dias_tomados=q.Dias_tomados,
                        Tipo_liquidacion=q.Tipo_liquidacion,
                        Tipo_vacacion=q.Tipo_vacacion
                    }).ToList();
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int get_id(int IdEmpresa)
        {
            try
            {
                int ID = 1;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    var lst = from q in Context.ro_Solicitud_Vacaciones_x_empleado
                              where q.IdEmpresa == IdEmpresa
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdSolicitud) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(ro_Solicitud_Vacaciones_x_empleado_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_Solicitud_Vacaciones_x_empleado Entity = new ro_Solicitud_Vacaciones_x_empleado
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdEmpleado = info.IdEmpleado,
                        IdSolicitud = info.IdSolicitud = get_id(info.IdEmpresa),
                        IdEstadoAprobacion = "PEN",
                        Fecha = DateTime.Now.Date,
                        Fecha_Desde = info.Fecha_Desde,
                        Fecha_Hasta = info.Fecha_Hasta,
                        Fecha_Retorno = info.Fecha_Retorno,
                        Observacion = info.Observacion,
                        Gozadas = info.Gozadas,                        
                        Estado = info.Estado = "A",
                        IdUsuario = info.IdUsuario,
                        Fecha_Transac = info.Fecha_Transac = DateTime.Now.Date
                    };
                    Context.ro_Solicitud_Vacaciones_x_empleado.Add(Entity);

                    #region Historico
                    foreach (var item in info.lst_vacaciones)
                    {
                        ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado add = new ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado
                        {
                            IdEmpresa=info.IdEmpresa,
                            IdSolicitud=info.IdSolicitud,
                            IdEmpleado = info.IdEmpleado,
                            Secuencia=item.Secuencia,
                            IdPeriodo_Inicio=item.IdPeriodo_Inicio,
                            IdPeriodo_Fin=item.IdPeriodo_Fin,
                            Observacion=info.Observacion,
                            Tipo_liquidacion=item.Tipo_liquidacion,
                            Tipo_vacacion=item.Tipo_vacacion,
                            
                        };
                        Context.ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado.Add(add);
                       
                    }
                    Context.SaveChanges();

                    foreach(var item in info.lst_vacaciones)
                    {
                        int dias = 0;
                        ro_historico_vacaciones_x_empleado Entity_his = Context.ro_historico_vacaciones_x_empleado.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa
                       && q.IdEmpleado == info.IdEmpleado
                       && q.IdPeriodo_Inicio == item.IdPeriodo_Inicio
                       && q.IdPeriodo_Fin==item.IdPeriodo_Fin);
                        if (Entity_his == null)
                            return false;
                        dias = Entity_his.DiasTomados;
                        Entity_his.DiasTomados = (item.Dias_tomados+dias);

                        Context.SaveChanges();

                    }


                    #endregion


                }
                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "(ro_Solicitud_Vacaciones_x_empleado_Data", Metodo = "guardarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }
        public bool modificarDB(ro_Solicitud_Vacaciones_x_empleado_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_Solicitud_Vacaciones_x_empleado Entity = Context.ro_Solicitud_Vacaciones_x_empleado.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa 
                    && q.IdEmpleado==info.IdEmpleado
                    && q.IdSolicitud == info.IdSolicitud);
                    if (Entity == null)
                        return false;
                         Entity.Fecha_Desde = info.Fecha_Desde;
                         Entity.Fecha_Hasta = info.Fecha_Hasta;
                         Entity.Fecha_Retorno = info.Fecha_Retorno;
                         Entity.Observacion = info.Observacion;
                         Entity.Gozadas = info.Gozadas;
                         Entity.IdUsuarioUltMod = info.IdUsuarioUltMod;
                         Entity.Fecha_UltMod = info.Fecha_UltMod = DateTime.Now;


                    #region Historico
                    var lst_det = Context.ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado.Where(v=>v.IdEmpresa==info.IdEmpresa && v.IdSolicitud==info.IdSolicitud );
                    Context.ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado.RemoveRange(lst_det);
                    foreach (var item in info.lst_vacaciones)
                    {
                        ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado add = new ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdSolicitud = info.IdSolicitud,
                            IdEmpleado = info.IdEmpleado,
                            Secuencia = item.Secuencia,
                            IdPeriodo_Inicio = item.IdPeriodo_Inicio,
                            IdPeriodo_Fin = item.IdPeriodo_Fin,
                            Observacion = item.Observacion,
                            Tipo_vacacion=item.Tipo_vacacion,
                            Tipo_liquidacion=item.Tipo_liquidacion
                        };
                        Context.ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado.Add(add);

                    }
                    Context.SaveChanges();

                    foreach (var item in info.lst_vacaciones)
                    {
                        int dias = 0;
                        ro_historico_vacaciones_x_empleado Entity_his = Context.ro_historico_vacaciones_x_empleado.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa
                       && q.IdEmpleado == info.IdEmpleado
                       && q.IdPeriodo_Inicio == item.IdPeriodo_Inicio
                       && q.IdPeriodo_Fin == item.IdPeriodo_Fin);
                        if (Entity_his == null)
                            return false;
                        dias = Entity_his.DiasTomados;
                        Entity_his.DiasTomados = (item.Dias_tomados + dias);

                        Context.SaveChanges();

                    }


                    #endregion



                }

                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "(ro_Solicitud_Vacaciones_x_empleado_Data", Metodo = "modificarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }
        public bool anularDB(ro_Solicitud_Vacaciones_x_empleado_Info info)
        {
            try
            {
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    ro_Solicitud_Vacaciones_x_empleado Entity = Context.ro_Solicitud_Vacaciones_x_empleado.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa
                                      && q.IdEmpleado == info.IdEmpleado
                                      && q.IdSolicitud == info.IdSolicitud);
                    if (Entity == null)
                        return false;
                    Entity.Estado = info.Estado = "I";
                    Entity.IdUsuario_Anu = info.IdUsuario_Anu;
                    Entity.FechaAnulacion = info.FechaAnulacion = DateTime.Now;


                    #region Historico
                    var lst_det = Context.ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado.Where(v => v.IdEmpresa == info.IdEmpresa && v.IdSolicitud == info.IdSolicitud );
                    Context.ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado.RemoveRange(lst_det);
                    
                    Context.SaveChanges();

                    foreach (var item in info.lst_vacaciones)
                    {
                        int dias = 0;
                        ro_historico_vacaciones_x_empleado Entity_his = Context.ro_historico_vacaciones_x_empleado.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa
                       && q.IdEmpleado == info.IdEmpleado
                       && q.IdPeriodo_Inicio == item.IdPeriodo_Inicio
                       && q.IdPeriodo_Fin == item.IdPeriodo_Fin);
                        if (Entity_his == null)
                            return false;
                        dias = Entity_his.DiasTomados;
                        Entity_his.DiasTomados = (item.Dias_tomados - dias);

                        Context.SaveChanges();
                    }


                    #endregion


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
