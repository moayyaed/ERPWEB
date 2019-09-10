using Core.Erp.Data.Reportes.Base;
using Core.Erp.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.CuentasPorCobrar
{
    public class CXC_004_Data
    {
        public List<CXC_004_Info> get_list(int IdEmpresa, int IdSucursal, decimal IdCliente, DateTime FechaIni, DateTime FechaFin, bool MostrarSaldo0)
        {
            try
            {
                int IdSucursalIni = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 999999 : IdSucursal;

                int IdClienteIni = Convert.ToInt32(IdCliente);
                int IdClienteFin = IdCliente == 0 ? 9999999 : Convert.ToInt32(IdCliente);

                FechaIni = FechaIni.Date;
                FechaFin = FechaFin.Date;

                List<CXC_004_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.SPCXC_004(IdEmpresa, IdSucursalIni, IdSucursalFin, IdClienteIni, IdClienteFin, FechaIni, FechaFin, MostrarSaldo0)
                             select new CXC_004_Info
                             {
                                IdEmpresa = q.IdEmpresa,
                                IdSucursal= q.IdSucursal,
                                IdBodega = q.IdBodega,
                                IdCliente=q.IdCliente,
                                IdCbteVta =q.IdCbteVta,
                                vt_NumFactura = q.vt_NumFactura,
                                vt_fecha =q.vt_fecha,
                                vt_fech_venc= q.vt_fech_venc,
                                pe_nombreCompleto =q.pe_nombreCompleto,
                                Su_Descripcion=q.Su_Descripcion,
                                Total=q.Total,
                                Saldo=q.Saldo,
                                Debe=q.Debe,
                                Haber=q.Haber,
                                Dias=q.Dias,
                                vt_tipoDoc = q.vt_tipoDoc,
                                 TipoDoc = q.TipoDoc

                             }).ToList();
                }
                return Lista;
            }
            catch (Exception EX)
            {

                throw;
            }
        }
    }
}
