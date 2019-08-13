using Core.Erp.Data.Compras;
using Core.Erp.Info.Compras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Compras
{
    public class com_ordencompra_local_resumen_Bus
    {
        com_ordencompra_local_resumen_Data oData = new com_ordencompra_local_resumen_Data();
        public com_ordencompra_local_resumen_Info get_info(int IdEmpresa, int IdSucursal, decimal IdOrdenCompra)
        {
            try
            {
                return oData.get_info(IdEmpresa, IdSucursal, IdOrdenCompra);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
