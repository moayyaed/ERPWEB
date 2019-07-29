create view vwfa_proforma_det_por_guia as
SELECT        dbo.fa_guia_remision_det.IdEmpresa, dbo.fa_guia_remision_det.IdSucursal, dbo.fa_guia_remision_det.IdProforma, dbo.fa_guia_remision_det.Secuencia, 
dbo.fa_guia_remision_det.IdProducto, dbo.fa_guia_remision_det.gi_cantidad, 
                         dbo.fa_guia_remision_det.gi_precio, dbo.fa_guia_remision_det.gi_por_desc, dbo.fa_guia_remision_det.gi_descuentoUni, dbo.fa_guia_remision_det.gi_PrecioFinal, 
                         dbo.fa_guia_remision_det.IdCod_Impuesto, dbo.fa_guia_remision_det.gi_por_iva, dbo.fa_guia_remision_det.gi_Iva, dbo.fa_guia_remision_det.gi_Total, dbo.fa_guia_remision_det.gi_Subtotal, in_Producto_1.pr_descripcion, 
                         dbo.in_presentacion.nom_presentacion, in_Producto_1.lote_num_lote, in_Producto_1.lote_fecha_vcto, dbo.fa_proforma.IdCliente, in_Producto_1.se_distribuye, dbo.in_ProductoTipo.tp_ManejaInven
FROM            dbo.in_presentacion INNER JOIN
                         dbo.in_Producto AS in_Producto_1 ON dbo.in_presentacion.IdEmpresa = in_Producto_1.IdEmpresa AND dbo.in_presentacion.IdPresentacion = in_Producto_1.IdPresentacion INNER JOIN
                         dbo.in_ProductoTipo ON in_Producto_1.IdProductoTipo = dbo.in_ProductoTipo.IdProductoTipo AND in_Producto_1.IdEmpresa = dbo.in_ProductoTipo.IdEmpresa RIGHT OUTER JOIN
                         dbo.fa_proforma INNER JOIN
                         dbo.fa_guia_remision_det ON dbo.fa_proforma.IdEmpresa = dbo.fa_guia_remision_det.IdEmpresa AND dbo.fa_proforma.IdSucursal = dbo.fa_guia_remision_det.IdSucursal
						  AND dbo.fa_proforma.IdProforma = dbo.fa_guia_remision_det.IdProforma ON 
                         in_Producto_1.IdEmpresa = dbo.fa_guia_remision_det.IdEmpresa AND in_Producto_1.IdProducto = dbo.fa_guia_remision_det.IdProducto
WHERE        (NOT EXISTS
                             (SELECT        IdEmpresa
                               FROM            dbo.fa_guia_remision_det AS f
                               WHERE        (dbo.fa_guia_remision_det.IdEmpresa = IdEmpresa_pf) AND (dbo.fa_guia_remision_det.IdSucursal = IdSucursal_pf) AND (dbo.fa_guia_remision_det.IdProforma = IdProforma) AND (dbo.fa_guia_remision_det.Secuencia = Secuencia_pf)))
                          AND (dbo.fa_proforma.estado = 1)