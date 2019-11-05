-- exec web.SPCONTA_011 1,'admin','2019/10/21','2019/10/27'
CREATE PROCEDURE [web].[SPCONTA_011]
(
@IdEmpresa int,
@IdUsuario varchar(200),
@FechaIni date,
@FechaFin date
)
AS
delete [web].[ct_CONTA_011] where IdUsuario = @IdUsuario
SET @FechaIni = CAST(@FechaIni AS DATE)
SET @FechaFin = CAST(@FechaFin AS DATE)
INSERT INTO [web].[ct_CONTA_011]
           ([IdEmpresa]
           ,[IdUsuario]
           ,[Secuencia]
           ,[Grupo]
           ,[Descripcion]
           ,[NombreC1]
           ,[NombreC2]
           ,[NombreC3]
           ,[NombreC4]
           ,[NombreC5]
		   ,[NombreC6]
		   ,[NombreC7]
           ,[Columna1]
           ,[Columna2]
           ,[Columna3]
           ,[Columna4]
           ,[Columna5]
		   ,[Columna6]
		   ,[Columna7])
select @IdEmpresa, @IdUsuario, 1, 1, 'Bancos','Saldo inicial', 'Ingresos', 'Egresos','','','','Saldo final',0,0,0,0,0,0,0
UNION ALL
select @IdEmpresa, @IdUsuario, 2, 2, 'Cuentas por cobrar clientes','Saldo inicial', 'Facturado', 'Cobrado','Cobros anticipados','Retenciones','Cruce de cuentas','Saldo final',0,0,0,0,0,0,0
UNION ALL
select @IdEmpresa, @IdUsuario, 3, 3, 'Cuentas por pagar','Saldo inicial', 'Facturas recibidas', 'Pagos','','','','Saldo final',0,0,0,0,0,0,0

BEGIN --BANCOS
	UPDATE [web].[ct_CONTA_011] SET Columna1 = A.SaldoInicial
	FROM(
		SELECT round(SUM(D.dc_Valor),2) AS SaldoInicial 
		FROM ct_cbtecble_det AS D INNER JOIN ba_Banco_Cuenta AS B
		ON B.IdEmpresa = D.IdEmpresa AND B.IdCtaCble = D.IdCtaCble inner join
		ct_cbtecble as c ON C.IdEmpresa = D.IdEmpresa AND C.IdTipoCbte = D.IdTipoCbte AND C.IdCbteCble = D.IdCbteCble
		WHERE D.IdEmpresa = @IdEmpresa AND C.cb_Fecha < @FechaIni
	) A
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 1

	UPDATE [web].[ct_CONTA_011] SET Columna2 = isnull(A.Valor,0)
	FROM(
		SELECT round(SUM(D.dc_Valor),2) AS Valor 
		FROM ct_cbtecble_det AS D INNER JOIN ba_Banco_Cuenta AS B
		ON B.IdEmpresa = D.IdEmpresa AND B.IdCtaCble = D.IdCtaCble inner join
		ct_cbtecble as c ON C.IdEmpresa = D.IdEmpresa AND C.IdTipoCbte = D.IdTipoCbte AND C.IdCbteCble = D.IdCbteCble
		WHERE D.IdEmpresa = @IdEmpresa AND C.cb_Fecha between @FechaIni and @FechaFin and 
		not exists(
			select r.IdEmpresa_Anu from ct_cbtecble_Reversado as r inner join ct_cbtecble as ct
			on ct.IdEmpresa = r.IdEmpresa_Anu and ct.IdTipoCbte  = r.IdTipoCbte_Anu and ct.IdCbteCble = r.IdCbteCble_Anu
			where ct.cb_Fecha < @FechaFin
			and ct.IdEmpresa = c.IdEmpresa and ct.IdTipoCbte = c.IdTipoCbte and ct.IdCbteCble = c.IdCbteCble
		) and not exists(
			select r.IdEmpresa_Anu from ct_cbtecble_Reversado as r inner join ct_cbtecble as ct
			on ct.IdEmpresa = r.IdEmpresa and ct.IdTipoCbte  = r.IdTipoCbte and ct.IdCbteCble = r.IdCbteCble
			where ct.cb_Fecha < @FechaFin
			and ct.IdEmpresa = c.IdEmpresa and ct.IdTipoCbte = c.IdTipoCbte and ct.IdCbteCble = c.IdCbteCble
		) and d.dc_Valor > 0
	)A
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 1

	UPDATE [web].[ct_CONTA_011] SET Columna3 = abs(isnull(A.Valor,0))
	FROM(
		SELECT round(SUM(D.dc_Valor),2) AS Valor 
		FROM ct_cbtecble_det AS D INNER JOIN ba_Banco_Cuenta AS B
		ON B.IdEmpresa = D.IdEmpresa AND B.IdCtaCble = D.IdCtaCble inner join
		ct_cbtecble as c ON C.IdEmpresa = D.IdEmpresa AND C.IdTipoCbte = D.IdTipoCbte AND C.IdCbteCble = D.IdCbteCble
		WHERE D.IdEmpresa = @IdEmpresa AND C.cb_Fecha between @FechaIni and @FechaFin and 
		not exists(
			select r.IdEmpresa_Anu from ct_cbtecble_Reversado as r inner join ct_cbtecble as ct
			on ct.IdEmpresa = r.IdEmpresa_Anu and ct.IdTipoCbte  = r.IdTipoCbte_Anu and ct.IdCbteCble = r.IdCbteCble_Anu
			where ct.cb_Fecha < @FechaFin
			and ct.IdEmpresa = c.IdEmpresa and ct.IdTipoCbte = c.IdTipoCbte and ct.IdCbteCble = c.IdCbteCble
		) and not exists(
			select r.IdEmpresa_Anu from ct_cbtecble_Reversado as r inner join ct_cbtecble as ct
			on ct.IdEmpresa = r.IdEmpresa and ct.IdTipoCbte  = r.IdTipoCbte and ct.IdCbteCble = r.IdCbteCble
			where ct.cb_Fecha < @FechaFin
			and ct.IdEmpresa = c.IdEmpresa and ct.IdTipoCbte = c.IdTipoCbte and ct.IdCbteCble = c.IdCbteCble
		) and d.dc_Valor < 0
	)A
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 1

	update [web].[ct_CONTA_011] set Columna7 = round(Columna1 + Columna2 - Columna3,2)
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 1
END

BEGIN --CUENTAS POR COBRAR CLIENTES
	UPDATE [web].[ct_CONTA_011] SET Columna1 = ROUND(isnull(A.Valor,0),2)
	FROM(
		/*select SUM(G.Valor) Valor 
		from(
		SELECT F.IdEmpresa, F.IdSucursal, F.IdBodega, F.IdCbteVta, ROUND(FR.Total - isnull(CXC.dc_ValorPago,0),2) Valor
		FROM fa_factura AS F INNER JOIN 
		fa_factura_resumen AS FR ON F.IdEmpresa = FR.IdEmpresa AND F.IdSucursal = FR.IdSucursal AND F.IdBodega = FR.IdBodega AND F.IdCbteVta = FR.IdCbteVta LEFT JOIN
		(
			SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota, D.dc_TipoDocumento, SUM(D.dc_ValorPago) dc_ValorPago
			FROM cxc_cobro AS C INNER JOIN
			cxc_cobro_det AS D ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal AND C.IdCobro = D.IdCobro
			WHERE C.cr_estado = 'A' AND D.estado = 'A' AND C.IdEmpresa = @IdEmpresa AND C.cr_fecha < @FechaIni
			GROUP BY D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota, D.dc_TipoDocumento
		) CXC ON F.IdEmpresa = CXC.IdEmpresa AND F.IdSucursal = CXC.IdSucursal AND F.IdBodega = CXC.IdBodega_Cbte AND F.IdCbteVta = CXC.IdCbte_vta_nota AND F.vt_tipoDoc = CXC.dc_TipoDocumento
		WHERE F.IdEmpresa = @IdEmpresa AND F.vt_fecha < @FechaIni AND F.Estado = 'A' 
		UNION ALL
		SELECT F.IdEmpresa, F.IdSucursal, F.IdBodega, F.IdNota, ROUND(FR.Total - isnull(CXC.dc_ValorPago,0),2) Saldo
		FROM fa_notaCreDeb AS F INNER JOIN 
		fa_notaCreDeb_resumen AS FR ON F.IdEmpresa = FR.IdEmpresa AND F.IdSucursal = FR.IdSucursal AND F.IdBodega = FR.IdBodega AND F.IdNota = FR.IdNota LEFT JOIN
		(
			SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota, D.dc_TipoDocumento, SUM(D.dc_ValorPago) dc_ValorPago
			FROM cxc_cobro AS C INNER JOIN
			cxc_cobro_det AS D ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal AND C.IdCobro = D.IdCobro
			WHERE C.cr_estado = 'A' AND D.estado = 'A' AND C.IdEmpresa = @IdEmpresa AND C.cr_fecha < @FechaIni
			GROUP BY D.IdEmpresa, D.IdSucursal, D.IdBodega_Cbte, D.IdCbte_vta_nota, D.dc_TipoDocumento
		) CXC ON F.IdEmpresa = CXC.IdEmpresa AND F.IdSucursal = CXC.IdSucursal AND F.IdBodega = CXC.IdBodega_Cbte AND F.IdNota = CXC.IdCbte_vta_nota AND F.CodDocumentoTipo = CXC.dc_TipoDocumento
		WHERE F.IdEmpresa = @IdEmpresa AND F.no_fecha < @FechaIni AND F.Estado = 'A'  AND F.CreDeb = 'D'
		
		)G*/
		SELECT SUM(D.dc_Valor) Valor FROM ct_cbtecble AS C INNER JOIN ct_cbtecble_det AS D
	ON C.IdEmpresa = D.IdEmpresa AND C.IdTipoCbte = D.IdTipoCbte AND C.IdCbteCble = D.IdCbteCble
	WHERE C.IdEmpresa = @IdEmpresa AND C.cb_Fecha < @FechaIni AND D.IdCtaCble = '11401'
	)A
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 2

	UPDATE [web].[ct_CONTA_011] SET Columna2 = round(isnull(A.Valor,0),2)
	from(
		select SUM(G.Valor) Valor
			from (
			SELECT SUM(FR.Total)Valor
			FROM fa_factura AS F INNER JOIN 
			fa_factura_resumen AS FR ON F.IdEmpresa = FR.IdEmpresa AND F.IdSucursal = FR.IdSucursal AND F.IdBodega = FR.IdBodega AND F.IdCbteVta = FR.IdCbteVta
			WHERE F.IdEmpresa = @IdEmpresa AND F.vt_fecha BETWEEN @FechaIni AND @FechaFin AND NOT EXISTS(
			SELECT rel.ct_IdEmpresa FROM fa_factura_x_ct_cbtecble AS rel inner join ct_cbtecble_Reversado as r on rel.vt_IdEmpresa = r.IdEmpresa
			and rel.ct_IdTipoCbte = r.IdTipoCbte and rel.ct_IdCbteCble = r.IdCbteCble inner join ct_cbtecble as ct on
			ct.IdEmpresa = r.IdEmpresa_Anu and ct.IdTipoCbte = r.IdTipoCbte_Anu and ct.IdCbteCble = r.IdCbteCble_Anu
			where ct.IdEmpresa = @IdEmpresa and ct.cb_Fecha <= @FechaFin
			and rel.vt_IdEmpresa = f.IdEmpresa and rel.vt_IdSucursal = f.IdSucursal and rel.vt_IdBodega = f.IdBodega and rel.vt_IdCbteVta = f.IdCbteVta
			) 
			UNION ALL
			SELECT SUM(FR.Total)
			FROM fa_notaCreDeb AS F INNER JOIN 
			fa_notaCreDeb_resumen AS FR ON F.IdEmpresa = FR.IdEmpresa AND F.IdSucursal = FR.IdSucursal AND F.IdBodega = FR.IdBodega AND F.IdNota = FR.IdNota
			WHERE F.IdEmpresa = @IdEmpresa AND F.Estado = 'A' AND F.no_fecha BETWEEN @FechaIni AND @FechaFin AND F.CreDeb = 'D'
		)G
	)A
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 2

	UPDATE [web].[ct_CONTA_011] SET Columna3 = round(isnull(A.Valor,0),2)
	from(
		select SUM(D.dc_ValorPago) Valor
		from cxc_cobro as c inner join 
		cxc_cobro_det as d on c.IdEmpresa = d.IdEmpresa and c.IdSucursal = d.IdSucursal and c.IdCobro = d.IdCobro inner join
		cxc_cobro_tipo as t on d.IdCobro_tipo = t.IdCobro_tipo
		where c.IdEmpresa = @IdEmpresa and c.cr_fecha between @FechaIni and @FechaFin and c.cr_estado = 'A' AND D.estado = 'A'
		AND t.IdMotivo_tipo_cobro not in ('NTCR','RET')
	)A
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 2

	UPDATE [web].[ct_CONTA_011] SET Columna4 = round(isnull(A.Valor,0),2)
	
	from(
	  SELECT SUM(G.Valor)Valor FROM(
			select ROUND(SUM(CD.cr_Valor),2) Valor
			from caj_Caja_Movimiento CM INNER JOIN
			caj_Caja_Movimiento_det AS CD ON CM.IdEmpresa = CD.IdEmpresa AND CM.IdTipocbte = CD.IdTipocbte AND CM.IdCbteCble = CD.IdCbteCble
			where CM.IdTipo_Persona = 'CLIENTE' AND NOT EXISTS(
			SELECT F.ct_IdEmpresa FROM cxc_cobro_x_ct_cbtecble AS F 
			WHERE CM.IdEmpresa = F.ct_IdEmpresa
			AND CM.IdTipocbte = F.ct_IdTipoCbte
			AND CM.IdCbteCble = F.ct_IdCbteCble
			
	)
	AND CM.IdEmpresa = @IdEmpresa AND CM.cm_fecha BETWEEN @FechaIni AND @FechaFin AND CM.Estado = 'A'
	UNION ALL
			SELECT SUM(ISNULL(C.cr_Excedente,0)) FROM cxc_cobro AS C
			WHERE C.IdEmpresa = @IdEmpresa AND C.cr_fecha BETWEEN @FechaIni AND @FechaFin AND C.cr_estado = 'A' AND ISNULL(C.cr_Excedente,0) > 0
			)G
	)A
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 2

	

	UPDATE [web].[ct_CONTA_011] SET Columna5 = round(isnull(A.Valor,0),2)
	from(
		select SUM(D.dc_ValorPago) Valor
		from cxc_cobro as c inner join 
		cxc_cobro_det as d on c.IdEmpresa = d.IdEmpresa and c.IdSucursal = d.IdSucursal and c.IdCobro = d.IdCobro inner join
		cxc_cobro_tipo as t on d.IdCobro_tipo = t.IdCobro_tipo
		where c.IdEmpresa = @IdEmpresa and c.cr_fecha between @FechaIni and @FechaFin and c.cr_estado = 'A' AND D.estado = 'A'
		AND t.IdMotivo_tipo_cobro = 'RET'
	)A
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 2

	UPDATE [web].[ct_CONTA_011] SET Columna6 = round(isnull(A.Valor,0),2)
	from(
	SELECT SUM(G.Valor) Valor FROM(
		select SUM(D.dc_ValorPago) Valor
		from cxc_cobro as c inner join 
		cxc_cobro_det as d on c.IdEmpresa = d.IdEmpresa and c.IdSucursal = d.IdSucursal and c.IdCobro = d.IdCobro
		where c.IdEmpresa = @IdEmpresa and c.cr_fecha between @FechaIni and @FechaFin and c.cr_estado = 'A' AND D.estado = 'A'
		AND D.IdCobro_tipo = 'NTCR'
		UNION ALL
		SELECT sum(fa_factura_resumen.Total) 
		FROM     fa_factura INNER JOIN
				fa_factura_x_ct_cbtecble ON fa_factura.IdEmpresa = fa_factura_x_ct_cbtecble.vt_IdEmpresa AND fa_factura.IdSucursal = fa_factura_x_ct_cbtecble.vt_IdSucursal AND fa_factura.IdBodega = fa_factura_x_ct_cbtecble.vt_IdBodega AND 
				fa_factura.IdCbteVta = fa_factura_x_ct_cbtecble.vt_IdCbteVta INNER JOIN
				ct_cbtecble_Reversado ON fa_factura_x_ct_cbtecble.ct_IdEmpresa = ct_cbtecble_Reversado.IdEmpresa AND fa_factura_x_ct_cbtecble.ct_IdTipoCbte = ct_cbtecble_Reversado.IdTipoCbte AND 
				fa_factura_x_ct_cbtecble.ct_IdCbteCble = ct_cbtecble_Reversado.IdCbteCble INNER JOIN
				ct_cbtecble ON ct_cbtecble_Reversado.IdEmpresa_Anu = ct_cbtecble.IdEmpresa AND ct_cbtecble_Reversado.IdTipoCbte_Anu = ct_cbtecble.IdTipoCbte AND ct_cbtecble_Reversado.IdCbteCble_Anu = ct_cbtecble.IdCbteCble INNER JOIN
				fa_factura_resumen ON fa_factura.IdEmpresa = fa_factura_resumen.IdEmpresa AND fa_factura.IdSucursal = fa_factura_resumen.IdSucursal AND fa_factura.IdBodega = fa_factura_resumen.IdBodega AND 
				fa_factura.IdCbteVta = fa_factura_resumen.IdCbteVta
		where fa_factura.vt_fecha < @FechaIni and ct_cbtecble.cb_Fecha between @FechaIni and @FechaFin
		UNION ALL
		SELECT sum(cxc_cobro.cr_TotalCobro)*-1
		FROM     cxc_cobro_x_ct_cbtecble INNER JOIN
		cxc_cobro ON cxc_cobro_x_ct_cbtecble.cbr_IdEmpresa = cxc_cobro.IdEmpresa AND cxc_cobro_x_ct_cbtecble.cbr_IdSucursal = cxc_cobro.IdSucursal AND cxc_cobro_x_ct_cbtecble.cbr_IdCobro = cxc_cobro.IdCobro INNER JOIN
		ct_cbtecble INNER JOIN
		ct_cbtecble_Reversado ON ct_cbtecble.IdEmpresa = ct_cbtecble_Reversado.IdEmpresa_Anu AND ct_cbtecble.IdTipoCbte = ct_cbtecble_Reversado.IdTipoCbte_Anu AND 
		ct_cbtecble.IdCbteCble = ct_cbtecble_Reversado.IdCbteCble_Anu ON cxc_cobro_x_ct_cbtecble.ct_IdEmpresa = ct_cbtecble_Reversado.IdEmpresa AND cxc_cobro_x_ct_cbtecble.ct_IdTipoCbte = ct_cbtecble_Reversado.IdTipoCbte AND 
		cxc_cobro_x_ct_cbtecble.ct_IdCbteCble = ct_cbtecble_Reversado.IdCbteCble
		where cxc_cobro.cr_fecha < @FechaIni and ct_cbtecble.cb_Fecha between @FechaIni and @FechaFin
		) G
	)A
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 2

	update [web].[ct_CONTA_011] set Columna7 = round(Columna1 + Columna2 - Columna3 - Columna4 - Columna5 - Columna6  ,2)
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 2
END

BEGIN --CUENTAS POR PAGAR

	UPDATE [web].[ct_CONTA_011] set Columna1 = round(isnull(A.Saldo,0),2)
	FROM(
	/*SELECT SUM(G.Saldo) Saldo FROM(
		SELECT OG.co_total - ISNULL(CAN.MontoAplicado,0) - ISNULL(re_valor_retencion,0) Saldo 
		FROM cp_orden_giro AS OG LEFT JOIN 
		(
			select f.IdEmpresa_cxp, f.IdTipoCbte_cxp, f.IdCbteCble_cxp, sum(MontoAplicado) MontoAplicado from cp_orden_pago_cancelaciones f inner join 
			ct_cbtecble as ct on f.IdEmpresa_pago = ct.IdEmpresa and f.IdTipoCbte_pago = ct.IdTipoCbte and f.IdCbteCble_pago = ct.IdCbteCble
			where f.IdEmpresa = @IdEmpresa and ct.cb_Fecha < @FechaIni
			group by f.IdEmpresa_cxp, f.IdTipoCbte_cxp, f.IdCbteCble_cxp
		)
		 AS CAN ON OG.IdEmpresa = CAN.IdEmpresa_cxp AND OG.IdTipoCbte_Ogiro = CAN.IdTipoCbte_cxp AND OG.IdCbteCble_Ogiro = CAN.IdCbteCble_cxp LEFT JOIN
		(
			SELECT C.IdEmpresa_Ogiro, C.IdTipoCbte_Ogiro, C.IdCbteCble_Ogiro, SUM(D.re_valor_retencion)re_valor_retencion  FROM cp_retencion AS C INNER JOIN cp_retencion_det AS D
			ON C.IdEmpresa = D.IdEmpresa AND C.IdRetencion = D.IdRetencion
			WHERE C.IdEmpresa = @IdEmpresa AND C.fecha < @FechaIni
			GROUP BY C.IdEmpresa_Ogiro, C.IdTipoCbte_Ogiro, C.IdCbteCble_Ogiro
		) R ON OG.IdEmpresa = R.IdEmpresa_Ogiro AND OG.IdTipoCbte_Ogiro = R.IdTipoCbte_Ogiro AND OG.IdCbteCble_Ogiro = R.IdCbteCble_Ogiro 

		WHERE og.IdEmpresa = @IdEmpresa and OG.Estado = 'A' AND OG.co_FechaFactura < @FechaIni 
		union all
		SELECT OG.cn_total - ISNULL(CAN.MontoAplicado,0) Saldo 
		FROM cp_nota_DebCre AS OG LEFT JOIN 
		(
			select f.IdEmpresa_cxp, f.IdTipoCbte_cxp, f.IdCbteCble_cxp, sum(MontoAplicado) MontoAplicado from cp_orden_pago_cancelaciones f inner join 
			ct_cbtecble as ct on f.IdEmpresa_pago = ct.IdEmpresa and f.IdTipoCbte_pago = ct.IdTipoCbte and f.IdCbteCble_pago = ct.IdCbteCble
			where f.IdEmpresa = @IdEmpresa and ct.cb_Fecha < @FechaIni
			group by f.IdEmpresa_cxp, f.IdTipoCbte_cxp, f.IdCbteCble_cxp
		)
		 AS CAN ON OG.IdEmpresa = CAN.IdEmpresa_cxp AND OG.IdTipoCbte_Nota = CAN.IdTipoCbte_cxp AND OG.IdCbteCble_Nota = CAN.IdCbteCble_cxp 
		WHERE og.IdEmpresa = @IdEmpresa and OG.Estado = 'A' AND OG.cn_fecha < @FechaIni AND OG.DebCre = 'D' 
		union all
		SELECT opd.Valor_a_pagar - ISNULL(CAN.MontoAplicado,0) Saldo 
		FROM cp_orden_pago as op inner join 
		cp_orden_pago_det as opd on op.IdEmpresa = opd.IdEmpresa and op.IdOrdenPago = opd.IdOrdenPago LEFT JOIN 
		(
			select f.IdEmpresa_cxp, f.IdTipoCbte_cxp, f.IdCbteCble_cxp, sum(MontoAplicado) MontoAplicado from cp_orden_pago_cancelaciones f inner join 
			ct_cbtecble as ct on f.IdEmpresa_pago = ct.IdEmpresa and f.IdTipoCbte_pago = ct.IdTipoCbte and f.IdCbteCble_pago = ct.IdCbteCble
			where f.IdEmpresa = @IdEmpresa and ct.cb_Fecha < @FechaIni
			group by f.IdEmpresa_cxp, f.IdTipoCbte_cxp, f.IdCbteCble_cxp
		)
		 AS CAN ON opd.IdEmpresa_cxp = CAN.IdEmpresa_cxp AND opd.IdTipoCbte_cxp = CAN.IdTipoCbte_cxp AND opd.IdCbteCble_cxp = CAN.IdCbteCble_cxp 
		WHERE op.IdEmpresa = @IdEmpresa and op.Estado = 'A' AND Fecha < @FechaIni AND IdTipo_Persona = 'PROVEE'  AND NOT EXISTS(
			SELECT IdEmpresa FROM cp_orden_giro ne
			where ne.IdEmpresa = opd.IdEmpresa_cxp
			and ne.IdTipoCbte_Ogiro = opd.IdTipoCbte_cxp
			and ne.IdCbteCble_Ogiro = opd.IdCbteCble_cxp
			)
			AND NOT EXISTS(
			SELECT IdEmpresa FROM cp_nota_DebCre ne
			where ne.idempresa = opd.IdEmpresa_cxp
			and ne.IdTipoCbte_Nota = opd.IdTipoCbte_cxp
			and ne.IdCbteCble_Nota = opd.IdCbteCble_cxp
			)
	) G
	*/
	SELECT SUM(D.dc_Valor)*-1 Saldo FROM ct_cbtecble AS C INNER JOIN ct_cbtecble_det AS D
	ON C.IdEmpresa = D.IdEmpresa AND C.IdTipoCbte = D.IdTipoCbte AND C.IdCbteCble = D.IdCbteCble
	WHERE C.IdEmpresa = @IdEmpresa AND C.cb_Fecha < @FechaIni AND D.IdCtaCble = '2110101'
	)A WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 3

	UPDATE [web].[ct_CONTA_011] set Columna2 = isnull(A.Valor,0)
	FROM(
		SELECT SUM(G.Valor) Valor FROM(
			SELECT SUM(OG.co_total) Valor FROM cp_orden_giro AS OG
			WHERE OG.IdEmpresa = @IdEmpresa AND OG.Estado = 'A' AND OG.co_FechaContabilizacion BETWEEN @FechaIni AND @FechaFin
			UNION ALL
			SELECT SUM(D.re_valor_retencion) *-1 FROM cp_retencion AS C INNER JOIN
			cp_retencion_det AS D ON C.IdEmpresa = D.IdEmpresa AND C.IdRetencion = D.IdRetencion 
			WHERE C.fecha BETWEEN @FechaIni AND @FechaFin AND C.IdEmpresa = @IdEmpresa AND C.Estado = 'A'
			UNION ALL
			SELECT SUM(CAN.MontoAplicado) *-1
			FROM cp_orden_pago_cancelaciones AS CAN INNER JOIN 
			ct_cbtecble AS C ON C.IdEmpresa = CAN.IdEmpresa_pago AND C.IdTipoCbte = CAN.IdTipoCbte_pago AND C.IdCbteCble = CAN.IdCbteCble_pago inner join
			cp_orden_pago as op on can.IdEmpresa_op = op.IdEmpresa and can.IdOrdenPago_op = op.IdOrdenPago
			WHERE C.IdEmpresa = @IdEmpresa AND C.cb_Fecha BETWEEN @FechaIni AND @FechaFin and op.IdTipo_Persona = 'PROVEE' AND
			EXISTS(
				SELECT * FROM cp_nota_DebCre AS NC 
				WHERE CAN.IdEmpresa_pago = NC.IdEmpresa AND CAN.IdTipoCbte_pago = NC.IdTipoCbte_Nota AND CAN.IdCbteCble_pago = NC.IdCbteCble_Nota
				AND NC.cn_fecha <= @FechaFin 
			)
			and exists(
				select d.IdEmpresa 
				from cp_conciliacion_Caja_det as d 
				where d.IdEmpresa_OP = can.IdEmpresa_op
				and d.IdOrdenPago_OP = can.IdOrdenPago_op
			)
			UNION ALL
			SELECT SUM(OG.cn_total) Valor FROM cp_nota_DebCre AS OG
			WHERE OG.IdEmpresa = @IdEmpresa AND OG.Estado = 'A' AND OG.DebCre = 'D' AND OG.Fecha_contable BETWEEN @FechaIni AND @FechaFin
			UNION ALL
			SELECT SUM(D.Valor_a_pagar) Valor FROM cp_orden_pago_det AS D INNER JOIN
			cp_orden_pago AS C ON C.IdEmpresa = D.IdEmpresa AND C.IdOrdenPago = D.IdOrdenPago
			WHERE D.IdEmpresa = @IdEmpresa AND C.Estado = 'A' AND C.Fecha BETWEEN @FechaIni AND @FechaFin 
			and c.IdTipo_Persona = 'PROVEE'
			AND NOT EXISTS(
			SELECT IdEmpresa FROM cp_orden_giro ne
			where ne.IdEmpresa = d.IdEmpresa_cxp
			and ne.IdTipoCbte_Ogiro = d.IdTipoCbte_cxp
			and ne.IdCbteCble_Ogiro = d.IdCbteCble_cxp
			)
			AND NOT EXISTS(
			SELECT IdEmpresa FROM cp_nota_DebCre ne
			where ne.idempresa = d.IdEmpresa_cxp
			and ne.IdTipoCbte_Nota = d.IdTipoCbte_cxp
			and ne.IdCbteCble_Nota = d.IdCbteCble_cxp
			)
		) G
	)A WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 3


	UPDATE [web].[ct_CONTA_011] set Columna3 = isnull(A.Valor,0)
	FROM(
	SELECT SUM(G.Valor) Valor FROM(
		SELECT SUM(CAN.MontoAplicado) Valor 
		FROM cp_orden_pago_cancelaciones AS CAN INNER JOIN 
		ct_cbtecble AS C ON C.IdEmpresa = CAN.IdEmpresa_pago AND C.IdTipoCbte = CAN.IdTipoCbte_pago AND C.IdCbteCble = CAN.IdCbteCble_pago inner join
		cp_orden_pago as op on can.IdEmpresa_op = op.IdEmpresa and can.IdOrdenPago_op = op.IdOrdenPago
		WHERE C.IdEmpresa = @IdEmpresa AND C.cb_Fecha BETWEEN @FechaIni AND @FechaFin and op.IdTipo_Persona = 'PROVEE' AND
		NOT EXISTS(
			SELECT * FROM cp_nota_DebCre AS NC
			WHERE CAN.IdEmpresa_pago = NC.IdEmpresa AND CAN.IdTipoCbte_pago = NC.IdTipoCbte_Nota AND CAN.IdCbteCble_pago = NC.IdCbteCble_Nota
			AND NC.cn_fecha <= @FechaFin
		)
		UNION ALL
		SELECT SUM(CAN.MontoAplicado) 
			FROM cp_orden_pago_cancelaciones AS CAN INNER JOIN 
			ct_cbtecble AS C ON C.IdEmpresa = CAN.IdEmpresa_pago AND C.IdTipoCbte = CAN.IdTipoCbte_pago AND C.IdCbteCble = CAN.IdCbteCble_pago inner join
			cp_orden_pago as op on can.IdEmpresa_op = op.IdEmpresa and can.IdOrdenPago_op = op.IdOrdenPago
			WHERE C.IdEmpresa = @IdEmpresa AND C.cb_Fecha BETWEEN @FechaIni AND @FechaFin and op.IdTipo_Persona = 'PROVEE' AND
			EXISTS(
				SELECT * FROM cp_nota_DebCre AS NC 
				WHERE CAN.IdEmpresa_pago = NC.IdEmpresa AND CAN.IdTipoCbte_pago = NC.IdTipoCbte_Nota AND CAN.IdCbteCble_pago = NC.IdCbteCble_Nota
				AND NC.cn_fecha <= @FechaFin 
			)
			and NOT exists(
				select d.IdEmpresa 
				from cp_conciliacion_Caja_det as d 
				where d.IdEmpresa_OP = can.IdEmpresa_op
				and d.IdOrdenPago_OP = can.IdOrdenPago_op
			)
		

	)G
	)A WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 3
	
	update [web].[ct_CONTA_011] set Columna7 = round(Columna1 + Columna2 - Columna3,2)
	WHERE [web].[ct_CONTA_011].IdEmpresa = @IdEmpresa
	and [web].[ct_CONTA_011].IdUsuario = @IdUsuario
	and [web].[ct_CONTA_011].Secuencia = 3
END


select * from [web].[ct_CONTA_011] where IdUsuario = @IdUsuario