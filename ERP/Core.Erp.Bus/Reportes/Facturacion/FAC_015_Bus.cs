using Core.Erp.Data.Reportes.Facturacion;
using Core.Erp.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Facturacion
{
    public class FAC_015_Bus
    {
        FAC_015_Data odata = new FAC_015_Data();
        public List<FAC_015_Info> GetList(int IdEmpresa, int IdSucursal, int IdAnio)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdSucursal, IdAnio);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
