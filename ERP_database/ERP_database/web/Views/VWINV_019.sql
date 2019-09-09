CREATE VIEW [web].[VWINV_019]
AS
SELECT dbo.in_Ing_Egr_Inven_det.IdEmpresa, dbo.in_Ing_Egr_Inven_det.IdSucursal, dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo, dbo.in_Ing_Egr_Inven_det.IdNumMovi, dbo.in_Ing_Egr_Inven_det.Secuencia, dbo.in_Ing_Egr_Inven_det.IdBodega, 
                  dbo.in_Ing_Egr_Inven_det.IdProducto, dbo.in_Producto.pr_codigo, dbo.in_Producto.pr_descripcion, dbo.in_movi_inven_tipo.tm_descripcion, dbo.in_movi_inven_tipo.cm_tipo_movi, dbo.tb_sucursal.Su_Descripcion, 
                  dbo.tb_bodega.bo_Descripcion, dbo.in_Ing_Egr_Inven.CodMoviInven, dbo.in_Ing_Egr_Inven.cm_observacion, dbo.in_Ing_Egr_Inven.cm_fecha, dbo.in_Ing_Egr_Inven.IdEstadoAproba, 
                  dbo.in_Ing_Egr_Inven_det.dm_cantidad_sinConversion, 
				  case when in_Ing_Egr_Inven.IdEstadoAproba = 'APRO' THEN dbo.in_Ing_Egr_Inven_det.mv_costo_sinConversion ELSE 0 END mv_costo_sinConversion, 
                  case when in_Ing_Egr_Inven.IdEstadoAproba = 'APRO' THEN dbo.in_Ing_Egr_Inven_det.dm_cantidad_sinConversion * dbo.in_Ing_Egr_Inven_det.mv_costo_sinConversion ELSE 0 END AS CostoTotal
FROM     dbo.in_Ing_Egr_Inven INNER JOIN
                  dbo.in_Ing_Egr_Inven_det ON dbo.in_Ing_Egr_Inven.IdEmpresa = dbo.in_Ing_Egr_Inven_det.IdEmpresa AND dbo.in_Ing_Egr_Inven.IdSucursal = dbo.in_Ing_Egr_Inven_det.IdSucursal AND 
                  dbo.in_Ing_Egr_Inven.IdMovi_inven_tipo = dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo AND dbo.in_Ing_Egr_Inven.IdNumMovi = dbo.in_Ing_Egr_Inven_det.IdNumMovi INNER JOIN
                  dbo.tb_bodega ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.tb_bodega.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdSucursal = dbo.tb_bodega.IdSucursal AND 
                  dbo.in_Ing_Egr_Inven_det.IdBodega = dbo.tb_bodega.IdBodega INNER JOIN
                  dbo.tb_sucursal ON dbo.tb_bodega.IdEmpresa = dbo.tb_sucursal.IdEmpresa AND dbo.tb_bodega.IdSucursal = dbo.tb_sucursal.IdSucursal INNER JOIN
                  dbo.in_Producto ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_Producto.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdProducto = dbo.in_Producto.IdProducto INNER JOIN
                  dbo.in_movi_inven_tipo ON dbo.in_Ing_Egr_Inven.IdEmpresa = dbo.in_movi_inven_tipo.IdEmpresa AND dbo.in_Ing_Egr_Inven.IdMovi_inven_tipo = dbo.in_movi_inven_tipo.IdMovi_inven_tipo
WHERE  (dbo.in_Ing_Egr_Inven.Estado = 'A')