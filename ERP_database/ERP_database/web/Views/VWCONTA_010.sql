CREATE VIEW web.VWCONTA_010
AS
SELECT ct_cbtecble_det.IdEmpresa, ct_cbtecble_det.IdTipoCbte, ct_cbtecble_det.IdCbteCble, ct_cbtecble_det.secuencia, ct_cbtecble_det.IdCtaCble, ct_cbtecble.cb_Fecha, ISNULL(ct_cbtecble.cb_Observacion,'')+ ' || ' +ISNULL(ct_cbtecble_det.dc_Observacion,'') AS cb_Observacion, 
                  ct_cbtecble_det.IdCentroCosto, ct_CentroCosto.cc_Descripcion, '['+ct_cbtecble_det.IdCtaCble+'] ' +ct_plancta.pc_Cuenta pc_Cuenta, ct_plancta.IdGrupoCble, ct_grupocble.gc_GrupoCble, ct_cbtecble_tipo.tc_TipoCbte,
				  case when ct_cbtecble_det.dc_Valor > 0 then ct_cbtecble_det.dc_Valor else 0 end as Debe, case when ct_cbtecble_det.dc_Valor < 0 then ABS(ct_cbtecble_det.dc_Valor) else 0 end as Haber
FROM     ct_cbtecble INNER JOIN
                  ct_cbtecble_det ON ct_cbtecble.IdEmpresa = ct_cbtecble_det.IdEmpresa AND ct_cbtecble.IdTipoCbte = ct_cbtecble_det.IdTipoCbte AND ct_cbtecble.IdCbteCble = ct_cbtecble_det.IdCbteCble INNER JOIN
                  ct_plancta ON ct_cbtecble_det.IdEmpresa = ct_plancta.IdEmpresa AND ct_cbtecble_det.IdCtaCble = ct_plancta.IdCtaCble INNER JOIN
                  ct_grupocble ON ct_plancta.IdGrupoCble = ct_grupocble.IdGrupoCble INNER JOIN
                  ct_cbtecble_tipo ON ct_cbtecble.IdEmpresa = ct_cbtecble_tipo.IdEmpresa AND ct_cbtecble.IdTipoCbte = ct_cbtecble_tipo.IdTipoCbte LEFT OUTER JOIN
                  ct_CentroCosto ON ct_cbtecble_det.IdEmpresa = ct_CentroCosto.IdEmpresa AND ct_cbtecble_det.IdCentroCosto = ct_CentroCosto.IdCentroCosto