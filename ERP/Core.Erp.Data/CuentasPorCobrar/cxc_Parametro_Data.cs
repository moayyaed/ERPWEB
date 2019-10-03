using Core.Erp.Info.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.CuentasPorCobrar
{
    public class cxc_Parametro_Data
    {
        public cxc_Parametro_Info get_info(int IdEmpresa)
        {
            try
            {
                cxc_Parametro_Info info = new cxc_Parametro_Info();
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    cxc_Parametro Entity = Context.cxc_Parametro.FirstOrDefault(q => q.IdEmpresa == IdEmpresa);
                    if (Entity == null) return null;
                    info = new cxc_Parametro_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        pa_IdCaja_x_cobros_x_CXC = Entity.pa_IdCaja_x_cobros_x_CXC,
                        pa_IdTipoCbteCble_CxC = Entity.pa_IdTipoCbteCble_CxC,
                        pa_IdTipoMoviCaja_x_Cobros_x_cliente = Entity.pa_IdTipoMoviCaja_x_Cobros_x_cliente,
                        DiasTransaccionesAFuturo = Entity.DiasTransaccionesAFuturo,
                        IdCtaCble_ProvisionFuente = Entity.IdCtaCble_ProvisionFuente,
                        IdCtaCble_ProvisionIva = Entity.IdCtaCble_ProvisionIva,
                        IdPunto_cargo_grupo_Fte = Entity.IdPunto_cargo_grupo_Fte,
                        IdPunto_cargo_Fte = Entity.IdPunto_cargo_Fte,
                        IdPunto_cargo_grupo_Iva = Entity.IdPunto_cargo_grupo_Iva,
                        IdPunto_cargo_Iva = Entity.IdPunto_cargo_Iva,
                        IdTipoCbte_LiquidacionRet = Entity.IdTipoCbte_LiquidacionRet,

                    };
                }
                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(cxc_Parametro_Info info)
        {
            try
            {
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    cxc_Parametro Entity = Context.cxc_Parametro.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa);
                    if (Entity == null)
                    {
                        Entity = new cxc_Parametro
                        {
                            IdEmpresa = info.IdEmpresa,
                            pa_IdCaja_x_cobros_x_CXC = info.pa_IdCaja_x_cobros_x_CXC,
                            pa_IdTipoCbteCble_CxC = info.pa_IdTipoCbteCble_CxC,
                            pa_IdTipoMoviCaja_x_Cobros_x_cliente = info.pa_IdTipoMoviCaja_x_Cobros_x_cliente,
                            DiasTransaccionesAFuturo = info.DiasTransaccionesAFuturo,
                            IdCtaCble_ProvisionFuente = info.IdCtaCble_ProvisionFuente,
                            IdCtaCble_ProvisionIva = info.IdCtaCble_ProvisionIva,
                            IdUsuario = info.IdUsuario,
                            FechaTransac = DateTime.Now,
                            IdPunto_cargo_grupo_Fte = info.IdPunto_cargo_grupo_Fte,
                            IdPunto_cargo_Fte = info.IdPunto_cargo_Fte,
                            IdPunto_cargo_grupo_Iva = info.IdPunto_cargo_grupo_Iva,
                            IdPunto_cargo_Iva = info.IdPunto_cargo_Iva,
                            IdTipoCbte_LiquidacionRet = info.IdTipoCbte_LiquidacionRet
                        };
                        Context.cxc_Parametro.Add(Entity);
                    }
                    else
                        {
                        Entity.pa_IdCaja_x_cobros_x_CXC = info.pa_IdCaja_x_cobros_x_CXC;
                        Entity.pa_IdTipoCbteCble_CxC = info.pa_IdTipoCbteCble_CxC;
                        Entity.pa_IdTipoMoviCaja_x_Cobros_x_cliente = info.pa_IdTipoMoviCaja_x_Cobros_x_cliente;
                        Entity.DiasTransaccionesAFuturo = info.DiasTransaccionesAFuturo;
                        Entity.IdCtaCble_ProvisionFuente = info.IdCtaCble_ProvisionFuente;
                        Entity.IdCtaCble_ProvisionIva = info.IdCtaCble_ProvisionIva;
                        Entity.IdUsuarioUltMod = info.IdUsuarioUltMod;
                        Entity.FechaUltMod = DateTime.Now;
                        Entity.IdPunto_cargo_grupo_Fte = info.IdPunto_cargo_grupo_Fte;
                        Entity.IdPunto_cargo_Fte = info.IdPunto_cargo_Fte;
                        Entity.IdPunto_cargo_grupo_Iva = info.IdPunto_cargo_grupo_Iva;
                        Entity.IdPunto_cargo_Iva = info.IdPunto_cargo_Iva;
                        Entity.IdTipoCbte_LiquidacionRet = info.IdTipoCbte_LiquidacionRet;
                    }
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
