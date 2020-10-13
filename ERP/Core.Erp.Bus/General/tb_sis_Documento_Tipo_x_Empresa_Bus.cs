using Core.Erp.Data.General;
using Core.Erp.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.General
{
    class tb_sis_Documento_Tipo_x_Empresa_Bus
    {
        tb_sis_Documento_Tipo_x_Empresa_Data odata = new tb_sis_Documento_Tipo_x_Empresa_Data();
        public List<tb_sis_Documento_Tipo_x_Empresa_Info> GetList(int IdEmpresa, bool MostrarNoAsignado)
        {
            try
            {
                return odata.GetList(IdEmpresa, MostrarNoAsignado);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(tb_sis_Documento_Tipo_x_Empresa_Info info)
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

        public bool EliminarDB(int IdEmpresa)
        {
            try
            {
                return odata.EliminarDB(IdEmpresa);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
