using Core.Erp.Data.CuentasPorPagar;
using Core.Erp.Info.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.CuentasPorPagar
{
    public class cp_proveedor_microempresa_Bus
    {
        cp_proveedor_microempresa_Data odata = new cp_proveedor_microempresa_Data();

        public cp_proveedor_microempresa_Info GetInfo(string Ruc)
        {
            try
            {
                return odata.GetInfo(Ruc);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
