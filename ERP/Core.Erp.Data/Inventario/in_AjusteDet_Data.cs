using Core.Erp.Info.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Data.Inventario
{
    public class in_AjusteDet_Data
    {
        public List<in_AjusteDet_Info> GetList(int IdEmpresa, int IdAjuste)
        {
            try
            {
                List<in_AjusteDet_Info> Lista = new List<in_AjusteDet_Info>();

                using (Entities_inventario db = new Entities_inventario())
                {
                    Lista = db.in_AjusteDet.Where(q => q.IdEmpresa == IdEmpresa && q.IdAjuste == IdAjuste).Select(q => new in_AjusteDet_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdAjuste = q.IdAjuste,
                        Secuencia = q.Secuencia,
                        IdProducto = q.IdProducto,
                        IdUnidadMedida = q.IdUnidadMedida,
                        StockSistema = q.StockSistema,
                        StockFisico = q.StockFisico,
                        Ajuste = q.Ajuste,
                        Costo = q.Costo
                    }).ToList();

                    return Lista;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
