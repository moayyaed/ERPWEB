using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Facturacion
{
    public class fa_factura_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public int IdBodega { get; set; }
        public decimal IdCbteVta { get; set; }
        public string CodCbteVta { get; set; }
        public string vt_tipoDoc { get; set; }
        //[RegularExpression(@"\d{3}", ErrorMessage = "El formato debe ser 000")]
        //[Required(ErrorMessage = "El campo establecimiento es obligatorio")]
        public string vt_serie1 { get; set; }
        //[RegularExpression(@"\d{3}", ErrorMessage = "El formato debe ser 000")]
        //[Required(ErrorMessage = "El campo punto de emisión es obligatorio")]
        public string vt_serie2 { get; set; }
        //[RegularExpression(@"\d{9}", ErrorMessage = "El formato debe ser 000000000")]
        //[Required(ErrorMessage = "El campo # documento es obligatorio")]
        public string vt_NumFactura { get; set; }
        public Nullable<System.DateTime> Fecha_Autorizacion { get; set; }
        public string vt_autorizacion { get; set; }
        [Required(ErrorMessage = "El campo cliente es obligatorio")]
        [Range(1,int.MaxValue, ErrorMessage = "El campo cliente es obligatorio")]
        public decimal IdCliente { get; set; }
        public Nullable<int> IdContacto { get; set; }
        [Required(ErrorMessage = "El campo vendedor es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo vendedor es obligatorio")]
        public int IdVendedor { get; set; }
        [Required(ErrorMessage = "El campo fecha es obligatorio")]
        public System.DateTime vt_fecha { get; set; }
        [Required(ErrorMessage = "El campo plazo es obligatorio")]
        public decimal vt_plazo { get; set; }
        [Required(ErrorMessage = "El campo fecha de vencimiento es obligatorio")]
        public System.DateTime vt_fech_venc { get; set; }
        [Required(ErrorMessage = "El campo término de pago es obligatorio")]
        public string vt_tipo_venta { get; set; }
        public string IdCatalogo_FormaPago { get; set; }
        public string vt_Observacion { get; set; }
        public string Estado { get; set; }
        [Required(ErrorMessage = "El campo caja es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El campo caja es obligatorio")]
        public int IdCaja { get; set; }
        [Required(ErrorMessage = "El campo punto de venta es obligatorio")]

        public Nullable<int> IdPuntoVta { get; set; }
        public Nullable<bool> esta_impresa { get; set; }
        public Nullable<System.DateTime> fecha_primera_cuota { get; set; }
        public Nullable<double> valor_abono { get; set; }

        public int IdNivel { get; set; }
        [Required(ErrorMessage = "El campo tipo de comprobante es obligatorio")]
        public int IdFacturaTipo { get; set; }

        public fa_factura_resumen_Info info_resumen { get; set; }

        #region Campos de auditoria
        public string IdUsuario { get; set; }
        public Nullable<System.DateTime> Fecha_Transaccion { get; set; }
        public string IdUsuarioUltModi { get; set; }
        public Nullable<System.DateTime> Fecha_UltMod { get; set; }
        public string IdUsuarioUltAnu { get; set; }        
        public Nullable<System.DateTime> Fecha_UltAnu { get; set; }
        //[Required(ErrorMessage = "El campo motivo anulación es obligatorio")]
        public string MotivoAnulacion { get; set; }
        public bool aprobada_enviar_sri { get; set; }
        #endregion

        #region Campos de usuario super admin
        public string ContraseniaAdministrador { get; set; }
        #endregion

        #region Campos que no existen en la tabla
        public List<fa_factura_det_Info> lst_det { get; set; }
        public List<fa_cuotas_x_doc_Info> lst_cuota { get; set; }
        
        public decimal? IdProforma { get; set; }
        public bool DescuentoReadOnly { get; set; }

        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int ModificarEspecial { get; set; }
        public int Anular { get; set; }
        public int SecuenciaModificar { get; set; }
        #endregion

        #region Campos super admin
        public string IdUsuarioAut { get; set; }
        public string contrasena_admin { get; set; }
        public bool PedirDesbloqueo { get; set; }
        #endregion

        #region CamposFactura_PuntoVenta
        public Nullable<int> IdTarjeta { get; set; }
        public string cr_Tarjeta { get; set; }
        public double CantidadItem { get; set; }
        public double Recibido { get; set; }
        public double Cambio { get; set; }
        public string SubtotalFactura { get; set; }
        public string IvaFactura { get; set; }
        public string TotalFactura { get; set; }
        public string Subtotal { get; set; }
        public string Iva { get; set; }
        public string Total { get; set; }
        public decimal IdPersona { get; set; }
        [Required(ErrorMessage = "El campo naturaleza es requerido")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "el campo naturaleza debe tener mínimo 1 caracter y máximo 25 caracteres")]
        public string pe_Naturaleza { get; set; }
        [Required(ErrorMessage = "El campo nombre completo es requerido")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "el campo nombre completo debe tener mínimo 1 caracter y máximo 200 caracteres")]
        public string pe_nombreCompleto { get; set; }
        [StringLength(150, MinimumLength = 0, ErrorMessage = "el campo razón social debe tener máximo 150 caracteres")]
        public string pe_razonSocial { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "el campo apellidos debe tener máximo 100 caracteres")]
        public string pe_apellido { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "el campo nombres debe tener máximo 100 caracteres")]
        public string pe_nombre { get; set; }
        public string IdTipoDocumento { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "el campo número documento debe tener mínimo 1 caracter y máximo 50 caracteres")]
        [Required(ErrorMessage = "El campo número documento es requerido")]
        public string pe_cedulaRuc { get; set; }
        [StringLength(150, MinimumLength = 0, ErrorMessage = "el campo dirección debe tener máximo 150 caracteres")]
        public string pe_direccion { get; set; }
        public string pe_telfono_Contacto { get; set; }
        public string pe_celular { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "el campo correo debe tener máximo 100 caracteres")]
        public string pe_correo { get; set; }
        #endregion
    }

    public class fa_factura_consulta_Info
    {
        public string NomContacto { get; set; }
        public string Ve_Vendedor { get; set; }
        public Nullable<double> vt_Subtotal0 { get; set; }
        public Nullable<double> vt_SubtotalIVA { get; set; }
        public Nullable<double> vt_iva { get; set; }
        public Nullable<double> vt_total { get; set; }
        public Nullable<int> IdEmpresa_in_eg_x_inv { get; set; }
        public Nullable<int> IdSucursal_in_eg_x_inv { get; set; }
        public Nullable<int> IdMovi_inven_tipo_in_eg_x_inv { get; set; }
        public Nullable<decimal> IdNumMovi_in_eg_x_inv { get; set; }
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public int IdBodega { get; set; }
        public decimal IdCbteVta { get; set; }
        public string CodCbteVta { get; set; }
        public DateTime vt_fecha { get; set; }
        public string Estado { get; set; }
        public bool EstadoBool { get; set; }
        public bool? esta_impresa { get; set; }
        public string vt_NumFactura { get; set; }
        public string vt_autorizacion { get; set; }
        public DateTime? Fecha_Autorizacion { get; set; }
    }
}
