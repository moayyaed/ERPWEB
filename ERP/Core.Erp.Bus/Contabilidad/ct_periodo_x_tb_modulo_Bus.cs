using Core.Erp.Data.Contabilidad;
using Core.Erp.Info.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Contabilidad
{
    public class ct_periodo_x_tb_modulo_Bus
    {
        ct_periodo_x_tb_modulo_Data odata = new ct_periodo_x_tb_modulo_Data();

        public List<ct_periodo_x_tb_modulo_Info> GetList(int IdEmpresa, int IdPeriodo)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdPeriodo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ct_periodo_x_tb_modulo_Info GetInfo(int IdEmpresa, int IdPeriodo, string IdModulo)
        {
            try
            {
                return odata.GetInfo(IdEmpresa, IdPeriodo, IdModulo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GuardarBD(ct_periodo_x_tb_modulo_Info info)
        {
            try
            {
                return odata.GuardarBD(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarBD(ct_periodo_x_tb_modulo_Info info)
        {
            try
            {
                return odata.ModificarBD(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
