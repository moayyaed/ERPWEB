using Core.Data.General;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.General
{
    public class tb_ColaCorreoParametros_Bus
    {
        tb_ColaCorreoParametros_Data odata = new tb_ColaCorreoParametros_Data();
        public tb_ColaCorreoParametros_Info GetInfo(int IdEmpresa)
        {
            try
            {
                return odata.GetInfo(IdEmpresa);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(tb_ColaCorreoParametros_Info info)
        {
            try
            {
                return odata.GuardarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
