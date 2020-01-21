CREATE VIEW [web].[VWCONTA_013]
AS
SELECT        dbo.ct_cbtecble_det.IdEmpresa, dbo.ct_cbtecble_det.IdTipoCbte, dbo.ct_cbtecble_det.IdCbteCble, dbo.ct_cbtecble_det.secuencia, dbo.ct_cbtecble_det.IdCtaCble, dbo.ct_plancta.pc_Cuenta, dbo.ct_cbtecble_det.dc_Valor, 
                         dbo.ct_cbtecble.cb_Observacion, dbo.ct_cbtecble_det.IdPunto_cargo_grupo, dbo.ct_cbtecble_det.IdPunto_cargo, dbo.ct_punto_cargo.nom_punto_cargo, dbo.ct_punto_cargo_grupo.nom_punto_cargo_grupo, 
                         dbo.ct_cbtecble.cb_Fecha, dbo.ct_cbtecble_tipo.tc_TipoCbte, '[' + dbo.ct_cbtecble_det.IdCtaCble + '] ' + dbo.ct_plancta.pc_Cuenta AS TituloGrupo, 'TOTAL ' + dbo.ct_plancta.pc_Cuenta AS TituloTotalGrupo, 
                         '[' + ISNULL(CAST(dbo.ct_cbtecble_det.IdPunto_cargo_grupo AS varchar), '') + '] ' + dbo.ct_punto_cargo_grupo.nom_punto_cargo_grupo AS nom_punto_cargo_grupoFiltro, 'TOTAL '+dbo.ct_punto_cargo_grupo.nom_punto_cargo_grupo TotalFinal
FROM            dbo.ct_cbtecble_det INNER JOIN
                         dbo.ct_cbtecble ON dbo.ct_cbtecble_det.IdEmpresa = dbo.ct_cbtecble.IdEmpresa AND dbo.ct_cbtecble_det.IdTipoCbte = dbo.ct_cbtecble.IdTipoCbte AND dbo.ct_cbtecble_det.IdCbteCble = dbo.ct_cbtecble.IdCbteCble INNER JOIN
                         dbo.ct_plancta ON dbo.ct_cbtecble_det.IdEmpresa = dbo.ct_plancta.IdEmpresa AND dbo.ct_cbtecble_det.IdCtaCble = dbo.ct_plancta.IdCtaCble INNER JOIN
                         dbo.ct_cbtecble_tipo ON dbo.ct_cbtecble.IdEmpresa = dbo.ct_cbtecble_tipo.IdEmpresa AND dbo.ct_cbtecble.IdTipoCbte = dbo.ct_cbtecble_tipo.IdTipoCbte LEFT OUTER JOIN
                         dbo.ct_punto_cargo ON dbo.ct_cbtecble_det.IdEmpresa = dbo.ct_punto_cargo.IdEmpresa AND dbo.ct_cbtecble_det.IdPunto_cargo = dbo.ct_punto_cargo.IdPunto_cargo LEFT OUTER JOIN
                         dbo.ct_punto_cargo_grupo ON dbo.ct_cbtecble_det.IdEmpresa = dbo.ct_punto_cargo_grupo.IdEmpresa AND dbo.ct_cbtecble_det.IdPunto_cargo_grupo = dbo.ct_punto_cargo_grupo.IdPunto_cargo_grupo
WHERE        (dbo.ct_cbtecble_det.IdPunto_cargo_grupo IS NOT NULL)