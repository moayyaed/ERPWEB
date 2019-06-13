CREATE VIEW vwba_Archivo_Transferencia_Det
AS
SELECT ba_Archivo_Transferencia_Det.IdEmpresa, ba_Archivo_Transferencia_Det.IdArchivo, ba_Archivo_Transferencia_Det.Secuencia, ba_Archivo_Transferencia_Det.Id_Item, ba_Archivo_Transferencia_Det.IdEmpresa_OP, 
                  ba_Archivo_Transferencia_Det.IdOrdenPago, ba_Archivo_Transferencia_Det.Secuencia_OP, ba_Archivo_Transferencia_Det.Estado, ba_Archivo_Transferencia_Det.Valor, ba_Archivo_Transferencia_Det.Secuencial_reg_x_proceso, 
                  ba_Archivo_Transferencia_Det.Contabilizado, ba_Archivo_Transferencia_Det.Fecha_proceso, cp_proveedor.IdTipoCta_acreditacion_cat, cp_proveedor.num_cta_acreditacion, cp_proveedor.IdBanco_acreditacion, 
                  cp_proveedor.pr_direccion, cp_proveedor.pr_correo, tb_persona.IdTipoDocumento, tb_persona.pe_cedulaRuc, tb_persona.pe_nombreCompleto
FROM     ba_Archivo_Transferencia_Det INNER JOIN
                  cp_orden_pago_det ON ba_Archivo_Transferencia_Det.IdEmpresa_OP = cp_orden_pago_det.IdEmpresa AND ba_Archivo_Transferencia_Det.IdOrdenPago = cp_orden_pago_det.IdOrdenPago AND 
                  ba_Archivo_Transferencia_Det.Secuencia_OP = cp_orden_pago_det.Secuencia INNER JOIN
                  cp_orden_pago ON cp_orden_pago_det.IdEmpresa = cp_orden_pago.IdEmpresa AND cp_orden_pago_det.IdOrdenPago = cp_orden_pago.IdOrdenPago INNER JOIN
                  tb_persona INNER JOIN
                  cp_proveedor ON tb_persona.IdPersona = cp_proveedor.IdPersona ON cp_orden_pago.IdPersona = cp_proveedor.IdPersona AND cp_orden_pago.IdEntidad = cp_proveedor.IdProveedor AND 
                  cp_orden_pago.IdEmpresa = cp_proveedor.IdEmpresa