using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.CuentasPorCobrar
{
   public class CXC_003_Data
    {
        public List<CXC_003_Info> get_list (int IdEmpresa, decimal IdCliente, DateTime Fecha_ini, DateTime Fecha_fin, string Tipo, bool MostrarSinRetencion)
        {
            try
            {
                List<CXC_003_Info> Lista;
                Fecha_ini = Fecha_ini.Date;
                Fecha_fin = Fecha_fin.Date;

                decimal IdClienteIni = IdCliente;
                decimal IdClienteFin = IdCliente == 0 ? 999999 : IdCliente;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.VWCXC_003
                             where q.IdEmpresa == IdEmpresa
                             && q.IdCliente >= IdClienteIni
                             && q.IdCliente <= IdClienteFin
                             && q.vt_fecha >= Fecha_ini
                             && q.vt_fecha <= Fecha_fin
                             && q.cr_EsElectronico == (string.IsNullOrEmpty(Tipo) ? q.cr_EsElectronico : (Tipo == "M" ? "NO" : "SI"))
                             && (MostrarSinRetencion==true ? q.dc_ValorPago == null : q.dc_ValorPago != null)
                             select new CXC_003_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega = q.IdBodega,
                                 IdCbteVta = q.IdCbteVta,
                                 vt_NumFactura = q.vt_NumFactura,
                                 pe_nombreCompleto = q.pe_nombreCompleto,
                                 pe_cedulaRuc = q.pe_cedulaRuc,
                                 cr_fecha = q.cr_fecha,
                                 cr_NumDocumento = q.cr_NumDocumento,
                                 IdCobro_tipo = q.IdCobro_tipo,
                                 IdMotivo_tipo_cobro = q.IdMotivo_tipo_cobro,
                                 PorcentajeRet = q.PorcentajeRet,
                                 Base = q.Base,
                                 ESRetenIVA = q.ESRetenIVA,
                                 ESRetenFTE = q.ESRetenFTE,
                                 cr_EsElectronico = q.cr_EsElectronico,
                                 tc_descripcion = q.tc_descripcion,
                                 TipoRetencion = q.TipoRetencion,
                                 IdCliente = q.IdCliente,
                                 vt_fecha = q.vt_fecha,
                                 dc_ValorPago = q.dc_ValorPago
                             }).ToList();
                }
                return Lista;
            }
            catch (Exception )
            {

                throw;
            }
        }
    }
}
