CREATE VIEW vwcxc_LiquidacionRetProvDet_PorCruzar
AS
SELECT        cxc_cobro_det.IdEmpresa, cxc_cobro_det.IdSucursal, cxc_cobro_det.IdCobro, cxc_cobro_det.secuencial, cxc_cobro_det.IdCobro_tipo, cxc_cobro_det.dc_ValorPago, cxc_cobro_tipo.tc_descripcion, cxc_cobro.cr_fecha, 
                         cxc_cobro.cr_observacion, cxc_cobro.cr_EsProvision, cxc_cobro.cr_estado, cxc_cobro_tipo_Param_conta_x_sucursal.IdCtaCble, cxc_cobro_tipo_Param_conta_x_sucursal.IdCtaCble + ' - ' + ct_plancta.pc_Cuenta AS pc_Cuenta, 
                         cxc_cobro_tipo.ESRetenIVA, cxc_cobro_tipo.ESRetenFTE, cxc_cobro.cr_NumDocumento, cxc_LiquidacionRetProvDet.IdLiquidacion
FROM            cxc_cobro_tipo INNER JOIN
                         cxc_cobro_det ON cxc_cobro_tipo.IdCobro_tipo = cxc_cobro_det.IdCobro_tipo INNER JOIN
                         cxc_cobro ON cxc_cobro_det.IdEmpresa = cxc_cobro.IdEmpresa AND cxc_cobro_det.IdSucursal = cxc_cobro.IdSucursal AND cxc_cobro_det.IdCobro = cxc_cobro.IdCobro INNER JOIN
                         cxc_cobro_tipo_Param_conta_x_sucursal ON cxc_cobro_tipo.IdCobro_tipo = cxc_cobro_tipo_Param_conta_x_sucursal.IdCobro_tipo INNER JOIN
                         ct_plancta ON cxc_cobro_tipo_Param_conta_x_sucursal.IdEmpresa = ct_plancta.IdEmpresa AND cxc_cobro_tipo_Param_conta_x_sucursal.IdEmpresa = ct_plancta.IdEmpresa AND 
                         cxc_cobro_tipo_Param_conta_x_sucursal.IdCtaCble = ct_plancta.IdCtaCble LEFT OUTER JOIN
                         cxc_LiquidacionRetProvDet ON cxc_cobro_det.IdEmpresa = cxc_LiquidacionRetProvDet.IdEmpresa AND cxc_cobro_det.IdSucursal = cxc_LiquidacionRetProvDet.IdSucursal AND 
                         cxc_cobro_det.IdCobro = cxc_LiquidacionRetProvDet.IdCobro AND cxc_cobro_det.secuencial = cxc_LiquidacionRetProvDet.secuencial
WHERE        (cxc_cobro.cr_estado = N'A') AND (cxc_LiquidacionRetProvDet.IdLiquidacion IS NULL) AND (cxc_cobro.cr_EsProvision = 1)