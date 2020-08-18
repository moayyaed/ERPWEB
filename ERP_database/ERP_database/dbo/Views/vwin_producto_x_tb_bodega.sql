CREATE VIEW [dbo].[vwin_producto_x_tb_bodega]
AS
SELECT dbo.in_producto_x_tb_bodega.IdEmpresa, dbo.in_producto_x_tb_bodega.IdSucursal, dbo.in_producto_x_tb_bodega.IdBodega, dbo.in_producto_x_tb_bodega.IdProducto, dbo.in_producto_x_tb_bodega.Stock_minimo, 
                  dbo.in_Producto.pr_descripcion, dbo.tb_bodega.bo_Descripcion, dbo.tb_sucursal.Su_Descripcion, dbo.in_categorias.ca_Categoria, dbo.in_Producto.pr_codigo, dbo.in_producto_x_tb_bodega.IdCtaCble_Costo, 
                  dbo.in_producto_x_tb_bodega.IdCtaCble_Costo + ' - ' + dbo.ct_plancta.pc_Cuenta AS pc_Cuenta, dbo.in_producto_x_tb_bodega.IdCtaCble_Inven, 
                  dbo.in_producto_x_tb_bodega.IdCtaCble_Inven + ' - ' + ct_plancta_1.pc_Cuenta AS pc_Cuenta_Inv, dbo.in_producto_x_tb_bodega.IdCtaCble_Vta,
				  dbo.in_producto_x_tb_bodega.IdCtaCble_Vta + ' - ' + ct.pc_Cuenta AS pc_Cuenta_Vta
FROM     dbo.in_producto_x_tb_bodega INNER JOIN
                  dbo.in_Producto ON dbo.in_producto_x_tb_bodega.IdEmpresa = dbo.in_Producto.IdEmpresa AND dbo.in_producto_x_tb_bodega.IdProducto = dbo.in_Producto.IdProducto INNER JOIN
                  dbo.tb_sucursal INNER JOIN
                  dbo.tb_bodega ON dbo.tb_sucursal.IdEmpresa = dbo.tb_bodega.IdEmpresa AND dbo.tb_sucursal.IdSucursal = dbo.tb_bodega.IdSucursal ON dbo.in_producto_x_tb_bodega.IdEmpresa = dbo.tb_bodega.IdEmpresa AND 
                  dbo.in_producto_x_tb_bodega.IdSucursal = dbo.tb_bodega.IdSucursal AND dbo.in_producto_x_tb_bodega.IdBodega = dbo.tb_bodega.IdBodega INNER JOIN
                  dbo.in_categorias ON dbo.in_Producto.IdEmpresa = dbo.in_categorias.IdEmpresa AND dbo.in_Producto.IdCategoria = dbo.in_categorias.IdCategoria LEFT OUTER JOIN
                  dbo.ct_plancta AS ct_plancta_1 ON dbo.in_producto_x_tb_bodega.IdCtaCble_Inven = ct_plancta_1.IdCtaCble AND dbo.in_producto_x_tb_bodega.IdEmpresa = ct_plancta_1.IdEmpresa LEFT OUTER JOIN
                  dbo.ct_plancta ON dbo.in_producto_x_tb_bodega.IdCtaCble_Costo = dbo.ct_plancta.IdCtaCble AND dbo.in_producto_x_tb_bodega.IdEmpresa = dbo.ct_plancta.IdEmpresa left join
				  ct_plancta as ct on in_producto_x_tb_bodega.IdEmpresa = ct.IdEmpresa and in_producto_x_tb_bodega.IdCtaCble_Vta = ct.IdCtaCble
WHERE  (dbo.in_Producto.Estado = 'A')