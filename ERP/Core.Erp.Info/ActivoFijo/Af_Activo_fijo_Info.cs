using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.ActivoFijo
{
    public class Af_Activo_fijo_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public int IdActivoFijo { get; set; }
        public string CodActivoFijo { get; set; }
        [Required(ErrorMessage = ("el campo nombre es obligatorio"))]
        public string Af_Nombre { get; set; }
        [Required(ErrorMessage = ("el campo tipo de activo fjo es obligatorio"))]
        public int IdActivoFijoTipo { get; set; }
        public int IdCategoriaAF { get; set; }
        public int IdSucursal { get; set; }
        public decimal IdDepartamento { get; set; }
        public decimal IdArea { get; set; }
        public string Af_NumSerie { get; set; }
        public System.DateTime Af_fecha_compra { get; set; }
        public System.DateTime Af_fecha_ini_depre { get; set; }
        public System.DateTime Af_fecha_fin_depre { get; set; }
        public double Af_costo_compra { get; set; }
        public double Af_Depreciacion_acum { get; set; }
        public int Af_Vida_Util { get; set; }
        public int Af_Meses_depreciar { get; set; }
        public double Af_porcentaje_deprec { get; set; }
        public string Af_observacion { get; set; }
        public string Estado { get; set; }
        [Required(ErrorMessage = ("el campo empleado encargado es obligatorio"))]
        public decimal IdEmpleadoEncargado { get; set; }
        [Required(ErrorMessage = ("el campo empleado custodio es obligatorio"))]
        public decimal IdEmpleadoCustodio { get; set; }
        public string Af_Codigo_Barra { get; set; }
        public string Estado_Proceso { get; set; }
        public double Af_ValorSalvamento { get; set; }
        [Required(ErrorMessage = ("el campo cantidad es obligatorio"))]
        public int Cantidad { get; set; }
        public Nullable<int> IdModelo { get; set; }
        public Nullable<int> IdMarca { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string IdUsuarioAnulacion { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        public string MotivoAnulacion { get; set; }


        //Campos que no existen en la tabla
        public bool EstadoBool { get; set; }
        public string Estado_Proceso_nombre { get; set; }
        public string IdCtaCble { get; set; }
        public string pc_Cuenta { get; set; }
        public byte[] imagen_af { get; set; }
        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }

    }

    public class Af_Activo_fijo_valores_Info
    {
        public int IdEmpresa { get; set; }
        public int IdActivoFijo { get; set; }
        public double v_activo { get; set; }
        public double v_depr_acum { get; set; }
        public double v_mejora { get; set; }
        public double v_baja { get; set; }
        public double v_neto { get; set; }



    }
}
