using Core.Erp.Data.FacturacionElectronica;
using Core.Erp.Info.FacturacionElectronica;
using Core.Erp.Info.FacturacionElectronica.Factura_V2;
using Core.Erp.Info.FacturacionElectronica.GuiaRemision;
using Core.Erp.Info.FacturacionElectronica.NotaCredito;
using Core.Erp.Info.FacturacionElectronica.NotaDebito;
using Core.Erp.Info.FacturacionElectronica.Retencion;
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
        public factura get_info_factura(DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                return odata.get_info_factura(FechaInicio, FechaFin);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public comprobanteRetencion get_info_retencion(DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                return odata.get_info_retencion(FechaInicio, FechaFin);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public guiaRemision get_info_guia(DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                return odata.get_info_guia(FechaInicio, FechaFin);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public notaCredito get_info_nota_credito(DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                return odata.get_info_nota_credito(FechaInicio, FechaFin);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public notaDebito get_info_nota_debito(DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                return odata.get_info_nota_debito(FechaInicio, FechaFin);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public factura get_facturas_eventos(DateTime FechaInicio, DateTime FechaFin)
        {
            try
            {
                return odata.get_facturas_eventos(FechaInicio, FechaFin);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
