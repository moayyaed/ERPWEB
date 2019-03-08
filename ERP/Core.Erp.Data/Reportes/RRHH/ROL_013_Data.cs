using Core.Erp.Info.Reportes.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.RRHH
{
    public class ROL_013_Data
    {
        public List<ROL_013_Info> get_list(int IdEmpresa, int IdNomina, int IdSucursal, int IdPeriodo)
        {
            try
            {
                int IdSucursalIni = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 9999 : IdSucursal;

                List<ROL_013_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = Context.SPROL_013(IdEmpresa, IdNomina, IdSucursalIni, IdSucursalFin, IdPeriodo)
                             .Select(q=> new ROL_013_Info
                              {
                                 IdDepartamento = q.IdDepartamento,
                                 de_descripcion = q.de_descripcion,
                                 ca_descripcion = q.ca_descripcion,
                                 em_fechaIngaRol = q.em_fechaIngaRol,
                                 Descripcion = q.de_descripcion,
                                 Empleado = q.Empleado,
                                 em_codigo = q.em_codigo,
                                 IdArea = q.IdArea,
                                 IdDivision = q.IdDivision,
                                 IdEmpresa = q.IdEmpresa,
                                 IdNominaTipoLiqui = q.IdNominaTipoLiqui,
                                 IdNomina_Tipo = q.IdNomina_Tipo,
                                 pe_FechaFin = q.pe_FechaFin,
                                 Prestamos = q.Prestamos,
                                 Sueldo = q.Sueldo
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
