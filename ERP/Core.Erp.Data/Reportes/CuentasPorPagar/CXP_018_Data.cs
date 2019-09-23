using Core.Erp.Info.Reportes.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.CuentasPorPagar
{
  public class CXP_018_Data
    {
        public List<CXP_018_Info> GetList(int IdEmpresa, int IdSucursal, decimal IdProveedor, DateTime fecha_corte, bool mostrarSaldo0, int IdClaseProveedor)
        {
            try
            {
                int IdSucursalIni = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 99999 : IdSucursal;

                decimal IdProveedorIni = IdProveedor;
                decimal IdProveedorFin = IdProveedor == 0 ? 99999 : IdProveedor;


                int IdClaseProveedorIni = IdClaseProveedor;
                int IdClaseProveedorFin = IdClaseProveedor == 0 ? 99999999 : IdClaseProveedor;
                List<CXP_018_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    var lst = Context.SPCXP_018(IdEmpresa, IdSucursalIni, IdSucursalFin, IdProveedorIni, IdProveedorFin, fecha_corte, mostrarSaldo0, IdClaseProveedorIni, IdClaseProveedorFin).Select(q => new CXP_018_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdSucursal = q.IdSucursal,
                        Codigo = q.Codigo,
                        co_factura = q.co_factura,
                        co_FechaFactura = q.co_FechaFactura,
                        co_FechaFactura_vct = q.co_FechaFactura_vct,
                        co_observacion = q.co_observacion,
                        co_subtotal_iva = q.co_subtotal_iva,
                        co_subtotal_siniva = q.co_subtotal_siniva,
                        co_total = q.co_total,
                        co_valoriva = q.co_valoriva,
                        IdCbteCble_Ogiro = q.IdCbteCble_Ogiro,
                        IdProveedor = q.IdProveedor,
                        IdTipoCbte_Ogiro = q.IdTipoCbte_Ogiro,
                        pe_nombreCompleto = q.pe_nombreCompleto,
                        Saldo = q.Saldo,
                        Su_Descripcion = q.Su_Descripcion,
                        ValorAbono = q.ValorAbono,
                        ValorNC = q.ValorNC,
                        ValorRetencion = q.ValorRetencion,
                        descripcion_clas_prove = q.descripcion_clas_prove,
                        IdClaseProveedor = q.IdClaseProveedor,
                        DiasVcto = q.DiasVcto,
                        Orden = (q.DiasVcto < 0 ? 1 : 2),
                        Grupo = (q.DiasVcto < 0 ? "VENCIDO" : "POR VENCER")
                    }).ToList();

                    var lstA = lst.GroupBy(q => new { q.Orden, q.IdProveedor }).Select(q => new
                    {
                        Orden = q.Key.Orden,
                        IdProveedor = q.Key.IdProveedor,
                        TotalDias = q.Min(g => g.DiasVcto)
                    }).ToList();

                    var Lista_Agrupada = lstA.Where(q=> q.IdProveedor==786).ToList();
                    var Lista_Rpte = lst.Where(q => q.IdProveedor == 786).ToList();

                    Lista = (from g in lstA
                             join q in lst
                             on new { g.Orden, g.IdProveedor } equals new { q.Orden, q.IdProveedor }
                             select new CXP_018_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 Codigo = q.Codigo,
                                 co_factura = q.co_factura,
                                 co_FechaFactura = q.co_FechaFactura,
                                 co_FechaFactura_vct = q.co_FechaFactura_vct,
                                 co_observacion = q.co_observacion,
                                 co_subtotal_iva = q.co_subtotal_iva,
                                 co_subtotal_siniva = q.co_subtotal_siniva,
                                 co_total = q.co_total,
                                 co_valoriva = q.co_valoriva,
                                 IdCbteCble_Ogiro = q.IdCbteCble_Ogiro,
                                 IdProveedor = q.IdProveedor,
                                 IdTipoCbte_Ogiro = q.IdTipoCbte_Ogiro,
                                 pe_nombreCompleto = q.pe_nombreCompleto,
                                 Saldo = q.Saldo,
                                 Su_Descripcion = q.Su_Descripcion,
                                 ValorAbono = q.ValorAbono,
                                 ValorNC = q.ValorNC,
                                 ValorRetencion = q.ValorRetencion,
                                 descripcion_clas_prove = q.descripcion_clas_prove,
                                 IdClaseProveedor = q.IdClaseProveedor,
                                 DiasVcto = q.DiasVcto,
                                 Orden = q.Orden,
                                 Grupo = q.Grupo,
                                 TotalDias = g.TotalDias
                             }).ToList();
                }
                
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
