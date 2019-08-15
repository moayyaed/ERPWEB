using Core.Erp.Info.Reportes.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.CuentasPorPagar
{
    public class CXP_017_Data
    {
        public List<CXP_017_Info> GetList(int IdEmpresa, List<decimal> IdOrdenPago)
        {
            try
            {
                List<CXP_017_Info> Lista;

                using (Entities_reportes db = new Entities_reportes())
                {
                    Lista = db.VWCXP_017.Where(q=>q.IdEmpresa == IdEmpresa && IdOrdenPago.Contains( q.IdOrdenPago)).Select(q => new CXP_017_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdOrdenPago = q.IdOrdenPago,
                        IdSucursal = q.IdSucursal,
                        Su_Descripcion = q.Su_Descripcion,
                        IdPersona = q.IdPersona,
                        pe_nombreCompleto = q.pe_nombreCompleto,
                        Fecha = q.Fecha,
                        Observacion = q.Observacion,
                        Total_OP = q.Total_OP,
                        Referencia = q.Referencia
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
