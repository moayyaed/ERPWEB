using Core.Erp.Data.Reportes.CuentasPorCobrar;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.CuentasPorCobrar
{
    public class CXC_012_Bus
    {
        CXC_012_Data odata = new CXC_012_Data();

        public List<CXC_012_Info> get_list(int IdEmpresa, int IdSucursal, decimal IdCliente, int Idtipo_cliente, DateTime fechaCorte, bool MostrarSoloCarteraVencida, int DiasVencimiento)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdSucursal, IdCliente, Idtipo_cliente, fechaCorte, MostrarSoloCarteraVencida, DiasVencimiento);
            }
            catch (Exception EX)
            {

                throw;
            }
        }
    }
}
