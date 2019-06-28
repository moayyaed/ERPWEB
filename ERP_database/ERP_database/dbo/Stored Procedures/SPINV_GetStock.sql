--exec SPINV_GetStock 2,1,1,72,'2019/12/01'
CREATE PROCEDURE SPINV_GetStock
(
@IdEmpresa int,
@IdSucursal int,
@IdBodega int,
@IdProducto numeric,
@FechaCorte date
)
AS

DECLARE @UltCosto float, @IdPeriodo int, @IdFecha int

SET @IdPeriodo = cast( cast(year(@FechaCorte) as varchar(4)) + right('00'+ cast(month(@FechaCorte) as varchar(2)),2)+ right('00'+ cast(day(@FechaCorte) as varchar(2)),2) as int)
select top 1 @UltCosto = c.costo, @IdFecha = IdFecha
from in_producto_x_tb_bodega_Costo_Historico as c 
where c.IdEmpresa = @IdEmpresa and c.IdSucursal = @IdSucursal and c.IdBodega = @IdBodega and c.IdProducto = @IdProducto and c.IdFecha <= @IdPeriodo
order by c.IdEmpresa, c.IdSucursal, c.IdBodega, c.IdProducto, c.IdFecha desc

SELECT d.IdEmpresa, d.IdSucursal, d.IdBodega, d.IdProducto, d.IdUnidadMedida,ISNULL(sum(dm_cantidad),0) Stock, ISNULL(@UltCosto,0) UltimoCosto, isnull(@IdFecha,0) IdFecha
FROM in_movi_inve_detalle AS D INNER JOIN
in_movi_inve AS C ON C.IdEmpresa = D.IdEmpresa AND C.IdSucursal = D.IdSucursal AND C.IdBodega = D.IdBodega AND C.IdMovi_inven_tipo = D.IdMovi_inven_tipo AND C.IdNumMovi = D.IdNumMovi
WHERE D.IdEmpresa = @IdEmpresa AND D.IdSucursal = @IdSucursal AND D.IdBodega = @IdBodega AND D.IdProducto = @IdProducto and c.cm_fecha <= @FechaCorte
group by d.IdEmpresa, d.IdSucursal, d.IdBodega, d.IdProducto, d.IdUnidadMedida