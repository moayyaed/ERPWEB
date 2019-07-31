CREATE VIEW web.VWINV_012
AS
SELECT in_Ing_Egr_Inven_det.IdEmpresa, in_Ing_Egr_Inven_det.IdSucursal, in_Ing_Egr_Inven_det.IdMovi_inven_tipo, in_Ing_Egr_Inven_det.IdNumMovi, in_Ing_Egr_Inven_det.Secuencia, in_Ing_Egr_Inven_det.IdBodega, in_Ing_Egr_Inven_det.IdProducto,
tb_sucursal.Su_Descripcion, tb_bodega.bo_Descripcion, in_Producto.pr_codigo, in_Producto.pr_descripcion, in_movi_inven_tipo.tm_descripcion, in_Ing_Egr_Inven.cm_fecha, in_Ing_Egr_Inven.cm_observacion, in_UnidadMedida.Descripcion NomUnidad,
in_Ing_Egr_Inven_det.dm_cantidad_sinConversion, in_Ing_Egr_Inven_det.mv_costo_sinConversion,in_Ing_Egr_Inven_det.dm_cantidad_sinConversion * in_Ing_Egr_Inven_det.mv_costo_sinConversion as CostoTotal, in_movi_inven_tipo.cm_tipo_movi, in_Ing_Egr_Inven.IdEstadoAproba,
in_Ing_Egr_Inven.CodMoviInven, mot.Desc_mov_inv MotivoCabecera, modd.Desc_mov_inv MotivoDetalle, cc.cc_Descripcion
FROM     in_movi_inve INNER JOIN
                  in_movi_inve_detalle ON in_movi_inve.IdEmpresa = in_movi_inve_detalle.IdEmpresa AND in_movi_inve.IdSucursal = in_movi_inve_detalle.IdSucursal AND in_movi_inve.IdBodega = in_movi_inve_detalle.IdBodega AND 
                  in_movi_inve.IdMovi_inven_tipo = in_movi_inve_detalle.IdMovi_inven_tipo AND in_movi_inve.IdNumMovi = in_movi_inve_detalle.IdNumMovi RIGHT OUTER JOIN
                  in_Producto INNER JOIN
                  in_Ing_Egr_Inven INNER JOIN
                  in_Ing_Egr_Inven_det ON in_Ing_Egr_Inven.IdEmpresa = in_Ing_Egr_Inven_det.IdEmpresa AND in_Ing_Egr_Inven.IdSucursal = in_Ing_Egr_Inven_det.IdSucursal AND 
                  in_Ing_Egr_Inven.IdMovi_inven_tipo = in_Ing_Egr_Inven_det.IdMovi_inven_tipo AND in_Ing_Egr_Inven.IdNumMovi = in_Ing_Egr_Inven_det.IdNumMovi ON in_Producto.IdEmpresa = in_Ing_Egr_Inven_det.IdEmpresa AND 
                  in_Producto.IdProducto = in_Ing_Egr_Inven_det.IdProducto INNER JOIN
                  in_movi_inven_tipo ON in_Ing_Egr_Inven.IdEmpresa = in_movi_inven_tipo.IdEmpresa AND in_Ing_Egr_Inven.IdMovi_inven_tipo = in_movi_inven_tipo.IdMovi_inven_tipo ON 
                  in_movi_inve_detalle.IdEmpresa = in_Ing_Egr_Inven_det.IdEmpresa_inv AND in_movi_inve_detalle.IdSucursal = in_Ing_Egr_Inven_det.IdSucursal_inv AND in_movi_inve_detalle.IdBodega = in_Ing_Egr_Inven_det.IdBodega_inv AND 
                  in_movi_inve_detalle.IdMovi_inven_tipo = in_Ing_Egr_Inven_det.IdMovi_inven_tipo_inv AND in_movi_inve_detalle.IdNumMovi = in_Ing_Egr_Inven_det.IdNumMovi_inv AND 
                  in_movi_inve_detalle.Secuencia = in_Ing_Egr_Inven_det.secuencia_inv LEFT OUTER JOIN
                  tb_sucursal INNER JOIN
                  tb_bodega ON tb_sucursal.IdEmpresa = tb_bodega.IdEmpresa AND tb_sucursal.IdSucursal = tb_bodega.IdSucursal ON in_Ing_Egr_Inven_det.IdEmpresa = tb_bodega.IdEmpresa AND 
                  in_Ing_Egr_Inven_det.IdSucursal = tb_bodega.IdSucursal AND in_Ing_Egr_Inven_det.IdBodega = tb_bodega.IdBodega INNER JOIN
				  in_UnidadMedida ON in_UnidadMedida.IdUnidadMedida = in_Ing_Egr_Inven_det.IdUnidadMedida_sinConversion inner join 
				  in_Motivo_Inven as mot on mot.IdEmpresa = in_Ing_Egr_Inven.IdEmpresa and mot.IdMotivo_Inv = in_Ing_Egr_Inven.IdMotivo_Inv left join
				  in_Motivo_Inven as modd on modd.IdEmpresa = in_Ing_Egr_Inven_det.IdEmpresa and modd.IdMotivo_Inv = in_Ing_Egr_Inven_det.IdMotivo_Inv left join
				  ct_CentroCosto as cc on cc.IdEmpresa = in_Ing_Egr_Inven_det.IdEmpresa and cc.IdCentroCosto = in_Ing_Egr_Inven_det.IdCentroCosto
where in_Ing_Egr_Inven.Estado = 'A'