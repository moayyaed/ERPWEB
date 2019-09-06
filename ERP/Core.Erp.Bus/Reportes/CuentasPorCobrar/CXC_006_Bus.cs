using Core.Erp.Data.Reportes.CuentasPorCobrar;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.CuentasPorCobrar
{
    public class CXC_006_Bus
    {
        CXC_006_Data odata = new CXC_006_Data();
        public List<CXC_006_Info> GetList(int IdEmpresa, int IdSucursal, decimal IdVendedor, decimal IdCliente, String IdCobro_tipo, DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdSucursal, IdVendedor, IdCliente, IdCobro_tipo, fecha_ini, fecha_fin);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
