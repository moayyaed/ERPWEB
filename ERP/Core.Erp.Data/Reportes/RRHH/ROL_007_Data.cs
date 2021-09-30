using Core.Erp.Info.Reportes.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.RRHH
{
    public class ROL_007_Data
    {
        public List<ROL_007_Info> get_list(int IdEmpresa, decimal IdEmpleado, int IdSolicitud)
        {
            try
            {
                List<ROL_007_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWROL_007
                             where q.IdEmpresa == IdEmpresa
                             && q.IdEmpleado == IdEmpleado
                             && q.IdSolicitud == IdSolicitud
                             select new ROL_007_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 pe_apellido = q.pe_apellido,
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
                                 pe_nombre = q.pe_nombre
                             }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
