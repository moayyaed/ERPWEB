using Core.Data.General;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.General
{
    public class tb_ColaCorreo_Bus
    {
        tb_ColaCorreo_Data odata = new tb_ColaCorreo_Data();
        public List<tb_ColaCorreo_Info> GetList(int IdEmpresa, DateTime FechaIni, DateTime FechaFin)
        {
            try
            {
                return odata.GetList(IdEmpresa, FechaIni, FechaFin);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tb_ColaCorreo_Info GetInfoPendienteEnviar()
        {
            try
            {
                return odata.GetInfoPendienteEnviar();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(tb_ColaCorreo_Info info)
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

        public bool ModificarDB(tb_ColaCorreo_Info info)
        {
            try
            {
                return odata.ModificarDB(info); 
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
