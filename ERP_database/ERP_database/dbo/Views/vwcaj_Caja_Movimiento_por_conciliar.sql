CREATE VIEW [dbo].[vwcaj_Caja_Movimiento_por_conciliar]
AS
SELECT dbo.caj_Caja_Movimiento.IdEmpresa, dbo.caj_Caja_Movimiento.IdTipocbte, dbo.caj_Caja_Movimiento.IdCbteCble, dbo.caj_Caja_Movimiento.IdCaja, dbo.caj_Caja_Movimiento.IdTipoMovi, dbo.caj_Caja_Movimiento_Tipo.tm_descripcion, 
                  dbo.caj_Caja_Movimiento.cm_valor, dbo.caj_Caja_Movimiento.cm_observacion, dbo.caj_Caja_Movimiento.cm_fecha, dbo.tb_persona.pe_nombreCompleto, dbo.caj_Caja_Movimiento.IdTipo_Persona, dbo.caj_Caja_Movimiento.IdEntidad, 
                  dbo.caj_Caja_Movimiento.IdPersona, DET.IdPunto_cargo_grupo, DET.IdPunto_cargo, DET.IdCentroCosto, DET.nom_punto_cargo_grupo, DET.nom_punto_cargo, DET.cc_Descripcion
FROM     dbo.caj_Caja_Movimiento INNER JOIN
                  dbo.caj_Caja_Movimiento_Tipo ON dbo.caj_Caja_Movimiento.IdEmpresa = dbo.caj_Caja_Movimiento_Tipo.IdEmpresa AND dbo.caj_Caja_Movimiento.IdTipoMovi = dbo.caj_Caja_Movimiento_Tipo.IdTipoMovi INNER JOIN
                  dbo.caj_Caja ON dbo.caj_Caja_Movimiento.IdEmpresa = dbo.caj_Caja.IdEmpresa AND dbo.caj_Caja_Movimiento.IdCaja = dbo.caj_Caja.IdCaja INNER JOIN
                  dbo.tb_persona ON dbo.caj_Caja_Movimiento.IdPersona = dbo.tb_persona.IdPersona LEFT JOIN
				  (
					SELECT D.IdEmpresa, D.IdTipoCbte, D.IdCbteCble, D.IdPunto_cargo, D.IdPunto_cargo_grupo, D.IdCentroCosto, CEN.cc_Descripcion, PC.nom_punto_cargo, PG.nom_punto_cargo_grupo
					FROM     dbo.ct_cbtecble_det AS D INNER JOIN
					dbo.caj_Caja_Movimiento AS CM ON D.IdEmpresa = CM.IdEmpresa AND D.IdTipoCbte = CM.IdTipocbte AND D.IdCbteCble = CM.IdCbteCble LEFT OUTER JOIN
					dbo.caj_Caja AS CA ON CM.IdCaja = CA.IdCaja AND CM.IdEmpresa = CA.IdEmpresa LEFT OUTER JOIN
					dbo.ct_CentroCosto AS CEN ON D.IdEmpresa = CEN.IdEmpresa AND D.IdCentroCosto = CEN.IdCentroCosto LEFT OUTER JOIN
					dbo.ct_punto_cargo AS PC ON D.IdEmpresa = PC.IdEmpresa AND D.IdPunto_cargo = PC.IdPunto_cargo LEFT OUTER JOIN
					dbo.ct_punto_cargo_grupo AS PG ON PC.IdEmpresa = PG.IdEmpresa AND PC.IdPunto_cargo_grupo = PG.IdPunto_cargo_grupo
					WHERE CA.IdCtaCble <> D.IdCtaCble
				  ) AS DET ON caj_Caja_Movimiento.IdEmpresa = DET.IdEmpresa AND caj_Caja_Movimiento.IdTipocbte = DET.IdTipoCbte AND caj_Caja_Movimiento.IdCbteCble = DET.IdCbteCble
WHERE  (dbo.caj_Caja_Movimiento.cm_Signo = N'-') AND (dbo.caj_Caja_Movimiento.Estado = 'A') AND (NOT EXISTS
                      (SELECT IdEmpresa
                       FROM      dbo.cp_conciliacion_Caja_det_x_ValeCaja AS F
                       WHERE   (dbo.caj_Caja_Movimiento.IdEmpresa = IdEmpresa_movcaja) AND (dbo.caj_Caja_Movimiento.IdTipocbte = IdTipocbte_movcaja) AND (dbo.caj_Caja_Movimiento.IdCbteCble = IdCbteCble_movcaja)))