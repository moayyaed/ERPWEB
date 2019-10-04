CREATE VIEW [dbo].[vwin_AjusteDet]
AS
SELECT in_AjusteDet.IdEmpresa, in_AjusteDet.IdAjuste, in_AjusteDet.Secuencia, in_AjusteDet.IdProducto, in_AjusteDet.IdUnidadMedida, in_AjusteDet.StockSistema, in_AjusteDet.StockFisico, in_AjusteDet.Ajuste, in_AjusteDet.Costo, 
                  in_Producto.pr_descripcion, in_Producto.pr_codigo, in_Producto.IdCategoria, in_Producto.IdLinea, in_categorias.ca_Categoria, in_linea.nom_linea
FROM     in_AjusteDet INNER JOIN
                  in_Producto ON in_AjusteDet.IdEmpresa = in_Producto.IdEmpresa AND in_AjusteDet.IdProducto = in_Producto.IdProducto INNER JOIN
                  in_categorias ON in_Producto.IdEmpresa = in_categorias.IdEmpresa AND in_Producto.IdCategoria = in_categorias.IdCategoria INNER JOIN
                  in_linea ON in_Producto.IdEmpresa = in_linea.IdEmpresa AND in_Producto.IdCategoria = in_linea.IdCategoria AND in_Producto.IdLinea = in_linea.IdLinea