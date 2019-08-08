using Core.Erp.Data.Reportes.Base;
using Core.Erp.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Facturacion
{
    public class FAC_020_Data
    {
        public List<FAC_020_Info> get_list(int IdEmpresa, int IdSucursal, int IdGuiaRemision)
        {
            try
            {
                List<FAC_020_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWFAC_020
                             where q.gi_IdEmpresa == IdEmpresa
                             && q.gi_IdSucursal == IdSucursal
                             && q.gi_IdGuiaRemision == IdGuiaRemision
                             select new FAC_020_Info
                             {
                                fa_IdEmpresa = q.fa_IdEmpresa,
                                fa_IdSucursal = q.fa_IdSucursal,
                                fa_IdBodega = q.fa_IdBodega,
                                fa_IdCbteVta = q.fa_IdCbteVta,
                                gi_IdEmpresa = q.gi_IdEmpresa,
                                gi_IdSucursal = q.gi_IdSucursal,
                                gi_IdBodega = q.gi_IdBodega,
                                gi_IdGuiaRemision = q.gi_IdGuiaRemision,
                                Secuencia = q.Secuencia,
                                IdProducto = q.IdProducto,
                                pr_codigo = q.pr_codigo,
                                pr_descripcion = q.pr_descripcion,
                                gi_cantidad = q.gi_cantidad,
                                gi_detallexItems = q.gi_detallexItems,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                CodDocumentoTipo = q.CodDocumentoTipo,
                                NumGuia_Preimpresa = q.NumGuia_Preimpresa,
                                CodGuiaRemision = q.CodGuiaRemision,
                                NUAutorizacion = q.NUAutorizacion,
                                Fecha_Autorizacion = q.Fecha_Autorizacion,
                                IdCliente = q.IdCliente,
                                IdTransportista = q.IdTransportista,
                                gi_fecha = q.gi_fecha,
                                gi_FechaFinTraslado = q.gi_FechaFinTraslado,
                                gi_FechaInicioTraslado = q.gi_FechaInicioTraslado,
                                gi_Observacion = q.gi_Observacion,
                                placa = q.placa,
                                Direccion_Origen = q.Direccion_Origen,
                                Direccion_Destino = q.Direccion_Destino,
                                Estado = q.Estado,
                                tr_Descripcion = q.tr_Descripcion,
                                NumComprobanteVenta = q.NumComprobanteVenta,
                                CedulaTransportista = q.CedulaTransportista,
                                NombreTransportista = q.NombreTransportista,
                                vt_fecha = q.vt_fecha,
                                vt_autorizacion = q.vt_autorizacion
                             }).ToList();
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
