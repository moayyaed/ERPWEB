using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.SeguridadAcceso
{
    public class seg_usuario_x_tb_sucursal_Info
    {
        public string IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public string Observacion { get; set; }
    }
}
