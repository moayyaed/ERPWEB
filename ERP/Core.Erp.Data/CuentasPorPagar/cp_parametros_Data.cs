using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Info.CuentasPorPagar;
namespace Core.Erp.Data.CuentasPorPagar
{
   public class cp_parametros_Data
    {
        public Boolean modificarDB(cp_parametros_Info info)
        {
            try
            {
                using (Entities_cuentas_por_pagar context = new Entities_cuentas_por_pagar())
                {
                    var selectBaParam = (from C in context.cp_parametros
                                         where C.IdEmpresa == info.IdEmpresa
                                         select C).Count();

                    if (selectBaParam == 0)
                    {
                        cp_parametros addressG = new cp_parametros();
                        addressG.IdEmpresa = info.IdEmpresa;
                        addressG.pa_TipoCbte_OG = info.pa_TipoCbte_OG;
                        addressG.pa_ctacble_iva = (info.pa_ctacble_iva == "") ? null : info.pa_ctacble_iva;
                        addressG.IdUsuario = info.IdUsuario;
                        addressG.pa_TipoCbte_NC = info.pa_TipoCbte_NC;
                        addressG.pa_TipoCbte_ND = info.pa_TipoCbte_ND;
                        addressG.pa_IdTipoCbte_x_Retencion = info.pa_IdTipoCbte_x_Retencion;
                        addressG.pa_TipoCbte_para_conci_x_antcipo = info.pa_TipoCbte_para_conci_x_antcipo;
                        addressG.DiasTransaccionesAFuturo = info.DiasTransaccionesAFuturo;
                        addressG.SeValidaCtaGasto = info.SeValidaCtaGasto;
                        context.cp_parametros.Add(addressG);
                        context.SaveChanges();
                    }
                    else
                    {
                        var contact = context.cp_parametros.FirstOrDefault(para => para.IdEmpresa == info.IdEmpresa);
                        if (contact != null)
                        {
                            contact.IdEmpresa = info.IdEmpresa;
                            contact.pa_TipoCbte_OG = info.pa_TipoCbte_OG;
                            contact.pa_ctacble_iva = (info.pa_ctacble_iva == "") ? null : info.pa_ctacble_iva;
                            contact.pa_TipoCbte_NC = info.pa_TipoCbte_NC;
                            contact.pa_TipoCbte_ND = info.pa_TipoCbte_ND;
                            contact.FechaUltMod = DateTime.Now;
                            contact.IdUsuarioUltMod = info.IdUsuario;
                            contact.pa_IdTipoCbte_x_Retencion = info.pa_IdTipoCbte_x_Retencion;
                            contact.pa_TipoCbte_para_conci_x_antcipo = info.pa_TipoCbte_para_conci_x_antcipo;
                            contact.DiasTransaccionesAFuturo = info.DiasTransaccionesAFuturo;
                            contact.SeValidaCtaGasto = info.SeValidaCtaGasto;
                            context.SaveChanges();
                        }
                    }
                }
                return true;
            }
            catch (Exception )
            {
                
                throw ;
            }
        }
        public cp_parametros_Info get_info(int IdEmpresa)
        {
            try
            {
                cp_parametros_Info Cbt = new cp_parametros_Info();
                using (Entities_cuentas_por_pagar Contex = new Entities_cuentas_por_pagar())
                {
                    var selectBaParam = from C in Contex.cp_parametros
                                        where C.IdEmpresa == IdEmpresa
                                        select C;

                    foreach (var item in selectBaParam)
                    {

                        Cbt.pa_TipoCbte_OG = Convert.ToInt32(item.pa_TipoCbte_OG);
                        Cbt.pa_ctacble_iva = item.pa_ctacble_iva;
                        Cbt.pa_TipoCbte_NC = item.pa_TipoCbte_NC;
                        Cbt.pa_TipoCbte_ND = item.pa_TipoCbte_ND;
                        Cbt.pa_IdTipoCbte_x_Retencion = item.pa_IdTipoCbte_x_Retencion;
                        Cbt.pa_TipoCbte_para_conci_x_antcipo = Convert.ToInt32(item.pa_TipoCbte_para_conci_x_antcipo);
                        Cbt.DiasTransaccionesAFuturo = item.DiasTransaccionesAFuturo;
                        Cbt.IdEmpresa = IdEmpresa;
                        Cbt.SeValidaCtaGasto = item.SeValidaCtaGasto ?? false;
                    }

                    if (selectBaParam.Count() == 0)
                        Cbt = null;
                }
                return (Cbt);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<cp_parametros_Info> get_list(int IdEmpresa)
        {
            try
            {
                List<cp_parametros_Info> lista = new List<cp_parametros_Info>();
                using (Entities_cuentas_por_pagar Contex = new Entities_cuentas_por_pagar())
                {
                    lista = (from q in Contex.cp_parametros
                             where q.IdEmpresa == IdEmpresa
                             select new cp_parametros_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 pa_TipoCbte_OG = q.pa_TipoCbte_OG,
                                 pa_ctacble_iva = q.pa_ctacble_iva,
                                 pa_TipoCbte_NC = q.pa_TipoCbte_NC,
                                 pa_TipoCbte_ND = q.pa_TipoCbte_ND,
                                 pa_IdTipoCbte_x_Retencion = q.pa_IdTipoCbte_x_Retencion,
                                 pa_TipoCbte_para_conci_x_antcipo = q.pa_TipoCbte_para_conci_x_antcipo,
                                 DiasTransaccionesAFuturo = q.DiasTransaccionesAFuturo,
                                 SeValidaCtaGasto = q.SeValidaCtaGasto ?? false
                                 
                             }).ToList();
                         }
                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
