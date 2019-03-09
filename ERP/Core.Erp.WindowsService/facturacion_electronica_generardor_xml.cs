using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.FacturacionElectronica;
using Core.Erp.Bus.FacturacionElectronica;
using System.Timers;
using System.Xml.Serialization;
using Core.Erp.Info.FacturacionElectronica.Factura_V2;
using System.IO;

namespace Core.Erp.WindowsService
{
    partial class facturacion_electronica_generardor_xml : ServiceBase
    {

        TipoComprobante_Bus bus_comprobante = new TipoComprobante_Bus();

        TipoComprobante_Info info_tipo_comprobante = new TipoComprobante_Info();
        private StreamWriter myWriter;

        public facturacion_electronica_generardor_xml()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: agregar código aquí para iniciar el servicio.
        }

        protected override void OnStop()
        {
            // TODO: agregar código aquí para realizar cualquier anulación necesaria para detener el servicio.
        }

        private void onTimeEvento(object sender, ElapsedEventArgs e)
        {
            try
            {
                #region Generar xml
                #region Generando facturas
                var info_factura = bus_comprobante.get_info_factura(DateTime.Now.AddMonths(-1).Date, DateTime.Now.Date);
                if (info_factura != null && info_factura.infoTributaria.secuencial!=null)
                {
                   
                    string sIdCbteFact = info_factura.infoTributaria.razonSocial.Substring(0, 3) + "FAC" + info_factura.infoTributaria.estab + "-" + info_factura.infoTributaria.ptoEmi + "-" + info_factura.infoTributaria.secuencial;
                    XmlSerializerNamespaces NamespaceObject = new XmlSerializerNamespaces();
                    NamespaceObject.Add("", "");
                    XmlSerializer mySerializer = new XmlSerializer(typeof(factura));
                    myWriter = new StreamWriter(@"C:\Comprobantes EFIXED\Repositorio de Comprobantes" + sIdCbteFact + ".xml");
                    mySerializer.Serialize(myWriter, info_factura, NamespaceObject);
                    myWriter.Close();
                }

                #endregion


                #region Generando retencion
                var info_retencion = bus_comprobante.get_info_factura(DateTime.Now.AddMonths(-1).Date, DateTime.Now.Date);
                if (info_retencion != null && info_retencion.infoTributaria.secuencial != null)
                {

                    string sIdCbteFact = info_retencion.infoTributaria.razonSocial.Substring(0, 3) + "RET" + info_retencion.infoTributaria.estab + "-" + info_retencion.infoTributaria.ptoEmi + "-" + info_retencion.infoTributaria.secuencial;
                    XmlSerializerNamespaces NamespaceObject = new XmlSerializerNamespaces();
                    NamespaceObject.Add("", "");
                    XmlSerializer mySerializer = new XmlSerializer(typeof(factura));
                    myWriter = new StreamWriter(@"C:\Comprobantes EFIXED\Repositorio de Comprobantes" + sIdCbteFact + ".xml");
                    mySerializer.Serialize(myWriter, info_retencion, NamespaceObject);
                    myWriter.Close();
                }

                #endregion


                #region Generando guia remision
                var info_guia = bus_comprobante.get_info_factura(DateTime.Now.AddMonths(-1).Date, DateTime.Now.Date);
                if (info_guia != null && info_guia.infoTributaria.secuencial != null)
                {

                    string sIdCbteFact = info_guia.infoTributaria.razonSocial.Substring(0, 3) + "GUI" + info_guia.infoTributaria.estab + "-" + info_guia.infoTributaria.ptoEmi + "-" + info_guia.infoTributaria.secuencial;
                    XmlSerializerNamespaces NamespaceObject = new XmlSerializerNamespaces();
                    NamespaceObject.Add("", "");
                    XmlSerializer mySerializer = new XmlSerializer(typeof(factura));
                    myWriter = new StreamWriter(@"C:\Comprobantes EFIXED\Repositorio de Comprobantes" + sIdCbteFact + ".xml");
                    mySerializer.Serialize(myWriter, info_guia, NamespaceObject);
                    myWriter.Close();
                }

                #endregion


                #region Generando nota credito
                var info_nc = bus_comprobante.get_info_factura(DateTime.Now.AddMonths(-1).Date, DateTime.Now.Date);
                if (info_nc != null && info_nc.infoTributaria.secuencial != null)
                {

                    string sIdCbteFact = info_nc.infoTributaria.razonSocial.Substring(0, 3) + "NTC" + info_nc.infoTributaria.estab + "-" + info_nc.infoTributaria.ptoEmi + "-" + info_nc.infoTributaria.secuencial;
                    XmlSerializerNamespaces NamespaceObject = new XmlSerializerNamespaces();
                    NamespaceObject.Add("", "");
                    XmlSerializer mySerializer = new XmlSerializer(typeof(factura));
                    myWriter = new StreamWriter(@"C:\Comprobantes EFIXED\Repositorio de Comprobantes" + sIdCbteFact + ".xml");
                    mySerializer.Serialize(myWriter, info_nc, NamespaceObject);
                    myWriter.Close();
                }

                #endregion


                #region Generando nota debito
                var info_nd = bus_comprobante.get_info_factura(DateTime.Now.AddMonths(-1).Date, DateTime.Now.Date);
                if (info_nd != null && info_nd.infoTributaria.secuencial != null)
                {

                    string sIdCbteFact = info_nd.infoTributaria.razonSocial.Substring(0, 3) + "NTD" + info_nd.infoTributaria.estab + "-" + info_nd.infoTributaria.ptoEmi + "-" + info_nd.infoTributaria.secuencial;
                    XmlSerializerNamespaces NamespaceObject = new XmlSerializerNamespaces();
                    NamespaceObject.Add("", "");
                    XmlSerializer mySerializer = new XmlSerializer(typeof(factura));
                    myWriter = new StreamWriter(@"C:\Comprobantes EFIXED\Repositorio de Comprobantes" + sIdCbteFact + ".xml");
                    mySerializer.Serialize(myWriter, info_nd, NamespaceObject);
                    myWriter.Close();
                }

                #endregion
                #endregion
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
