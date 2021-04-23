using Core.Data.General;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.General
{
    public class tb_TarjetaCredito_Bus
    {
        tb_TarjetaCredito_Data odata = new tb_TarjetaCredito_Data();
        public List<tb_TarjetaCredito_Info> GetList(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                return odata.getList(IdEmpresa, MostrarAnulados);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public tb_TarjetaCredito_Info GetInfo(int IdEmpresa, int IdTarjeta)
        {
            try
            {
                return odata.getInfo(IdEmpresa, IdTarjeta);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GuardarDB(tb_TarjetaCredito_Info info)
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

        public bool ModificarDB(tb_TarjetaCredito_Info info)
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

        public bool AnularDB(tb_TarjetaCredito_Info info)
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
