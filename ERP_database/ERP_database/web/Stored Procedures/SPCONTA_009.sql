
--exec [web].[SPCONTA_009] 1,'20/08/2019','20/09/2019','admin',0,0,'001'
CREATE PROCEDURE [web].[SPCONTA_009]
(
@IdEmpresa int,
@FechaIni datetime,
@FechaFin datetime,
@IdUsuario varchar(200),
@MostrarSaldo0 bit,
@MostrarAcumulado bit,
@IdCentroCosto varchar(200)
)
AS
delete web.ct_CONTA_009 where IdUsuario = @IdUsuario
declare @IdAnio int
set @IdAnio = year(@FechaFin)

declare @cc_Descripcion varchar(500)
select @cc_Descripcion = cc_Descripcion from ct_centrocosto where IdCentroCosto = @IdCentroCosto and IdEmpresa = @IdEmpresa
set @cc_Descripcion = isnull(@cc_Descripcion,'Todos')

PRINT 'INSERTO CUENTAS DE ER'
BEGIN --INSERTO CUENTAS DE ER
	INSERT INTO [web].[ct_CONTA_009]
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
			   ,[Naturaleza]
			   ,[cc_Descripcion]
			   ,[pc_cuenta_padre])
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
	ct_grupocble.gc_Orden,0,0,0,ct_plancta.pc_Naturaleza,'',''
	FROM            ct_anio_fiscal_x_cuenta_utilidad RIGHT OUTER JOIN
			ct_plancta ON ct_anio_fiscal_x_cuenta_utilidad.IdEmpresa = ct_plancta.IdEmpresa AND ct_anio_fiscal_x_cuenta_utilidad.IdCtaCble = ct_plancta.IdCtaCble LEFT OUTER JOIN
			ct_grupocble ON ct_plancta.IdGrupoCble = ct_grupocble.IdGrupoCble
	WHERE        (ISNULL(ct_anio_fiscal_x_cuenta_utilidad.IdanioFiscal, @IdAnio) = @IdAnio) AND ct_plancta.IdEmpresa = @IdEmpresa and ct_grupocble.gc_estado_financiero = 'ER'


	INSERT INTO [web].[ct_CONTA_009]
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
			   ,[Naturaleza]
			   ,[cc_Descripcion]
			   ,[pc_cuenta_padre])
	select @IdUsuario, d.IdEmpresa, d.IdCtaCble+isnull(d.IdCentroCosto,'000'), isnull(case when len(d.IdCentroCosto) >7 then substring(d.IdCentroCosto,0,7) else d.IdCentroCosto end,'') IdCentroCosto, pc.pc_Cuenta+' '+isnull(cc.cc_Descripcion,'No asignado'),d.IdCtaCble,0 EsCtaUtilidad,pc.IdNivelCta+1 IdNivelCta, pc.IdGrupoCble, g.gc_GrupoCble, g.gc_estado_financiero, g.gc_Orden,
	ROUND(sum(d.dc_Valor),2) Valor,0,0,pc.pc_Naturaleza, isnull(ct.cc_Descripcion,'No asignado'),''
	from ct_cbtecble_det as d left join 
	ct_CentroCosto as ct on d.IdEmpresa = ct.IdEmpresa and case when len(d.IdCentroCosto) >7 then substring(d.IdCentroCosto,0,7) else d.IdCentroCosto end = ct.IdCentroCosto inner join 
	ct_cbtecble as c on c.IdEmpresa = d.IdEmpresa and c.IdTipoCbte = d.IdTipoCbte and c.IdCbteCble = d.IdCbteCble inner join
	web.tb_FiltroReportes as f on c.IdEmpresa = f.IdEmpresa and c.IdSucursal = f.IdSucursal and f.IdUsuario = @IdUsuario inner join
	ct_plancta as pc on d.IdEmpresa = pc.IdEmpresa and d.IdCtaCble = pc.IdCtaCble inner join
	ct_grupocble as g on pc.IdGrupoCble = g.IdGrupoCble left join
	ct_CentroCosto as cc on d.IdEmpresa = cc.IdEmpresa and d.IdCentroCosto = cc.IdCentroCosto
	where d.IdEmpresa = @IdEmpresa and c.cb_Fecha between iif(@MostrarAcumulado = 1, c.cb_Fecha,@FechaIni) and @FechaFin
	and g.gc_estado_financiero = 'ER' and d.IdCtaCble not like '4%'
	and 
	@IdCentroCosto = case when D.IdCtaCble not like '6%' then
	(case when len(@IdCentroCosto) > 0 
	then 
		case when len(d.IdCentroCosto) >4 then substring(d.IdCentroCosto,0,4) else d.IdCentroCosto end  
	else @IdCentroCosto end) else @IdCentroCosto END
	group by d.IdEmpresa, d.IdCtaCble, d.IdCentroCosto,pc.pc_Cuenta,ct.cc_Descripcion,d.IdCtaCble,pc.IdNivelCta, pc.IdGrupoCble, g.gc_GrupoCble, g.gc_estado_financiero, g.gc_Orden,pc.pc_naturaleza,ct.cc_Descripcion,cc.cc_Descripcion
	having ROUND(sum(d.dc_Valor),2) != 0
	UNION ALL
	select IdUsuario, IdEmpresa,IdCtaCble, IdCentroCosto, pc_Cuenta, IdCtaCblePadre, EsCtaUtilidad, IdNivelCta, IdGrupoCble, gc_GrupoCble, gc_estado_financiero, gc_Orden, sum(Valor)Valor, SaldoFinal, SaldoFinalNaturaleza, pc_Naturaleza,cc_Descripcion,pc_cuenta_padre 
from(
select @IdUsuario IdUsuario, d.IdEmpresa, d.IdCtaCble+isnull(ct.IdCentroCosto,'000') IdCtaCble, isnull(case when len(d.IdCentroCosto) >4 then substring(d.IdCentroCosto,0,4) else d.IdCentroCosto end,'') IdCentroCosto, pc.pc_Cuenta+' '+isnull(ct.cc_Descripcion,'No asignado') pc_Cuenta,d.IdCtaCble IdCtaCblePadre,0 EsCtaUtilidad,pc.IdNivelCta+1 IdNivelCta, pc.IdGrupoCble, g.gc_GrupoCble, g.gc_estado_financiero, g.gc_Orden,
	ROUND(sum(d.dc_Valor),2) Valor,0 SaldoFinal,0 SaldoFinalNaturaleza,pc.pc_Naturaleza, isnull(ct.cc_Descripcion,'No asignado') cc_Descripcion,'' pc_cuenta_padre
	from ct_cbtecble_det as d left join 
	ct_CentroCosto as ct on d.IdEmpresa = ct.IdEmpresa and case when len(d.IdCentroCosto) >4 then substring(d.IdCentroCosto,0,4) else d.IdCentroCosto end = ct.IdCentroCosto inner join 
	ct_cbtecble as c on c.IdEmpresa = d.IdEmpresa and c.IdTipoCbte = d.IdTipoCbte and c.IdCbteCble = d.IdCbteCble inner join
	web.tb_FiltroReportes as f on c.IdEmpresa = f.IdEmpresa and c.IdSucursal = f.IdSucursal and f.IdUsuario = @IdUsuario inner join
	ct_plancta as pc on d.IdEmpresa = pc.IdEmpresa and d.IdCtaCble = pc.IdCtaCble inner join
	ct_grupocble as g on pc.IdGrupoCble = g.IdGrupoCble left join
	ct_CentroCosto as cc on d.IdEmpresa = cc.IdEmpresa and d.IdCentroCosto = cc.IdCentroCosto
	where d.IdEmpresa = @IdEmpresa and c.cb_Fecha between iif(@MostrarAcumulado = 1, c.cb_Fecha,@FechaIni) and @FechaFin
	and g.gc_estado_financiero = 'ER' and d.IdCtaCble like '4%'
	and @IdCentroCosto = 
	(case when len(@IdCentroCosto) > 0 
	then 
		case when len(d.IdCentroCosto) >4 then substring(d.IdCentroCosto,0,4) else d.IdCentroCosto end  
	else @IdCentroCosto end)
	group by d.IdEmpresa, d.IdCtaCble, ct.IdCentroCosto,d.IdCentroCosto,pc.pc_Cuenta,ct.cc_Descripcion,d.IdCtaCble,pc.IdNivelCta, pc.IdGrupoCble, g.gc_GrupoCble, g.gc_estado_financiero, g.gc_Orden,pc.pc_naturaleza,ct.cc_Descripcion,cc.cc_Descripcion
	having ROUND(sum(d.dc_Valor),2) != 0
) a
group by IdUsuario, IdEmpresa,IdCtaCble, IdCentroCosto, pc_Cuenta, IdCtaCblePadre, EsCtaUtilidad, IdNivelCta, IdGrupoCble, gc_GrupoCble, gc_estado_financiero, gc_Orden, SaldoFinal, SaldoFinalNaturaleza, pc_Naturaleza,cc_Descripcion,pc_cuenta_padre 
END

BEGIN --SUMATORIA ASCENDENTE

DECLARE @Contador int

select @Contador = max(IdNivelCta) 
from web.ct_CONTA_009
where IdUsuario = @IdUsuario
and IdEmpresa = @IdEmpresa


	WHILE @Contador > 0
	BEGIN

		UPDATE web.ct_CONTA_009
		SET [SaldoFinal] = A.[SaldoFinal]
		FROM(
		SELECT        IdEmpresa, IdCtaCblePadre
           ,SUM([SaldoFinal])[SaldoFinal]
		FROM            web.ct_CONTA_009
		where web.ct_CONTA_009.IdEmpresa = @IdEmpresa
		and web.ct_CONTA_009.IdUsuario = @IdUsuario
		GROUP BY IdEmpresa, IdCtaCblePadre
		
		) A where web.ct_CONTA_009.IdEmpresa = a.IdEmpresa
		and web.ct_CONTA_009.IdCtaCble = a.IdCtaCblePadre
		and web.ct_CONTA_009.IdUsuario = @IdUsuario
		and web.ct_CONTA_009.IdEmpresa = @IdEmpresa

		SET @Contador = @Contador - 1
	END

END

IF(@MostrarSaldo0 = 0)
	BEGIN
		DELETE web.ct_CONTA_009
		WHERE SaldoFinal = 0
		and IdUsuario = @IdUsuario
	END

update web.ct_CONTA_009 set EsCuentaMovimiento = 1 
where IdUsuario = @IdUsuario
and IdEmpresa = @IdEmpresa
and not exists(
select * from web.ct_CONTA_009 as f
where f.IdEmpresa = web.ct_CONTA_009.IdEmpresa
and f.IdCtaCblePadre = web.ct_CONTA_009.IdCtaCble
and f.IdEmpresa = @IdEmpresa
and f.IdUsuario = @IdUsuario
)

UPDATE web.ct_CONTA_009 SET SaldoFinalNaturaleza = iif(Naturaleza = 'C', SaldoFinal * -1, SaldoFinal)
where web.ct_CONTA_009.IdEmpresa = @IdEmpresa 
and web.ct_CONTA_009.IdUsuario = @IdUsuario

DELETE [web].[ct_CONTA_009] 
WHERE IdEmpresa = @IdEmpresa and IdUsuario = @IdUsuario AND (IdGrupoCble <> 'CTS_P' OR IdCtaCblePadre like '51%') AND EsCuentaMovimiento = 1 and IdCtaCble not like '4%'

DELETE [web].[ct_CONTA_009] 
WHERE IdEmpresa = @IdEmpresa and IdUsuario = @IdUsuario AND (IdCtaCble like '52%' OR IdCtaCble like '53%') AND IdNivelCta in (2,3,4) 

UPDATE [web].[ct_CONTA_009] SET IdCtaCblePadre = substring(IdCtaCblePadre,0,3)
WHERE IdEmpresa = @IdEmpresa and IdUsuario = @IdUsuario AND (IdCtaCble like '52%' OR IdCtaCble like '53%') AND IdNivelCta = 5


UPDATE [web].[ct_CONTA_009] SET pc_cuenta_padre = pc.pc_Cuenta
FROM ct_plancta as pc
WHERE [web].[ct_CONTA_009].IdEmpresa = @IdEmpresa and [web].[ct_CONTA_009].IdUsuario = @IdUsuario AND ([web].[ct_CONTA_009].IdCtaCble like '52%' OR [web].[ct_CONTA_009].IdCtaCble like '53%') AND [web].[ct_CONTA_009].IdNivelCta = 5
and pc.IdCtaCble = [web].[ct_CONTA_009].IdCtaCblePadre


update web.ct_CONTA_009 set EsCuentaMovimiento = 1 
where IdUsuario = @IdUsuario
and IdEmpresa = @IdEmpresa
and not exists(
select * from web.ct_CONTA_009 as f
where f.IdEmpresa = web.ct_CONTA_009.IdEmpresa
and f.IdCtaCblePadre = web.ct_CONTA_009.IdCtaCble
and f.IdEmpresa = @IdEmpresa
and f.IdUsuario = @IdUsuario
)

SELECT IdUsuario,IdEmpresa,IdCtaCble,IdCentroCosto,pc_Cuenta,IdCtaCblePadre,EsCtaUtilidad,IdNivelCta,IdGrupoCble,gc_GrupoCble,gc_estado_financiero,gc_Orden,SaldoFinal,SaldoFinalNaturaleza,EsCuentaMovimiento,Naturaleza,cc_Descripcion, pc_cuenta_padre,@cc_Descripcion FiltroCC
FROM [web].[ct_CONTA_009] 
WHERE IdEmpresa = @IdEmpresa and IdUsuario = @IdUsuario