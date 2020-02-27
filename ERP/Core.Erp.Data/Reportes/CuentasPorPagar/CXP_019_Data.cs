using Core.Erp.Info.Reportes.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.CuentasPorPagar
{
    public class CXP_019_Data
    {
        public List<CXP_019_Info> get_list(int IdEmpresa, DateTime fecha, int IdSucursal,int IdClaseProveedor, decimal IdProveedor, bool no_mostrar_en_conciliacion, bool no_mostrar_saldo_0)
        {
            try
            {
               decimal IdProveedor_ini = IdProveedor;
                decimal IdProveedor_fin = IdProveedor == 0 ? 99999999 : IdProveedor;

                int IdSucursalIni = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 9999 : IdSucursal;


                int IdClaseProveedorIni = IdClaseProveedor;
                int IdClaseProveedorFin = IdClaseProveedor == 0 ? 99999999 : IdClaseProveedor;
                fecha = fecha.Date;
                List<CXP_019_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    if (no_mostrar_en_conciliacion && no_mostrar_saldo_0)
                    {
                        Lista = (from q in Context.SPCXP_019(IdEmpresa, fecha, IdSucursalIni, IdSucursalFin, IdClaseProveedorIni, IdClaseProveedorFin, IdProveedor_ini, IdProveedor_fin)
                                 where q.Saldo > 0
                                 select new CXP_019_Info
                                 {
                                     IdRow = q.IdRow,
                                     IdEmpresa = q.IdEmpresa,
                                     IdProveedor = q.IdProveedor,
                                     nom_proveedor = q.nom_proveedor,
                                     Valor_a_pagar = q.Valor_a_pagar,
                                     MontoAplicado = q.MontoAplicado,
                                     Saldo = q.Saldo,
                                     Ruc_Proveedor = q.Ruc_Proveedor,
                                     representante_legal = q.representante_legal,
                                     x_Vencer = q.x_Vencer,
                                     Vencido = q.Vencido,
                                     Vencido_1_30 = q.Vencido_1_30,
                                     Vencido_31_60 = q.Vencido_31_60,
                                     Vencido_60_90 = q.Vencido_60_90,
                                     Vencido_mayor_90 = q.Vencido_mayor_90,
                                     Su_Descripcion = q.Su_Descripcion,
                                     descripcion_clas_prove = q.descripcion_clas_prove,
                                     IdClaseProveedor = q.IdClaseProveedor,
                                     EsRelacionado = q.EsRelacionado
                                 }).ToList();
                    }
                    else
                        if (!no_mostrar_en_conciliacion && no_mostrar_saldo_0)
                    {
                        Lista = (from q in Context.SPCXP_019(IdEmpresa, fecha, IdSucursalIni, IdSucursalFin, IdClaseProveedorIni, IdClaseProveedorFin, IdProveedor_ini, IdProveedor_fin)
                                 where q.Saldo > 0
                                 select new CXP_019_Info
                                 {
                                     IdRow = q.IdRow,
                                     IdEmpresa = q.IdEmpresa,
                                     IdProveedor = q.IdProveedor,
                                     nom_proveedor = q.nom_proveedor,
                                     Valor_a_pagar = q.Valor_a_pagar,
                                     MontoAplicado = q.MontoAplicado,
                                     Saldo = q.Saldo,
                                     Ruc_Proveedor = q.Ruc_Proveedor,
                                     representante_legal = q.representante_legal,
                                     x_Vencer = q.x_Vencer,
                                     Vencido = q.Vencido,
                                     Vencido_1_30 = q.Vencido_1_30,
                                     Vencido_31_60 = q.Vencido_31_60,
                                     Vencido_60_90 = q.Vencido_60_90,
                                     Vencido_mayor_90 = q.Vencido_mayor_90,
                                     Su_Descripcion = q.Su_Descripcion,
                                     descripcion_clas_prove = q.descripcion_clas_prove,
                                     IdClaseProveedor = q.IdClaseProveedor,
                                     EsRelacionado = q.EsRelacionado
                                 }).ToList();
                    }
                    else
                        if (no_mostrar_en_conciliacion && !no_mostrar_saldo_0)
                    {
                        Lista = (from q in Context.SPCXP_019(IdEmpresa, fecha, IdSucursalIni, IdSucursalFin, IdClaseProveedorIni, IdClaseProveedorFin, IdProveedor_ini, IdProveedor_fin)
                                 select new CXP_019_Info
                                 {
                                     IdRow = q.IdRow,
                                     IdEmpresa = q.IdEmpresa,
                                     IdProveedor = q.IdProveedor,
                                     nom_proveedor = q.nom_proveedor,
                                     Valor_a_pagar = q.Valor_a_pagar,
                                     MontoAplicado = q.MontoAplicado,
                                     Saldo = q.Saldo,
                                     Ruc_Proveedor = q.Ruc_Proveedor,
                                     representante_legal = q.representante_legal,
                                     x_Vencer = q.x_Vencer,
                                     Vencido = q.Vencido,
                                     Vencido_1_30 = q.Vencido_1_30,
                                     Vencido_31_60 = q.Vencido_31_60,
                                     Vencido_60_90 = q.Vencido_60_90,
                                     Vencido_mayor_90 = q.Vencido_mayor_90,
                                     Su_Descripcion = q.Su_Descripcion,
                                     descripcion_clas_prove = q.descripcion_clas_prove,
                                     IdClaseProveedor = q.IdClaseProveedor,
                                     EsRelacionado = q.EsRelacionado
                                 }).ToList();
                    }
                    else
                    Lista = (from q in Context.SPCXP_019(IdEmpresa, fecha, IdSucursalIni, IdSucursalFin, IdClaseProveedorIni, IdClaseProveedorFin ,IdProveedor_ini, IdProveedor_fin)
                             select new CXP_019_Info
                             {
                                 IdRow = q.IdRow,
                                 IdEmpresa = q.IdEmpresa,
                                 IdProveedor = q.IdProveedor,
                                 nom_proveedor = q.nom_proveedor,
                                 Valor_a_pagar = q.Valor_a_pagar,
                                 MontoAplicado = q.MontoAplicado,
                                 Saldo = q.Saldo,
                                 Ruc_Proveedor = q.Ruc_Proveedor,
                                 representante_legal = q.representante_legal,
                                 x_Vencer = q.x_Vencer,
                                 Vencido = q.Vencido,
                                 Vencido_1_30 = q.Vencido_1_30,
                                 Vencido_31_60 = q.Vencido_31_60,
                                 Vencido_60_90 = q.Vencido_60_90,
                                 Vencido_mayor_90 = q.Vencido_mayor_90,
                                 Su_Descripcion = q.Su_Descripcion,
                                 descripcion_clas_prove = q.descripcion_clas_prove,
                                 IdClaseProveedor = q.IdClaseProveedor,
                                 EsRelacionado = q.EsRelacionado
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
