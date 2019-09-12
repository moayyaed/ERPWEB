using Core.Erp.Info.Banco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Banco
{
    public class ba_Cbte_Ban_tipo_Data
    {
        public List<ba_Cbte_Ban_tipo_Info> get_list()
        {
            try
            {
                List<ba_Cbte_Ban_tipo_Info> Lista = new List<ba_Cbte_Ban_tipo_Info>();
                using (Entities_banco Context = new Entities_banco())
                {
                    Lista = Context.ba_Cbte_Ban_tipo.Select(q => new ba_Cbte_Ban_tipo_Info
                    {
                        CodTipoCbteBan = q.CodTipoCbteBan,
                        Descripcion = q.Descripcion,
                        Signo = q.Signo,
                        orden = q.orden
                    }).ToList();
    
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
