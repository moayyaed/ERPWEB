using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.RRHH
{
   public class ro_Solicitud_Vacaciones_x_empleado_det_Data
    {

        public List<ro_Solicitud_Vacaciones_x_empleado_det_Info> get_list(int Idempresa, decimal IdSolicitud)
        {
            try
            {
                List<ro_Solicitud_Vacaciones_x_empleado_det_Info> lst ;

                string sql = " SELECT sol_det.IdEmpresa, sol_det.IdSolicitud, sol_det.Secuencia, sol_det.IdEmpleado, sol_det.IdPeriodo_Inicio, sol_det.IdPeriodo_Fin, sol_det.Dias_tomados, sol_det.Observacion, sol_det.Tipo_liquidacion, sol_det.Tipo_vacacion, "+
                             " hist.FechaIni, hist.FechaFin " +
                             " FROM  dbo.ro_Solicitud_Vacaciones_x_empleado_x_historico_vacaciones_x_empleado AS sol_det INNER JOIN " +
                             " dbo.ro_historico_vacaciones_x_empleado AS hist ON sol_det.IdEmpresa = hist.IdEmpresa AND sol_det.IdEmpleado = hist.IdEmpleado AND sol_det.IdPeriodo_Inicio = hist.IdPeriodo_Inicio AND " +
                             " sol_det.IdPeriodo_Fin = hist.IdPeriodo_Fin" +
                             " where sol_det.IdEmpresa="+Idempresa+" and sol_det.IdSolicitud ="+IdSolicitud+"";
                
                using (Entities_rrhh context=new Entities_rrhh())
                {

                  return  lst = context.Database.SqlQuery<ro_Solicitud_Vacaciones_x_empleado_det_Info>(sql).ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
