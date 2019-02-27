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
using static Core.Erp.Info.Helps.cl_enumeradores;
namespace Core.Erp.Info.FacturacionElectronica
{
    public class TipoComprobante_Info
    {
        public string IdComprobante { get; set; }
        public DateTime Fecha { get; set; }
        public eTipoDocumento TipoCbte { get; set; }
        public string Observacion { get; set; }

        public List<factura>  CbteFactura { get; set; }
        public List<comprobanteRetencion>  cbteRet { get; set; }
        public List<notaCredito>  cbteNC { get; set; }
        public List<notaDebito>  cbteDeb { get; set; }
        public List<guiaRemision>  cbtGR { get; set; }


        public TipoComprobante_Info()
        {
            cbteRet = new List<comprobanteRetencion>();
            cbteRet = new List<comprobanteRetencion>();
            cbteNC = new List<notaCredito>();
            cbteDeb = new List<notaDebito>();
            cbtGR = new List<guiaRemision>();
        }
    }
}