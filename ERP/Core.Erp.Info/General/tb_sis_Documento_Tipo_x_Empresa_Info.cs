using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.General
{
    public class tb_sis_Documento_Tipo_x_Empresa_Info
    {
        public int IdEmpresa { get; set; }
        public string codDocumentoTipo { get; set; }
        public int Posicion { get; set; }
        public string Descripcion { get; set; }
        public bool Seleccionado { get; set; }
    }
}
