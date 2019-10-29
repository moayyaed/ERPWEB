using Core.Erp.Data.Contabilidad;
using Core.Erp.Info.Contabilidad;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Contabilidad
{
    public class ct_grupo_x_Tipo_Gasto_Bus
    {
        ct_grupo_x_Tipo_Gasto_Data odata = new ct_grupo_x_Tipo_Gasto_Data();
        public List<ct_grupo_x_Tipo_Gasto_Info> get_list(int IdEmpresa, bool mostrar_anulados)
        {
            try
            {
                return odata.get_list(IdEmpresa, mostrar_anulados);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ct_grupo_x_Tipo_Gasto_Info get_info(int IdEmpresa, int IdTipo_Gasto)
        {
            try
            {
                return odata.get_info(IdEmpresa, IdTipo_Gasto);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(ct_grupo_x_Tipo_Gasto_Info info)
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
        public bool modificarDB(ct_grupo_x_Tipo_Gasto_Info info)
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
        public bool anularDB(ct_grupo_x_Tipo_Gasto_Info info)
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

        public List<ct_grupo_x_Tipo_Gasto_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args, int IdEmpresa)
        {
            try
            {
                return odata.get_list_bajo_demanda(args, IdEmpresa);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ct_grupo_x_Tipo_Gasto_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args, int IdEmpresa)
        {
            try
            {
                return odata.get_info_bajo_demanda(args, IdEmpresa);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ct_grupo_x_Tipo_Gasto_Info get_info_nuevo(int IdEmpresa, int IdTipoGasto_padre)
        {
            try
            {
                return odata.get_info_nuevo(IdEmpresa, IdTipoGasto_padre);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
