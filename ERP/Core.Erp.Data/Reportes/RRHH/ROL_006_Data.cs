using Core.Erp.Info.Reportes.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.RRHH
{
    public class ROL_006_Data
    {
        public List<ROL_006_Info> get_list(int IdEmpresa, decimal IdEmpleado)
        {
            try
            {
                List<ROL_006_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWROL_006
                             where q.IdEmpresa == IdEmpresa
                             && q.IdEmpleado == IdEmpleado
                             select new ROL_006_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 pe_nombre = q.pe_nombre,
                                 pe_cedulaRuc = q.pe_cedulaRuc,
                                 IdEmpleado = q.IdEmpleado,
                                 IdSolicitud = q.IdSolicitud,
                                 Fecha = q.Fecha,
                                 Fecha_Desde = q.Fecha_Desde,
                                 Fecha_Hasta = q.Fecha_Hasta,
                                 Fecha_Retorno = q.Fecha_Retorno,
                                 Observacion = q.Observacion,
                                 de_descripcion = q.de_descripcion,
                                 em_fechaIngaRol = q.em_fechaIngaRol,
                                 ca_descripcion = q.ca_descripcion,
                                 ValorCancelado = q.ValorCancelado,
                                 FechaPago = q.FechaPago,
                                 Anio = q.Anio,
                                 Mes = q.Mes,
                                 Total_Remuneracion = q.Total_Remuneracion,
                                 Total_Vacaciones = q.Total_Vacaciones,
                                 Valor_Cancelar = q.Valor_Cancelar,
                             }).ToList();
                }
                Lista.ForEach(v => v.periodo = v.Anio.ToString() + "-" + v.Mes.ToString());
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
