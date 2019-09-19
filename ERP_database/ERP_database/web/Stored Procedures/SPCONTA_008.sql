--exec [web].[SPCONTA_008] 1,'2019/09/01','2019/09/30','admin',0,1
CREATE PROCEDURE [web].[SPCONTA_008]
(
@IdEmpresa int,
@FechaIni datetime,
@FechaFin datetime,
@IdUsuario varchar(200),
@MostrarSaldo0 bit,
@MostrarAcumulado bit
)
AS
delete web.ct_CONTA_008 where IdUsuario = @IdUsuario
declare @IdAnio int
set @IdAnio = year(@FechaFin)

PRINT 'INSERTO CUENTAS DE ER'
BEGIN --INSERTO CUENTAS DE ER
	INSERT INTO [web].[ct_CONTA_008]
			   ([IdUsuario]
			   ,[IdEmpresa]
			   ,[IdCtaCble]
			   ,[IdCentroCosto]
			   ,[pc_Cuenta]
			   ,[IdCtaCblePadre]
			   ,[EsCtaUtilidad]
			   ,[IdNivelCta]
			   ,[IdGrupoCble]
			   ,[gc_GrupoCble]
			   ,[gc_estado_financiero]
			   ,[gc_Orden]
			   ,[SaldoFinal]
			   ,[SaldoFinalNaturaleza]
			   ,[EsCuentaMovimiento]
			   ,[Naturaleza])
	SELECT @IdUsuario, 
	ct_plancta.IdEmpresa, 
	ct_plancta.IdCtaCble,
	'', 
	ct_plancta.pc_Cuenta, 
	ct_plancta.IdCtaCblePadre, 
	CASE WHEN ct_anio_fiscal_x_cuenta_utilidad.IdCtaCble = ct_plancta.IdCtaCble THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS EsCtaUtilidad, 
	ct_plancta.IdNivelCta,
	ct_plancta.IdGrupoCble, 
	ct_grupocble.gc_GrupoCble, 
	ct_grupocble.gc_estado_financiero, 
	ct_grupocble.gc_Orden,0,0,0,ct_plancta.pc_Naturaleza
	FROM            ct_anio_fiscal_x_cuenta_utilidad RIGHT OUTER JOIN
			ct_plancta ON ct_anio_fiscal_x_cuenta_utilidad.IdEmpresa = ct_plancta.IdEmpresa AND ct_anio_fiscal_x_cuenta_utilidad.IdCtaCble = ct_plancta.IdCtaCble LEFT OUTER JOIN
			ct_grupocble ON ct_plancta.IdGrupoCble = ct_grupocble.IdGrupoCble
	WHERE        (ISNULL(ct_anio_fiscal_x_cuenta_utilidad.IdanioFiscal, @IdAnio) = @IdAnio) AND ct_plancta.IdEmpresa = @IdEmpresa and ct_grupocble.gc_estado_financiero = 'ER'


	INSERT INTO [web].[ct_CONTA_008]
			   ([IdUsuario]
			   ,[IdEmpresa]
			   ,[IdCtaCble]
			   ,[IdCentroCosto]
			   ,[pc_Cuenta]
			   ,[IdCtaCblePadre]
			   ,[EsCtaUtilidad]
			   ,[IdNivelCta]
			   ,[IdGrupoCble]
			   ,[gc_GrupoCble]
			   ,[gc_estado_financiero]
			   ,[gc_Orden]
			   ,[SaldoFinal]
			   ,[SaldoFinalNaturaleza]
			   ,[EsCuentaMovimiento]
			   ,[Naturaleza])
	select @IdUsuario, d.IdEmpresa, d.IdCtaCble+isnull(d.IdCentroCosto,'000'), isnull(d.IdCentroCosto,'') IdCentroCosto, isnull(ct.cc_Descripcion,'No asignado'),d.IdCtaCble,0 EsCtaUtilidad,pc.IdNivelCta+1 IdNivelCta, pc.IdGrupoCble, g.gc_GrupoCble, g.gc_estado_financiero, g.gc_Orden,
	ROUND(sum(d.dc_Valor),2) Valor,0,0,pc.pc_Naturaleza
	from ct_cbtecble_det as d left join 
	ct_CentroCosto as ct on d.IdEmpresa = ct.IdEmpresa and d.IdCentroCosto = ct.IdCentroCosto inner join 
	ct_cbtecble as c on c.IdEmpresa = d.IdEmpresa and c.IdTipoCbte = d.IdTipoCbte and c.IdCbteCble = d.IdCbteCble inner join
	web.tb_FiltroReportes as f on c.IdEmpresa = f.IdEmpresa and c.IdSucursal = f.IdSucursal and f.IdUsuario = @IdUsuario inner join
	ct_plancta as pc on d.IdEmpresa = pc.IdEmpresa and d.IdCtaCble = pc.IdCtaCble inner join
	ct_grupocble as g on pc.IdGrupoCble = g.IdGrupoCble
	where d.IdEmpresa = @IdEmpresa and c.cb_Fecha between iif(@MostrarAcumulado = 1, c.cb_Fecha,@FechaIni) and @FechaFin
	and g.gc_estado_financiero = 'ER'
	group by d.IdEmpresa, d.IdCtaCble, d.IdCentroCosto,ct.cc_Descripcion,d.IdCtaCble,pc.IdNivelCta, pc.IdGrupoCble, g.gc_GrupoCble, g.gc_estado_financiero, g.gc_Orden,pc.pc_naturaleza
	having ROUND(sum(d.dc_Valor),2) != 0
END

BEGIN --SUMATORIA ASCENDENTE

DECLARE @Contador int

select @Contador = max(IdNivelCta) 
from web.ct_CONTA_008
where IdUsuario = @IdUsuario
and IdEmpresa = @IdEmpresa


	WHILE @Contador > 0
	BEGIN

		UPDATE web.ct_CONTA_008 
		SET [SaldoFinal] = A.[SaldoFinal]
		FROM(
		SELECT        IdEmpresa, IdCtaCblePadre
           ,SUM([SaldoFinal])[SaldoFinal]
		FROM            web.ct_CONTA_008
		where web.ct_CONTA_008.IdEmpresa = @IdEmpresa
		and web.ct_CONTA_008.IdUsuario = @IdUsuario
		GROUP BY IdEmpresa, IdCtaCblePadre
		
		) A where web.ct_CONTA_008.IdEmpresa = a.IdEmpresa
		and web.ct_CONTA_008.IdCtaCble = a.IdCtaCblePadre
		and web.ct_CONTA_008.IdUsuario = @IdUsuario
		and web.ct_CONTA_008.IdEmpresa = @IdEmpresa

		SET @Contador = @Contador - 1
	END

END

IF(@MostrarSaldo0 = 0)
	BEGIN
		DELETE web.ct_CONTA_008
		WHERE SaldoFinal = 0
		and IdUsuario = @IdUsuario
	END

update web.ct_CONTA_008 set EsCuentaMovimiento = 1 
where IdUsuario = @IdUsuario
and IdEmpresa = @IdEmpresa
and not exists(
select * from web.ct_CONTA_008 as f
where f.IdEmpresa = web.ct_CONTA_008.IdEmpresa
and f.IdCtaCblePadre = web.ct_CONTA_008.IdCtaCble
and f.IdEmpresa = @IdEmpresa
and f.IdUsuario = @IdUsuario
)

UPDATE web.ct_CONTA_008 SET SaldoFinalNaturaleza = iif(Naturaleza = 'C', SaldoFinal * -1, SaldoFinal)
where web.ct_CONTA_008.IdEmpresa = @IdEmpresa 
and web.ct_CONTA_008.IdUsuario = @IdUsuario

SELECT IdUsuario,IdEmpresa,IdCtaCble,IdCentroCosto,pc_Cuenta,IdCtaCblePadre,EsCtaUtilidad,IdNivelCta,IdGrupoCble,gc_GrupoCble,gc_estado_financiero,gc_Orden,SaldoFinal,SaldoFinalNaturaleza,EsCuentaMovimiento,Naturaleza 
FROM [web].[ct_CONTA_008] 
WHERE IdEmpresa = @IdEmpresa and IdUsuario = @IdUsuario