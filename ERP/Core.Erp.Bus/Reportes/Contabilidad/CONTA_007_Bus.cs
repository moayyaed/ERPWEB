using Core.Erp.Data.Reportes.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Contabilidad
{
    public class CONTA_007_Bus
    {
        CONTA_007_Data odata = new CONTA_007_Data();
        public List<CONTA_007_Info> GetList(int IdEmpresa, string IdUsuario, DateTime fecha_ini, DateTime fecha_fin, bool MostarAcumulado, bool MostarDetalle)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdUsuario, fecha_ini, fecha_fin, MostarAcumulado, MostarDetalle);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
