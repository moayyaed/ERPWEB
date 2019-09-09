using Core.Erp.Data.ActivoFijo;
using Core.Erp.Info.ActivoFijo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.ActivoFijo
{
    public class Af_Marca_Bus
    {
        Af_Marca_Data oData = new Af_Marca_Data();

        public List<Af_Marca_Info> GetList(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                return oData.get_list(IdEmpresa, mostrar_anulados);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Af_Marca_Info GetInfo(int IdEmpresa, int IdModelo)
        {
            try
            {
                return oData.get_info(IdEmpresa, IdModelo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(Af_Marca_Info info)
        {
            try
            {
                return oData.guardarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarDB(Af_Marca_Info info)
        {
            try
            {
                return oData.modificarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AnularDB(Af_Marca_Info info)
        {
            try
            {
                return oData.anularDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
