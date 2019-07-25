create view vwct_anio_fiscal_x_tb_sucursal as
select 
a.IdEmpresa, a.IdSucursal, c.Su_Descripcion, a.IdanioFiscal,a.IdTipoCbte, a.IdCbteCble, b.cb_Observacion
from ct_anio_fiscal_x_tb_sucursal a
inner join tb_sucursal c on c.IdEmpresa = a.IdEmpresa
inner join ct_cbtecble b on a.IdEmpresa = b.IdEmpresa and a.IdTipoCbte = b.IdTipoCbte and a.IdCbteCble = b.IdCbteCble