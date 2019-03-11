using Core.Erp.Data.RRHH;
using Core.Erp.Info.RRHH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.RRHH
{
    public class ro_rdep_Bus
    {
        ro_rdep_Data oData = new ro_rdep_Data();

        public List<ro_rdep_Info> GetList(int IdEmpresa, int IdSucursal, int IdNomina_Tipo, int IdAnio)
        {
            try
            {
                return oData.GetList(IdEmpresa, IdSucursal, IdNomina_Tipo, IdAnio);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ro_rdep_Info GetInfo(int IdEmpresa, int IdSucursal, int IdNomina_tipo, int pe_anio, int IdEmpleado )
        {
            try
            {
                return oData.GetInfo(IdEmpresa, IdSucursal, IdNomina_tipo, pe_anio, IdEmpleado);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ro_rdep_Info> GenerarRDEP(int IdEmpresa, int IdSucursal, int IdAnio, int IdNomina_Tipo)
        {
            try
            {
                return oData.GenerarRDEP(IdEmpresa, IdSucursal, IdAnio, IdNomina_Tipo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GuardarBD(ro_rdep_Info info)
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

        public bool ModificarBD(ro_rdep_Info info)
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
    }
}
