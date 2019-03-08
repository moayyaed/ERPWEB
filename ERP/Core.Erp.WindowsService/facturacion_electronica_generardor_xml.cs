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
               // info_tipo_comprobante = bus_comprobante.get_list_comprobantes(DateTime.Now.AddMonths(-1).Date, DateTime.Now.Date);


                #region Generando facturas
               
                //info_tipo_comprobante.CbteFactura.ForEach(item =>
                //{

                //  string  sIdCbteFact = item.infoTributaria.razonSocial.Substring(0, 3) + "FAC" + item.infoTributaria.estab + "-" + item.infoTributaria.ptoEmi + "-" + item.infoTributaria.secuencial;

                //    XmlSerializerNamespaces NamespaceObject = new XmlSerializerNamespaces();
                //    NamespaceObject.Add("", "");
                //    XmlSerializer mySerializer = new XmlSerializer(typeof(factura));
                //    myWriter = new StreamWriter(@"C:\Comprobantes EFIXED\Repositorio de Comprobantes" + sIdCbteFact + ".xml");
                //    mySerializer.Serialize(myWriter, item, NamespaceObject);
                //    myWriter.Close();


                //}
                //);
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
