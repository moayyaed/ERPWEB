using Core.Erp.Data.Facturacion;
using Core.Erp.Info.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Facturacion
{
    public class fa_factura_tipo_Bus
    {
        fa_factura_tipo_Data odata = new fa_factura_tipo_Data();

        public List<fa_factura_tipo_Info> GetList(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                return odata.GetList(IdEmpresa, MostrarAnulados);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public fa_factura_tipo_Info GetInfo(int IdEmpresa, int IdFacturaTipo)
        {
            try
            {
                return odata.GetInfo(IdEmpresa, IdFacturaTipo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool GuardarDB(fa_factura_tipo_Info info)
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

        public bool ModificarDB(fa_factura_tipo_Info info)
        {
            try
            {
                return odata.ModificarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool AnularDB(fa_factura_tipo_Info info)
        {
            try
            {
                return odata.AnularDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
