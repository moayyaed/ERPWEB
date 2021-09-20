using Core.Erp.Data.Reportes.Contabilidad;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Contabilidad
{
    public class CONTA_016_Bus
    {
        CONTA_016_Data odata = new CONTA_016_Data();
        public List<CONTA_016_Info> GetList(int IdEmpresa, DateTime FechaDesde, DateTime FechaHasta)
        {
            return odata.GetList(IdEmpresa, FechaDesde, FechaHasta);
        }
    }
}
