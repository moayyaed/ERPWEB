--exec SPCONTA_CierreAnual 2,8,2019
CREATE PROCEDURE [dbo].[SPCONTA_CierreAnual]
(
@IdEmpresa int,
@IdSucursal int,
@IdAnio int
)
AS

select d.IdEmpresa, d.IdCtaCble, ISNULL(ROW_NUMBER() over(order by d.IdEmpresa),0) as Secuencia, d.IdCtaCble + ' - '+ pc.pc_Cuenta, round(sum(d.dc_Valor),2) dc_Valor, case when round(sum(d.dc_Valor),2) > 0 then round(sum(d.dc_Valor),2) else 0 end as dc_Valor_Debe,
case when round(sum(d.dc_Valor),2) < 0 then abs(round(sum(d.dc_Valor),2)) else 0 end as dc_Valor_Haber, d.IdCentroCosto, cc.cc_Descripcion, d.IdPunto_cargo_grupo, pg.nom_punto_cargo_grupo,
d.IdPunto_cargo, pp.nom_punto_cargo
from ct_cbtecble as c inner join 
ct_cbtecble_det as d on c.IdEmpresa = d.IdEmpresa and c.IdTipoCbte = d.IdTipoCbte and c.IdCbteCble = d.IdCbteCble inner join 
ct_plancta as pc on pc.IdEmpresa = d.IdEmpresa and pc.IdCtaCble = d.IdCtaCble inner join
ct_grupocble as g on pc.IdGrupoCble = g.IdGrupoCble left join 
ct_CentroCosto cc on cc.IdEmpresa = d.IdEmpresa and cc.IdCentroCosto = d.IdCentroCosto left join
ct_punto_cargo as pp on pp.IdEmpresa = d.IdEmpresa and pp.IdPunto_cargo = d.IdPunto_cargo left join
ct_punto_cargo_grupo as pg on pg.IdEmpresa = d.IdEmpresa and pg.IdPunto_cargo_grupo = d.IdPunto_cargo_grupo 
where d.IdEmpresa = @IdEmpresa and year(c.cb_Fecha) = @IdAnio and g.gc_estado_financiero = 'ER' and c.IdSucursal = @IdSucursal
group by d.IdEmpresa, d.IdCtaCble, pc.pc_Cuenta,d.IdCentroCosto, cc.cc_Descripcion, d.IdPunto_cargo_grupo, pg.nom_punto_cargo_grupo,
d.IdPunto_cargo, pp.nom_punto_cargo
having round(sum(dc_Valor),2) <> 0
UNION ALL
select d.IdEmpresa, isnull(A.IdCtaCbleCierre,''), 9999999 as Secuencia, A.IdCtaCbleCierre+' - '+cta.pc_Cuenta, round(sum(d.dc_Valor),2) dc_Valor, case when round(sum(d.dc_Valor),2) > 0 then round(sum(d.dc_Valor),2) else 0 end as dc_Valor_Debe,
case when round(sum(d.dc_Valor),2) < 0 then abs(round(sum(d.dc_Valor),2)) else 0 end as dc_Valor_Haber, NULL IdCentroCosto, NULL cc_Descripcion, NULL IdPunto_cargo_grupo, NULL nom_punto_cargo_grupo,
NULL IdPunto_cargo, NULL nom_punto_cargo
from ct_cbtecble as c inner join 
ct_cbtecble_det as d on c.IdEmpresa = d.IdEmpresa and c.IdTipoCbte = d.IdTipoCbte and c.IdCbteCble = d.IdCbteCble inner join 
ct_plancta as pc on pc.IdEmpresa = d.IdEmpresa and pc.IdCtaCble = d.IdCtaCble inner join
ct_grupocble as g on pc.IdGrupoCble = g.IdGrupoCble left JOIN 
ct_anio_fiscal_x_cuenta_utilidad AS A ON A.IdanioFiscal = year(c.cb_Fecha) AND A.IdanioFiscal = C.IdEmpresa left join
ct_plancta as cta on cta.IdEmpresa = a.IdEmpresa and a.IdCtaCbleCierre = cta.IdCtaCble
where d.IdEmpresa = @IdEmpresa and year(c.cb_Fecha) = @IdAnio and g.gc_estado_financiero = 'ER' and c.IdSucursal = @IdSucursal
group by d.IdEmpresa, A.IdCtaCbleCierre, cta.pc_Cuenta