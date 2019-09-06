using Core.Erp.Data.Reportes.Base;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.CuentasPorCobrar
{
    public class CXC_006_Data
    {
        public List<CXC_006_Info> GetList(int IdEmpresa, int IdSucursal, decimal IdVendedor, decimal IdCliente, String IdCobro_tipo, DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                int IdSucursalIni = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 9999 : IdSucursal;

                decimal IdClienteIni = IdCliente;
                decimal IdClienteFin = IdCliente == 0 ? 9999999999 : IdCliente;

                decimal IdVendedorIni = IdVendedor;
                decimal IdVendedorFin = IdVendedor == 0 ? 9999999999 : IdVendedor;

                fecha_ini = fecha_ini.Date;
                fecha_fin = fecha_fin.Date;

                List<CXC_006_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = Context.VWCXC_006.Where(q => q.IdEmpresa == IdEmpresa
                    && IdSucursalIni <= q.IdSucursal
                    && q.IdSucursal <= IdSucursalFin
                    && IdVendedorIni <= q.IdVendedor
                    && q.IdVendedor <= IdVendedorFin
                    && IdClienteIni <= q.IdCliente
                    && q.IdCliente <= IdClienteFin
                    && q.IdCobro_tipo == IdCobro_tipo
                    && fecha_ini <= q.cr_fecha
                    && q.cr_fecha <= fecha_fin).Select(q => new CXC_006_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdSucursal = q.IdSucursal,
                        IdBodega = q.IdBodega,
                        IdCobro = q.IdCobro,
                        secuencial = q.secuencial,
                        cr_fecha = q.cr_fecha,
                        vt_tipoDoc = q.vt_tipoDoc,
                        IdCbteVta = q.IdCbteVta,
                        Ve_Vendedor = q.Ve_Vendedor,
                        IdCliente = q.IdCliente,
                        vt_fecha = q.vt_fecha,
                        IdCobro_tipo = q.IdCobro_tipo,
                        NumCotizacion = q.NumCotizacion,
                        pe_nombreCompleto = q.pe_nombreCompleto,
                        NumOPr = q.NumOPr,
                        CodCbteVta = q.CodCbteVta,
                        IdVendedor = q.IdVendedor,
                        vt_NumFactura = q.vt_NumFactura,
                        vt_fech_venc = q.vt_fech_venc,
                        DiasAtraso = q.DiasAtraso,
                        ValorPago = q.ValorPago,
                        BaseComision = q.BaseComision
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(IdCobro_tipo))
                    Lista = Lista.Where(q => q.IdCobro_tipo == IdCobro_tipo).ToList();
                return Lista;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
