CREATE VIEW vwcp_ConciliacionAnticipoDetAnt_x_cruzar
AS
SELECT cp_orden_pago_det.IdEmpresa, cp_orden_pago_det.IdOrdenPago, cp_orden_pago.IdSucursal,cp_orden_pago.IdEntidad IdProveedor, tb_persona.pe_nombreCompleto, cp_orden_pago.Observacion, cp_orden_pago.Fecha, 
cp_orden_pago_det.Valor_a_pagar ValorOP,ISNULL(DET.MontoAplicado,0)MontoAplicado, cp_orden_pago_det.Valor_a_pagar - ISNULL(DET.MontoAplicado,0) AS Saldo
FROM     cp_orden_pago INNER JOIN
                  cp_orden_pago_det ON cp_orden_pago.IdEmpresa = cp_orden_pago_det.IdEmpresa AND cp_orden_pago.IdOrdenPago = cp_orden_pago_det.IdOrdenPago INNER JOIN
                  cp_orden_pago_cancelaciones ON cp_orden_pago_det.IdEmpresa = cp_orden_pago_cancelaciones.IdEmpresa_op AND cp_orden_pago_det.IdOrdenPago = cp_orden_pago_cancelaciones.IdOrdenPago_op AND 
                  cp_orden_pago_det.Secuencia = cp_orden_pago_cancelaciones.Secuencia_op INNER JOIN
                  cp_orden_pago_tipo_x_empresa ON cp_orden_pago.IdTipo_op = cp_orden_pago_tipo_x_empresa.IdTipo_op AND cp_orden_pago.IdEmpresa = cp_orden_pago_tipo_x_empresa.IdEmpresa INNER JOIN
                  tb_persona ON cp_orden_pago.IdPersona = tb_persona.IdPersona LEFT JOIN 
				  (
				  SELECT D.IdEmpresa, D.IdOrdenPago, SUM(D.MontoAplicado)MontoAplicado FROM cp_ConciliacionAnticipoDetAnt AS D 
				  GROUP BY D.IdEmpresa, D.IdOrdenPago
				  ) AS DET ON cp_orden_pago.IdEmpresa = DET.IdEmpresa AND cp_orden_pago.IdOrdenPago = DET.IdOrdenPago
WHERE  (cp_orden_pago_tipo_x_empresa.Dispara_Alerta = 1) AND (cp_orden_pago.IdTipo_Persona = 'PROVEE') AND (cp_orden_pago.Estado = 'A')