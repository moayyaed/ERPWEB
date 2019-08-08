using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Facturacion
{
    public class FAC_002_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public int IdBodega { get; set; }
        public decimal IdNota { get; set; }
        public int Secuencia { get; set; }
        public string Serie1 { get; set; }
        public string Serie2 { get; set; }
        public string NumNota_Impresa { get; set; }
        public string pe_nombreCompleto { get; set; }
        public string pe_cedulaRuc { get; set; }
        public System.DateTime no_fecha { get; set; }
        public Nullable<System.DateTime> FechaDocumentoAplica { get; set; }
        public string sc_observacion { get; set; }
        public decimal IdProducto { get; set; }
        public string pr_codigo { get; set; }
        public string pr_descripcion { get; set; }
        public string DetalleAdicional { get; set; }
        public double sc_precioFinal { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public double SubtotalIva { get; set; }
        public double SubtotalSinIva { get; set; }
        public double SubtotalAntesDescuento { get; set; }
        public double sc_subtotal { get; set; }
        public double TotalDescuento { get; set; }
        public double sc_iva { get; set; }
        public double sc_total { get; set; }
        public string DocumentoAplicado { get; set; }
        public string NumAutorizacion { get; set; }
        public Nullable<System.DateTime> Fecha_Autorizacion { get; set; }
        public string CreDeb { get; set; }
        public string CodDocumentoTipo { get; set; }
    }
}
