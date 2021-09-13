using Core.Erp.Data.RRHH;
using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.RRHH
{
    public class ro_biometrico_Bus
    {
        ro_biometrico_Data odata = new ro_biometrico_Data();
        public List<ro_biometrico_Info> get_list(int IdEmpresa, bool estado)
        {
            try
            {
                return odata.get_list(IdEmpresa, estado);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ro_biometrico_Info get_info(int IdEmpresa, int IdBiometrico)
        {
            try
            {
                return odata.get_info(IdEmpresa, IdBiometrico);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(ro_biometrico_Info info)
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

        public bool modificarDB(ro_biometrico_Info info)
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

        public bool anularDB(ro_biometrico_Info info)
        {
            try
            {
                return odata.anularDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
