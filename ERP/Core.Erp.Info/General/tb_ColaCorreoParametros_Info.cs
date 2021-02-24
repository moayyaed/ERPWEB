using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.General
{
    public class tb_ColaCorreoParametros_Info
    {
        public int IdEmpresa { get; set; }
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
        public int Puerto { get; set; }
        public string Host { get; set; }
        public bool PermitirSSL { get; set; }
        public string CorreoCopia { get; set; }
        public string IdUsuarioCreacion { get; set; }
    }
}
