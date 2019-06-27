CREATE VIEW web.VWINV_018
AS
SELECT in_AjusteDet.IdEmpresa, in_AjusteDet.IdAjuste,in_AjusteDet.Secuencia, in_Ajuste.IdSucursal, in_Ajuste.IdBodega, in_Ajuste.IdMovi_inven_tipo_ing, in_Ajuste.IdMovi_inven_tipo_egr, in_Ajuste.IdNumMovi_ing, in_Ajuste.IdNumMovi_egr, in_Ajuste.IdCatalogo_Estado, 
                  in_Ajuste.Estado, in_Ajuste.Fecha, in_Ajuste.Observacion, tb_sucursal.Su_Descripcion, tb_bodega.bo_Descripcion, movi_ing.tm_descripcion AS tm_descripcion_ing, movi_egr.tm_descripcion AS tm_descripcion_egr, 
                  in_AjusteDet.IdProducto, in_AjusteDet.IdUnidadMedida, in_AjusteDet.StockSistema, in_AjusteDet.StockFisico, in_AjusteDet.Ajuste, in_AjusteDet.Costo, in_Producto.pr_descripcion, in_UnidadMedida.Descripcion AS NomUnidadMedida, 
                  in_categorias.ca_Categoria, in_linea.nom_linea
FROM     in_UnidadMedida INNER JOIN
                  in_Ajuste INNER JOIN
                  in_AjusteDet ON in_Ajuste.IdEmpresa = in_AjusteDet.IdEmpresa AND in_Ajuste.IdAjuste = in_AjusteDet.IdAjuste INNER JOIN
                  tb_bodega ON in_Ajuste.IdEmpresa = tb_bodega.IdEmpresa AND in_Ajuste.IdEmpresa = tb_bodega.IdEmpresa AND in_Ajuste.IdSucursal = tb_bodega.IdSucursal AND in_Ajuste.IdSucursal = tb_bodega.IdSucursal AND 
                  in_Ajuste.IdBodega = tb_bodega.IdBodega AND in_Ajuste.IdBodega = tb_bodega.IdBodega INNER JOIN
                  tb_sucursal ON tb_bodega.IdEmpresa = tb_sucursal.IdEmpresa AND tb_bodega.IdSucursal = tb_sucursal.IdSucursal INNER JOIN
                  in_Producto ON in_AjusteDet.IdEmpresa = in_Producto.IdEmpresa AND in_AjusteDet.IdProducto = in_Producto.IdProducto INNER JOIN
                  in_linea INNER JOIN
                  in_categorias ON in_linea.IdEmpresa = in_categorias.IdEmpresa AND in_linea.IdCategoria = in_categorias.IdCategoria ON in_Producto.IdEmpresa = in_linea.IdEmpresa AND in_Producto.IdCategoria = in_linea.IdCategoria AND 
                  in_Producto.IdLinea = in_linea.IdLinea ON in_UnidadMedida.IdUnidadMedida = in_AjusteDet.IdUnidadMedida INNER JOIN
                  in_movi_inven_tipo AS movi_ing ON in_Ajuste.IdEmpresa = movi_ing.IdEmpresa AND in_Ajuste.IdMovi_inven_tipo_ing = movi_ing.IdMovi_inven_tipo INNER JOIN
                  in_movi_inven_tipo AS movi_egr ON in_Ajuste.IdEmpresa = movi_egr.IdEmpresa AND in_Ajuste.IdMovi_inven_tipo_egr = movi_egr.IdMovi_inven_tipo