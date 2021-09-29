﻿using System;
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
                        AnioServicio = q.AnioServicio,
                        Dias_q_Corresponde = q.Dias_q_Corresponde,
                        Dias_a_disfrutar = q.Dias_a_disfrutar,
                        Dias_pendiente = q.Dias_pendiente,
                        Anio_Desde = q.Anio_Desde,
                        Anio_Hasta = q.Anio_Hasta,
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
                        AnioServicio = Entity.AnioServicio,
                        Dias_q_Corresponde = Entity.Dias_q_Corresponde,
                        Dias_a_disfrutar = Entity.Dias_a_disfrutar,
                        Dias_pendiente = Entity.Dias_pendiente,
                        Anio_Desde = Entity.Anio_Desde,
                        Anio_Hasta = Entity.Anio_Hasta,
                        Fecha_Desde = Entity.Fecha_Desde,
                        Fecha_Hasta = Entity.Fecha_Hasta,
                        Fecha_Retorno = Entity.Fecha_Retorno,
                        Observacion = Entity.Observacion,
                        Gozadas = Entity.Gozadas,
                        Estado = Entity.Estado,
                    };
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
                    info.Dias_pendiente = info.Dias_q_Corresponde - info.Dias_a_disfrutar;
                    ro_Solicitud_Vacaciones_x_empleado Entity = new ro_Solicitud_Vacaciones_x_empleado
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdEmpleado = info.IdEmpleado,
                        IdSolicitud = info.IdSolicitud = get_id(info.IdEmpresa),
                        IdEstadoAprobacion = "PEN",
                        Fecha = DateTime.Now.Date,
                        AnioServicio = info.AnioServicio,
                        Dias_q_Corresponde = info.Dias_q_Corresponde,
                        Dias_a_disfrutar = info.Dias_a_disfrutar,
                        Dias_pendiente = info.Dias_pendiente,
                        Anio_Desde = info.Anio_Desde,
                        Anio_Hasta = info.Anio_Hasta,
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
                            Observacion=item.Observacion,

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
                         Entity.AnioServicio = info.AnioServicio;
                         Entity.Dias_q_Corresponde = info.Dias_q_Corresponde;
                         Entity.Dias_a_disfrutar = info.Dias_a_disfrutar;
                         Entity.Dias_pendiente = info.Dias_pendiente;
                         Entity.Anio_Desde = info.Anio_Desde;
                         Entity.Anio_Hasta = info.Anio_Hasta;
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
