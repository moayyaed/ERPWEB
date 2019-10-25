--exec [web].[SPCONTA_012] 1,201910, 'GASTO'
CREATE procedure [web].[SPCONTA_012]
(
@IdEmpresa int,
@IdPeriodo int,
@IdGrupoCble varchar(10)
)AS
BEGIN
--set @IdPeriodo = 201608
declare @Fecha_ini datetime
declare @Fecha_fin datetime

select @Fecha_ini = pe_FechaIni, @Fecha_fin = pe_FechaFin from ct_periodo where @IdPeriodo = ct_periodo.IdPeriodo

SELECT        ct_cbtecble.IdEmpresa, ct_grupo_x_Tipo_Gasto.nivel, 
						 iif(ct_grupo_x_Tipo_Gasto_1.nivel = 1, ct_grupo_x_Tipo_Gasto.orden, ct_grupo_x_Tipo_Gasto_1.orden) as orden, ct_grupo_x_Tipo_Gasto.IdTipo_Gasto, 
                         iif(ct_grupo_x_Tipo_Gasto_1.nivel = 1, ct_grupo_x_Tipo_Gasto_1.orden,ct_grupo_x_Tipo_Gasto_2.orden) as orden_tipo_gasto,ct_grupo_x_Tipo_Gasto_1.nom_tipo_Gasto, 
						 iif(ct_grupo_x_Tipo_Gasto_1.nivel = 1, ct_grupo_x_Tipo_Gasto_1.nom_tipo_Gasto,ct_grupo_x_Tipo_Gasto_2.nom_tipo_Gasto) AS nom_tipo_Gasto_padre, 
						 ISNULL(SUM(ct_cbtecble_det.dc_Valor), 0) 
                         AS dc_Valor, CAST(ct_grupo_x_Tipo_Gasto.IdTipo_Gasto AS varchar) AS IdCta, ct_grupo_x_Tipo_Gasto.nom_tipo_Gasto AS nom_cuenta,''nom_grupo_CC, ct_grupocble.gc_GrupoCble,ct_plancta.IdGrupoCble
FROM            ct_cbtecble INNER JOIN
                         ct_cbtecble_det ON ct_cbtecble.IdEmpresa = ct_cbtecble_det.IdEmpresa AND ct_cbtecble.IdTipoCbte = ct_cbtecble_det.IdTipoCbte AND 
                         ct_cbtecble.IdCbteCble = ct_cbtecble_det.IdCbteCble INNER JOIN
                         ct_cbtecble_tipo ON ct_cbtecble.IdEmpresa = ct_cbtecble_tipo.IdEmpresa AND ct_cbtecble.IdTipoCbte = ct_cbtecble_tipo.IdTipoCbte INNER JOIN
                         ct_plancta ON ct_cbtecble_det.IdEmpresa = ct_plancta.IdEmpresa AND ct_cbtecble_det.IdCtaCble = ct_plancta.IdCtaCble inner JOIN
                         ct_grupo_x_Tipo_Gasto AS ct_grupo_x_Tipo_Gasto_1 INNER JOIN
                         ct_grupo_x_Tipo_Gasto ON ct_grupo_x_Tipo_Gasto_1.IdEmpresa = ct_grupo_x_Tipo_Gasto.IdEmpresa AND 
                         ct_grupo_x_Tipo_Gasto_1.IdTipo_Gasto = ct_grupo_x_Tipo_Gasto.IdTipo_Gasto_Padre LEFT OUTER JOIN
                         ct_grupo_x_Tipo_Gasto AS ct_grupo_x_Tipo_Gasto_2 ON ct_grupo_x_Tipo_Gasto_1.IdEmpresa = ct_grupo_x_Tipo_Gasto_2.IdEmpresa AND 
                         ct_grupo_x_Tipo_Gasto_1.IdTipo_Gasto_Padre = ct_grupo_x_Tipo_Gasto_2.IdTipo_Gasto ON ct_plancta.IdEmpresa = ct_grupo_x_Tipo_Gasto.IdEmpresa AND 
                         ct_plancta.IdTipo_Gasto = ct_grupo_x_Tipo_Gasto.IdTipo_Gasto INNER JOIN ct_grupocble on ct_plancta.IdGrupoCble = ct_grupocble.IdGrupoCble
where ct_cbtecble.cb_Fecha between @Fecha_ini and @Fecha_fin and ct_cbtecble.IdEmpresa = @IdEmpresa  and ct_plancta.IdGrupoCble = @IdGrupoCble
and not exists(
select * from ct_cbtecble_Reversado R 
where R.IdEmpresa = ct_cbtecble.IdEmpresa and r.IdTipoCbte = ct_cbtecble.IdTipoCbte and r.IdCbteCble = ct_cbtecble.IdCbteCble
) and not exists(
select * from ct_cbtecble_Reversado R 
where R.IdEmpresa_Anu = ct_cbtecble.IdEmpresa and r.IdTipoCbte_Anu = ct_cbtecble.IdTipoCbte and r.IdCbteCble_Anu = ct_cbtecble.IdCbteCble
)
GROUP BY ct_cbtecble.IdEmpresa, ct_grupo_x_Tipo_Gasto.nivel, ct_grupo_x_Tipo_Gasto.orden, ct_grupo_x_Tipo_Gasto.IdTipo_Gasto, ct_grupo_x_Tipo_Gasto_1.orden,ct_grupo_x_Tipo_Gasto_2.orden,
                         ct_grupo_x_Tipo_Gasto.nom_tipo_Gasto,ct_grupo_x_Tipo_Gasto_1.nivel, ct_grupo_x_Tipo_Gasto_1.nom_tipo_Gasto,ct_grupo_x_Tipo_Gasto_2.nom_tipo_Gasto,ct_grupocble.gc_GrupoCble,ct_plancta.IdGrupoCble


END