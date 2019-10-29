using Core.Erp.Data.General;
using Core.Erp.Data.Reportes.Base;
using Core.Erp.Info.Reportes.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Erp.Data.Reportes.Contabilidad
{
    public class CONTA_012_Data
    {
        public List<CONTA_012_Info> get_list(int IdEmpresa, int IdPeriodo, int numero)
        {
            try
            {
                List<CONTA_012_Info> Lista = new List<CONTA_012_Info>();
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = (from q in Context.SPCONTA_012(IdEmpresa, IdPeriodo)
                             select new CONTA_012_Info
                             {
                                IdEmpresa = q.IdEmpresa,
                                dc_Valor = q.dc_Valor,
                                IdCta = q.IdCta,
                                IdTipo_Gasto = q.IdTipo_Gasto,
                                nivel = q.nivel,
                                nom_cuenta = q.nom_cuenta,
                                nom_grupo_CC = q.nom_grupo_CC,
                                nom_tipo_Gasto = q.nom_tipo_Gasto,
                                nom_tipo_Gasto_padre = q.nom_tipo_Gasto_padre,
                                orden = q.orden,
                                orden_tipo_gasto = q.orden_tipo_gasto,

                                Col1 = numero ==1 ? (float?)q.dc_Valor : null,
                                Col2 = numero == 2 ? (float?)q.dc_Valor : null,
                                Col3 = numero == 3 ? (float?)q.dc_Valor : null,
                                Col4 = numero == 4 ? (float?)q.dc_Valor : null,
                                Col5 = numero == 5 ? (float?)q.dc_Valor : null,
                                Col6 = numero == 6 ? (float?)q.dc_Valor : null,
                                Col7 = numero == 7 ? (float?)q.dc_Valor : null,
                                Col8 = numero == 8 ? (float?)q.dc_Valor : null,
                                Col9 = numero == 9 ? (float?)q.dc_Valor : null,
                                Col10 = numero == 10 ? (float?)q.dc_Valor : null,
                                Col11 = numero == 11 ? (float?)q.dc_Valor : null,
                                Col12 = numero == 12 ? (float?)q.dc_Valor : null
                             }).ToList();
                }
                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
