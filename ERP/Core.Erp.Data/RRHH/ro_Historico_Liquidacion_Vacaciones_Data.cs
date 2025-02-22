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
  public  class ro_Historico_Liquidacion_Vacaciones_Data
    {
        ro_Solicitud_Vacaciones_x_empleado_det_Data odata_det = new ro_Solicitud_Vacaciones_x_empleado_det_Data();
        ro_Historico_Liquidacion_Vacaciones_Det_Data odata_det_liq = new ro_Historico_Liquidacion_Vacaciones_Det_Data();

        public List<ro_Historico_Liquidacion_Vacaciones_Info> get_list(int IdEmpresa,DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                List<ro_Historico_Liquidacion_Vacaciones_Info> Lista;
                FechaInicio = FechaInicio.Date;
                FechaFin = FechaFin.Date;
                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = (from q in Context.vwro_Historico_Liquidacion_Vacaciones
                             where q.IdEmpresa == IdEmpresa
                              && q.Fecha_Desde >= FechaInicio
                                 && q.Fecha_Desde <= FechaFin
                             select new ro_Historico_Liquidacion_Vacaciones_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdEmpleado = q.IdEmpleado,
                                 IdSolicitud = q.IdSolicitud,
                                 IdEstadoAprobacion = q.IdEstadoAprobacion,
                                 Fecha = q.Fecha,

                                 Fecha_Desde = q.Fecha_Desde,
                                 Fecha_Hasta = q.Fecha_Hasta,
                                 Fecha_Retorno = q.Fecha_Retorno,
                                 Gozadas = q.Gozadas,
                                 pe_nombre_completo = q.pe_nombreCompleto,
                                 Estado = q.Estado,
                                 EstadoBool = q.Estado == "A" ? true : false,

                                 IdOrdenPago=q.IdOrdenPago,
                                 IdEmpresa_OP=q.IdEmpresa_OP,
                                 IdTipo_op=q.IdTipo_op
                                 
                             }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ro_Historico_Liquidacion_Vacaciones_Info get_info(int IdEmpresa,  decimal IdSolicitud)
        {
            try
            {
                ro_Historico_Liquidacion_Vacaciones_Info info = new ro_Historico_Liquidacion_Vacaciones_Info();

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    vwro_Historico_Liquidacion_Vacaciones Entity = Context.vwro_Historico_Liquidacion_Vacaciones.FirstOrDefault(q => q.IdEmpresa == IdEmpresa
                   && q.IdSolicitud == IdSolicitud);
                    if (Entity == null) return null;

                    info = new ro_Historico_Liquidacion_Vacaciones_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdEmpleado = Entity.IdEmpleado,
                        IdSolicitud = Entity.IdSolicitud,
                        IdLiquidacion=Entity.IdLiquidacion==null?0:Convert.ToInt32(Entity.IdLiquidacion),
                        IdEstadoAprobacion = Entity.IdEstadoAprobacion,
                        Fecha = Entity.Fecha,
                        Fecha_Desde = Entity.Fecha_Desde,
                        Fecha_Hasta = Entity.Fecha_Hasta,
                        Fecha_Retorno = Entity.Fecha_Retorno,
                        Observaciones = Entity.Observacion,
                        Gozadas = Entity.Gozadas,
                        Estado = Entity.Estado,
                    };

                    info.lst_periodos = odata_det.get_list(IdEmpresa, IdSolicitud);
                    if(Entity.IdLiquidacion!=null)
                    info.lst_detalle = odata_det_liq.Get_Lis(IdEmpresa, IdSolicitud);

                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Boolean guardarDB(ro_Historico_Liquidacion_Vacaciones_Info info)
        {
            try
            {

                using (Entities_rrhh context = new Entities_rrhh())
                {

                    var contact = context.ro_Historico_Liquidacion_Vacaciones.FirstOrDefault(obj => obj.IdEmpresa == info.IdEmpresa &&
                       obj.IdLiquidacion == info.IdLiquidacion && obj.IdEmpleado == info.IdEmpleado);
                    if (contact == null)
                    {
                        context.ro_Historico_Liquidacion_Vacaciones.Add(new ro_Historico_Liquidacion_Vacaciones
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdSolicitud = info.IdSolicitud,
                            IdLiquidacion = info.IdLiquidacion = getId(info.IdEmpresa, Convert.ToInt32(info.IdEmpleado)),
                            IdEmpresa_OP = info.IdEmpresa_OP,
                            IdOrdenPago = info.IdOrdenPago,
                            IdEmpleado = info.IdEmpleado,
                            ValorCancelado = info.ValorCancelado=info.lst_detalle.Sum(s =>s.Valor_Cancelar),
                            FechaPago = DateTime.Now,
                            Observaciones = info.Observaciones,
                            IdUsuario = info.IdUsuario,
                            Estado = "A",
                            Fecha_Transac = DateTime.Now,
                        });
                        
                    }
                   else
                   {
                            var lst_delete = context.ro_Historico_Liquidacion_Vacaciones_Det.Where(obj => obj.IdEmpresa == info.IdEmpresa &&
                            obj.IdLiquidacion == info.IdLiquidacion);
                            context.ro_Historico_Liquidacion_Vacaciones_Det.RemoveRange(lst_delete);
                            contact.Observaciones = info.Observaciones;
                        contact.ValorCancelado = info.ValorCancelado = info.lst_detalle.Sum(s => s.Valor_Cancelar);
                        contact.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                            contact.FechaHoraAnul = DateTime.Now;
                            contact.MotiAnula = info.MotiAnula;
                    }
                    foreach (var item in info.lst_detalle)
                    {
                        context.ro_Historico_Liquidacion_Vacaciones_Det.Add(new ro_Historico_Liquidacion_Vacaciones_Det
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdLiquidacion = info.IdLiquidacion,
                            Secuencia = item.Secuencia,
                            Anio = item.Anio,
                            Mes = item.Mes,
                            Total_Remuneracion = item.Total_Remuneracion,
                            Total_Vacaciones = item.Total_Vacaciones,
                            Valor_Cancelar = item.Valor_Cancelar,
                        });
                    }
                    context.SaveChanges();
            }
                return true;
            }
            catch (Exception ex )
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "ro_Historico_Liquidacion_Vacaciones_Data", Metodo = "guardarDB", IdUsuario =info.IdUsuario});
                return false;
            }

        }
      
        public Boolean anularDB(ro_Historico_Liquidacion_Vacaciones_Info info)
        {
            try
            {
               
                    using (Entities_rrhh context = new Entities_rrhh())
                    {
                        var contact = context.ro_Historico_Liquidacion_Vacaciones.First(obj => obj.IdEmpresa == info.IdEmpresa &&
                             obj.IdLiquidacion == info.IdLiquidacion && obj.IdEmpleado == info.IdEmpleado);
                        contact.Estado = "I";
                        contact.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                        contact.FechaHoraAnul = DateTime.Now;
                        contact.MotiAnula = info.MotiAnula;
                    context.SaveChanges();
                    }

                    return true;
               
            }
            catch (Exception )
            {
               
                throw ;
            }
        }
     
        public int getId(int IdEmpresa, decimal IdEmpleado)
        {
            try
            {
                int Id;
                Entities_rrhh OEEmpleado = new Entities_rrhh();
                var select = from q in OEEmpleado.ro_Historico_Liquidacion_Vacaciones
                             where q.IdEmpresa == IdEmpresa 
                             select q;
                if (select.ToList().Count() == 0)
                {
                    Id = 1;
                }
                else
                {
                    var select_em = (from q in OEEmpleado.ro_Historico_Liquidacion_Vacaciones
                                     where q.IdEmpresa == IdEmpresa 
                                     select q.IdLiquidacion).Max();
                    Id = Convert.ToInt32(select_em.ToString()) + 1;
                }
                return Id;
            }
            catch (Exception )
            {
                
                throw;
            }
        }

       }
}
