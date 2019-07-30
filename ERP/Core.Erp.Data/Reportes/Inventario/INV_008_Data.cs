using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Inventario
{
    public class INV_008_Data
    {
        public List<INV_008_Info> GetList(int IdEmpresa, int IdSucursal, int IdBodega, int IdProducto, DateTime fecha_ini, DateTime fecha_fin, string IdCentroCosto, int IdMovi_inven_tipo, decimal IdNumMovi)
        {
            try
            {
                int IdSucursalIni = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 9999 : IdSucursal;

                int IdProductoIni = IdProducto;
                int IdProductoFin = IdProducto == 0 ? 999999999 : IdProducto;
                List<INV_008_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = Context.VWINV_008.Where(q=>q.IdEmpresa == IdEmpresa
                    && IdSucursalIni <= q.IdSucursal
                    && q.IdSucursal <= IdSucursalFin
                    && q.IdBodega == IdBodega
                    && IdProductoIni <= q.IdProducto
                    && q.IdProducto <= IdProductoFin
                    && fecha_ini <= q.cm_fecha
                    && q.cm_fecha <= fecha_fin
                    && q.IdCentroCosto == (string.IsNullOrEmpty(IdCentroCosto) ? q.IdCentroCosto : IdCentroCosto)
                    && q.IdMovi_inven_tipo == IdMovi_inven_tipo
                    && q.IdNumMovi == IdNumMovi
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
                        tm_descripcion = q.tm_descripcion

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
