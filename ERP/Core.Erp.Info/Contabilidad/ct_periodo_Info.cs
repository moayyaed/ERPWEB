using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Contabilidad
{
    public class ct_periodo_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        [Required(ErrorMessage = "El campo periodo es obligatorio")]
        public int IdPeriodo { get; set; }
        [Required(ErrorMessage = "El campo año es obligatorio")]
        public int IdanioFiscal { get; set; }
        [Required(ErrorMessage = "El campo mes es obligatorio")]
        public int pe_mes { get; set; }
        [Required(ErrorMessage = "El campo fecha incio es obligatorio")]
        public System.DateTime pe_FechaIni { get; set; }
        [Required(ErrorMessage = "El campo fecha fin es obligatorio")]
        public System.DateTime pe_FechaFin { get; set; }
        public string pe_cerrado { get; set; }
        public string pe_estado { get; set; }
        public bool EstadoBool { get; set; }
        //Campos que no existen en la tabla
        public bool pe_cerrado_bool { get; set; }
        public string nom_periodo_combo { get; set; }
        public string smes { get; set; }
        public string AnioMes { get; set; }
        public string IdUsuario { get; set; }
        public List<ct_periodo_x_tb_modulo_Info> lst_periodo_x_modulo { get; set; }
        public bool Seleccionado { get; set; }
        public List<ct_periodo_Info> lst_periodo { get; set; }

        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }

    }
}
