-- exec [EntidadRegulatoria].[GenerarDINARDAP]1,1,1,'01/09/2019','30/09/2019'
CREATE PROCEDURE [EntidadRegulatoria].[GenerarDINARDAP]
	@IdEmpresa as int,	
	@SucursalIni as int,
	@SucursalFin as int,
	@fechaIni as datetime,
	@fechaFin as datetime
AS
BEGIN

	SET NOCOUNT ON;


select      Facturas_y_notas_deb.IdEmpresa ,Facturas_y_notas_deb.IdSucursal,Facturas_y_notas_deb.IdBodega,Facturas_y_notas_deb.IdCliente,Facturas_y_notas_deb.Codigo,Facturas_y_notas_deb.IdCbteVta,Facturas_y_notas_deb.CodCbteVta,Facturas_y_notas_deb.vt_tipoDoc,Facturas_y_notas_deb.vt_serie1,Facturas_y_notas_deb.vt_serie2,
            Facturas_y_notas_deb.vt_NumFactura,Facturas_y_notas_deb.Su_Descripcion,RTRIM(LTRIM(Facturas_y_notas_deb.pe_nombreCompleto)) AS pe_nombreCompleto,Facturas_y_notas_deb.pe_cedulaRuc,
			Facturas_y_notas_deb.Valor_Original as Valor_Original,
			isnull(Cobros_x_fac.dc_ValorPago,0) as Total_Pagado,
            
			IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fecha,@fechaFin)>=0  ,  Facturas_y_notas_deb.Valor_Original -(isnull( Cobros_x_fac.dc_ValorPago,0)) ,0) Valor_x_Vencer,
			IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fecha,@fechaFin)>=0 and  DATEDIFF( day,Facturas_y_notas_deb.vt_fecha,@fechaFin )<=30 and (DATEDIFF(DAY,Facturas_y_notas_deb.vt_fech_venc,@fechaFin)<=0),  Facturas_y_notas_deb.Valor_Original - isnull( Cobros_x_fac.dc_ValorPago,0) ,0) x_Vencer_1_30_Dias,
			IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fecha,@fechaFin )>30 and  DATEDIFF( day,Facturas_y_notas_deb.vt_fecha,@fechaFin )<=90 and (DATEDIFF(DAY,Facturas_y_notas_deb.vt_fech_venc,@fechaFin)<=0),   Facturas_y_notas_deb.Valor_Original -  isnull( Cobros_x_fac.dc_ValorPago,0) ,0) x_Vencer_31_90_Dias,
			IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fecha,@fechaFin )>90 and  DATEDIFF( day,Facturas_y_notas_deb.vt_fecha,@fechaFin )<=180 and (DATEDIFF(DAY,Facturas_y_notas_deb.vt_fech_venc,@fechaFin)<=0),  Facturas_y_notas_deb.Valor_Original - isnull( Cobros_x_fac.dc_ValorPago,0) ,0) x_Vencer_91_180_Dias,
			IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fecha,@fechaFin )>180 and  DATEDIFF( day,Facturas_y_notas_deb.vt_fecha,@fechaFin )<=360 and (DATEDIFF(DAY,Facturas_y_notas_deb.vt_fech_venc,@fechaFin)<=0),  Facturas_y_notas_deb.Valor_Original - isnull( Cobros_x_fac.dc_ValorPago,0) ,0) x_Vencer_181_360_Dias,
			IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fecha,@fechaFin )>360 and (DATEDIFF(DAY,Facturas_y_notas_deb.vt_fech_venc,@fechaFin)<=0),  Facturas_y_notas_deb.Valor_Original - isnull( Cobros_x_fac.dc_ValorPago,0) ,0) x_Vencer_Mayor_a_360Dias,

			ROUND(IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin)>=0 ,  Facturas_y_notas_deb.Valor_Original -isnull( Cobros_x_fac.dc_ValorPago,0) ,0),2) Valor_Vencido,
			IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin)>=0 and  DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin )<=30 and (DATEDIFF(DAY,Facturas_y_notas_deb.vt_fech_venc,@fechaFin)>0),  Facturas_y_notas_deb.Valor_Original -isnull( Cobros_x_fac.dc_ValorPago,0) ,0) Vencido_1_30_Dias,
			IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin )>30 and  DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin )<=90 and (DATEDIFF(DAY,Facturas_y_notas_deb.vt_fech_venc,@fechaFin)>0),   Facturas_y_notas_deb.Valor_Original - isnull( Cobros_x_fac.dc_ValorPago,0) ,0) Vencido_31_90_Dias,
			IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin )>90 and  DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin )<=180 and (DATEDIFF(DAY,Facturas_y_notas_deb.vt_fech_venc,@fechaFin)>0),  Facturas_y_notas_deb.Valor_Original -isnull( Cobros_x_fac.dc_ValorPago,0) ,0) Vencido_91_180_Dias,
			IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin )>180 and  DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin )<=360 and (DATEDIFF(DAY,Facturas_y_notas_deb.vt_fech_venc,@fechaFin)>0),  Facturas_y_notas_deb.Valor_Original -isnull( Cobros_x_fac.dc_ValorPago,0) ,0) Vencido_181_360_Dias,
			IIF( DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin )>360,  Facturas_y_notas_deb.Valor_Original - isnull( Cobros_x_fac.dc_ValorPago,0) ,0) Vencido_Mayor_a_360Dias
			,Facturas_y_notas_deb.vt_fech_venc,Facturas_y_notas_deb.vt_fecha,Facturas_y_notas_deb.Idtipo_cliente, iif(DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin )<0,0,DATEDIFF( day,Facturas_y_notas_deb.vt_fech_venc,@fechaFin )) Dias_Vencidos,
			cast( Facturas_y_notas_deb.Valor_Original-isnull( Cobros_x_fac.dc_ValorPago,0) as numeric(10,2))Total,Facturas_y_notas_deb.pe_telefonoOfic
			,Facturas_y_notas_deb.Cod_Provincia ,Facturas_y_notas_deb.Cod_Ciudad,Facturas_y_notas_deb.cod_parroquia,Facturas_y_notas_deb.pe_Naturaleza,iif(Facturas_y_notas_deb.pe_Naturaleza='J','',Facturas_y_notas_deb.pe_sexo)pe_sexo
			,Facturas_y_notas_deb.IdTipoDocumento,iif(Facturas_y_notas_deb.pe_Naturaleza='J','',Facturas_y_notas_deb.IdEstadoCivil)IdEstadoCivil,CAST(ISNULL(Facturas_y_notas_deb.Plazo,0) AS NUMERIC)Plazo,Facturas_y_notas_deb.cod_entidad_dinardap
			,NULL cr_fechaCobro
			,NULL Valor_cobrado
 from 
(

					SELECT        F.IdEmpresa, F.IdSucursal, F.IdBodega, fa_cliente.IdCliente, fa_cliente.Codigo, F.IdCbteVta, F.CodCbteVta, F.vt_tipoDoc, F.vt_serie1, F.vt_serie2, F.vt_NumFactura, tb_sucursal.Su_Descripcion, 
								LTRIM(tb_persona.pe_nombreCompleto) AS pe_nombreCompleto, 
								CASE WHEN tb_persona.pe_Naturaleza = 'NATU' AND tb_persona.IdTipoDocumento = 'RUC' AND LEN(tb_persona.pe_cedulaRuc) > 10 THEN
								SUBSTRING(tb_persona.pe_cedulaRuc,0,11) ELSE tb_persona.pe_cedulaRuc END pe_cedulaRuc, 
								
								cast(FD.Total as float) AS Valor_Original, case when f.vt_plazo = 0 then dateadd(day,1,F.vt_fech_venc) else f.vt_fech_venc end as vt_fech_venc, F.vt_fecha, 
								fa_cliente.Idtipo_cliente, fa_cliente_contactos.Telefono AS pe_telefonoOfic, tb_provincia.Cod_Provincia, tb_ciudad.Cod_Ciudad, tb_parroquia.cod_parroquia, 
								tb_persona.pe_Naturaleza, tb_persona.pe_sexo, 
								
								CASE WHEN tb_persona.pe_Naturaleza = 'NATU' AND tb_persona.IdTipoDocumento = 'RUC' AND LEN(tb_persona.pe_cedulaRuc) > 10 THEN
								'CED' ELSE tb_persona.IdTipoDocumento END IdTipoDocumento, 
								
								tb_persona.IdEstadoCivil, case when F.vt_plazo = 0 then 1 else DATEDIFF(DAY,F.VT_FECHA, F.vt_fech_venc) /*F.vt_plazo*/ end AS Plazo, tb_empresa.cod_entidad_dinardap
					FROM            fa_factura AS F INNER JOIN
								fa_factura_resumen AS FD ON F.IdEmpresa = FD.IdEmpresa AND F.IdSucursal = FD.IdSucursal AND F.IdBodega = FD.IdBodega AND F.IdCbteVta = FD.IdCbteVta INNER JOIN
								fa_cliente ON F.IdEmpresa = fa_cliente.IdEmpresa AND F.IdCliente = fa_cliente.IdCliente INNER JOIN
								fa_cliente_contactos on f.IdEmpresa = fa_cliente_contactos.IdEmpresa and f.IdCliente = fa_cliente_contactos.IdCliente and f.IdContacto = fa_cliente_contactos.IdContacto INNER JOIN
								tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona INNER JOIN
								tb_sucursal ON F.IdEmpresa = tb_sucursal.IdEmpresa AND F.IdSucursal = tb_sucursal.IdSucursal AND F.Estado = 'A' INNER JOIN
								tb_ciudad ON fa_cliente_contactos.IdCiudad = tb_ciudad.IdCiudad INNER JOIN
								tb_provincia ON tb_ciudad.IdProvincia = tb_provincia.IdProvincia INNER JOIN
								tb_empresa ON F.IdEmpresa = tb_empresa.IdEmpresa LEFT OUTER JOIN
								tb_parroquia ON fa_cliente_contactos.IdParroquia = tb_parroquia.IdParroquia
					WHERE        (F.vt_fecha <=  @fechaFin) AND F.IdEmpresa = @IdEmpresa AND F.Estado = 'A'

						 
						 	

		
-- *******************************************************************************************************************************************
-- notas de debito
union 


SELECT        fa_notaCreDeb.IdEmpresa, fa_notaCreDeb.IdSucursal, fa_notaCreDeb.IdBodega, fa_cliente.IdCliente, fa_cliente.Codigo, fa_notaCreDeb.IdNota, fa_notaCreDeb.CodNota, 
                         CASE WHEN dbo.fa_notaCreDeb.CodDocumentoTipo IS NULL THEN 'NTDB' ELSE dbo.fa_notaCreDeb.CodDocumentoTipo END AS CodDocumentoTipo, fa_notaCreDeb.Serie1, fa_notaCreDeb.Serie2, 
                         ISNULL(fa_notaCreDeb.NumNota_Impresa, fa_notaCreDeb.IdNota) AS Expr1, tb_sucursal.Su_Descripcion, RTRIM(LTRIM(tb_persona.pe_nombreCompleto)) AS Expr2, 
                         
						 CASE WHEN tb_persona.pe_Naturaleza = 'NATU' AND tb_persona.IdTipoDocumento = 'RUC' AND LEN(tb_persona.pe_cedulaRuc) > 10 THEN
								SUBSTRING(tb_persona.pe_cedulaRuc,0,11) ELSE tb_persona.pe_cedulaRuc END pe_cedulaRuc
						 , cast(fa_notaCreDeb_resumen.Total as float), case when fa_notaCreDeb.no_fecha_venc = fa_notaCreDeb.no_fecha then dateadd(day,1,fa_notaCreDeb.no_fecha) else fa_notaCreDeb.no_fecha_venc end, fa_notaCreDeb.no_fecha, fa_cliente.Idtipo_cliente, 
                         fa_cliente_contactos.Telefono AS pe_telefonoOfic, tb_provincia.Cod_Provincia, tb_ciudad.Cod_Ciudad, tb_parroquia.cod_parroquia, tb_persona.pe_Naturaleza, 
                         tb_persona.pe_sexo, 
						 CASE WHEN tb_persona.pe_Naturaleza = 'NATU' AND tb_persona.IdTipoDocumento = 'RUC' AND LEN(tb_persona.pe_cedulaRuc) > 10 THEN
								'CED' ELSE tb_persona.IdTipoDocumento END IdTipoDocumento, 
						  tb_persona.IdEstadoCivil, case when fa_notaCreDeb.no_fecha = fa_notaCreDeb.no_fecha_venc then 1 else DATEDIFF(day, fa_notaCreDeb.no_fecha, fa_notaCreDeb.no_fecha_venc) end AS Plazo, tb_empresa.cod_entidad_dinardap
FROM            fa_notaCreDeb INNER JOIN
                         fa_notaCreDeb_resumen ON fa_notaCreDeb.IdEmpresa = fa_notaCreDeb_resumen.IdEmpresa AND fa_notaCreDeb.IdSucursal = fa_notaCreDeb_resumen.IdSucursal AND 
                         fa_notaCreDeb.IdBodega = fa_notaCreDeb_resumen.IdBodega AND fa_notaCreDeb.IdNota = fa_notaCreDeb_resumen.IdNota INNER JOIN
                         fa_cliente ON fa_notaCreDeb.IdEmpresa = fa_cliente.IdEmpresa AND fa_notaCreDeb.IdCliente = fa_cliente.IdCliente INNER JOIN 
						 fa_cliente_contactos on fa_notaCreDeb.IdEmpresa = fa_cliente_contactos.IdEmpresa and fa_notaCreDeb.IdCliente = fa_cliente_contactos.IdCliente and fa_notaCreDeb.IdContacto = fa_cliente_contactos.IdContacto INNER JOIN
                         tb_sucursal ON fa_notaCreDeb.IdEmpresa = tb_sucursal.IdEmpresa AND fa_notaCreDeb.IdSucursal = tb_sucursal.IdSucursal INNER JOIN
                         tb_persona ON fa_cliente.IdPersona = tb_persona.IdPersona INNER JOIN
                         tb_ciudad ON fa_cliente_contactos.IdCiudad = tb_ciudad.IdCiudad INNER JOIN
                         tb_provincia ON tb_ciudad.IdProvincia = tb_provincia.IdProvincia INNER JOIN
                         tb_empresa ON fa_notaCreDeb.IdEmpresa = tb_empresa.IdEmpresa LEFT OUTER JOIN
                         tb_parroquia ON fa_cliente_contactos.IdParroquia = tb_parroquia.IdParroquia 
WHERE        (fa_notaCreDeb.CreDeb = 'D') AND (fa_notaCreDeb.no_fecha  <=  @fechaFin) AND (fa_notaCreDeb.Estado = 'A') AND fa_notaCreDeb.IdEmpresa = @IdEmpresa


) as  Facturas_y_notas_deb left join
(

		SELECT                   dbo.cxc_cobro.IdEmpresa, dbo.cxc_cobro.IdSucursal,  dbo.cxc_cobro_det.dc_TipoDocumento, dbo.cxc_cobro_det.IdBodega_Cbte, 
								 dbo.cxc_cobro_det.IdCbte_vta_nota, sum(dbo.cxc_cobro_det.dc_ValorPago) as dc_ValorPago ,
								 Cobros_x_periodo.Valor_cobrado, Cobros_x_periodo.cr_fechaCobro
		FROM                     dbo.cxc_cobro INNER JOIN
								 dbo.cxc_cobro_det ON dbo.cxc_cobro.IdEmpresa = dbo.cxc_cobro_det.IdEmpresa AND dbo.cxc_cobro.IdSucursal = dbo.cxc_cobro_det.IdSucursal AND 
								 dbo.cxc_cobro.IdCobro = dbo.cxc_cobro_det.IdCobro 
		                         and dbo.cxc_cobro.cr_estado='A' LEFT JOIN(
									select sum(det.dc_ValorPago) Valor_cobrado,
									max(cab.cr_fechaCobro)cr_fechaCobro,
									det.IdEmpresa,
									det.IdSucursal,
									det.IdBodega_Cbte,
									det.IdCbte_vta_nota,
									det.dc_TipoDocumento
									from cxc_cobro_det det, cxc_cobro cab 									
									where cab.cr_fechaCobro between @fechaIni and @fechaFin AND cab.IdEmpresa = det.IdEmpresa and cab.IdCobro = det.IdCobro
									group by det.IdEmpresa,det.IdSucursal,det.IdBodega_Cbte,det.IdCbte_vta_nota, det.dc_TipoDocumento
								 )Cobros_x_periodo on Cobros_x_periodo.IdEmpresa = cxc_cobro_det.IdEmpresa and Cobros_x_periodo.IdSucursal = cxc_cobro_det.IdSucursal
								 and Cobros_x_periodo.IdBodega_Cbte = cxc_cobro_det.IdBodega_Cbte and Cobros_x_periodo.IdCbte_vta_nota = cxc_cobro_det.IdCbte_vta_nota
								 and Cobros_x_periodo.dc_TipoDocumento = cxc_cobro_det.dc_TipoDocumento 								 
WHERE					 dbo.cxc_cobro.cr_fechaCobro <= @fechaFin AND cxc_cobro.IdEmpresa = @IdEmpresa
		GROUP BY                 dbo.cxc_cobro.IdEmpresa, dbo.cxc_cobro.IdSucursal,  dbo.cxc_cobro_det.dc_TipoDocumento, dbo.cxc_cobro_det.IdBodega_Cbte, 
								 dbo.cxc_cobro_det.IdCbte_vta_nota,Cobros_x_periodo.Valor_cobrado,Cobros_x_periodo.cr_fechaCobro

) as Cobros_x_fac
on Facturas_y_notas_deb.IdEmpresa=Cobros_x_fac.IdEmpresa
and Facturas_y_notas_deb.IdSucursal=Cobros_x_fac.IdSucursal
and Facturas_y_notas_deb.IdBodega=Cobros_x_fac.IdBodega_Cbte
and Facturas_y_notas_deb.IdCbteVta=Cobros_x_fac.IdCbte_vta_nota
and Facturas_y_notas_deb.vt_tipoDoc=Cobros_x_fac.dc_TipoDocumento
where 
    ROUND(Facturas_y_notas_deb.Valor_Original - isnull(Cobros_x_fac.dc_ValorPago,0),2)>60
	--and Facturas_y_notas_deb.vt_fech_venc < @FechaFin
	and Facturas_y_notas_deb.vt_fecha <= @fechaFin
	--and Facturas_y_notas_deb.IdCliente = 737
END