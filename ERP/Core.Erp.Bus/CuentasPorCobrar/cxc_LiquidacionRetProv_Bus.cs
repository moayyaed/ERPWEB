using Core.Erp.Bus.General;
using Core.Erp.Data.CuentasPorCobrar;
using Core.Erp.Info.CuentasPorCobrar;
using Core.Erp.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.CuentasPorCobrar
{
    public class cxc_LiquidacionRetProv_Bus
    {
        cxc_LiquidacionRetProv_Data odata = new cxc_LiquidacionRetProv_Data();
        public List<cxc_LiquidacionRetProv_Info> get_list(int IdEmpresa, DateTime Fecha_ini, DateTime Fecha_fin)
        {
            try
            {
                return odata.get_list(IdEmpresa, Fecha_ini, Fecha_fin);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public cxc_LiquidacionRetProv_Info get_info(int IdEmpresa, decimal IdLiquidacion)
        {
            try
            {
                return odata.get_info(IdEmpresa, IdLiquidacion);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool guardarDB(cxc_LiquidacionRetProv_Info info)
        {
            try
            {
                return odata.guardarDB(info);
            }
            catch (Exception ex)
            {
                tb_LogError_Bus LogData = new tb_LogError_Bus();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "cxc_cobro_Bus", Metodo = "guardarDB", IdUsuario = info.IdUsuarioCreacion });
                return false;
            }
        }

        public bool modificarDB(cxc_LiquidacionRetProv_Info info)
        {
            try
            {
                return odata.modificarDB(info);
            }
            catch (Exception ex)
            {
                tb_LogError_Bus LogData = new tb_LogError_Bus();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "cxc_cobro_Bus", Metodo = "modificarDB", IdUsuario = info.IdUsuarioModificacion });
                return false;
            }
        }

        public bool anularDB(cxc_LiquidacionRetProv_Info info)
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
