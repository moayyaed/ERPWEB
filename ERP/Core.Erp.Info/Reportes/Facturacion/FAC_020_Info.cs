using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Facturacion
{
    public class FAC_020_Info
    {
        public Nullable<int> fa_IdEmpresa { get; set; }
        public Nullable<int> fa_IdSucursal { get; set; }
        public Nullable<int> fa_IdBodega { get; set; }
        public Nullable<decimal> fa_IdCbteVta { get; set; }
        public Nullable<int> gi_IdEmpresa { get; set; }
        public Nullable<int> gi_IdSucursal { get; set; }
        public Nullable<int> gi_IdBodega { get; set; }
        public Nullable<decimal> gi_IdGuiaRemision { get; set; }
        public int Secuencia { get; set; }
        public decimal IdProducto { get; set; }
        public string pr_codigo { get; set; }
        public string pr_descripcion { get; set; }
        public double gi_cantidad { get; set; }
        public string gi_detallexItems { get; set; }
        public string pe_nombreCompleto { get; set; }
        public string pe_cedulaRuc { get; set; }
        public string CodDocumentoTipo { get; set; }
        public string NumGuia_Preimpresa { get; set; }
        public string CodGuiaRemision { get; set; }
        public string NUAutorizacion { get; set; }
        public Nullable<System.DateTime> Fecha_Autorizacion { get; set; }
        public decimal IdCliente { get; set; }
        public decimal IdTransportista { get; set; }
        public System.DateTime gi_fecha { get; set; }
        public System.DateTime gi_FechaFinTraslado { get; set; }
        public System.DateTime gi_FechaInicioTraslado { get; set; }
        public string gi_Observacion { get; set; }
        public string placa { get; set; }
        public string Direccion_Origen { get; set; }
        public string Direccion_Destino { get; set; }
        public bool Estado { get; set; }
        public string tr_Descripcion { get; set; }
        public string NumComprobanteVenta { get; set; }
        public string CedulaTransportista { get; set; }
        public string NombreTransportista { get; set; }
        public Nullable<System.DateTime> vt_fecha { get; set; }
        public string vt_autorizacion { get; set; }
        public string Su_Descripcion { get; set; }
        public string Su_Direccion { get; set; }
    }
}
