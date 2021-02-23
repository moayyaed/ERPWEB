using Core.Erp.Data.CuentasPorCobrar;
using Core.Erp.Info.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.CuentasPorCobrar
{
    public class cxc_SeguimientoCartera_Bus
    {
        cxc_SeguimientoCartera_Data odata = new cxc_SeguimientoCartera_Data();

        public List<cxc_SeguimientoCartera_Info> getList(int IdEmpresa, decimal IdAlumno, bool MostrarAnulados, DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                return odata.getList(IdEmpresa, IdAlumno, MostrarAnulados, fecha_ini, fecha_fin);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<cxc_SeguimientoCartera_Info> getList_x_Alumno(int IdEmpresa, decimal IdAlumno)
        {
            try
            {
                return odata.getList_x_Alumno(IdEmpresa, IdAlumno);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public cxc_SeguimientoCartera_Info get_info(int IdEmpresa, int IdSeguimiento)
        {
            try
            {
                return odata.get_info(IdEmpresa, IdSeguimiento);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int getId(int IdEmpresa)
        {
            try
            {
                return odata.getId(IdEmpresa);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(cxc_SeguimientoCartera_Info info)
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

        public bool anularDB(cxc_SeguimientoCartera_Info info)
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

        public bool EnviarCorreoDB(cxc_SeguimientoCartera_Info info)
        {
            try
            {
                return odata.enviarcorreoDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
