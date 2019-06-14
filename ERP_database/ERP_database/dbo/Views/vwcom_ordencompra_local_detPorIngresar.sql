CREATE view vwcom_ordencompra_local_detPorIngresar as
SELECT        ocd.IdEmpresa, ocd.IdSucursal, ocd.IdOrdenCompra, ocd.Secuencia, ocd.IdProducto, p.pr_descripcion, ocd.do_Cantidad, ocd.do_precioFinal, ISNULL(SUM(invd.dm_cantidad_sinConversion), 0) AS CantidadIngresada, 
                         ocd.do_Cantidad - ISNULL(SUM(invd.dm_cantidad_sinConversion), 0) AS Saldo, ocd.IdUnidadMedida, per.pe_nombreCompleto, oc.oc_observacion, oc.oc_fecha, oc.IdProveedor
FROM            dbo.com_ordencompra_local_det AS ocd LEFT OUTER JOIN
                         dbo.in_Ing_Egr_Inven_det AS invd ON ocd.IdEmpresa = invd.IdEmpresa_oc AND ocd.IdSucursal = invd.IdSucursal_oc AND ocd.IdOrdenCompra = invd.IdOrdenCompra AND ocd.Secuencia = invd.Secuencia_oc INNER JOIN
                         dbo.in_Producto AS p ON p.IdEmpresa = ocd.IdEmpresa AND p.IdProducto = ocd.IdProducto INNER JOIN
                         dbo.com_ordencompra_local AS oc ON oc.IdEmpresa = ocd.IdEmpresa AND oc.IdSucursal = ocd.IdSucursal AND oc.IdOrdenCompra = ocd.IdOrdenCompra INNER JOIN
                         dbo.cp_proveedor AS pro ON pro.IdEmpresa = oc.IdEmpresa AND pro.IdProveedor = oc.IdProveedor INNER JOIN
                         dbo.tb_persona AS per ON per.IdPersona = pro.IdPersona
WHERE        (oc.Estado = 'A')
GROUP BY ocd.IdEmpresa, ocd.IdSucursal, ocd.IdOrdenCompra, ocd.Secuencia, ocd.IdProducto, p.pr_descripcion, ocd.do_Cantidad, ocd.do_precioFinal, ocd.IdUnidadMedida, per.pe_nombreCompleto, oc.oc_observacion, oc.oc_fecha, 
                         oc.IdProveedor
HAVING        (ocd.do_Cantidad - ISNULL(SUM(invd.dm_cantidad_sinConversion), 0) <> 0)