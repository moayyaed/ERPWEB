using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.RRHH
{
    public class ro_PrestamoMasivo_Det_Data
    {
        public List<ro_PrestamoMasivo_Det_Info> get_list(int IdEmpresa, decimal IdCarga)
        {
            try
            {
                List<ro_PrestamoMasivo_Det_Info> Lista;

                using (Entities_rrhh Context = new Entities_rrhh())
                {
                    Lista = (from q in Context.ro_PrestamoMasivo_Det
                             where q.IdEmpresa == IdEmpresa
                                   & q.IdCarga == IdCarga
                             select new ro_PrestamoMasivo_Det_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdCarga = q.IdCarga,
                                 IdSucursal = q.IdSucursal,
                                 SaldoInicial = q.SaldoInicial,
                                 TotalCuota = q.TotalCuota,
                                 Saldo = q.Saldo,
                                 FechaPago = q.FechaPago,
                                 Secuencia = q.Secuencia,
                                 IdPrestamo = q.IdPrestamo,
                                 IdEmpleado = q.IdEmpleado,
                                 IdRubro = q.IdRubro,
                                 IdNominaTipoLiqui = q.IdNominaTipoLiqui
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
