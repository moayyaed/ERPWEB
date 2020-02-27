using Core.Erp.Data.Reportes.CuentasPorPagar;
using Core.Erp.Info.Reportes.CuentasPorPagar;
using System;
using System.Collections.Generic;

namespace Core.Erp.Bus.Reportes.CuentasPorPagar
{
    public class CXP_019_Bus
    {
        CXP_019_Data odata = new CXP_019_Data();
    
        public List<CXP_019_Info> get_list(int IdEmpresa,DateTime fecha, int IdSucursal,int IdClaseProveedor, decimal IdProveedor, bool no_mostrar_en_conciliacion, bool no_mostrar_saldo_0)
        {
            try
            {
                return odata.get_list(IdEmpresa, fecha, IdSucursal, IdClaseProveedor,IdProveedor, no_mostrar_en_conciliacion, no_mostrar_saldo_0);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
