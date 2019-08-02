CREATE VIEW [web].[VWFAC_013_diario]
AS
SELECT fa_factura_x_ct_cbtecble.vt_IdEmpresa, fa_factura_x_ct_cbtecble.vt_IdSucursal, fa_factura_x_ct_cbtecble.vt_IdBodega, fa_factura_x_ct_cbtecble.vt_IdCbteVta, fa_factura_x_ct_cbtecble.ct_IdEmpresa, 
                  fa_factura_x_ct_cbtecble.ct_IdTipoCbte, fa_factura_x_ct_cbtecble.ct_IdCbteCble, ct_cbtecble_det.secuencia, ct_cbtecble_det.IdCtaCble, ct_cbtecble_det.IdCentroCosto, ct_plancta.pc_Cuenta, ct_CentroCosto.cc_Descripcion,
				  ct_cbtecble_det.dc_Valor, CASE WHEN ct_cbtecble_det.dc_Valor > 0 THEN ct_cbtecble_det.dc_Valor ELSE 0 END AS dc_Valor_Debe,
				  CASE WHEN ct_cbtecble_det.dc_Valor < 0 THEN ct_cbtecble_det.dc_Valor*-1 ELSE 0 END AS dc_Valor_Haber
FROM     ct_plancta INNER JOIN
                  ct_cbtecble_det ON ct_plancta.IdEmpresa = ct_cbtecble_det.IdEmpresa AND ct_plancta.IdCtaCble = ct_cbtecble_det.IdCtaCble INNER JOIN
                  fa_factura_x_ct_cbtecble ON ct_cbtecble_det.IdEmpresa = fa_factura_x_ct_cbtecble.ct_IdEmpresa AND ct_cbtecble_det.IdTipoCbte = fa_factura_x_ct_cbtecble.ct_IdTipoCbte AND 
                  ct_cbtecble_det.IdCbteCble = fa_factura_x_ct_cbtecble.ct_IdCbteCble LEFT OUTER JOIN
                  ct_CentroCosto ON ct_cbtecble_det.IdEmpresa = ct_CentroCosto.IdEmpresa AND ct_cbtecble_det.IdCentroCosto = ct_CentroCosto.IdCentroCosto