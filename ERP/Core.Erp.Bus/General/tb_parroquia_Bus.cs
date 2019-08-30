using Core.Erp.Data.General;
using Core.Erp.Info.General;
using DevExpress.Web;
using System;
using System.Collections.Generic;

namespace Core.Erp.Bus.General
{
    public class tb_parroquia_Bus
    {
        tb_parroquia_Data odata = new tb_parroquia_Data();
        public List<tb_parroquia_Info> get_list( string IdCiudad, bool mostrar_anulados)
        {
            try
            {
                return odata.get_list(IdCiudad, mostrar_anulados);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tb_parroquia_Info get_info(string IdParroquia)
        {
            try
            {
                return odata.get_info(IdParroquia);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(tb_parroquia_Info info)
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

        public bool modificarDB(tb_parroquia_Info info)
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

        public bool anularDB(tb_parroquia_Info info)
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

        public List<tb_parroquia_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args, string IdCiudad)
        {
            try
            {
                return odata.get_list_bajo_demanda(args, IdCiudad);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tb_parroquia_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args, string IdCiudad)
        {
            try
            {
                return odata.get_info_bajo_demanda(args, IdCiudad);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
