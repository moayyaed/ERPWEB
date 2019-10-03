using Core.Erp.Data.CuentasPorCobrar;
using Core.Erp.Info.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.CuentasPorCobrar
{
    public class cxc_LiquidacionRetProvDet_Bus
    {
        cxc_LiquidacionRetProvDet_Data odata = new cxc_LiquidacionRetProvDet_Data();

        public List<cxc_LiquidacionRetProvDet_Info> GetList(int IdEmpresa, decimal IdLiquidacion)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdLiquidacion);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
