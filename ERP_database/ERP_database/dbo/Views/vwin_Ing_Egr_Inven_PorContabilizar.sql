CREATE VIEW [dbo].[vwin_Ing_Egr_Inven_PorContabilizar]
AS
SELECT dbo.in_Ing_Egr_Inven_det.IdEmpresa, dbo.in_Ing_Egr_Inven_det.IdSucursal, dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo, dbo.in_Ing_Egr_Inven_det.IdNumMovi, dbo.in_Ing_Egr_Inven_det.Secuencia, 
                  dbo.in_Ing_Egr_Inven_det.IdEmpresa_inv, dbo.in_Ing_Egr_Inven_det.IdSucursal_inv, dbo.in_Ing_Egr_Inven_det.IdBodega_inv, dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo_inv, dbo.in_Ing_Egr_Inven_det.IdNumMovi_inv, 
                  dbo.in_Ing_Egr_Inven_det.secuencia_inv, dbo.in_categorias.IdCtaCtble_Inve, dbo.in_categorias.IdCtaCtble_Costo, dbo.in_Motivo_Inven.IdCtaCble AS IdCtaCble_Motivo, dbo.in_parametro.P_IdCtaCble_transitoria_transf_inven, 
                  dbo.in_Ing_Egr_Inven_det.dm_cantidad * dbo.in_Ing_Egr_Inven_det.mv_costo AS Valor, dbo.in_Ing_Egr_Inven.Estado, CASE WHEN dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo = in_parametro.IdMovi_inven_tipo_egresoBodegaOrigen OR
                  dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo = in_parametro.IdMovi_inven_tipo_ingresoBodegaDestino THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS EsTransferencia, dbo.in_Ing_Egr_Inven_det.IdCentroCosto, 
                  in_Motivo_Inven_1.IdCtaCble IdCtaCble_MotivoDet 
FROM     dbo.in_Ing_Egr_Inven_det INNER JOIN
                  dbo.in_Producto ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_Producto.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdProducto = dbo.in_Producto.IdProducto INNER JOIN
                  dbo.in_categorias ON dbo.in_Producto.IdEmpresa = dbo.in_categorias.IdEmpresa AND dbo.in_Producto.IdCategoria = dbo.in_categorias.IdCategoria INNER JOIN
                  dbo.tb_bodega ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.tb_bodega.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdSucursal = dbo.tb_bodega.IdSucursal AND 
                  dbo.in_Ing_Egr_Inven_det.IdBodega = dbo.tb_bodega.IdBodega INNER JOIN
                  dbo.in_Ing_Egr_Inven ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_Ing_Egr_Inven.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdSucursal = dbo.in_Ing_Egr_Inven.IdSucursal AND 
                  dbo.in_Ing_Egr_Inven_det.IdMovi_inven_tipo = dbo.in_Ing_Egr_Inven.IdMovi_inven_tipo AND dbo.in_Ing_Egr_Inven_det.IdNumMovi = dbo.in_Ing_Egr_Inven.IdNumMovi INNER JOIN
                  dbo.in_Motivo_Inven ON dbo.in_Ing_Egr_Inven.IdEmpresa = dbo.in_Motivo_Inven.IdEmpresa AND dbo.in_Ing_Egr_Inven.IdMotivo_Inv = dbo.in_Motivo_Inven.IdMotivo_Inv INNER JOIN
                  dbo.in_parametro ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = dbo.in_parametro.IdEmpresa LEFT OUTER JOIN
                  dbo.in_Motivo_Inven AS in_Motivo_Inven_1 ON dbo.in_Ing_Egr_Inven_det.IdEmpresa = in_Motivo_Inven_1.IdEmpresa AND dbo.in_Ing_Egr_Inven_det.IdMotivo_Inv = in_Motivo_Inven_1.IdMotivo_Inv
WHERE  (dbo.in_Ing_Egr_Inven_det.IdSucursal_inv IS NOT NULL)