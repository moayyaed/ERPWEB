using Core.Data.Facturacion;
using Core.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Facturacion
{
    public class fa_PuntoVta_x_seg_usuario_Bus
    {
        fa_PuntoVta_x_seg_usuario_Data odata = new fa_PuntoVta_x_seg_usuario_Data();
        public List<fa_PuntoVta_x_seg_usuario_Info> get_list(int IdEmpresa, int IdPuntoVta)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdPuntoVta);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public fa_PuntoVta_x_seg_usuario_Info get_info(int IdEmpresa, int IdPuntoVta, int Secuencia)
        {
            try
            {
                return odata.get_info(IdEmpresa, IdPuntoVta, Secuencia);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool guardarDB(fa_PuntoVta_x_seg_usuario_Info info)
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
    }
}
