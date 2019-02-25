using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.CuentasPorCobrar
{
   public class CXC_008_Data
    {
        public List<CXC_008_Info> GetList(int IdEmpresa, int IdSucursal, decimal IdCliente, DateTime fecha_ini, DateTime fecha_fin, bool mostrar_anulados)
        {
            try
            {
                List<CXC_008_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    if(mostrar_anulados==true)
                    {
                        Lista = Context.VWCXC_008.Where(q => q.IdEmpresa == IdEmpresa
                    && q.IdSucursal == IdSucursal
                    && q.IdCliente == IdCliente
                    && q.vt_fecha == fecha_fin
                    && q.cr_estado == "I").Select(q => new CXC_008_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdBodega = q.IdBodega,
                        cr_estado = q.cr_estado,
                        cr_fecha = q.cr_fecha,
                        cr_observacion = q.cr_observacion,
                        dc_ValorPago = q.dc_ValorPago,
                        IdCbteVta = q.IdCbteVta,
                        IdCliente = q.IdCliente,
                        IdCobro = q.IdCobro,
                        IdCobro_tipo = q.IdCobro_tipo,
                        IdSucursal = q.IdSucursal,
                        pe_nombreCompleto = q.pe_nombreCompleto,
                        Su_Descripcion = q.Su_Descripcion,
                        tc_descripcion = q.tc_descripcion,
                        vt_fecha = q.vt_fecha,
                        vt_NumFactura = q.vt_NumFactura,
                        vt_tipoDoc = q.vt_tipoDoc
                    }).ToList();
                    }
                    else
                    {
                        Lista = Context.VWCXC_008.Where(q => q.IdEmpresa == IdEmpresa
                    && q.IdSucursal == IdSucursal
                    && q.IdCliente == IdCliente
                    && q.vt_fecha == fecha_fin
                    && q.cr_estado == "A").Select(q => new CXC_008_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdBodega = q.IdBodega,
                        cr_estado = q.cr_estado,
                        cr_fecha = q.cr_fecha,
                        cr_observacion = q.cr_observacion,
                        dc_ValorPago = q.dc_ValorPago,
                        IdCbteVta = q.IdCbteVta,
                        IdCliente = q.IdCliente,
                        IdCobro = q.IdCobro,
                        IdCobro_tipo = q.IdCobro_tipo,
                        IdSucursal = q.IdSucursal,
                        pe_nombreCompleto = q.pe_nombreCompleto,
                        Su_Descripcion = q.Su_Descripcion,
                        tc_descripcion = q.tc_descripcion,
                        vt_fecha = q.vt_fecha,
                        vt_NumFactura = q.vt_NumFactura,
                        vt_tipoDoc = q.vt_tipoDoc

                    }).ToList();
                    }
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
