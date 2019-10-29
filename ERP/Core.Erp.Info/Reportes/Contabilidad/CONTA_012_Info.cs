using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Contabilidad
{
    public class CONTA_012_Info
    {
        public int IdEmpresa { get; set; }
        public Nullable<int> nivel { get; set; }
        public Nullable<int> orden { get; set; }
        public int IdTipo_Gasto { get; set; }
        public Nullable<int> orden_tipo_gasto { get; set; }
        public string nom_tipo_Gasto { get; set; }
        public string nom_tipo_Gasto_padre { get; set; }
        public double dc_Valor { get; set; }
        public string IdCta { get; set; }
        public string nom_cuenta { get; set; }
        public string nom_grupo_CC { get; set; }

        #region Campos que no existen en la tabla
        public string Nom1 { get; set; }
        public string Nom2 { get; set; }
        public string Nom3 { get; set; }
        public string Nom4 { get; set; }
        public string Nom5 { get; set; }
        public string Nom6 { get; set; }
        public string Nom7 { get; set; }
        public string Nom8 { get; set; }
        public string Nom9 { get; set; }
        public string Nom10 { get; set; }
        public string Nom11 { get; set; }
        public string Nom12 { get; set; }
        public float? Col1 { get; set; }
        public float? Col2 { get; set; }
        public float? Col3 { get; set; }
        public float? Col4 { get; set; }
        public float? Col5 { get; set; }
        public float? Col6 { get; set; }
        public float? Col7 { get; set; }
        public float? Col8 { get; set; }
        public float? Col9 { get; set; }
        public float? Col10 { get; set; }
        public float? Col11 { get; set; }
        public float? Col12 { get; set; }

        #endregion
    }
}
