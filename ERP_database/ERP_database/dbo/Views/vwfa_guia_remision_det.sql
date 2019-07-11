CREATE VIEW dbo.vwfa_guia_remision_det
AS
SELECT        dbo.fa_guia_remision_det.IdEmpresa, dbo.fa_guia_remision_det.IdSucursal, dbo.fa_guia_remision_det.IdBodega, dbo.fa_guia_remision_det.IdGuiaRemision, 
                         dbo.fa_guia_remision_det.Secuencia, dbo.fa_guia_remision_det.IdProducto, dbo.fa_guia_remision_det.gi_cantidad,  
                           
                         
                          dbo.fa_guia_remision_det.gi_detallexItems,  dbo.in_Producto.pr_codigo, 
                         dbo.in_Producto.pr_descripcion, dbo.in_Producto.pr_descripcion_2
FROM            dbo.fa_guia_remision_det INNER JOIN
                         dbo.in_Producto ON dbo.fa_guia_remision_det.IdEmpresa = dbo.in_Producto.IdEmpresa AND dbo.fa_guia_remision_det.IdProducto = dbo.in_Producto.IdProducto

GO



GO


