using Core.Erp.Data.Facturacion;
using Core.Erp.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Facturacion
{
    public class fa_ProbabilidadCobro_Bus
    {
        fa_ProbabilidadCobro_Data odata = new fa_ProbabilidadCobro_Data();
        fa_ProbabilidadCobroDet_Data odata_det = new fa_ProbabilidadCobroDet_Data();

        public List<fa_ProbabilidadCobro_Info> get_list(int IdEmpresa, bool mostrar_anulados)
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
        public fa_ProbabilidadCobro_Info get_info(int IdEmpresa, int IdProbabilidad)
        {
            try
            {
                return odata.get_info(IdEmpresa, IdProbabilidad);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(fa_ProbabilidadCobro_Info info)
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

        public bool ModificarDB(fa_ProbabilidadCobro_Info info)
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

        public bool AnularDB(fa_ProbabilidadCobro_Info info)
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
