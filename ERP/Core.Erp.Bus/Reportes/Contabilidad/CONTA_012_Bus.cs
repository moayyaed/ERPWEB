using Core.Erp.Data.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Contabilidad
{
    public class CONTA_012_Bus
    {
        CONTA_012_Data odata = new CONTA_012_Data();
        public List<CONTA_012_Info> get_list(int IdEmpresa, int IdPeriodo)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdPeriodo);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
