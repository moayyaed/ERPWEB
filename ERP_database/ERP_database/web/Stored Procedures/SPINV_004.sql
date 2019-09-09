
CREATE PROCEDURE [web].[SPINV_004]
(
@IdEmpresa int,
@IdSucursalIni int,
@IdSucursalFin int,
@IdBodegaIni int,
@IdBodegaFin int,
@IdProductoIni numeric,
@IdProductoFin numeric,
@IdMarcaIni int,
@IdMarcaFin int
)
as
SELECT        in_producto_x_tb_bodega.IdEmpresa, in_producto_x_tb_bodega.IdSucursal, in_producto_x_tb_bodega.IdBodega, in_producto_x_tb_bodega.IdProducto, in_Producto.pr_descripcion AS NomProducto, 
                         in_presentacion.nom_presentacion AS NomPresentacion, tb_sucursal.Su_Descripcion AS NomSucursal, tb_bodega.bo_Descripcion AS NomBodega, in_ProductoTipo.tp_descripcion AS NomTipo, 
                         in_Marca.Descripcion AS NomMarca, in_Producto.IdMarca, in_producto_x_tb_bodega.Stock_minimo, ISNULL(mov.StockActual, 0) AS StockActual, uc.costo, uc.IdFecha, 
						 
						 case when ISNULL(mov.StockActual, 0) < 0 then abs(ISNULL(mov.StockActual, 0)) + in_producto_x_tb_bodega.Stock_minimo else in_producto_x_tb_bodega.Stock_minimo -  ISNULL(mov.StockActual, 0) end as CantidadMinPedir

FROM            in_ProductoTipo RIGHT OUTER JOIN
                         in_Producto LEFT OUTER JOIN
                         in_Marca ON in_Producto.IdEmpresa = in_Marca.IdEmpresa AND in_Producto.IdMarca = in_Marca.IdMarca LEFT OUTER JOIN
                         in_presentacion ON in_Producto.IdEmpresa = in_presentacion.IdEmpresa AND in_Producto.IdPresentacion = in_presentacion.IdPresentacion ON in_ProductoTipo.IdEmpresa = in_Producto.IdEmpresa AND 
                         in_ProductoTipo.IdProductoTipo = in_Producto.IdProductoTipo RIGHT OUTER JOIN
                         tb_bodega INNER JOIN
                         tb_sucursal ON tb_bodega.IdEmpresa = tb_sucursal.IdEmpresa AND tb_bodega.IdSucursal = tb_sucursal.IdSucursal RIGHT OUTER JOIN
                         in_producto_x_tb_bodega ON tb_bodega.IdBodega = in_producto_x_tb_bodega.IdBodega AND tb_bodega.IdSucursal = in_producto_x_tb_bodega.IdSucursal AND tb_bodega.IdEmpresa = in_producto_x_tb_bodega.IdEmpresa ON
                          in_Producto.IdEmpresa = in_producto_x_tb_bodega.IdEmpresa AND in_Producto.IdProducto = in_producto_x_tb_bodega.IdProducto LEFT OUTER JOIN
                             (SELECT        p.IdEmpresa, d.IdSucursal, d.IdBodega, ISNULL(p.IdProducto_padre, d.IdProducto) AS IdProducto, SUM(d.dm_cantidad) AS StockActual
                               FROM            in_movi_inve_detalle AS d INNER JOIN
                                                         in_Producto AS p ON d.IdEmpresa = p.IdEmpresa AND d.IdProducto = p.IdProducto
														 where isnull(p.lote_fecha_vcto,getdate()) >= GETDATE()
														 and d.IdEmpresa = @IdEmpresa and d.IdSucursal between @IdSucursalIni and @IdSucursalFin
														 and d.IdBodega between @IdBodegaIni and @IdBodegaFin and ISNULL(p.IdProducto_padre, d.IdProducto) between @IdProductoIni and @IdProductoFin
														 and p.IdMarca between @IdMarcaIni and @IdMarcaFin and p.Estado = 'A'
                               GROUP BY p.IdEmpresa, d.IdSucursal, d.IdBodega, ISNULL(p.IdProducto_padre, d.IdProducto)) AS mov ON in_producto_x_tb_bodega.IdEmpresa = mov.IdEmpresa AND in_producto_x_tb_bodega.IdSucursal = mov.IdSucursal AND 
                         in_producto_x_tb_bodega.IdBodega = mov.IdBodega AND in_producto_x_tb_bodega.IdProducto = mov.IdProducto LEFT JOIN
						 (
						 
							select a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdProducto, a.IdFecha, a.costo from (
							SELECT ROW_NUMBER() over(partition by IdEmpresa, IdSucursal, IdBodega, IdProducto order by IdEmpresa, IdSucursal, IdBodega, IdProducto, IdFecha desc) IdRow, 
							c1.IdEmpresa, c1.IdSucursal, c1.IdBodega, c1.IdProducto, c1.IdFecha, c1.costo
							FROM in_producto_x_tb_bodega_Costo_Historico as c1 where
							c1.IdEmpresa = @IdEmpresa and c1.IdSucursal between @IdSucursalIni and @IdSucursalFin and c1.IdBodega between @IdBodegaIni and @IdBodegaFin and c1.IdProducto between @IdProductoIni and @IdProductoFin
							) a WHERE IdRow = 1
						  
						 ) uc on in_producto_x_tb_bodega.IdEmpresa = uc.IdEmpresa and in_producto_x_tb_bodega.IdSucursal = uc.IdSucursal and in_producto_x_tb_bodega.IdBodega = uc.IdBodega and in_producto_x_tb_bodega.IdProducto = uc.IdProducto
WHERE        in_producto_x_tb_bodega.IdEmpresa = @IdEmpresa and in_producto_x_tb_bodega.IdSucursal between @IdSucursalIni and @IdSucursalFin and in_producto_x_tb_bodega.IdBodega between @IdBodegaIni and @IdBodegaFin and
in_producto_x_tb_bodega.IdProducto between @IdProductoIni and @IdProductoFin and (in_Producto.se_distribuye = 0) 
AND (in_Producto.IdProducto_padre IS NULL) and in_producto_x_tb_bodega.Stock_minimo > 0 and ISNULL(mov.StockActual, 0) <= in_producto_x_tb_bodega.Stock_minimo and in_producto.IdMarca between @IdMarcaIni and @IdMarcaFin AND in_Producto.Estado = 'A'
order by in_Marca.Descripcion