CREATE VIEW web.VWFAC_008_diario
AS
SELECT fa_notaCreDeb_x_ct_cbtecble.no_IdEmpresa, fa_notaCreDeb_x_ct_cbtecble.no_IdSucursal, fa_notaCreDeb_x_ct_cbtecble.no_IdBodega, fa_notaCreDeb_x_ct_cbtecble.no_IdNota, fa_notaCreDeb_x_ct_cbtecble.ct_IdEmpresa, 
                  fa_notaCreDeb_x_ct_cbtecble.ct_IdTipoCbte, fa_notaCreDeb_x_ct_cbtecble.ct_IdCbteCble, ct_cbtecble_det.secuencia, ct_cbtecble_det.IdCtaCble, ct_cbtecble_det.IdPunto_cargo_grupo, ct_cbtecble_det.IdPunto_cargo, 
                  ct_cbtecble_det.IdCentroCosto, ct_CentroCosto.cc_Descripcion, ct_punto_cargo.nom_punto_cargo, ct_punto_cargo_grupo.nom_punto_cargo_grupo, ct_plancta.pc_Cuenta, ct_cbtecble_det.dc_Valor,
				  CASE WHEN ct_cbtecble_det.dc_Valor > 0 THEN ct_cbtecble_det.dc_Valor ELSE 0 END AS dc_Valor_Debe, case when ct_cbtecble_det.dc_Valor <0 then ct_cbtecble_det.dc_Valor*-1 else 0 end as dc_Valor_Haber
FROM     ct_CentroCosto RIGHT OUTER JOIN
                  ct_plancta INNER JOIN
                  ct_cbtecble_det ON ct_plancta.IdEmpresa = ct_cbtecble_det.IdEmpresa AND ct_plancta.IdCtaCble = ct_cbtecble_det.IdCtaCble INNER JOIN
                  fa_notaCreDeb_x_ct_cbtecble ON ct_cbtecble_det.IdEmpresa = fa_notaCreDeb_x_ct_cbtecble.ct_IdEmpresa AND ct_cbtecble_det.IdTipoCbte = fa_notaCreDeb_x_ct_cbtecble.ct_IdTipoCbte AND 
                  ct_cbtecble_det.IdCbteCble = fa_notaCreDeb_x_ct_cbtecble.ct_IdCbteCble ON ct_CentroCosto.IdEmpresa = ct_cbtecble_det.IdEmpresa AND ct_CentroCosto.IdCentroCosto = ct_cbtecble_det.IdCentroCosto LEFT OUTER JOIN
                  ct_punto_cargo_grupo INNER JOIN
                  ct_punto_cargo ON ct_punto_cargo_grupo.IdEmpresa = ct_punto_cargo.IdEmpresa AND ct_punto_cargo_grupo.IdPunto_cargo_grupo = ct_punto_cargo.IdPunto_cargo_grupo ON 
                  ct_cbtecble_det.IdEmpresa = ct_punto_cargo.IdEmpresa AND ct_cbtecble_det.IdPunto_cargo = ct_punto_cargo.IdPunto_cargo