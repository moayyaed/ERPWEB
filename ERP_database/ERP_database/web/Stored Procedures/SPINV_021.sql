CREATE PROCEDURE web.SPINV_021
(
@IdEmpresa int,
@IdSucursalIni int,
@IdSucursalFin int,
@IdBodegaIni int,
@IdBodegaFin int,
@FechaCorte date
)
AS
SELECT dbo.in_Ing_Egr_Inven.IdEmpresa, dbo.in_Ing_Egr_Inven.IdSucursal, dbo.in_Ing_Egr_Inven.IdMovi_inven_tipo, dbo.in_Ing_Egr_Inven.IdNumMovi, dbo.in_movi_inven_tipo.tm_descripcion, dbo.tb_persona.pe_nombreCompleto, 
                  dbo.in_Ing_Egr_Inven.signo, dbo.in_Ing_Egr_Inven.cm_fecha, dbo.in_Ing_Egr_Inven.cm_observacion, dbo.in_Ing_Egr_Inven.Estado, dbo.in_Ing_Egr_Inven.IdBodega, dbo.tb_bodega.bo_Descripcion, dbo.tb_sucursal.Su_Descripcion, 
                  dbo.in_Ing_Egr_Inven.CodMoviInven, dbo.in_Motivo_Inven.Desc_mov_inv, dbo.in_Ing_Egr_Inven.IdMotivo_Inv, dbo.in_Ing_Egr_Inven.IdEstadoAproba, dbo.in_Catalogo.Nombre AS EstadoAprobacion, og.co_factura,co_FechaContabilizacion
FROM     dbo.in_Motivo_Inven INNER JOIN
                  dbo.tb_persona INNER JOIN
                  dbo.cp_proveedor ON dbo.tb_persona.IdPersona = dbo.cp_proveedor.IdPersona INNER JOIN
                  dbo.in_Ing_Egr_Inven INNER JOIN
                  dbo.com_parametro ON dbo.in_Ing_Egr_Inven.IdEmpresa = dbo.com_parametro.IdEmpresa AND dbo.in_Ing_Egr_Inven.IdMovi_inven_tipo = dbo.com_parametro.IdMovi_inven_tipo_OC ON 
                  dbo.cp_proveedor.IdEmpresa = dbo.in_Ing_Egr_Inven.IdEmpresa AND dbo.cp_proveedor.IdProveedor = dbo.in_Ing_Egr_Inven.IdResponsable INNER JOIN
                  dbo.in_movi_inven_tipo ON dbo.in_Ing_Egr_Inven.IdEmpresa = dbo.in_movi_inven_tipo.IdEmpresa AND dbo.in_Ing_Egr_Inven.IdMovi_inven_tipo = dbo.in_movi_inven_tipo.IdMovi_inven_tipo ON 
                  dbo.in_Motivo_Inven.IdEmpresa = dbo.in_Ing_Egr_Inven.IdEmpresa AND dbo.in_Motivo_Inven.IdMotivo_Inv = dbo.in_Ing_Egr_Inven.IdMotivo_Inv LEFT OUTER JOIN
                  dbo.in_Catalogo ON dbo.in_Ing_Egr_Inven.IdEstadoAproba = dbo.in_Catalogo.IdCatalogo LEFT OUTER JOIN
                  dbo.tb_sucursal INNER JOIN
                  dbo.tb_bodega ON dbo.tb_sucursal.IdEmpresa = dbo.tb_bodega.IdEmpresa AND dbo.tb_sucursal.IdSucursal = dbo.tb_bodega.IdSucursal ON dbo.in_Ing_Egr_Inven.IdEmpresa = dbo.tb_bodega.IdEmpresa AND 
                  dbo.in_Ing_Egr_Inven.IdSucursal = dbo.tb_bodega.IdSucursal AND dbo.in_Ing_Egr_Inven.IdBodega = dbo.tb_bodega.IdBodega LEFT OUTER JOIN
                      (SELECT d.IdEmpresa, d.inv_IdSucursal, d.inv_IdMovi_inven_tipo, d.inv_IdNumMovi, MAX(c.co_factura) AS co_factura, max(c.co_FechaContabilizacion) co_FechaContabilizacion
                       FROM      dbo.cp_orden_giro_det_ing_x_oc AS d INNER JOIN
                                         dbo.cp_orden_giro AS c ON c.IdEmpresa = d.IdEmpresa AND c.IdTipoCbte_Ogiro = d.IdTipoCbte_Ogiro AND c.IdCbteCble_Ogiro = d.IdCbteCble_Ogiro
										 where c.IdEmpresa = @IdEmpresa and c.co_FechaContabilizacion <= @FechaCorte
                       GROUP BY d.IdEmpresa, d.inv_IdSucursal, d.inv_IdMovi_inven_tipo, d.inv_IdNumMovi) AS og ON og.IdEmpresa = dbo.in_Ing_Egr_Inven.IdEmpresa AND og.inv_IdSucursal = dbo.in_Ing_Egr_Inven.IdSucursal AND 
                  og.inv_IdMovi_inven_tipo = dbo.in_Ing_Egr_Inven.IdMovi_inven_tipo AND og.inv_IdNumMovi = dbo.in_Ing_Egr_Inven.IdNumMovi
where in_Ing_Egr_Inven.IdEmpresa = @IdEmpresa and in_Ing_Egr_Inven.IdSucursal between @IdSucursalIni and @IdSucursalFin and in_Ing_Egr_Inven.IdBodega between @IdBodegaIni and @IdBodegaFin and 
in_Ing_Egr_Inven.cm_fecha <= @FechaCorte and in_Ing_Egr_Inven.Estado = 'A' and co_factura is null