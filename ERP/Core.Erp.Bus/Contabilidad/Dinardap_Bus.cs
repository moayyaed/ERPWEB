using Core.Erp.Data.Contabilidad;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Contabilidad
{
    public class Dinardap_Bus
    {
        Dinardap_Data oData = new Dinardap_Data();

        public List<CXC_010_Info> get_info(int IdEmpresa, int IdPeriodo, int IdSucursal)
        {
            try
            {
                return oData.get_info(IdEmpresa, IdPeriodo, IdSucursal);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
