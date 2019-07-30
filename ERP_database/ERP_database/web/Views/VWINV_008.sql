CREATE VIEW [web].[VWINV_008]
AS
SELECT in_Ing_Egr_Inven_det.IdEmpresa, in_Ing_Egr_Inven_det.IdSucursal, in_Ing_Egr_Inven_det.IdMovi_inven_tipo, in_Ing_Egr_Inven_det.IdNumMovi, in_Ing_Egr_Inven_det.Secuencia, tb_sucursal.Su_Descripcion, tb_bodega.bo_Descripcion, 
                  in_movi_inven_tipo.cm_tipo_movi, in_movi_inven_tipo.tm_descripcion, in_Motivo_Inven.Desc_mov_inv, tb_persona.pe_nombreCompleto, in_Producto.pr_codigo, in_Producto.pr_descripcion, in_Ing_Egr_Inven.IdEstadoAproba, 
                  in_Ing_Egr_Inven_det.dm_cantidad, in_Ing_Egr_Inven_det.mv_costo, in_Ing_Egr_Inven_det.IdOrdenCompra, in_Ing_Egr_Inven_det.IdCentroCosto, in_Ing_Egr_Inven_det.IdMotivo_Inv, in_Ing_Egr_Inven.cm_fecha, 
                  in_Ing_Egr_Inven.Estado, in_Ing_Egr_Inven_det.IdBodega, in_Ing_Egr_Inven_det.IdProducto
FROM     in_Producto INNER JOIN
                  in_Ing_Egr_Inven_det INNER JOIN
                  in_Ing_Egr_Inven ON in_Ing_Egr_Inven_det.IdEmpresa = in_Ing_Egr_Inven.IdEmpresa AND in_Ing_Egr_Inven_det.IdSucursal = in_Ing_Egr_Inven.IdSucursal AND 
                  in_Ing_Egr_Inven_det.IdMovi_inven_tipo = in_Ing_Egr_Inven.IdMovi_inven_tipo AND in_Ing_Egr_Inven_det.IdNumMovi = in_Ing_Egr_Inven.IdNumMovi INNER JOIN
                  in_movi_inven_tipo ON in_Ing_Egr_Inven_det.IdEmpresa = in_movi_inven_tipo.IdEmpresa AND in_Ing_Egr_Inven_det.IdMovi_inven_tipo = in_movi_inven_tipo.IdMovi_inven_tipo ON 
                  in_Producto.IdEmpresa = in_Ing_Egr_Inven_det.IdEmpresa AND in_Producto.IdProducto = in_Ing_Egr_Inven_det.IdProducto INNER JOIN
                  tb_bodega ON in_Ing_Egr_Inven_det.IdEmpresa = tb_bodega.IdEmpresa AND in_Ing_Egr_Inven_det.IdSucursal = tb_bodega.IdSucursal AND in_Ing_Egr_Inven_det.IdBodega = tb_bodega.IdBodega INNER JOIN
                  tb_sucursal ON tb_bodega.IdEmpresa = tb_sucursal.IdEmpresa AND tb_bodega.IdSucursal = tb_sucursal.IdSucursal LEFT OUTER JOIN
                  in_Motivo_Inven ON in_Ing_Egr_Inven_det.IdMotivo_Inv = in_Motivo_Inven.IdMotivo_Inv AND in_Ing_Egr_Inven_det.IdEmpresa = in_Motivo_Inven.IdEmpresa LEFT OUTER JOIN
                  tb_persona INNER JOIN
                  cp_proveedor ON tb_persona.IdPersona = cp_proveedor.IdPersona ON in_Ing_Egr_Inven.IdEmpresa = cp_proveedor.IdEmpresa AND in_Ing_Egr_Inven.IdResponsable = cp_proveedor.IdProveedor LEFT OUTER JOIN
                  ct_CentroCosto ON in_Ing_Egr_Inven_det.IdEmpresa = ct_CentroCosto.IdEmpresa AND in_Ing_Egr_Inven_det.IdCentroCosto = ct_CentroCosto.IdCentroCosto
WHERE  (in_Ing_Egr_Inven.Estado = 'A')