create view vwct_anio_fiscal_masivo as
select * from ct_anio_fiscal a where not exists 
(
select * from ct_periodo p where a.IdanioFiscal=p.IdanioFiscal
)