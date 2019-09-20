using Core.Erp.Data.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Contabilidad
{
    public class CONTA_009_Bus
    {
        CONTA_009_Data odata = new CONTA_009_Data();
        public List<CONTA_009_Info> get_list(int IdEmpresa, DateTime fechaIni, DateTime fechaFin, string IdUsuario, bool mostrarSaldo0, bool MostrarSaldoAcumulado,string IdCentroCosto)
        {
            try
            {
                return odata.get_list(IdEmpresa, fechaIni, fechaFin, IdUsuario, mostrarSaldo0, MostrarSaldoAcumulado, IdCentroCosto);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
