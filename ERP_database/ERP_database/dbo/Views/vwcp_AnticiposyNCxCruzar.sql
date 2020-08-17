CREATE VIEW vwcp_AnticiposyNCxCruzar
AS
select 'NOTA DE CREDITO' AS Tipo,a.IdEmpresa,a.IdTipoCbte_Nota, a.IdCbteCble_Nota, a.IdSucursal, a.IdProveedor, d.pe_nombreCompleto, a.cn_observacion, a.cn_fecha, a.cn_total, isnull(sum(b.MontoAplicado),0) MontoAplicado, DBO.BankersRounding(a.cn_total - ISNULL(sum(b.MontoAplicado),0),2) Saldo
from cp_nota_DebCre as a left join
cp_orden_pago_cancelaciones as b on a.IdEmpresa = b.IdEmpresa_pago and a.IdTipoCbte_Nota = b.IdTipoCbte_pago and a.IdCbteCble_Nota = b.IdCbteCble_pago inner join
cp_proveedor as c on a.IdEmpresa = c.IdEmpresa and a.IdProveedor = c.IdProveedor inner join
tb_persona as d on d.IdPersona = c.IdPersona 
where a.DebCre = 'C' AND A.Estado = 'A'
group by a.IdEmpresa,a.IdTipoCbte_Nota, a.IdCbteCble_Nota, a.IdSucursal, a.IdProveedor, d.pe_nombreCompleto, a.cn_observacion, a.cn_fecha, a.cn_total
HAVING DBO.BankersRounding(a.cn_total - ISNULL(sum(b.MontoAplicado),0),2) > 0
UNION ALL
SELECT  a.Descripcion as Tipo, dbo.cp_orden_pago_det.IdEmpresa, dbo.cp_orden_pago_det.IdOrdenPago, dbo.cp_orden_pago_det.IdOrdenPago, dbo.cp_orden_pago.IdSucursal, dbo.cp_orden_pago.IdEntidad AS IdProveedor, dbo.tb_persona.pe_nombreCompleto, dbo.cp_orden_pago.Observacion, 
                  dbo.cp_orden_pago.Fecha, dbo.cp_orden_pago_det.Valor_a_pagar AS ValorOP, ISNULL(DET.MontoAplicado, 0) AS MontoAplicado, ROUND(dbo.cp_orden_pago_det.Valor_a_pagar - ISNULL(DET.MontoAplicado, 0), 2) AS Saldo
FROM     dbo.cp_orden_pago INNER JOIN
                  dbo.cp_orden_pago_det ON dbo.cp_orden_pago.IdEmpresa = dbo.cp_orden_pago_det.IdEmpresa AND dbo.cp_orden_pago.IdOrdenPago = dbo.cp_orden_pago_det.IdOrdenPago INNER JOIN
                  dbo.cp_orden_pago_cancelaciones ON dbo.cp_orden_pago_det.IdEmpresa = dbo.cp_orden_pago_cancelaciones.IdEmpresa_op AND dbo.cp_orden_pago_det.IdOrdenPago = dbo.cp_orden_pago_cancelaciones.IdOrdenPago_op AND 
                  dbo.cp_orden_pago_det.Secuencia = dbo.cp_orden_pago_cancelaciones.Secuencia_op INNER JOIN
                  dbo.cp_orden_pago_tipo_x_empresa ON dbo.cp_orden_pago.IdTipo_op = dbo.cp_orden_pago_tipo_x_empresa.IdTipo_op AND dbo.cp_orden_pago.IdEmpresa = dbo.cp_orden_pago_tipo_x_empresa.IdEmpresa INNER JOIN
                  dbo.tb_persona ON dbo.cp_orden_pago.IdPersona = dbo.tb_persona.IdPersona LEFT OUTER JOIN
                      (SELECT IdEmpresa, IdOrdenPago, SUM(MontoAplicado) AS MontoAplicado
                       FROM      dbo.cp_ConciliacionAnticipoDetAnt AS D
                       GROUP BY IdEmpresa, IdOrdenPago) AS DET ON dbo.cp_orden_pago.IdEmpresa = DET.IdEmpresa AND dbo.cp_orden_pago.IdOrdenPago = DET.IdOrdenPago inner join
cp_orden_pago_tipo as a on dbo.cp_orden_pago.IdTipo_op = a.IdTipo_op
WHERE  (dbo.cp_orden_pago_tipo_x_empresa.Dispara_Alerta = 1) AND (dbo.cp_orden_pago.IdTipo_Persona = 'PROVEE') AND (dbo.cp_orden_pago.Estado = 'A') AND (ROUND(dbo.cp_orden_pago_det.Valor_a_pagar - ISNULL(DET.MontoAplicado, 
                  0), 2) > 0)