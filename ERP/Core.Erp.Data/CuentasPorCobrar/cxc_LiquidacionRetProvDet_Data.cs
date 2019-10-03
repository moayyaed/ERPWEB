using Core.Erp.Info.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.CuentasPorCobrar
{
    public class cxc_LiquidacionRetProvDet_Data
    {
        public List<cxc_LiquidacionRetProvDet_Info> get_list(int IdEmpresa, decimal IdLiquidacion)
        {
            try
            {
                List<cxc_LiquidacionRetProvDet_Info> Lista;
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    Lista = (from q in Context.vwcxc_LiquidacionRetProvDet
                             where q.IdEmpresa == IdEmpresa
                             && q.IdLiquidacion == IdLiquidacion
                             select new cxc_LiquidacionRetProvDet_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdLiquidacion = q.IdLiquidacion,
                                 IdCobro = q.IdCobro,
                                 secuencial = q.secuencial,
                                 IdCobro_tipo = q.IdCobro_tipo,
                                 dc_ValorPago = q.dc_ValorPago,
                                 tc_descripcion = q.tc_descripcion,
                                 cr_fecha = q.cr_fecha,
                                 cr_observacion = q.cr_observacion,
                                 cr_EsProvision = q.cr_EsProvision,
                                 cr_estado = q.cr_estado,
                                 IdCtaCble = q.IdCtaCble,
                                 pc_Cuenta = q.pc_Cuenta,
                                 ESRetenIVA = q.ESRetenIVA,
                                 ESRetenFTE = q.ESRetenFTE,
                                 cr_NumDocumento = q.cr_NumDocumento
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
