using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Contabilidad
{
    public class ct_periodo_x_tb_modulo_Info
    {
        public int IdEmpresa { get; set; }
        public int IdPeriodo { get; set; }
        public string IdModulo { get; set; }
        public bool Cerrado { get; set; }
        public string IdUsuario { get; set; }
        public string IdUsuarioUltModi { get; set; }
        public Nullable<System.DateTime> FechaTransac { get; set; }
        public Nullable<System.DateTime> FechaUltModi { get; set; }

        #region Campos que no existen en la tabla
        public string Descripcion { get; set; }
        public int Secuencia { get; set; }
        #endregion
    }
}
