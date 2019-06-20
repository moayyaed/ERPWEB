using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.CuentasPorPagar;
namespace Core.Erp.Data.CuentasPorPagar
{
   public class cp_nota_DebCre_Data
    {
        public List<cp_nota_DebCre_Info> get_lst(int IdEmpresa, int IdSucursal, string DebCre,  DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                List<cp_nota_DebCre_Info> Lista;
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    Lista = (from q in Context.vwcp_nota_DebCre
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.DebCre == DebCre
                             && q.cn_fecha >= fecha_ini && q.cn_fecha <= fecha_fin
                             orderby q.IdCbteCble_Nota descending
                             select new cp_nota_DebCre_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdCbteCble_Nota = q.IdCbteCble_Nota,
                                 IdTipoCbte_Nota = q.IdTipoCbte_Nota,
                                 DebCre = q.DebCre,
                                 IdProveedor = q.IdProveedor,
                                 IdTipoNota = q.IdTipoNota,
                                 IdSucursal = q.IdSucursal,
                                 cn_fecha = q.cn_fecha,
                                 Fecha_contable = q.Fecha_contable,
                                 cn_Fecha_vcto = q.cn_Fecha_vcto,
                                 cn_serie1 = q.cn_serie1,
                                 cn_serie2 = q.cn_serie2,
                                 cn_Nota = q.cn_Nota,
                                 cn_observacion = q.cn_observacion,
                                 cn_subtotal_iva = q.cn_subtotal_iva,
                                 cn_subtotal_siniva = q.cn_subtotal_siniva,
                                 cn_baseImponible = q.cn_baseImponible,
                                 cn_Por_iva = q.cn_Por_iva,
                                 cn_Ice_base = q.cn_Ice_base,
                                 cn_Ice_por = q.cn_Ice_por,
                                 cn_Ice_valor = q.cn_Ice_valor,
                                 cn_Serv_por = q.cn_Serv_por,
                                 cn_Serv_valor = q.cn_Serv_valor,
                                 cn_BaseSeguro = q.cn_BaseSeguro,
                                 cn_total = q.cn_total,
                                 cn_vaCoa = q.cn_vaCoa,
                                 cn_Autorizacion = q.cn_Autorizacion,
                                 cn_Autorizacion_Imprenta = q.cn_Autorizacion_Imprenta,
                                 cn_num_doc_modificado = q.cn_num_doc_modificado,
                                 IdCod_ICE = q.IdCod_ICE,
                                 IdIden_credito = q.IdIden_credito,
                                 IdCtaCble_Acre = q.IdCtaCble_Acre,
                                 IdTipoServicio = q.IdTipoServicio,
                                 IdCtaCble_IVA = q.IdCtaCble_IVA,
                                 Estado = q.Estado,
                                 info_proveedor = new cp_proveedor_Info
                                 {
                                     info_persona = new Info.General.tb_persona_Info
                                     {
                                         pe_apellido = q.pe_apellido,
                                         pe_nombre = q.pe_nombre,
                                         pe_nombreCompleto = q.pe_nombreCompleto,
                                         pe_cedulaRuc = q.pe_cedulaRuc
                                     }
                                 },

                                 EstadoBool = q.Estado == "A" ? true : false
                                 
                             }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }
     
        public cp_nota_DebCre_Info get_info(int IdEmpresa, int IdTipoCbte_Nota, decimal IdCbteCble_Nota)
        {
            try
            {
                cp_nota_DebCre_Info info = new cp_nota_DebCre_Info();
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_nota_DebCre Entity = Context.cp_nota_DebCre.Where(q => q.IdEmpresa == IdEmpresa
                    && q.IdTipoCbte_Nota == IdTipoCbte_Nota
                    && q.IdCbteCble_Nota == IdCbteCble_Nota).FirstOrDefault();
                    if (Entity == null) return null;
                    info = new cp_nota_DebCre_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdCbteCble_Nota = Entity.IdCbteCble_Nota,
                        IdTipoCbte_Nota = Entity.IdTipoCbte_Nota,
                        DebCre = Entity.DebCre,
                        IdProveedor = Entity.IdProveedor,
                        IdTipoNota = Entity.IdTipoNota,
                        IdSucursal = Entity.IdSucursal,
                        cn_fecha = Entity.cn_fecha,
                        Fecha_contable = Entity.Fecha_contable,
                        cn_Fecha_vcto = Entity.cn_Fecha_vcto,
                        cn_serie1 = Entity.cn_serie1,
                        cn_serie2 = Entity.cn_serie2,
                        cn_Nota = Entity.cn_Nota,
                        cn_observacion = Entity.cn_observacion,
                        cn_subtotal_iva = Entity.cn_subtotal_iva,
                        cn_subtotal_siniva = Entity.cn_subtotal_siniva,
                        cn_baseImponible = Entity.cn_baseImponible,
                        cn_Por_iva = Entity.cn_Por_iva,
                        cn_Ice_base = Entity.cn_Ice_base,
                        cn_Ice_por = Entity.cn_Ice_por,
                        cn_Ice_valor = Entity.cn_Ice_valor,
                        cn_Serv_por = Entity.cn_Serv_por,
                        cn_Serv_valor = Entity.cn_Serv_valor,
                        cn_BaseSeguro = Entity.cn_BaseSeguro,
                        cn_total = Entity.cn_total,
                        cn_vaCoa = Entity.cn_vaCoa,
                        cn_Autorizacion = Entity.cn_Autorizacion,
                        cn_Autorizacion_Imprenta = Entity.cn_Autorizacion_Imprenta,
                        cn_num_doc_modificado = Entity.cn_num_doc_modificado,
                        IdCod_ICE = Entity.IdCod_ICE,
                        IdIden_credito = Entity.IdIden_credito,
                        IdCtaCble_Acre = Entity.IdCtaCble_Acre,
                        IdTipoServicio = Entity.IdTipoServicio,
                        IdCtaCble_IVA = Entity.IdCtaCble_IVA,
                        ConvenioTributacion_bool = Entity.ConvenioTributacion == "SI" ? true : false,
                        PagoSujetoRetencion_bool = Entity.PagoSujetoRetencion == "SI" ? true : false
                    };
                }
                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool guardarDB(cp_nota_DebCre_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_nota_DebCre Entity = new cp_nota_DebCre
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdCbteCble_Nota = info.IdCbteCble_Nota,
                        IdTipoCbte_Nota = info.IdTipoCbte_Nota,
                        cod_nota=info.cod_nota,
                        DebCre = info.DebCre,
                        IdProveedor = info.IdProveedor,
                        IdTipoNota = info.IdTipoNota,
                        IdSucursal = info.IdSucursal,
                        cn_fecha = info.cn_fecha,
                        Fecha_contable = info.Fecha_contable,
                        cn_Fecha_vcto = info.cn_Fecha_vcto.Date,
                        cn_serie1 = info.cn_serie1,
                        cn_serie2 = info.cn_serie2,
                        cn_Nota = info.cn_Nota,
                        cn_observacion = info.cn_observacion,
                        cn_subtotal_iva = info.cn_subtotal_iva,
                        cn_subtotal_siniva = info.cn_subtotal_siniva,
                        cn_baseImponible = info.cn_baseImponible,
                        cn_Por_iva = info.cn_Por_iva,
                        cn_Ice_base = info.cn_Ice_base,
                        cn_Ice_por = info.cn_Ice_por,
                        cn_Ice_valor = info.cn_Ice_valor,
                        cn_Serv_por = info.cn_Serv_por,
                        cn_Serv_valor = info.cn_Serv_valor,
                        cn_BaseSeguro = info.cn_BaseSeguro,
                        cn_total = info.cn_total,
                        cn_vaCoa = info.cn_vaCoa,
                        cn_Autorizacion = info.cn_Autorizacion,
                        cn_Autorizacion_Imprenta = info.cn_Autorizacion_Imprenta,
                        cn_num_doc_modificado = info.cn_num_doc_modificado,
                        IdIden_credito = info.IdIden_credito,
                        IdCtaCble_Acre = info.IdCtaCble_Acre,
                        IdTipoServicio = info.IdTipoServicio,
                        IdCtaCble_IVA = info.IdCtaCble_IVA,
                        IdUsuario = info.IdUsuario,
                        Fecha_Transac = info.Fecha_Transac = DateTime.Now,
                        Estado = info.Estado,
                        ConvenioTributacion = info.ConvenioTributacion_bool == true ? "SI" : "NO",
                        PagoSujetoRetencion = info.PagoSujetoRetencion_bool == true ? "SI" : "NO",
                    };
                    Context.cp_nota_DebCre.Add(Entity);
                    

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception )
            {

                throw;
            }
        }
        public bool modificarDB(cp_nota_DebCre_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_nota_DebCre Entity = Context.cp_nota_DebCre.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa
                    && q.IdTipoCbte_Nota == info.IdTipoCbte_Nota
                    && q.IdCbteCble_Nota == info.IdCbteCble_Nota);
                    if (Entity == null) return false;
                    {

                        Entity.IdEmpresa = info.IdEmpresa;
                        Entity.IdProveedor = info.IdProveedor;
                        Entity.IdTipoNota = info.IdTipoNota;
                        Entity.IdSucursal = info.IdSucursal;
                        Entity.cn_fecha = info.cn_fecha;
                        Entity.cod_nota = info.cod_nota;
                        Entity.Fecha_contable = info.Fecha_contable;
                        Entity.cn_Fecha_vcto = info.cn_Fecha_vcto.Date;
                        Entity.cn_Nota = info.cn_Nota;
                        Entity.cn_observacion = info.cn_observacion;
                        Entity.cn_subtotal_iva = info.cn_subtotal_iva;
                        Entity.cn_subtotal_siniva = info.cn_subtotal_siniva;
                        Entity.cn_baseImponible = info.cn_baseImponible;
                        Entity.cn_Por_iva = info.cn_Por_iva;
                        Entity.cn_Ice_base = info.cn_Ice_base;
                        Entity.cn_Ice_por = info.cn_Ice_por;
                        Entity.cn_Ice_valor = info.cn_Ice_valor;
                        Entity.cn_Serv_por = info.cn_Serv_por;
                        Entity.cn_Serv_valor = info.cn_Serv_valor;
                        Entity.cn_BaseSeguro = info.cn_BaseSeguro;
                        Entity.cn_total = info.cn_total;
                        Entity.cn_Autorizacion = info.cn_Autorizacion;
                        Entity.cn_Autorizacion_Imprenta = info.cn_Autorizacion_Imprenta;
                        Entity.cn_num_doc_modificado = info.cn_num_doc_modificado;
                        Entity.IdCod_ICE = info.IdCod_ICE;
                        Entity.IdIden_credito = info.IdIden_credito;
                        Entity.IdCtaCble_Acre = info.IdCtaCble_Acre;
                        Entity.IdTipoServicio = info.IdTipoServicio;
                        Entity.IdCtaCble_IVA = info.IdCtaCble_IVA;
                        Entity.ConvenioTributacion = info.ConvenioTributacion_bool == true ? "SI" : "NO";
                        Entity.PagoSujetoRetencion = info.PagoSujetoRetencion_bool == true ? "SI" : "NO";


                    };
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool anularDB(cp_nota_DebCre_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar Context = new Entities_cuentas_por_pagar())
                {
                    cp_nota_DebCre Entity = Context.cp_nota_DebCre.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa
                    && q.IdTipoCbte_Nota == info.IdTipoCbte_Nota
                    && q.IdCbteCble_Nota == info.IdCbteCble_Nota);
                    if (Entity == null) return false;
                    {
                        Entity.IdTipoCbte_Anulacion = info.IdTipoCbte_Anulacion;
                        Entity.IdCbteCble_Anulacion = info.IdCbteCble_Anulacion;
                        Entity.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                        Entity.Fecha_UltAnu = info.Fecha_UltAnu;
                        Entity.Estado = "I";
                    };
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

    
    }
}
