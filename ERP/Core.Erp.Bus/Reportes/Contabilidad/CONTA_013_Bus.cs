using Core.Erp.Data.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;

namespace Core.Erp.Bus.Reportes.Contabilidad
{
    public class CONTA_013_Bus
    {
        CONTA_013_Data odata = new CONTA_013_Data();
        public List<CONTA_013_Info> get_list(int IdEmpresa, int IdPunto_cargo_grupo, int IdPunto_cargo, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdPunto_cargo_grupo, IdPunto_cargo, fechaIni, fechaFin);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
