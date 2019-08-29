using Core.Erp.Data.Contabilidad;
using Core.Erp.Info.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Contabilidad
{
    public class ct_ClasificacionEBIT_Bus
    {
        ct_ClasificacionEBIT_Data oData = new ct_ClasificacionEBIT_Data();

        public List<ct_ClasificacionEBIT_Info> GetList()
        {
            try
            {
                return oData.GetList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ct_ClasificacionEBIT_Info GetInfo(int IdClasificacionEBIT)
        {
            try
            {
                return oData.GetInfo(IdClasificacionEBIT);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GuardarBD(ct_ClasificacionEBIT_Info info)
        {
            try
            {
                return oData.GuardarBD(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarBD(ct_ClasificacionEBIT_Info info)
        {
            try
            {
                return oData.ModificarBD(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AnularBD(ct_ClasificacionEBIT_Info info)
        {
            try
            {
                return oData.AnularBD(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
