using Core.Erp.Data.RRHH;
using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;

namespace Core.Erp.Bus.RRHH
{
   public class ro_canasta_basica_Bus
    {
        ro_canasta_basica_Data odata = new ro_canasta_basica_Data();
        public List<ro_canasta_basica_Info> get_list()
        {
            try
            {
                return odata.get_list();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ro_canasta_basica_Info get_info(int IdEmpresa, int IdCargo)
        {
            try
            {
                return odata.get_info(IdEmpresa, IdCargo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(ro_canasta_basica_Info info)
        {
            try
            {
                return odata.guardarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool modificarDB(ro_canasta_basica_Info info)
        {
            try
            {

                return odata.modificarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

      
    }
}
