using Core.Erp.Info.Reportes.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Reportes.CuentasPorPagar
{
  public class CXP_014_Data
    {
         public List<CXP_014_Info> GetList(int IdEmpresa,int IdSucursal, decimal IdProveedor, DateTime fecha_ini, DateTime fecha_fin)

        {
            try
            {
                List<CXP_014_Info> Lista;
                decimal IdProveedorIni = IdProveedor;
                decimal IdProveedorFin = IdProveedor == 0 ? 9999 : IdProveedor;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = Context.VWCXP_014.Where (q=> q.IdEmpresa == IdEmpresa
                    && q.IdSucursal == IdSucursal
                    && IdProveedorIni <= q.IdProveedor
                    && q.IdProveedor <= IdProveedorFin
                    && fecha_ini <= q.co_FechaFactura
                    && q.co_FechaFactura <= fecha_fin
                    ).Select(q => new CXP_014_Info
                    {

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
