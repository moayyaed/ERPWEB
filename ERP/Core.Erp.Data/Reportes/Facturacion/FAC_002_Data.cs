using Core.Erp.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
using Core.Erp.Data.General;

namespace Core.Erp.Data.Reportes.Facturacion
{
    public class FAC_002_Data
    {
        public List<FAC_002_Info> get_list(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdNota)
        {
            try
            {
                List<FAC_002_Info> Lista = new List<FAC_002_Info>();
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWFAC_002
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.IdBodega == IdBodega
                             && q.IdNota == IdNota
                             select new FAC_002_Info
                             {
                                 Fecha_Autorizacion = q.Fecha_Autorizacion,
                                 IdBodega = q.IdBodega,
                                 IdEmpresa = q.IdEmpresa,
                                 IdProducto = q.IdProducto,
                                 IdSucursal = q.IdSucursal,
                                 pr_descripcion = q.pr_descripcion,
                                 Secuencia = q.Secuencia,
                                 Celular = q.Celular,
                                 CodDocumentoTipo = q.CodDocumentoTipo,
                                 Correo = q.Correo,
                                 CreDeb = q.CreDeb,
                                 DetalleAdicional = q.DetalleAdicional,
                                 Direccion = q.Direccion,
                                 DocumentoAplicado = q.DocumentoAplicado,
                                 FechaDocumentoAplica = q.FechaDocumentoAplica,
                                 IdNota = q.IdNota,
                                 no_fecha = q.no_fecha,
                                 NumAutorizacion = q.NumAutorizacion,
                                 NumNota_Impresa = q.NumNota_Impresa,
                                 pe_cedulaRuc = q.pe_cedulaRuc,
                                 pe_nombreCompleto = q.pe_nombreCompleto,
                                 pr_codigo = q.pr_codigo,
                                 sc_iva = q.sc_iva,
                                 sc_observacion = q.sc_observacion,
                                 sc_precioFinal = q.sc_precioFinal,
                                 sc_subtotal = q.sc_subtotal,
                                 sc_total = q.sc_total,
                                 Serie1 = q.Serie1,
                                 Serie2 = q.Serie2,
                                 SubtotalAntesDescuento = q.SubtotalAntesDescuento,
                                 SubtotalIva = q.SubtotalIva,
                                 SubtotalSinIva = q.SubtotalSinIva,
                                 Telefono = q.Telefono,
                                 TotalDescuento = q.TotalDescuento,
                                 sc_cantidad = q.sc_cantidad
                             }).ToList();
                }

                if (Lista.Count > 0)
                {
                    var Detalle = Lista[0];
                    
                    if (!string.IsNullOrEmpty(Detalle.NumNota_Impresa) && string.IsNullOrEmpty(Detalle.NumAutorizacion))
                    {
                        tb_empresa_Data odataEmpresa = new tb_empresa_Data();
                        tb_sis_Documento_Tipo_Talonario_Data odataTalonario = new tb_sis_Documento_Tipo_Talonario_Data();
                        string[] Array = Detalle.NumNota_Impresa.Split('-');
                        string ClaveAcceso = odataTalonario.GeneraClaveAcceso(Detalle.no_fecha, Detalle.CreDeb == "C" ? "04" : "05", odataEmpresa.get_info(IdEmpresa).em_ruc, Detalle.Serie1 + Detalle.Serie2, Array[2]);
                        Lista.ForEach(q => q.NumAutorizacion = ClaveAcceso);
                    }
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
