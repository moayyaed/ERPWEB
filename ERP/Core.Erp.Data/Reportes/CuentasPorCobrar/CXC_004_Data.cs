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
        public List<CXC_004_Info> get_list(int IdEmpresa, int IdSucursal, decimal IdCliente, int Idtipo_cliente, DateTime FechaIni, DateTime FechaFin, bool MostrarSaldo0)
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
