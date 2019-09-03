using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.Inventario
{
    public class INV_008_Data
    {
        public List<INV_008_Info> GetList(int IdEmpresa, int IdSucursal, int IdBodega, int IdProducto, DateTime fecha_ini, DateTime fecha_fin, string IdCentroCosto, string signo, int IdMovi_inven_tipo)
        {
            try
            {
                int IdProductoIni = IdProducto;
                int IdProductoFin = IdProducto == 0 ? 999999999 : IdProducto;

                int IdSucursalIni = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 99999999 : IdSucursal;

                int IdBodegaIni = IdBodega;
                int IdBodegaFin = IdBodega == 0 ? 99999999 : IdBodega;

                int IdMovi_inven_tipoIni = IdMovi_inven_tipo;
                int IdMovi_inven_tipoFin = IdMovi_inven_tipo == 0 ? 99999999 : IdMovi_inven_tipo;

                List<INV_008_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {                    
                    Lista = Context.VWINV_008.Where(
                        q=>q.IdEmpresa == IdEmpresa
                        && IdSucursalIni <= q.IdSucursal
                        && q.IdSucursal <= IdSucursalFin
                        && IdBodegaIni <= q.IdBodega
                        && q.IdBodega <= IdBodegaFin
                        && IdProductoIni <= q.IdProducto
                        && q.IdProducto <= IdProductoFin
                        && fecha_ini <= q.cm_fecha
                        && q.cm_fecha <= fecha_fin
                        && q.IdCentroCosto == (string.IsNullOrEmpty(IdCentroCosto) ? q.IdCentroCosto : IdCentroCosto)
                        && q.cm_tipo_movi == (string.IsNullOrEmpty(signo) ? q.cm_tipo_movi : signo )
                        && IdMovi_inven_tipoIni <= q.IdMovi_inven_tipo
                        && q.IdMovi_inven_tipo <= IdMovi_inven_tipoFin
                    ).Select(q => new INV_008_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        bo_Descripcion = q.bo_Descripcion,
                        cm_fecha = q.cm_fecha,
                        cm_tipo_movi = q.cm_tipo_movi,
                        Desc_mov_inv = q.Desc_mov_inv,
                        dm_cantidad = q.dm_cantidad,
                        Estado = q.Estado,
                        IdBodega = q.IdBodega,
                        IdCentroCosto = q.IdCentroCosto,
                        IdEstadoAproba = q.IdEstadoAproba,
                        IdMotivo_Inv = q.IdMotivo_Inv,
                        IdMovi_inven_tipo = q.IdMovi_inven_tipo,
                        IdNumMovi = q.IdNumMovi,
                        IdOrdenCompra = q.IdOrdenCompra,
                        IdProducto = q.IdProducto,
                        IdSucursal = q.IdSucursal,
                        mv_costo = q.mv_costo,
                        pe_nombreCompleto = q.pe_nombreCompleto,
                        pr_codigo = q.pr_codigo,
                        pr_descripcion = q.pr_descripcion,
                        Secuencia = q.Secuencia,
                        Su_Descripcion = q.Su_Descripcion,
                        tm_descripcion = q.tm_descripcion,
                        cc_Descripcion = q.cc_Descripcion
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
