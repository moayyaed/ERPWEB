using Core.Erp.Data.RRHH;
using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
namespace Core.Erp.Bus.RRHH
{
    public class ro_empleado_x_rubro_acumulado_Bus
    {
        ro_empleado_x_rubro_acumulado_Data odata = new ro_empleado_x_rubro_acumulado_Data();
        public List<ro_empleado_x_rubro_acumulado_Info> get_list(int IdEmpresa)
        {
            try
            {
                return odata.get_list(IdEmpresa);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ro_empleado_x_rubro_acumulado_Info get_info(int IdEmpresa, decimal IdEmpleado, string IdRubro)
        {
            try
            {
                return odata.get_info(IdEmpresa, IdEmpleado,IdRubro);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(ro_empleado_x_rubro_acumulado_Info info)
        {
            try
            {
                return odata.guardarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(ro_empleado_x_rubro_acumulado_Info info)
        {
            try
            {
                return odata.modificarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB(ro_empleado_x_rubro_acumulado_Info info)
        {
            try
            {
                return odata.anularDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool si_existe(ro_empleado_x_rubro_acumulado_Info info)
        {
            try
            {
                return odata.si_existe(info.IdEmpresa, info.IdEmpleado, info.IdRubro);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
