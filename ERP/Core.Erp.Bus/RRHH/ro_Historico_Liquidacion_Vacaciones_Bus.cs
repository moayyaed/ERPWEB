using Core.Erp.Bus.General;
using Core.Erp.Data.RRHH;
using Core.Erp.Info.General;
using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Core.Erp.Bus.RRHH
{
    public  class ro_Historico_Liquidacion_Vacaciones_Bus
    {
        #region MyRegion
        ro_Solicitud_Vacaciones_x_empleado_Bus bus_solicitud = new ro_Solicitud_Vacaciones_x_empleado_Bus();
        ro_Historico_Liquidacion_Vacaciones_Data data = new ro_Historico_Liquidacion_Vacaciones_Data();
        ro_Solicitud_Vacaciones_x_empleado_Info info_solicitud = new ro_Solicitud_Vacaciones_x_empleado_Info();
        ro_rol_detalle_x_rubro_acumulado_Bus bus_rubros_acumulados = new ro_rol_detalle_x_rubro_acumulado_Bus();
        ro_Historico_Liquidacion_Vacaciones_Det_Bus bus_detalle = new ro_Historico_Liquidacion_Vacaciones_Det_Bus();

        #endregion
        public Boolean guardarDB(ro_Historico_Liquidacion_Vacaciones_Info Info)
        {
            try
            {

                return data.guardarDB(Info);

            }
            catch (Exception ex )
            {
                tb_LogError_Bus LogData = new tb_LogError_Bus();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "ro_Historico_Liquidacion_Vacaciones_Bus", Metodo = "guardarDB", IdUsuario = Info.IdUsuario });
                return false;
            }

        }
        public Boolean modificarDB(ro_Historico_Liquidacion_Vacaciones_Info Info)
        {
            try
            {
               
                      return data.guardarDB(Info);

            }
            catch (Exception ex)
            {
                tb_LogError_Bus LogData = new tb_LogError_Bus();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "ro_Historico_Liquidacion_Vacaciones_Bus", Metodo = "modificarDB", IdUsuario = Info.IdUsuario });
                return false;
            }

        }

        public List<ro_Historico_Liquidacion_Vacaciones_Info> get_list(int IdEmpresa, DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {

                return data.get_list(IdEmpresa,FechaInicio, FechaFin);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ro_Historico_Liquidacion_Vacaciones_Info get_info(int IdEmpresa, decimal IdLiquidacion)
        {
            try
            {
                ro_Historico_Liquidacion_Vacaciones_Info info = new ro_Historico_Liquidacion_Vacaciones_Info();
                info= data.get_info(IdEmpresa, IdLiquidacion);
               info.lst_detalle= bus_detalle.Get_Lis(IdEmpresa, IdLiquidacion);
                return info;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public Boolean anularDB(ro_Historico_Liquidacion_Vacaciones_Info Info)
        {
            try
            {
                Info.ValorCancelado = Info.lst_detalle.Sum(v => v.Valor_Cancelar);
                if (data.anularDB(Info))
                {
                   
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
        }      
       public List<ro_Historico_Liquidacion_Vacaciones_Det_Info> obtener_valores(int IdEmpresa, decimal IdEmpleado, DateTime Anio_Desde, DateTime Anio_Hasta, int dias)
        {
            try
            {
                    int secuancia = 1;
                List<ro_Historico_Liquidacion_Vacaciones_Det_Info> lst_detalle=new List<ro_Historico_Liquidacion_Vacaciones_Det_Info>();
                    while (Anio_Desde < Anio_Hasta)
                    {
                        double valor_provision = 0;
                        valor_provision = bus_rubros_acumulados.get_vac_x_mes_x_anio(info_solicitud.IdEmpresa, IdEmpleado, Anio_Desde.Year, Anio_Desde.Month);
                        ro_Historico_Liquidacion_Vacaciones_Det_Info info_det = new ro_Historico_Liquidacion_Vacaciones_Det_Info();
                        info_det.Anio = Anio_Desde.Year;
                        info_det.Mes = Anio_Desde.Month;
                        info_det.Total_Remuneracion = valor_provision * 24;
                        info_det.Total_Vacaciones = valor_provision;
                        if (valor_provision != 0)
                        {
                            info_det.Valor_Cancelar = (valor_provision / 15) * dias;
                        }

                        Anio_Desde = Anio_Desde.AddMonths(1);
                        info_det.Secuencia = secuancia;
                        lst_detalle.Add(info_det);
                        secuancia++;
                    }
                    return lst_detalle;

                

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
