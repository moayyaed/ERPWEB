CREATE VIEW web.VWFAC_018
AS
select nc.IdEmpresa, nc.IdSucursal, nc.IdBodega, nc.IdNota, s.Su_Descripcion, nc.no_fecha,
nc.IdCliente,t.IdTipoNota, t.No_Descripcion,
CASE WHEN nc.NaturalezaNota = 'SRI' THEN
nc.Serie1+'-'+nc.Serie2+'-'+nc.NumNota_Impresa
ELSE CAST(NC.IdNota AS VARCHAR(20)) END
as NumNota, p.pe_nombreCompleto, p.pe_cedulaRuc, 
isnull(ncx.Valor_Aplicado,0) ValorAplicado, 
f.vt_tipoDoc+'-' + (case when f.vt_tipoDoc = 'FACT' THEN f.vt_serie1+'-'+f.vt_serie2+'-'+f.vt_NumFactura 
ELSE 
case when nd.NaturalezaNota = 'SRI' THEN nd.Serie1+'-'+nd.Serie2+'-'+nd.NumNota_Impresa ELSE cast(nd.IdNota as varchar(20)) END
END )
AS NumDocumentoAplica,ncx.NumDocumento NumDocumentoReemplazo, ncd.Subtotal0, ncd.SubtotalIVA, ncd.ValorIva, ncd.Total, nc.Estado, case when nc.Estado = 'I' THEN 'ANULADAS' ELSE 'ACTIVAS' END AS NomEstado, case when nc.Estado = 'I' THEN 2 ELSE 1 END AS Orden
from fa_notaCreDeb as nc left join 
fa_notaCreDeb_x_fa_factura_NotaDeb ncx on nc.IdEmpresa = ncx.IdEmpresa_nt and nc.IdSucursal = ncx.IdSucursal_nt and nc.IdBodega = ncx.IdBodega_nt and nc.IdNota = ncx.IdNota_nt inner join 
fa_cliente as c on nc.IdEmpresa = c.IdEmpresa and nc.IdCliente = c.IdCliente inner join 
tb_persona as p on c.IdPersona = p.IdPersona LEFT JOIN 
fa_TipoNota AS t on t.IdEmpresa = nc.IdEmpresa and t.IdTipoNota = nc.IdTipoNota inner join
tb_sucursal as s on s.IdEmpresa = nc.IdEmpresa and s.IdSucursal = nc.IdSucursal left join
fa_factura as f on ncx.IdEmpresa_fac_nd_doc_mod = f.IdEmpresa and f.IdSucursal = ncx.IdSucursal_fac_nd_doc_mod and f.IdBodega = ncx.IdBodega_fac_nd_doc_mod and f.IdCbteVta = ncx.IdCbteVta_fac_nd_doc_mod and f.vt_tipoDoc = ncx.vt_tipoDoc LEFT JOIN
fa_notaCreDeb as nd on ncx.IdEmpresa_fac_nd_doc_mod = nd.IdEmpresa and ncx.IdSucursal_fac_nd_doc_mod = nd.IdSucursal and ncx.IdBodega_fac_nd_doc_mod = nd.IdBodega and ncx.IdNota_nt = nd.IdNota and ncx.vt_tipoDoc = nd.CodDocumentoTipo left join
(
select x1.IdEmpresa, x1.IdSucursal, x1.IdBodega, x1.IdNota, sum(x1.Subtotal0)Subtotal0, sum(x1.SubtotalIVA)SubtotalIVA, sum(x1.sc_iva) ValorIva, sum(x1.sc_total) Total 
from (
select x.IdEmpresa, x.IdSucursal, x.IdBodega, x.IdNota, 
case when x.vt_por_iva = 0 then x.sc_subtotal else 0 end as Subtotal0,
case when x.vt_por_iva != 0 then x.sc_subtotal else 0 end as SubtotalIVA,
x.sc_iva, x.sc_total 
from fa_notaCreDeb_det as x) x1
group by x1.IdEmpresa, x1.IdSucursal, x1.IdBodega, x1.IdNota
)
as ncd on nc.IdEmpresa = ncd.IdEmpresa and nc.IdSucursal = ncd.IdSucursal and nc.IdBodega = ncd.IdBodega and nc.IdNota = ncd.IdNota
where nc.CreDeb = 'C'