using Core.Erp.Data.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Contabilidad
{
    public class CONTA_011_Bus
    {
        CONTA_011_Data odata = new CONTA_011_Data();
        public List<CONTA_011_Info> get_list(int IdEmpresa, string IdUsuario, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdUsuario, fechaIni, fechaFin);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
