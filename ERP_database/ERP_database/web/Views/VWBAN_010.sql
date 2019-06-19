CREATE VIEW web.VWBAN_010
AS
SELECT dbo.ba_Archivo_Transferencia_Det.IdEmpresa, dbo.ba_Archivo_Transferencia_Det.IdArchivo, dbo.ba_Archivo_Transferencia_Det.Secuencia, dbo.ba_Archivo_Transferencia_Det.IdEmpresa_OP, 
                  dbo.ba_Archivo_Transferencia_Det.IdOrdenPago, dbo.ba_Archivo_Transferencia_Det.Secuencia_OP, dbo.ba_Archivo_Transferencia_Det.Estado, dbo.ba_Archivo_Transferencia_Det.Valor, 
                  dbo.ba_Archivo_Transferencia_Det.Secuencial_reg_x_proceso, dbo.ba_Archivo_Transferencia_Det.Contabilizado, dbo.ba_Archivo_Transferencia_Det.Fecha_proceso, dbo.cp_proveedor.IdTipoCta_acreditacion_cat, 
                  dbo.cp_proveedor.num_cta_acreditacion, dbo.cp_proveedor.IdBanco_acreditacion, dbo.cp_proveedor.pr_direccion, dbo.cp_proveedor.pr_correo, dbo.tb_persona.IdTipoDocumento, dbo.tb_persona.pe_cedulaRuc, 
                  dbo.tb_persona.pe_nombreCompleto, dbo.tb_banco.CodigoLegal AS CodigoLegalBanco, dbo.ba_Archivo_Transferencia_Det.Referencia, dbo.cp_orden_pago.IdTipo_Persona, dbo.cp_orden_pago.IdPersona, 
                  dbo.cp_orden_pago.IdEntidad, dbo.ct_cbtecble.cb_Fecha, tb_banco.ba_descripcion
FROM     dbo.ba_Archivo_Transferencia INNER JOIN
                  dbo.ba_Archivo_Transferencia_Det INNER JOIN
                  dbo.cp_orden_pago_det ON dbo.ba_Archivo_Transferencia_Det.IdEmpresa_OP = dbo.cp_orden_pago_det.IdEmpresa AND dbo.ba_Archivo_Transferencia_Det.IdOrdenPago = dbo.cp_orden_pago_det.IdOrdenPago AND 
                  dbo.ba_Archivo_Transferencia_Det.Secuencia_OP = dbo.cp_orden_pago_det.Secuencia INNER JOIN
                  dbo.cp_orden_pago ON dbo.cp_orden_pago_det.IdEmpresa = dbo.cp_orden_pago.IdEmpresa AND dbo.cp_orden_pago_det.IdOrdenPago = dbo.cp_orden_pago.IdOrdenPago INNER JOIN
                  dbo.tb_persona INNER JOIN
                  dbo.cp_proveedor ON dbo.tb_persona.IdPersona = dbo.cp_proveedor.IdPersona ON dbo.cp_orden_pago.IdPersona = dbo.cp_proveedor.IdPersona AND dbo.cp_orden_pago.IdEntidad = dbo.cp_proveedor.IdProveedor AND 
                  dbo.cp_orden_pago.IdEmpresa = dbo.cp_proveedor.IdEmpresa ON dbo.ba_Archivo_Transferencia.IdEmpresa = dbo.ba_Archivo_Transferencia_Det.IdEmpresa AND 
                  dbo.ba_Archivo_Transferencia.IdArchivo = dbo.ba_Archivo_Transferencia_Det.IdArchivo INNER JOIN
                  dbo.tb_banco ON dbo.cp_proveedor.IdBanco_acreditacion = dbo.tb_banco.IdBanco INNER JOIN
                  dbo.ct_cbtecble ON dbo.cp_orden_pago_det.IdEmpresa_cxp = dbo.ct_cbtecble.IdEmpresa AND dbo.cp_orden_pago_det.IdTipoCbte_cxp = dbo.ct_cbtecble.IdTipoCbte AND 
                  dbo.cp_orden_pago_det.IdCbteCble_cxp = dbo.ct_cbtecble.IdCbteCble