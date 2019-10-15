using Core.Erp.Data.Facturacion;
using Core.Erp.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Facturacion
{
    public class fa_ProbabilidadCobroDet_Bus
    {
        fa_ProbabilidadCobroDet_Data odata = new fa_ProbabilidadCobroDet_Data();
        public List<fa_ProbabilidadCobroDet_Info> get_list(int IdEmpresa, int IdProbabilidad)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdProbabilidad);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<fa_ProbabilidadCobroDet_Info> get_list_x_ingresar(int IdEmpresa)
        {
            try
            {
                return odata.get_list_x_ingresar(IdEmpresa);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(fa_ProbabilidadCobroDet_Info info)
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

        public bool EliminarDB(fa_ProbabilidadCobroDet_Info info)
        {
            try
            {
                return odata.eliminarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
