--EXEC web.SPINV_022 1,1,1,2,2,0,99999,'',0,0,0,'2019/10/31',0,0,9999,0
CREATE PROCEDURE [web].[SPINV_022]
(
@IdEmpresa int,
@IdSucursal_ini int,
@IdSucursal_fin int,
@IdBodega_ini int,
@IdBodega_fin int,
@IdProducto_ini numeric,
@IdProducto_fin numeric,
@IdCategoria varchar(20),
@IdLinea int,
@IdGrupo int,
@IdSubGrupo int,
@fecha_corte datetime,
@mostrar_stock_0 bit,
@IdMarcaIni int,
@IdMarcaFin int,
@ConsiderarNoAprobados bit
)
AS
BEGIN

DELETE web.in_SPINV_022

BEGIN --INSERTO EN TABLA PK DE PRODUCTOS A MOSTRAR
	INSERT INTO web.in_SPINV_022
	SELECT in_producto_x_tb_bodega.IdEmpresa, in_producto_x_tb_bodega.IdSucursal, in_producto_x_tb_bodega.IdBodega, in_producto_x_tb_bodega.IdProducto, 0 AS Expr1, 0 AS Expr2, 0 AS Expr3, in_Producto.IdCategoria, in_Producto.IdLinea, 
		in_Producto.IdGrupo, in_Producto.IdSubGrupo, in_Producto.IdMarca,NULL,0,0,0,0
	FROM     in_producto_x_tb_bodega INNER JOIN
		in_Producto ON in_producto_x_tb_bodega.IdEmpresa = in_Producto.IdEmpresa AND in_producto_x_tb_bodega.IdProducto = in_Producto.IdProducto
	where in_producto_x_tb_bodega.IdEmpresa = @IdEmpresa
	AND IdSucursal between @IdSucursal_ini and @IdSucursal_fin
	AND IdBodega BETWEEN @IdBodega_ini and @IdBodega_fin
	and isnull(in_Producto.IdProducto_padre,in_Producto.IdProducto) between @IdProducto_ini and @IdProducto_fin
	and in_Producto.IdMarca between @IdMarcaIni and @IdMarcaFin
END

BEGIN --FILTRO POR CATEGORIZACION
	IF(@IdCategoria <> '')
	BEGIN
		DELETE web.in_SPINV_022 
		WHERE IdCategoria <>  @IdCategoria

		IF(@IdLinea != 0)
			BEGIN
				DELETE web.in_SPINV_022 
				WHERE IdLinea != @IdLinea

				IF(@IdGrupo != 0)
					BEGIN
					DELETE web.in_SPINV_022 
					WHERE IdGrupo != @IdGrupo
						IF(@IdSubGrupo != 0)
							BEGIN
								DELETE web.in_SPINV_022 
								WHERE IdSubGrupo != @IdSubGrupo
							END
					END					
			END	
	END
END

BEGIN --ACTUALIZO STOCK Y COSTO A LA FECHA
if(@ConsiderarNoAprobados = 1)
BEGIN
	UPDATE web.in_SPINV_022 SET Stock = ROUND(A.cantidad,2), Costo_total = A.costo_total, Costo_promedio = IIF(A.cantidad = 0, 0 ,A.costo_total / A.cantidad)
	FROM(
	SELECT det.IdEmpresa, det.IdSucursal, det.IdBodega, det.IdProducto, sum(dm_cantidad) cantidad, sum(dm_cantidad * mv_costo) costo_total
	FROM in_Ing_Egr_Inven cab inner join
	in_Ing_Egr_Inven_det det 
	on cab.IdEmpresa = det.IdEmpresa
	and cab.IdSucursal = det.IdSucursal
	and cab.IdBodega = det.IdBodega
	and cab.IdMovi_inven_tipo = det.IdMovi_inven_tipo
	and cab.IdNumMovi = det.IdNumMovi
	inner join web.in_SPINV_022 sp
	on sp.IdEmpresa = det.IdEmpresa
	and sp.IdSucursal = det.IdSucursal
	and sp.IdBodega = det.IdBodega
	and sp.IdProducto = det.IdProducto INNER JOIN
	in_Motivo_Inven AS mot on mot.IdEmpresa = cab.IdEmpresa
	and mot.IdMotivo_Inv = cab.IdMotivo_Inv and mot.Genera_Movi_Inven = 'S'
	AND CAB.Estado = 'A'
	WHERE cab.cm_fecha <= @fecha_corte
	group by det.IdEmpresa, det.IdSucursal, det.IdBodega, det.IdProducto
	) A
	WHERE web.in_SPINV_022.IdEmpresa = A.IdEmpresa
	AND web.in_SPINV_022.IdSucursal = A.IdSucursal
	AND web.in_SPINV_022.IdBodega = A.IdBodega
	and web.in_SPINV_022.IdProducto = A.IdProducto
END
ELSE
	UPDATE web.in_SPINV_022 SET Stock = ROUND(A.cantidad,2), Costo_total = A.costo_total, Costo_promedio = IIF(A.cantidad = 0, 0 ,A.costo_total / A.cantidad)
	FROM(
	SELECT det.IdEmpresa, det.IdSucursal, det.IdBodega, det.IdProducto, sum(dm_cantidad) cantidad, sum(dm_cantidad * mv_costo) costo_total
	FROM in_movi_inve cab inner join
	in_movi_inve_detalle det 
	on cab.IdEmpresa = det.IdEmpresa
	and cab.IdSucursal = det.IdSucursal
	and cab.IdBodega = det.IdBodega
	and cab.IdMovi_inven_tipo = det.IdMovi_inven_tipo
	and cab.IdNumMovi = det.IdNumMovi
	inner join web.in_SPINV_003 sp
	on sp.IdEmpresa = det.IdEmpresa
	and sp.IdSucursal = det.IdSucursal
	and sp.IdBodega = det.IdBodega
	and sp.IdProducto = det.IdProducto
	WHERE cab.cm_fecha <= @fecha_corte	
	group by det.IdEmpresa, det.IdSucursal, det.IdBodega, det.IdProducto
	) A
	WHERE web.in_SPINV_022.IdEmpresa = A.IdEmpresa
	AND web.in_SPINV_022.IdSucursal = A.IdSucursal
	AND web.in_SPINV_022.IdBodega = A.IdBodega
	and web.in_SPINV_022.IdProducto = A.IdProducto
END

IF(@mostrar_stock_0 = 0)--ELIMINO STOCK 0 SI EL PARAMETRO LO DICE
BEGIN
	DELETE web.in_SPINV_022 
	WHERE Stock = 0
END

BEGIN --ACTUALIZO DATOS DE ULTIMA COMPRA

update web.in_SPINV_022 set FechaUltCompra = a.oc_fecha, CostoUltCompra = a.do_precioFinal, CostoTotalUltCompra = Stock * a.do_precioFinal, DiasEnInventario = DATEDIFF(day,a.oc_fecha,@fecha_corte),
VariacionNIC = Costo_total - (Stock * a.do_precioFinal)
from
(
	select c.IdEmpresa, c.IdSucursalInv, c.IdBodega, c.IdProducto, a.oc_fecha, b.do_precioFinal
	from com_ordencompra_local a inner join com_ordencompra_local_det as b on
	a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdOrdenCompra = b.IdOrdenCompra inner join 
	(select d.IdEmpresa, d.IdSucursal,d.IdProducto, max(d.IdOrdenCompra) IdOrdenCompra,
	det.IdSucursal as IdSucursalInv, det.IdBodega
	from com_ordencompra_local as c inner join 
	com_ordencompra_local_det as d on c.IdEmpresa = d.IdEmpresa and c.IdSucursal = d.IdSucursal and c.IdOrdenCompra = d.IdOrdenCompra inner join
	in_Ing_Egr_Inven_det as det on d.IdEmpresa = det.IdEmpresa_oc and d.IdSucursal = det.IdSucursal_oc and d.IdOrdenCompra = det.IdOrdenCompra and d.Secuencia = det.Secuencia_oc inner join
	in_movi_inve_detalle as md on md.IdEmpresa = det.IdEmpresa_inv and md.IdSucursal = det.IdSucursal_inv and md.IdBodega = det.IdBodega_inv and md.IdMovi_inven_tipo = det.IdMovi_inven_tipo_inv
	and md.IdNumMovi = det.IdNumMovi_inv and md.Secuencia = det.secuencia_inv inner join
	web.in_SPINV_022 as sp on det.IdEmpresa = sp.IdEmpresa and det.IdSucursal = sp.IdSucursal and det.IdBodega = sp.IdBodega and det.IdProducto = sp.IdProducto
	group by d.IdEmpresa, d.IdSucursal,d.IdProducto,det.IdSucursal, det.IdBodega
	) as c on b.IdEmpresa = c.IdEmpresa and b.IdSucursal = c.IdSucursal and b.IdOrdenCompra = c.IdOrdenCompra and b.IdProducto = c.IdProducto
) a
where web.in_SPINV_022.IdEmpresa = a.IdEmpresa
and web.in_SPINV_022.IdSucursal = a.IdSucursalInv
and web.in_SPINV_022.IdBodega = a.IdBodega
and web.in_SPINV_022.IdProducto = a.IdProducto


END

SELECT sp.IdEmpresa, sp.IdSucursal, sp.IdBodega, sp.IdProducto, sp.Stock, sp.Costo_promedio, sp.Costo_total, s.Su_Descripcion, b.bo_Descripcion, p.pr_codigo, p.pr_descripcion, p.lote_num_lote, p.lote_fecha_vcto, c.IdCategoria, c.ca_Categoria, 
                  l.IdLinea, l.nom_linea, g.IdGrupo, g.nom_grupo, sg.IdSubgrupo, sg.nom_subgrupo, pr.IdPresentacion, pr.nom_presentacion, sp.IdMarca, mar.Descripcion AS NomMarca, uni.IdUnidadMedida, uni.Descripcion NomUnidad,
				  sp.FechaUltCompra, sp.CostoUltCompra, sp.CostoTotalUltCompra, sp.DiasEnInventario, sp.VariacionNIC
FROM     in_linea AS l INNER JOIN
                  in_grupo AS g INNER JOIN
                  in_subgrupo AS sg ON g.IdEmpresa = sg.IdEmpresa AND g.IdCategoria = sg.IdCategoria AND g.IdLinea = sg.IdLinea AND g.IdGrupo = sg.IdGrupo ON l.IdEmpresa = g.IdEmpresa AND l.IdCategoria = g.IdCategoria AND 
                  l.IdLinea = g.IdLinea INNER JOIN
                  in_categorias AS c ON l.IdEmpresa = c.IdEmpresa AND l.IdCategoria = c.IdCategoria RIGHT OUTER JOIN
                  web.in_SPINV_022 AS sp INNER JOIN
                  in_Producto AS p ON sp.IdEmpresa = p.IdEmpresa AND sp.IdProducto = p.IdProducto LEFT OUTER JOIN
                  tb_sucursal AS s INNER JOIN
                  tb_bodega AS b ON s.IdEmpresa = b.IdEmpresa AND s.IdSucursal = b.IdSucursal ON sp.IdEmpresa = b.IdEmpresa AND sp.IdSucursal = b.IdSucursal AND sp.IdBodega = b.IdBodega ON sg.IdEmpresa = p.IdEmpresa AND 
                  sg.IdCategoria = p.IdCategoria AND sg.IdLinea = p.IdLinea AND sg.IdGrupo = p.IdGrupo AND sg.IdSubgrupo = p.IdSubGrupo LEFT OUTER JOIN
                  in_presentacion AS pr ON p.IdEmpresa = pr.IdEmpresa AND p.IdPresentacion = pr.IdPresentacion LEFT OUTER JOIN
                  in_Marca AS mar ON mar.IdEmpresa = sp.IdEmpresa AND mar.IdMarca = sp.IdMarca inner join
				  in_UnidadMedida as uni on p.IdUnidadMedida_Consumo = uni.IdUnidadMedida
END