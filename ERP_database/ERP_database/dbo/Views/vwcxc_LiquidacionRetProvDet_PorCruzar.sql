CREATE VIEW [dbo].[vwcxc_LiquidacionRetProvDet_PorCruzar]
AS
SELECT dbo.cxc_cobro_det.IdEmpresa, dbo.cxc_cobro_det.IdSucursal, dbo.cxc_cobro_det.IdCobro, dbo.cxc_cobro_det.secuencial, dbo.cxc_cobro_det.IdCobro_tipo, dbo.cxc_cobro_det.dc_ValorPago, dbo.cxc_cobro_tipo.tc_descripcion, 
                  dbo.cxc_cobro.cr_fecha, dbo.cxc_cobro.cr_observacion, dbo.cxc_cobro.cr_EsProvision, dbo.cxc_cobro.cr_estado, dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdCtaCble, 
                  dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdCtaCble + ' - ' + dbo.ct_plancta.pc_Cuenta AS pc_Cuenta, dbo.cxc_cobro_tipo.ESRetenIVA, dbo.cxc_cobro_tipo.ESRetenFTE, dbo.cxc_cobro.cr_NumDocumento, 
                  dbo.cxc_LiquidacionRetProvDet.IdLiquidacion,   CASE WHEN cxc_cobro_det.dc_TipoDocumento = 'FACT' THEN fa_factura.vt_serie1 + '-' + fa_factura.vt_serie2 + '-' + fa_factura.vt_NumFactura ELSE CASE WHEN fa_notaCreDeb.NaturalezaNota = 'INT' THEN ISNULL(fa_notaCreDeb.CodNota,
                   CAST(fa_notaCreDeb.IdNota AS VARCHAR(20))) END END AS vt_NumFactura, dbo.tb_persona.pe_nombreCompleto
FROM     dbo.cxc_cobro_tipo INNER JOIN
                  dbo.cxc_cobro_det ON dbo.cxc_cobro_tipo.IdCobro_tipo = dbo.cxc_cobro_det.IdCobro_tipo INNER JOIN
                  dbo.cxc_cobro ON dbo.cxc_cobro_det.IdEmpresa = dbo.cxc_cobro.IdEmpresa AND dbo.cxc_cobro_det.IdSucursal = dbo.cxc_cobro.IdSucursal AND dbo.cxc_cobro_det.IdCobro = dbo.cxc_cobro.IdCobro INNER JOIN
                  dbo.cxc_cobro_tipo_Param_conta_x_sucursal ON dbo.cxc_cobro_tipo.IdCobro_tipo = dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdCobro_tipo INNER JOIN
                  dbo.ct_plancta ON dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdEmpresa = dbo.ct_plancta.IdEmpresa AND dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdEmpresa = dbo.ct_plancta.IdEmpresa AND 
                  dbo.cxc_cobro_tipo_Param_conta_x_sucursal.IdCtaCble = dbo.ct_plancta.IdCtaCble LEFT OUTER JOIN
                  dbo.cxc_LiquidacionRetProvDet ON dbo.cxc_cobro_det.IdEmpresa = dbo.cxc_LiquidacionRetProvDet.IdEmpresa AND dbo.cxc_cobro_det.IdSucursal = dbo.cxc_LiquidacionRetProvDet.IdSucursal AND 
                  dbo.cxc_cobro_det.IdCobro = dbo.cxc_LiquidacionRetProvDet.IdCobro AND dbo.cxc_cobro_det.secuencial = dbo.cxc_LiquidacionRetProvDet.secuencial INNER JOIN
                  dbo.fa_cliente ON dbo.cxc_cobro.IdEmpresa = dbo.fa_cliente.IdEmpresa AND dbo.cxc_cobro.IdCliente = dbo.fa_cliente.IdCliente INNER JOIN
                  dbo.tb_persona ON dbo.fa_cliente.IdPersona = dbo.tb_persona.IdPersona LEFT OUTER JOIN
                  dbo.fa_factura ON dbo.cxc_cobro_det.IdEmpresa = dbo.fa_factura.IdEmpresa AND dbo.cxc_cobro_det.IdSucursal = dbo.fa_factura.IdSucursal AND dbo.cxc_cobro_det.IdBodega_Cbte = dbo.fa_factura.IdBodega AND 
                  dbo.cxc_cobro_det.IdCbte_vta_nota = dbo.fa_factura.IdCbteVta AND dbo.cxc_cobro_det.dc_TipoDocumento = dbo.fa_factura.vt_tipoDoc LEFT OUTER JOIN
                  dbo.fa_notaCreDeb ON dbo.cxc_cobro_det.IdEmpresa = dbo.fa_notaCreDeb.IdEmpresa AND dbo.cxc_cobro_det.IdSucursal = dbo.fa_notaCreDeb.IdSucursal AND dbo.cxc_cobro_det.IdBodega_Cbte = dbo.fa_notaCreDeb.IdBodega AND 
                  dbo.cxc_cobro_det.IdCbte_vta_nota = dbo.fa_notaCreDeb.IdNota AND dbo.cxc_cobro_det.dc_TipoDocumento = dbo.fa_notaCreDeb.CodDocumentoTipo
WHERE  (dbo.cxc_cobro.cr_estado = N'A') AND (dbo.cxc_LiquidacionRetProvDet.IdLiquidacion IS NULL) AND (dbo.cxc_cobro.cr_EsProvision = 1)