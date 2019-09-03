using Core.Erp.Data.Reportes.Banco;
using Core.Erp.Info.Reportes.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Reportes.Banco
{
    public class BAN_014_Bus
    {
        BAN_014_Data odata = new BAN_014_Data();

        public List<BAN_014_Info> GetList(int IdEmpresa, int IdBanco, decimal IdPersona, DateTime FechaIni, DateTime FechaFin)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdBanco, IdPersona, FechaIni, FechaFin);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
