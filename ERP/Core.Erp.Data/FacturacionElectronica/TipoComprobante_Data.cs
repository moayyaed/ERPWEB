using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.FacturacionElectronica.Factura_V2;
using Core.Erp.Info.FacturacionElectronica.GuiaRemision;
using Core.Erp.Info.FacturacionElectronica.NotaCredito;
using Core.Erp.Info.FacturacionElectronica.NotaDebito;
using Core.Erp.Info.FacturacionElectronica.Retencion;
using Core.Erp.Info.FacturacionElectronica;

namespace Core.Erp.Data.FacturacionElectronica
{
  public  class TipoComprobante_Data
    {
       

        public List<TipoComprobante_Info> get_list(DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                List<TipoComprobante_Info> listado_documentos = new List<TipoComprobante_Info>();


                #region Facturas Fixed





                #endregion


                return listado_documentos;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
