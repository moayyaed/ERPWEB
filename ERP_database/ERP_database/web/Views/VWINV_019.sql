CREATE VIEW web.VWINV_019
AS
SELECT in_Ing_Egr_Inven_det.IdEmpresa, in_Ing_Egr_Inven_det.IdSucursal, in_Ing_Egr_Inven_det.IdMovi_inven_tipo, in_Ing_Egr_Inven_det.IdNumMovi, in_Ing_Egr_Inven_det.Secuencia, in_Ing_Egr_Inven_det.IdBodega, 
                  in_Ing_Egr_Inven_det.IdProducto, in_Producto.pr_codigo, in_Producto.pr_descripcion, in_movi_inven_tipo.tm_descripcion, in_movi_inven_tipo.cm_tipo_movi, tb_sucursal.Su_Descripcion, tb_bodega.bo_Descripcion, 
                  in_Ing_Egr_Inven.CodMoviInven, in_Ing_Egr_Inven.cm_observacion, in_Ing_Egr_Inven.cm_fecha, in_Ing_Egr_Inven.IdEstadoAproba, in_Ing_Egr_Inven_det.dm_cantidad_sinConversion, in_Ing_Egr_Inven_det.mv_costo_sinConversion,
				  in_Ing_Egr_Inven_det.dm_cantidad_sinConversion * in_Ing_Egr_Inven_det.mv_costo_sinConversion CostoTotal
FROM     in_Ing_Egr_Inven INNER JOIN
                  in_Ing_Egr_Inven_det ON in_Ing_Egr_Inven.IdEmpresa = in_Ing_Egr_Inven_det.IdEmpresa AND in_Ing_Egr_Inven.IdSucursal = in_Ing_Egr_Inven_det.IdSucursal AND 
                  in_Ing_Egr_Inven.IdMovi_inven_tipo = in_Ing_Egr_Inven_det.IdMovi_inven_tipo AND in_Ing_Egr_Inven.IdNumMovi = in_Ing_Egr_Inven_det.IdNumMovi INNER JOIN
                  tb_bodega ON in_Ing_Egr_Inven_det.IdEmpresa = tb_bodega.IdEmpresa AND in_Ing_Egr_Inven_det.IdSucursal = tb_bodega.IdSucursal AND in_Ing_Egr_Inven_det.IdBodega = tb_bodega.IdBodega INNER JOIN
                  tb_sucursal ON tb_bodega.IdEmpresa = tb_sucursal.IdEmpresa AND tb_bodega.IdSucursal = tb_sucursal.IdSucursal INNER JOIN
                  in_Producto ON in_Ing_Egr_Inven_det.IdEmpresa = in_Producto.IdEmpresa AND in_Ing_Egr_Inven_det.IdProducto = in_Producto.IdProducto INNER JOIN
                  in_movi_inven_tipo ON in_Ing_Egr_Inven.IdEmpresa = in_movi_inven_tipo.IdEmpresa AND in_Ing_Egr_Inven.IdMovi_inven_tipo = in_movi_inven_tipo.IdMovi_inven_tipo
WHERE in_Ing_Egr_Inven.Estado = 'A'