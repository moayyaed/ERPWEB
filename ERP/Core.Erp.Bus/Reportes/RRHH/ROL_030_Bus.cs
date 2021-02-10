using Core.Erp.Data.Reportes.RRHH;
using Core.Erp.Info.Reportes.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.RRHH
{
    public class ROL_030_Bus
    {
        ROL_030_Data odata = new ROL_030_Data();
        public List<ROL_030_Info> get_list( int IdEmpresa, int IdSucursal, int IdNominaTipo, int IdNominaTipoLiqui,int IdPeriodo)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdSucursal, IdNominaTipo, IdNominaTipoLiqui, IdPeriodo);
            }
            catch (Exception)
            {

                throw;
            }
        }

  }
}
