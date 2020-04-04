using Core.Erp.Data.Reportes.CuentasPorPagar;
using Core.Erp.Info.Reportes.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.CuentasPorPagar
{
    public class CXP_020_Bus
    {
        CXP_020_Data odata = new CXP_020_Data();

        public List<CXP_020_Info> GetList(int IdEmpresa, int IdTipoCbte, decimal IdCbteCble)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdTipoCbte, IdCbteCble);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
