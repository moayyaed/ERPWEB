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
    public class CONTA_014_Bus
    {
        CONTA_014_Data odata = new CONTA_014_Data();
        public List<CONTA_014_Info> GetList(int IdEmpresa, DateTime FechaDesde, DateTime FechaHasta, bool MostrarAcumulado, bool ValoresOPorcentaje)
        {
            return odata.GetList(IdEmpresa, FechaDesde, FechaHasta, MostrarAcumulado, ValoresOPorcentaje);
        }
    }
}
