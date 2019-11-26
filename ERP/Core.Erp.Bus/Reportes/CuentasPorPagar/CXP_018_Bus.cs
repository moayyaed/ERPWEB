using Core.Erp.Data.Reportes.CuentasPorPagar;
using Core.Erp.Info.Reportes.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.CuentasPorPagar
{
    public class CXP_018_Bus
    {
        CXP_018_Data odata = new CXP_018_Data();
        public List<CXP_018_Info> GetList(int IdEmpresa, int IdSucursal, decimal IdProveedor, DateTime fecha_corte, bool mostrarSaldo0, int IdClaseProveedor, bool FiltrarFechaContable)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdSucursal, IdProveedor, fecha_corte, mostrarSaldo0, IdClaseProveedor, FiltrarFechaContable);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
