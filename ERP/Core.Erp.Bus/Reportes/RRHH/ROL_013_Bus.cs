using Core.Erp.Data.Reportes.RRHH;
using Core.Erp.Info.Reportes.RRHH;
using System;
using System.Collections.Generic;

namespace Core.Erp.Bus.Reportes.RRHH
{
    public class ROL_013_Bus
    {
        ROL_013_Data odata = new ROL_013_Data();

        public List<ROL_013_Info> get_list(int IdEmpresa, int IdNomina,int IdNominaTipoLiqui, int IdSucursal, int IdPeriodo)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdNomina, IdNominaTipoLiqui, IdSucursal, IdPeriodo);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
