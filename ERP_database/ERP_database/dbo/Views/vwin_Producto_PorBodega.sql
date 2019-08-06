CREATE VIEW [dbo].[vwin_Producto_PorBodega]
AS
SELECT in_producto_x_tb_bodega.IdEmpresa, in_producto_x_tb_bodega.IdSucursal, in_producto_x_tb_bodega.IdBodega, in_producto_x_tb_bodega.IdProducto, in_Producto.pr_codigo, in_Producto.pr_descripcion, 
                  SUM(ISNULL(in_Ing_Egr_Inven_det.dm_cantidad, 0)) AS Stock, in_categorias.ca_Categoria AS nom_categoria
FROM     in_ProductoTipo INNER JOIN
                  in_producto_x_tb_bodega INNER JOIN
                  in_Producto ON in_producto_x_tb_bodega.IdEmpresa = in_Producto.IdEmpresa AND in_producto_x_tb_bodega.IdProducto = in_Producto.IdProducto ON in_ProductoTipo.IdEmpresa = in_Producto.IdEmpresa AND 
                  in_ProductoTipo.IdProductoTipo = in_Producto.IdProductoTipo INNER JOIN
                  in_categorias ON in_Producto.IdEmpresa = in_categorias.IdEmpresa AND in_Producto.IdCategoria = in_categorias.IdCategoria LEFT OUTER JOIN
                  in_Ing_Egr_Inven INNER JOIN
                  in_Ing_Egr_Inven_det ON in_Ing_Egr_Inven.IdEmpresa = in_Ing_Egr_Inven_det.IdEmpresa AND in_Ing_Egr_Inven.IdSucursal = in_Ing_Egr_Inven_det.IdSucursal AND 
                  in_Ing_Egr_Inven.IdMovi_inven_tipo = in_Ing_Egr_Inven_det.IdMovi_inven_tipo AND in_Ing_Egr_Inven.IdNumMovi = in_Ing_Egr_Inven_det.IdNumMovi ON in_Producto.IdEmpresa = in_Ing_Egr_Inven_det.IdEmpresa AND 
                  in_Producto.IdProducto = in_Ing_Egr_Inven_det.IdProducto
WHERE  (ISNULL(in_Ing_Egr_Inven.Estado, 'A') = 'A') AND (in_Producto.Estado = 'A')
GROUP BY in_producto_x_tb_bodega.IdEmpresa, in_producto_x_tb_bodega.IdSucursal, in_producto_x_tb_bodega.IdBodega, in_producto_x_tb_bodega.IdProducto, in_Producto.pr_codigo, in_Producto.pr_descripcion, in_categorias.ca_Categoria