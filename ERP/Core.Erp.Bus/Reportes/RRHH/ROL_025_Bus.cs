using Core.Erp.Data.Reportes.RRHH;
using Core.Erp.Info.Reportes.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.RRHH
{
    public class ROL_025_Bus
    {
        ROL_025_Data odata = new ROL_025_Data();
        public List<ROL_025_Info> get_list(int IdEmpresa, int IdSucursal, int IdRol, int IdNomina_Tipo, int IdPeriodo)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdSucursal, IdRol, IdNomina_Tipo, IdPeriodo);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
