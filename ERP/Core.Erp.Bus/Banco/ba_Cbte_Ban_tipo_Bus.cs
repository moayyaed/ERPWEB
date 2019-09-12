using Core.Erp.Data.Banco;
using Core.Erp.Info.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Bus.Banco
{
    public class ba_Cbte_Ban_tipo_Bus
    {
        ba_Cbte_Ban_tipo_Data odata = new ba_Cbte_Ban_tipo_Data();

        public List<ba_Cbte_Ban_tipo_Info> get_list()
        {
            try
            {
                return odata.get_list();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
