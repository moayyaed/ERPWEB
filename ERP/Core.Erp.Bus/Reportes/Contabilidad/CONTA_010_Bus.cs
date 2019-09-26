using Core.Erp.Data.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;

namespace Core.Erp.Bus.Reportes.Contabilidad
{
    public class CONTA_010_Bus
    {
        CONTA_010_Data odata = new CONTA_010_Data();
        public List<CONTA_010_Info> get_list(int IdEmpresa, string IdGrupoCble, string IdCtaCble, DateTime fechaini, DateTime fechafin)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdGrupoCble, IdCtaCble, fechaini, fechafin);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
