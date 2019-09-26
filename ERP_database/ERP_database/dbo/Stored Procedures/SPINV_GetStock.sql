--exec SPINV_GetStock 1,1,2,'2019/12/10'
CREATE PROCEDURE [dbo].[SPINV_GetStock]
(
@IdEmpresa int,
@IdSucursal int,
@IdBodega int,
@FechaCorte date
)
AS

DECLARE @IdPeriodo int, @IdFecha int

SET @IdPeriodo = cast( cast(year(@FechaCorte) as varchar(4)) + right('00'+ cast(month(@FechaCorte) as varchar(2)),2)+ right('00'+ cast(day(@FechaCorte) as varchar(2)),2) as int)


SELECT d.IdEmpresa, d.IdSucursal, d.IdBodega, d.IdProducto, d.IdUnidadMedida,ISNULL(sum(dm_cantidad),0) Stock, ISNULL(CostoHistorico.costo,0) UltimoCosto, isnull(CostoHistorico.IdFecha,0) IdFecha,p.pr_descripcion,p.pr_codigo,
p.IdCategoria, p.IdGrupo, p.IdSubGrupo, p.IdLinea, ca.ca_Categoria, gr.nom_grupo, li.nom_linea
FROM in_movi_inve_detalle AS D INNER JOIN
in_movi_inve AS C ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal AND C.IdBodega = D.IdBodega AND C.IdMovi_inven_tipo = D.IdMovi_inven_tipo AND C.IdNumMovi = D.IdNumMovi LEFT JOIN
(
select* from(
select ROW_NUMBER() over(partition by c.IdEmpresa, c.IdSucursal, c.IdBodega, c.IdProducto order by c.IdEmpresa, c.IdSucursal, c.IdBodega, c.IdProducto, c.IdFecha desc) IdRow, c.IdEmpresa, c.IdSucursal, c.IdBodega, c.IdProducto, c.costo, IdFecha
from in_producto_x_tb_bodega_Costo_Historico as c 
where c.IdEmpresa = @IdEmpresa and c.IdSucursal = @IdSucursal and c.IdBodega = @IdBodega and c.IdFecha <= @IdPeriodo
) a where a.IdRow = 1
) CostoHistorico on CostoHistorico.IdEmpresa = d.IdEmpresa and CostoHistorico.IdSucursal = d.IdSucursal and CostoHistorico.IdBodega = d.IdBodega and CostoHistorico.IdProducto = d.IdProducto left join
in_Producto as p on p.IdEmpresa = d.IdEmpresa and p.IdProducto = d.IdProducto left join
in_categorias as ca on ca.IdEmpresa = p.IdEmpresa and ca.IdCategoria = p.IdCategoria left join
in_linea as li on p.IdEmpresa = li.IdEmpresa and p.IdCategoria = li.IdCategoria and p.IdLinea = li.IdLinea left join
in_grupo as gr on gr.IdEmpresa = p.IdEmpresa and gr.IdCategoria = p.IdCategoria and gr.IdLinea = p.IdLinea and gr.IdGrupo = p.IdGrupo
WHERE D.IdEmpresa = @IdEmpresa AND D.IdSucursal = @IdSucursal AND D.IdBodega = @IdBodega AND c.cm_fecha <= @FechaCorte
group by d.IdEmpresa, d.IdSucursal, d.IdBodega, d.IdProducto, d.IdUnidadMedida,CostoHistorico.costo,CostoHistorico.IdFecha, p.pr_descripcion,p.pr_codigo,p.IdCategoria, p.IdGrupo, p.IdSubGrupo, p.IdLinea, ca.ca_Categoria, gr.nom_grupo, li.nom_linea