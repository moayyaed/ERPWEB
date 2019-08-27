using Core.Erp.Data.Reportes.CuentasPorCobrar;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.CuentasPorCobrar
{
    public class CXC_004_Bus
    {
        CXC_004_Data odata = new CXC_004_Data();

        public List<CXC_004_Info> get_list(int IdEmpresa, int IdSucursal, decimal IdCliente, int Idtipo_cliente, DateTime FechaIni, DateTime FechaFin , bool MostrarSaldo0)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdSucursal, IdCliente, Idtipo_cliente, FechaIni, FechaFin, MostrarSaldo0);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
