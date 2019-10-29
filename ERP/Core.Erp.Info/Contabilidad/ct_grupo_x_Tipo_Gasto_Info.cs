using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Contabilidad
{
    public class ct_grupo_x_Tipo_Gasto_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipo_Gasto { get; set; }
        public Nullable<int> IdTipo_Gasto_Padre { get; set; }
        [Required(ErrorMessage = "El campo nombre es obligatorio")]
        public string nom_tipo_Gasto { get; set; }
        public bool estado { get; set; }
        public Nullable<int> nivel { get; set; }
        public Nullable<int> orden { get; set; }

        public string nom_tipo_Gasto_Padre { get; set; }
    }
}
