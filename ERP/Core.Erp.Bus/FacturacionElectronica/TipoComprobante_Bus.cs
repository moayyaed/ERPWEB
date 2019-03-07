using Core.Erp.Data.FacturacionElectronica;
using Core.Erp.Info.FacturacionElectronica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.FacturacionElectronica
{
   public class TipoComprobante_Bus
    {

        TipoComprobante_Data odata = new TipoComprobante_Data();
        public TipoComprobante_Info get_list_comprobantes(DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                return odata.get_list_comprobantes(FechaInicio, FechaFin);
            }
            catch (Exception)
            {

                throw;
            }
        }
        }
}
