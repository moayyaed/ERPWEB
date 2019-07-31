
--web.SPINV_009 2,'admin',1,8,1,8,1,9,1,99999,1,99999,1,99999
CREATE PROCEDURE [web].[SPINV_009]
(
@IdEmpresa int,
@IdUsuario varchar(50),
@IdSucursalIni int,
@IdSucursalFin int,
@IdBodegaIni int,
@IdBodegaFin int,
@IdCategoriaIni int,
@IdCategoriaFin int,
@IdLineaIni int,
@IdLineaFin int,
@IdGrupoIni int,
@IdGrupoFin int,
@IdSubGrupoIni int,
@IdSubGrupoFin int,
@ConsiderarSinAprobar bit,
@MostrarSinMovimiento bit,
@FechaIni date,
@FechaFin date
)
AS
DELETE [web].[in_SPINV_009] WHERE IdUsuario = @IdUsuario

INSERT INTO [web].[in_SPINV_009]
           ([IdEmpresa]
           ,[IdUsuario]
           ,[IdProducto]
           ,[IdSucursal]
           ,[IdBodega]
           ,[IdCategoria]
           ,[IdLinea]
           ,[IdGrupo]
           ,[IdSubGrupo]
           ,[pr_codigo]
           ,[pr_descripcion]
           ,[IdUnidadMedida]
           ,[NomUnidadMedida]
           ,[NomCategoria]
           ,[NomLinea]
           ,[NomGrupo]
           ,[NomSubGrupo]
           ,[CantidadInicial]
           ,[CostoInicial]
           ,[CantidadIngreso]
           ,[CostoIngreso]
           ,[CantidadEgreso]
           ,[CostoEgreso]
           ,[CantidadFinal]
           ,[CostoFinal])
select pxb.IdEmpresa, @IdUsuario, pxb.IdProducto, pxb.IdSucursal, pxb.IdBodega, p.IdCategoria, p.IdLinea, p.IdGrupo, p.IdSubGrupo,
p.pr_codigo, p.pr_descripcion, p.IdUnidadMedida_Consumo, u.Descripcion, c.ca_Categoria, l.nom_linea, g.nom_grupo, sg.nom_subgrupo,
0,0,0,0,0,0,0,0
from 
in_producto_x_tb_bodega as pxb inner join 
in_Producto as p on pxb.IdEmpresa = p.IdEmpresa and pxb.IdProducto = p.IdProducto inner join
tb_bodega as b on pxb.IdEmpresa = b.IdEmpresa and pxb.IdSucursal = b.IdSucursal and pxb.IdBodega = b.IdBodega inner join
tb_sucursal as s on s.IdEmpresa = b.IdEmpresa and s.IdSucursal = b.IdSucursal inner join
in_subgrupo as sg on p.IdEmpresa = sg.IdEmpresa and p.IdCategoria = sg.IdCategoria and p.IdLinea = sg.IdLinea and p.IdGrupo = sg.IdGrupo and p.IdSubGrupo = sg.IdSubgrupo inner join
in_grupo as g on sg.IdEmpresa = g.IdEmpresa and sg.IdCategoria = g.IdCategoria and sg.IdLinea = g.IdLinea and sg.IdGrupo = g.IdGrupo inner join
in_linea as l on l.IdEmpresa = g.IdEmpresa and l.IdCategoria = g.IdCategoria and l.IdLinea = g.IdLinea inner join
in_categorias as c on c.IdEmpresa = l.IdEmpresa and c.IdCategoria = l.IdCategoria inner join 
in_UnidadMedida as u on p.IdUnidadMedida_Consumo = u.IdUnidadMedida
where pxb.IdEmpresa = @IdEmpresa and pxb.IdSucursal between @IdSucursalIni and @IdSucursalFin and pxb.IdBodega between @IdBodegaIni and @IdBodegaFin
and p.IdCategoria between @IdCategoriaIni and @IdCategoriaFin and p.IdLinea between @IdLineaIni and @IdLineaFin and p.IdGrupo between @IdGrupoIni and @IdGrupoFin and p.IdSubGrupo between @IdSubGrupoIni and @IdSubGrupoFin


PRINT 'INSERTO SALDO INICIAL'
BEGIN
	UPDATE [web].[in_SPINV_009] SET CantidadInicial = A.Cantidad, CostoInicial = A.Costo
	FROM
	(
	SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto, SUM(D.dm_cantidad)Cantidad, sum(d.dm_cantidad *d.mv_costo) Costo
	FROM in_Ing_Egr_Inven AS C INNER JOIN 
	in_Ing_Egr_Inven_det AS D ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal AND C.IdMovi_inven_tipo = D.IdMovi_inven_tipo AND C.IdNumMovi = D.IdNumMovi
	WHERE D.IdEmpresa = @IdEmpresa AND D.IdSucursal BETWEEN @IdSucursalIni AND @IdSucursalFin AND D.IdBodega BETWEEN @IdBodegaIni AND @IdBodegaFin AND C.cm_fecha < @FechaIni
	AND @ConsiderarSinAprobar = 1 and c.Estado = 'A'
	GROUP BY D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto
	UNION ALL
	SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto, SUM(D.dm_cantidad)Cantidad, sum(d.dm_cantidad *d.mv_costo) Costo
	FROM in_movi_inve AS C INNER JOIN 
	in_movi_inve_detalle AS D ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal and c.IdBodega = d.IdBodega AND C.IdMovi_inven_tipo = D.IdMovi_inven_tipo AND C.IdNumMovi = D.IdNumMovi
	WHERE D.IdEmpresa = @IdEmpresa AND D.IdSucursal BETWEEN @IdSucursalIni AND @IdSucursalFin AND D.IdBodega BETWEEN @IdBodegaIni AND @IdBodegaFin AND C.cm_fecha < @FechaIni
	AND @ConsiderarSinAprobar = 0
	GROUP BY D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto
	)A
	WHERE [web].[in_SPINV_009].IdEmpresa = A.IdEmpresa
	AND [web].[in_SPINV_009].IdSucursal = A.IdSucursal
	AND [web].[in_SPINV_009].IdBodega = A.IdBodega
	AND [web].[in_SPINV_009].IdProducto = A.IdProducto
	AND [web].[in_SPINV_009].IdUsuario = @IdUsuario
END

PRINT 'INSERTO INGRESOS'
BEGIN
	UPDATE [web].[in_SPINV_009] SET CantidadIngreso = A.Cantidad, CostoIngreso = A.Costo
	FROM
	(
	SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto, SUM(D.dm_cantidad)Cantidad, sum(d.dm_cantidad *d.mv_costo) Costo
	FROM in_Ing_Egr_Inven AS C INNER JOIN 
	in_Ing_Egr_Inven_det AS D ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal AND C.IdMovi_inven_tipo = D.IdMovi_inven_tipo AND C.IdNumMovi = D.IdNumMovi
	WHERE D.IdEmpresa = @IdEmpresa AND D.IdSucursal BETWEEN @IdSucursalIni AND @IdSucursalFin AND D.IdBodega BETWEEN @IdBodegaIni AND @IdBodegaFin AND C.cm_fecha between @FechaIni and @FechaFin
	and d.dm_cantidad > 0 and c.Estado = 'A'
	AND @ConsiderarSinAprobar = 1
	GROUP BY D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto
	UNION ALL
	SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto, SUM(D.dm_cantidad)Cantidad, sum(d.dm_cantidad *d.mv_costo) Costo
	FROM in_movi_inve AS C INNER JOIN 
	in_movi_inve_detalle AS D ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal and c.IdBodega = d.IdBodega AND C.IdMovi_inven_tipo = D.IdMovi_inven_tipo AND C.IdNumMovi = D.IdNumMovi
	WHERE D.IdEmpresa = @IdEmpresa AND D.IdSucursal BETWEEN @IdSucursalIni AND @IdSucursalFin AND D.IdBodega BETWEEN @IdBodegaIni AND @IdBodegaFin AND C.cm_fecha between @FechaIni and @FechaFin
	and d.dm_cantidad > 0
	AND @ConsiderarSinAprobar = 0
	GROUP BY D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto
	)A
	WHERE [web].[in_SPINV_009].IdEmpresa = A.IdEmpresa
	AND [web].[in_SPINV_009].IdSucursal = A.IdSucursal
	AND [web].[in_SPINV_009].IdBodega = A.IdBodega
	AND [web].[in_SPINV_009].IdProducto = A.IdProducto
	AND [web].[in_SPINV_009].IdUsuario = @IdUsuario
END

PRINT 'INSERTO EGRESOS'
BEGIN
	UPDATE [web].[in_SPINV_009] SET CantidadEgreso = A.Cantidad, CostoEgreso = A.Costo
	FROM
	(
	SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto, SUM(D.dm_cantidad)Cantidad, sum(d.dm_cantidad *d.mv_costo) Costo
	FROM in_Ing_Egr_Inven AS C INNER JOIN 
	in_Ing_Egr_Inven_det AS D ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal AND C.IdMovi_inven_tipo = D.IdMovi_inven_tipo AND C.IdNumMovi = D.IdNumMovi
	WHERE D.IdEmpresa = @IdEmpresa AND D.IdSucursal BETWEEN @IdSucursalIni AND @IdSucursalFin AND D.IdBodega BETWEEN @IdBodegaIni AND @IdBodegaFin AND C.cm_fecha between @FechaIni and @FechaFin
	and d.dm_cantidad < 0 and c.Estado = 'A'
	AND @ConsiderarSinAprobar = 1
	GROUP BY D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto
	UNION ALL
	SELECT D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto, SUM(D.dm_cantidad)Cantidad, sum(d.dm_cantidad *d.mv_costo) Costo
	FROM in_movi_inve AS C INNER JOIN 
	in_movi_inve_detalle AS D ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal and c.IdBodega = d.IdBodega AND C.IdMovi_inven_tipo = D.IdMovi_inven_tipo AND C.IdNumMovi = D.IdNumMovi
	WHERE D.IdEmpresa = @IdEmpresa AND D.IdSucursal BETWEEN @IdSucursalIni AND @IdSucursalFin AND D.IdBodega BETWEEN @IdBodegaIni AND @IdBodegaFin AND C.cm_fecha between @FechaIni and @FechaFin
	and d.dm_cantidad < 0
	AND @ConsiderarSinAprobar = 0
	GROUP BY D.IdEmpresa, D.IdSucursal, D.IdBodega, D.IdProducto
	)A
	WHERE [web].[in_SPINV_009].IdEmpresa = A.IdEmpresa
	AND [web].[in_SPINV_009].IdSucursal = A.IdSucursal
	AND [web].[in_SPINV_009].IdBodega = A.IdBodega
	AND [web].[in_SPINV_009].IdProducto = A.IdProducto
	AND [web].[in_SPINV_009].IdUsuario = @IdUsuario
END

PRINT 'CALCULO FINAL'
BEGIN
	UPDATE [web].[in_SPINV_009] SET CantidadFinal = CantidadInicial + CantidadIngreso + CantidadEgreso, CostoFinal = CostoInicial + CostoIngreso + CostoEgreso WHERE IdUsuario = @IdUsuario
END

PRINT 'ELIMINAR REGISTROS SIN MOVIMIENTO'
IF(@MostrarSinMovimiento = 0)
BEGIN
	DELETE [web].[in_SPINV_009] WHERE IdUsuario = @IdUsuario AND CantidadInicial = 0 AND CantidadIngreso = 0 AND CantidadEgreso = 0 AND CantidadFinal = 0
END

select * from [web].[in_SPINV_009] where IdUsuario = @IdUsuario