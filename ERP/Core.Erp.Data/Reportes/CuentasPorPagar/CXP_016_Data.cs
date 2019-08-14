using Core.Erp.Info.Reportes.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.CuentasPorPagar
{
    public class CXP_016_Data
    {
        public List<CXP_016_Info> GetList(int IdEmpresa, string IdUsuario, bool MostrarSaldo0,DateTime FechaIni, DateTime FechaFin, int IdClaseProveedor)
        {
            try
            {
                List<CXP_016_Info> Lista;
                FechaIni = FechaIni.Date;
                FechaFin = FechaFin.Date;

                int IdClaseProveedorIni = IdClaseProveedor;
                int IdClaseProveedorFin = IdClaseProveedor == 0 ? 99999999 : IdClaseProveedor;
                using (Entities_reportes db = new Entities_reportes())
                {
                    Lista = db.SPCXP_016(IdEmpresa, IdUsuario, MostrarSaldo0, FechaIni, FechaFin, IdClaseProveedorIni, IdClaseProveedorFin).Select(q=> new CXP_016_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdSucursal = q.IdSucursal,
                        IdProveedor = q.IdProveedor,
                        IdUsuario = q.IdUsuario,
                        Su_Descripcion = q.Su_Descripcion,
                        pe_CedulaRuc = q.pe_CedulaRuc,
                        pe_nombreCompleto = q.pe_nombreCompleto,
                        SaldoInicial = q.SaldoInicial,
                        Compra = q.Compra,
                        Retenciones = q.Retenciones,
                        Pagos = q.Pagos,
                        Saldo = q.Saldo,
                        IdProveedorClase = q.IdProveedorClase,
                        DescripcionClase = q.DescripcionClase
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
