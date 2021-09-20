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
    public class CONTA_015_Bus
    {
        CONTA_015_Data odata = new CONTA_015_Data();
        public List<CONTA_015_Info> GetList(int IdEmpresa, DateTime FechaDesde, DateTime FechaHasta)
        {
            return odata.GetList(IdEmpresa, FechaDesde, FechaHasta);
        }
    }
}
