CREATE VIEW web.VWCXP_005
AS
SELECT cp_ConciliacionAnticipo.IdEmpresa, cp_ConciliacionAnticipo.IdConciliacion, cp_ConciliacionAnticipo.IdTipoCbte, cp_ConciliacionAnticipo.IdCbteCble, ct_cbtecble_det.secuencia, ct_cbtecble_det.IdCtaCble, ct_plancta.pc_Cuenta, 
                  ct_cbtecble_det.dc_Valor, CASE WHEN ct_cbtecble_det.dc_Valor > 0 THEN ct_cbtecble_det.dc_Valor ELSE 0 END AS dc_Valor_Debe, CASE WHEN ct_cbtecble_det.dc_Valor < 0 THEN ABS(ct_cbtecble_det.dc_Valor) 
                  ELSE 0 END AS dc_Valor_Haber, cp_ConciliacionAnticipo.Fecha, cp_ConciliacionAnticipo.Observacion, cp_ConciliacionAnticipo.Estado, seg_usuario.Nombre AS NomUsuario
FROM     cp_ConciliacionAnticipo INNER JOIN
                  ct_plancta INNER JOIN
                  ct_cbtecble_det ON ct_plancta.IdEmpresa = ct_cbtecble_det.IdEmpresa AND ct_plancta.IdCtaCble = ct_cbtecble_det.IdCtaCble ON cp_ConciliacionAnticipo.IdEmpresa = ct_cbtecble_det.IdEmpresa AND 
                  cp_ConciliacionAnticipo.IdTipoCbte = ct_cbtecble_det.IdTipoCbte AND cp_ConciliacionAnticipo.IdCbteCble = ct_cbtecble_det.IdCbteCble LEFT OUTER JOIN
                  seg_usuario ON cp_ConciliacionAnticipo.IdUsuarioCreacion = seg_usuario.IdUsuario