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
        public List<cxc_LiquidacionRetProvDet_Info> get_list(int IdEmpresa, int IdSucursal, decimal IdLiquidacion)
        {
            try
            {
                List<cxc_LiquidacionRetProvDet_Info> Lista;
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    Lista = (from q in Context.vwcxc_LiquidacionRetProvDet
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.IdLiquidacion == IdLiquidacion
                             select new cxc_LiquidacionRetProvDet_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdLiquidacion = q.IdLiquidacion,
                                 Secuencia = q.Secuencia,
                                 IdCobro_tipo = q.IdCobro_tipo,
                                 IdCobro = q.IdCobro,
                                 secuencial = q.secuencial,
                                 Valor = q.dc_ValorPago,
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
                                 cr_NumDocumento = q.cr_NumDocumento,
                                 vt_NumFactura = q.vt_NumFactura,
                                 pe_nombreCompleto = q.pe_nombreCompleto
                             }).ToList();
                }
                
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<cxc_LiquidacionRetProvDet_Info> GetList_X_Cruzar(int IdEmpresa, int IdSucursal)
        {
            try
            {
                List<cxc_LiquidacionRetProvDet_Info> Lista;
                using (Entities_cuentas_por_cobrar Context = new Entities_cuentas_por_cobrar())
                {
                    Lista = (from q in Context.vwcxc_LiquidacionRetProvDet_PorCruzar
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             select new cxc_LiquidacionRetProvDet_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdCobro = q.IdCobro,
                                 secuencial = q.secuencial,
                                 IdCobro_tipo = q.IdCobro_tipo,
                                 Valor = q.dc_ValorPago,
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
                                 cr_NumDocumento = q.cr_NumDocumento,
                                 vt_NumFactura = q.vt_NumFactura,
                                 pe_nombreCompleto = q.pe_nombreCompleto
                             }).ToList();

                    Lista.ForEach(q=> q.SecuencialCobro = q.IdCobro.ToString() + q.secuencial.ToString());
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
