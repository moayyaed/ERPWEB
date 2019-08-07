using Core.Erp.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.Facturacion
{
  public  class FAC_013_diario_Data
    {
        public List<FAC_013_diario_Info> GetList(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta)
        {
            try
            {
                List<FAC_013_diario_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = Context.VWFAC_013_diario.Where(
                        q => q.vt_IdEmpresa == IdEmpresa
                        && q.vt_IdSucursal == IdSucursal
                        && q.vt_IdBodega == IdBodega
                        && q.vt_IdCbteVta == IdCbteVta
                        ).Select(q => new FAC_013_diario_Info
                        {
                            vt_IdCbteVta = q.vt_IdCbteVta,
                            ct_IdEmpresa = q.ct_IdEmpresa,
                            vt_IdBodega = q.vt_IdBodega,
                            vt_IdSucursal = q.vt_IdSucursal,
                            cc_Descripcion = q.cc_Descripcion,
                            ct_IdCbteCble = q.ct_IdCbteCble,
                            ct_IdTipoCbte = q.ct_IdTipoCbte,
                            dc_Valor = q.dc_Valor,
                            dc_Valor_Debe = q.dc_Valor_Debe,
                            dc_Valor_Haber = q.dc_Valor_Haber,
                            IdCentroCosto = q.IdCentroCosto,
                            IdCtaCble = q.IdCtaCble,
                            pc_Cuenta = q.pc_Cuenta,
                            secuencia = q.secuencia,
                            vt_IdEmpresa = q.vt_IdEmpresa

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
