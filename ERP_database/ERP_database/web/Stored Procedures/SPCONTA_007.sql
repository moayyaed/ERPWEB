--EXEC web.SPCONTA_007 1,'admin','2019/08/01','2019/08/31',1,1
create PROCEDURE web.SPCONTA_007
(
@IdEmpresa int,
@IdUsuario varchar(200),
@FechaIni date,
@FechaFin date,
@MostrarAcumulado bit,
@MostrarDetalle bit
)
AS

DELETE [web].[ct_CONTA_007] WHERE IdUsuario = @IdUsuario

DECLARE @UtilidadPerdida numeric(18,2), @Anio int, @IdCtaCbleUtilidad varchar(200), @pc_cuenta_utilidad varchar(500)
SET @Anio = year(@FechaFin)

PRINT 'GET CUENTA UTILIDAD'
BEGIN --GET CUENTA UTILIDAD
	SELECT @IdCtaCbleUtilidad = u.IdCtaCble,
	@pc_cuenta_utilidad = pc.pc_Cuenta
	FROM ct_anio_fiscal_x_cuenta_utilidad as u inner join ct_plancta as pc
	on u.IdEmpresa = pc.IdEmpresa and u.IdCtaCble = pc.IdCtaCble
	WHERE u.IdEmpresa = @IdEmpresa AND IdanioFiscal = @Anio
END

PRINT 'INSERTO DATA INICIAL'
BEGIN --INSERTO DATA INICIAL
	INSERT INTO [web].[ct_CONTA_007]
			   ([IdEmpresa]
			   ,[IdUsuario]
			   ,[Secuencia]
			   ,[IdCtaCble]
			   ,[pc_cuenta]
			   ,[Tipo]
			   ,[TipoOrden]
			   ,[Clasificacion]
			   ,[ClasificacionOrden]
			   ,[Valor])
		   
	SELECT @IdEmpresa, @IdUsuario,'EBITDA-UTI','','UTILIDAD NETA','EBITDA',1,'UTILIDAD NETA',1,0.00
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBITDA-UTI-'+@IdCtaCbleUtilidad,@IdCtaCbleUtilidad,@pc_cuenta_utilidad,'EBITDA',1,'UTILIDAD NETA',1,0.00
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBITDA-INT','','INTERESES','EBITDA',1,'INTERESES',2,0.00
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBITDA-INT-'+PC.IdCtaCble,pc.IdCtaCble,pc.pc_Cuenta,'EBITDA',1,'INTERESES',2,0.00
	from ct_plancta as pc inner join ct_ClasificacionEBIT as e on pc.IdClasificacionEBIT = e.IdClasificacionEBIT 
	where e.ebit_Codigo = 'INT' AND PC.IdEmpresa = @IdEmpresa AND PC.pc_Estado = 'A'
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBITDA-IMP','','IMPUESTOS','EBITDA',1,'IMPUESTOS',3,0.00
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBITDA-IMP-'+PC.IdCtaCble,pc.IdCtaCble,pc.pc_Cuenta,'EBITDA',1,'IMPUESTOS',3,0.00
	from ct_plancta as pc inner join ct_ClasificacionEBIT as e on pc.IdClasificacionEBIT = e.IdClasificacionEBIT 
	where e.ebit_Codigo = 'IMP' AND PC.IdEmpresa = @IdEmpresa AND PC.pc_Estado = 'A'
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBITDA-DEP','','DEPRECIACIONES','EBITDA',1,'DEPRECIACIONES',4,0.00
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBITDA-DEP-'+PC.IdCtaCble,pc.IdCtaCble,pc.pc_Cuenta,'EBITDA',1,'DEPRECIACIONES',4,0.00
	from ct_plancta as pc inner join ct_ClasificacionEBIT as e on pc.IdClasificacionEBIT = e.IdClasificacionEBIT 
	where e.ebit_Codigo = 'DEP' AND PC.IdEmpresa = @IdEmpresa AND PC.pc_Estado = 'A'
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBITDA-AMO','','AMORTIZACIONES','EBITDA',1,'AMORTIZACIONES',5,0.00
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBITDA-AMO-'+PC.IdCtaCble,pc.IdCtaCble,pc.pc_Cuenta,'EBITDA',1,'AMORTIZACIONES',5,0.00
	from ct_plancta as pc inner join ct_ClasificacionEBIT as e on pc.IdClasificacionEBIT = e.IdClasificacionEBIT 
	where e.ebit_Codigo = 'AMO' AND PC.IdEmpresa = @IdEmpresa AND PC.pc_Estado = 'A'
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBIT-UTI','','UTILIDAD NETA','EBIT',2,'UTILIDAD NETA',1,0.00
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBIT-UTI-'+@IdCtaCbleUtilidad,@IdCtaCbleUtilidad,@pc_cuenta_utilidad,'EBIT',2,'UTILIDAD NETA',1,0.00
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBIT-INT','','INTERESES','EBIT',2,'INTERESES',2,0.00
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBIT-INT-'+PC.IdCtaCble,pc.IdCtaCble,pc.pc_Cuenta,'EBIT',2,'INTERESES',2,0.00
	from ct_plancta as pc inner join ct_ClasificacionEBIT as e on pc.IdClasificacionEBIT = e.IdClasificacionEBIT 
	where e.ebit_Codigo = 'INT' AND PC.IdEmpresa = @IdEmpresa AND PC.pc_Estado = 'A'
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBIT-IMP','','IMPUESTOS','EBIT',2,'IMPUESTOS',3,0.00
	UNION ALL
	SELECT @IdEmpresa, @IdUsuario,'EBIT-IMP-'+PC.IdCtaCble,pc.IdCtaCble,pc.pc_Cuenta,'EBIT',2,'IMPUESTOS',3,0.00
	from ct_plancta as pc inner join ct_ClasificacionEBIT as e on pc.IdClasificacionEBIT = e.IdClasificacionEBIT 
	where e.ebit_Codigo = 'IMP' AND PC.IdEmpresa = @IdEmpresa AND PC.pc_Estado = 'A'
END

PRINT 'CALCULO UTILIDAD'
BEGIN --CALCULO UTILIDAD
	SELECT @UtilidadPerdida = ROUND(sum(dc_Valor),2)
	FROM ct_cbtecble AS C INNER JOIN ct_cbtecble_det AS D
	ON C.IdEmpresa = D.IdEmpresa AND C.IdTipoCbte = D.IdTipoCbte AND C.IdCbteCble = D.IdCbteCble INNER JOIN
	ct_plancta AS PC ON PC.IdEmpresa = D.IdEmpresa AND PC.IdCtaCble = D.IdCtaCble INNER JOIN ct_grupocble AS G
	ON PC.IdGrupoCble = G.IdGrupoCble INNER JOIN WEB.tb_FiltroReportes AS F ON C.IdEmpresa = F.IdEmpresa AND C.IdSucursal = F.IdSucursal AND F.IdUsuario = @IdUsuario
	WHERE C.IdEmpresa = @IdEmpresa	AND C.cb_Fecha BETWEEN IIF(@MostrarAcumulado = 1,@FechaIni , C.cb_Fecha) AND @FechaFin and f.IdUsuario = @IdUsuario
	and g.gc_estado_financiero = 'ER'

	SET @UtilidadPerdida = ISNULL(@UtilidadPerdida,0)*-1

	UPDATE [web].[ct_CONTA_007] SET Valor = @UtilidadPerdida 
	WHERE IdEmpresa = @IdEmpresa and IdUsuario = @IdUsuario AND @IdCtaCbleUtilidad = IdCtaCble
END

PRINT 'ACTUALIZO VALORES DE CUENTAS DE MOVIMIENTO'
BEGIN --ACTUALIZO VALORES DE CUENTAS DE MOVIMIENTO
	update [web].[ct_CONTA_007] set Valor = A.Valor
	FROM(
	SELECT d.IdEmpresa, d.IdCtaCble, round(sum(d.dc_Valor),2) Valor FROM ct_plancta as pc inner join ct_ClasificacionEBIT AS Cla on pc.IdClasificacionEBIT = cla.IdClasificacionEBIT
	inner join ct_cbtecble_det as d on d.IdEmpresa = pc.IdEmpresa and d.IdCtaCble = pc.IdCtaCble inner join ct_cbtecble as c
	on c.IdEmpresa = d.IdEmpresa and c.IdTipoCbte = d.IdTipoCbte and c.IdCbteCble = d.IdCbteCble INNER JOIN WEB.tb_FiltroReportes AS F ON C.IdEmpresa = F.IdEmpresa AND C.IdSucursal = F.IdSucursal AND F.IdUsuario = @IdUsuario
	where C.IdEmpresa = @IdEmpresa	AND C.cb_Fecha BETWEEN IIF(@MostrarAcumulado = 1,@FechaIni , C.cb_Fecha) AND @FechaFin and f.IdUsuario = @IdUsuario	
	GROUP BY d.IdEmpresa,d.IdCtaCble
	) A
	WHERE [web].[ct_CONTA_007].IdEmpresa = A.IdEmpresa and
	[web].[ct_CONTA_007].IdCtaCble = A.IdCtaCble and 
	[web].[ct_CONTA_007].IdUsuario = @IdUsuario
END

PRINT 'ACTUALIZO VALORES DE GRUPOS EBIT'
BEGIN --ACTUALIZO VALORES DE GRUPOS EBIT
	update WEB.ct_CONTA_007 set Valor = A.Valor
	FROM(
	SELECT Tipo, Clasificacion, SUM(Valor) Valor FROM WEB.ct_CONTA_007
	WHERE IdEmpresa = @IdEmpresa AND IdUsuario = @IdUsuario
	group by Tipo, Clasificacion
	) A WHERE WEB.ct_CONTA_007.Tipo = A.Tipo
	AND WEB.ct_CONTA_007.Clasificacion = A.Clasificacion
	AND WEB.ct_CONTA_007.IdCtaCble = ''
	AND WEB.ct_CONTA_007.IdEmpresa = @IdEmpresa
	AND WEB.ct_CONTA_007.IdUsuario = @IdUsuario
END

if(@MostrarDetalle = 0)
BEGIN
	DELETE [web].[ct_CONTA_007] WHERE IdEmpresa = @IdEmpresa and IdUsuario = @IdUsuario
	AND IdCtaCble <> ''
END

select* from [web].[ct_CONTA_007] WHERE IdEmpresa = @IdEmpresa and IdUsuario = @IdUsuario