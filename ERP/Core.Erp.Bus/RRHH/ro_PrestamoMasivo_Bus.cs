using Core.Erp.Bus.General;
using Core.Erp.Data;
using Core.Erp.Data.RRHH;
using Core.Erp.Info.General;
using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.RRHH
{
    public class ro_PrestamoMasivo_Bus
    {
        ro_PrestamoMasivo_Data odata = new ro_PrestamoMasivo_Data();
        ro_PrestamoMasivo_Det_Data odata_det = new ro_PrestamoMasivo_Det_Data();

        public List<ro_PrestamoMasivo_Info> get_list(int IdEmpresa, DateTime fecha_ini, DateTime fecha_fin, int IdSucursal, bool MostrarAnulados)
        {
            try
            {
                return odata.get_list(IdEmpresa, fecha_ini, fecha_fin, IdSucursal, MostrarAnulados);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(ro_PrestamoMasivo_Info info)
        {
            try
            {
                if (odata.guardarDB(info))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                tb_LogError_Bus LogData = new tb_LogError_Bus();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "ro_prestamo_Bus", Metodo = "guardarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }

        public bool modificarDB(ro_PrestamoMasivo_Info info)
        {
            try
            {
                if (odata.modificarDB(info))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                tb_LogError_Bus LogData = new tb_LogError_Bus();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "ro_prestamo_Bus", Metodo = "modificarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }

        public bool anularDB(ro_PrestamoMasivo_Info info)
        {
            try
            {
                if (odata.anularDB(info))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
