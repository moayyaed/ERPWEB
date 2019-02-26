using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.Inventario;
using Core.Erp.Bus.Inventario;
using Core.Erp.Info.Helps;
using DevExpress.Web.Mvc;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.Contabilidad;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    [SessionTimeout]
    public class DistribucionPorLoteController : Controller
    {
        #region Variables
        in_Ing_Egr_Inven_distribucion_Bus bus_ing_inv = new in_Ing_Egr_Inven_distribucion_Bus();
        in_Ing_Egr_Inven_det_Bus bus_det_ing_inv = new in_Ing_Egr_Inven_det_Bus();
        in_Ing_Egr_Inven_distribucion_lst List_in_Ing_Egr_Inven_det = new in_Ing_Egr_Inven_distribucion_lst();
        lst_x_distribuir lst_x_distribuir_lis = new lst_x_distribuir();
        in_parametro_Bus bus_in_param = new in_parametro_Bus();
        in_Ing_Egr_Inven_Bus bus_ingreso = new in_Ing_Egr_Inven_Bus();
        string mensaje = string.Empty;
        decimal IdProducto_padre = 0;
        List<in_Producto_Info> list_productos = new List<in_Producto_Info>();
        List<in_Ing_Egr_Inven_distribucion_Info> lst_x_distribuir = new List<in_Ing_Egr_Inven_distribucion_Info>();
        in_Producto_Bus bus_producto = new in_Producto_Bus();
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();

        #endregion

        #region Index / Metodos
        public ActionResult Index()
        {
            cl_filtros_Info model = new cl_filtros_Info();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            return View(model);
        }
        public ActionResult GridViewPartial_distribuion(DateTime? fecha_ini, DateTime? fecha_fin)
        {
            ViewBag.fecha_ini = fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : fecha_ini;
            ViewBag.fecha_fin = fecha_fin == null ? DateTime.Now.Date : fecha_fin;
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var model = bus_ing_inv.get_list(IdEmpresa,Convert.ToDateTime(fecha_ini),Convert.ToDateTime(fecha_fin));
            return PartialView("_GridViewPartial_distribuion", model);
        }
        private void cargar_combos(int IdEmpresa)
        {
            in_movi_inven_tipo_Bus bus_tipo = new in_movi_inven_tipo_Bus();
            var lst_tipo = bus_tipo.get_list(IdEmpresa, false);
            ViewBag.lst_tipo = lst_tipo;

            in_Motivo_Inven_Bus bus_motivo = new in_Motivo_Inven_Bus();
            var lst_motivo = bus_motivo.get_list(IdEmpresa, cl_enumeradores.eTipoIngEgr.ING.ToString(), false);
            ViewBag.lst_motivo = lst_motivo;

        }
        private bool validar(in_Ing_Egr_Inven_distribucion_Info i_validar, ref string msg)
        {

            if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.cm_fecha, cl_enumeradores.eModulo.INV, i_validar.IdSucursal, ref msg))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Grids

        [ValidateInput(false)]

        public ActionResult GridViewPartial_distribuidos(int IdSucursal=0, int IdMovi_inven_tipo=0, decimal IdNumMovi=0)
        {
            bus_ing_inv = new in_Ing_Egr_Inven_distribucion_Bus();
            List<in_Ing_Egr_Inven_distribucion_Info> model = new List<in_Ing_Egr_Inven_distribucion_Info>();
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            if (IdSucursal != 0 & IdMovi_inven_tipo != 0 & IdNumMovi != 0 & List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession)).Count() == 0)
            {
                model = bus_ing_inv.get_list_distribuido(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
               List_in_Ing_Egr_Inven_det.set_list( model, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            }
            else
            {
                model = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            }
            return PartialView("_GridViewPartial_distribuidos", model);
        }
        public ActionResult GridViewPartial_por_distribuir(int IdSucursal = 0, int IdMovi_inven_tipo = 0, decimal IdNumMovi = 0)
        {
            List<in_Ing_Egr_Inven_distribucion_Info> model = new List<in_Ing_Egr_Inven_distribucion_Info>();
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            if (IdSucursal != 0 & IdMovi_inven_tipo != 0 & IdNumMovi != 0 &lst_x_distribuir_lis.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession)).Count() == 0)
            {
                model = bus_ing_inv.get_list_x_distribuir(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
               lst_x_distribuir_lis.set_list(  model, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            }
            else
            {
                model = lst_x_distribuir_lis.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            }
            return PartialView("_GridViewPartial_por_distribuir", model);
        }
        public ActionResult GridViewPartial_distribucion_det()
        {
            if (SessionFixed.IdProducto_padre_dist != null)
                IdProducto_padre = Convert.ToDecimal(SessionFixed.IdProducto_padre_dist);
            cargar_combos_detalle(IdProducto_padre);
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.producto =Session["NombreProducto"];
            List<in_Ing_Egr_Inven_distribucion_Info> model = new List<in_Ing_Egr_Inven_distribucion_Info>();
            model = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            return PartialView("_GridViewPartial_distribucion_det", model);
        }
        private void cargar_combos_detalle(decimal IdProducto_padre=0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_producto = bus_producto.get_list_combo_hijo(IdEmpresa, IdProducto_padre);
            ViewBag.lst_producto = lst_producto;
            Session["list_productos"] = lst_producto;
            in_UnidadMedida_Bus bus_unidad = new in_UnidadMedida_Bus();
            var lst_unidad = bus_unidad.get_list(false);
            ViewBag.lst_unidad = lst_unidad;
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0 , int IdSucursal = 0, int IdMovi_inven_tipo = 0, decimal IdNumMovi = 0, string signo="")
        {
            
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            in_parametro_Info i_param = bus_in_param.get_info(IdEmpresa);
            if (i_param == null)
                return RedirectToAction("Index");

            in_Ing_Egr_Inven_distribucion_Info model = new in_Ing_Egr_Inven_distribucion_Info();
            model = bus_ing_inv.get_info(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi, signo);
            
            if (model == null)
            {
                model = new in_Ing_Egr_Inven_distribucion_Info();
                var info_mov = bus_ingreso.get_info(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
                model.IdEmpresa = info_mov.IdEmpresa;
                model.IdSucursal = info_mov.IdSucursal;
                model.IdBodega =Convert.ToInt32( info_mov.IdBodega);
             
             }
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(in_Ing_Egr_Inven_distribucion_Info model)
        {
            bus_ing_inv = new in_Ing_Egr_Inven_distribucion_Bus();
            model.lst_distribuido = List_in_Ing_Egr_Inven_det.get_list(model.IdTransaccionSession);
            model.lst_x_distribuir = lst_x_distribuir_lis.get_list(model.IdTransaccionSession);
            model.cm_fecha = DateTime.Now.Date;
            if (!validar(model, ref mensaje))
            {
                cargar_combos(model.IdEmpresa);
                ViewBag.mensaje = mensaje;
                return View(model);
            }
            model.IdUsuario = SessionFixed.IdUsuario.ToString();
            model.IdEmpresa =Convert.ToInt32( SessionFixed.IdEmpresa);
            if (!bus_ing_inv.guardarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
        
        #region update and delete fila distribuida
        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate_list_dis([ModelBinder(typeof(DevExpressEditorsBinder))] in_Ing_Egr_Inven_distribucion_Info info_det)
        {
            DateTime? lote_fecha_fab;
            DateTime? lote_fecha_vcto;
            string lote_num_lote = "";
            string pr_descripcion = "";
            string unidad_medida = "";
            in_Ing_Egr_Inven_distribucion_Info model = new in_Ing_Egr_Inven_distribucion_Info();
            if (ModelState.IsValid)
            {
                if(info_det.IdProducto!=0)
                {
                    if (Session["list_productos"] != null)
                    {
                        var list = Session["list_productos"] as List<in_Producto_Info>;
                        var info_ = list.Where(v => v.IdProducto == info_det.IdProducto).FirstOrDefault();
                        lote_fecha_fab = info_.lote_fecha_fab;
                        lote_fecha_vcto = info_.lote_fecha_vcto;
                        lote_num_lote = info_.lote_num_lote;
                        pr_descripcion = info_.pr_descripcion;
                        unidad_medida = info_.IdUnidadMedida;

                        info_det.lote_fecha_fab = lote_fecha_fab;
                        info_det.lote_fecha_vcto = lote_fecha_vcto;
                        info_det.lote_num_lote = lote_num_lote;
                        info_det.pr_descripcion = pr_descripcion;
                        info_det.IdUnidadMedida = unidad_medida;
                        List_in_Ing_Egr_Inven_det.UpdateRow(info_det);
                    }


                }
                model.lst_distribuido = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal( SessionFixed.IdTransaccionSession));

                // actualizar lista distribuidas
                lst_x_distribuir =lst_x_distribuir_lis.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));
                foreach (var item in lst_x_distribuir)
                {
                    item.can_distribuida = model.lst_distribuido.Sum(v => v.dm_cantidad);
                    item.can_x_distribuir = item.can_total - item.can_distribuida;

                }
                lst_x_distribuir_lis.set_list( lst_x_distribuir, Convert.ToDecimal(SessionFixed.IdTransaccionSession));
                if (SessionFixed.IdProducto_padre_dist != null)
                    IdProducto_padre = Convert.ToDecimal(SessionFixed.IdProducto_padre_dist);
                cargar_combos_detalle(IdProducto_padre);
            }
            return PartialView("_GridViewPartial_distribuidos", model.lst_distribuido);
        }

        public ActionResult EditingDelete_list_dis(int secuencia_distribucion)
        {
            List_in_Ing_Egr_Inven_det.DeleteRow(secuencia_distribucion);
            in_Ing_Egr_Inven_distribucion_Info model = new in_Ing_Egr_Inven_distribucion_Info();
            model.lst_distribuido = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            // actualizar lista distribuidas
            lst_x_distribuir = lst_x_distribuir_lis.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            foreach (var item in lst_x_distribuir)
            {
                item.can_distribuida = model.lst_distribuido.Sum(v => v.dm_cantidad);
                item.can_x_distribuir = item.can_total - item.can_distribuida;

            }
            lst_x_distribuir_lis.set_list(lst_x_distribuir, Convert.ToDecimal(SessionFixed.IdTransaccionSession));


            if (SessionFixed.IdProducto_padre_dist != null)
                IdProducto_padre = Convert.ToDecimal(SessionFixed.IdProducto_padre_dist);
            cargar_combos_detalle(IdProducto_padre);
            return PartialView("_GridViewPartial_distribuidos", model.lst_distribuido);
        }
        #endregion

        #region Funciones new, update, delete, agregar distribucion
        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] in_Ing_Egr_Inven_distribucion_Info info_det)
        {
            in_Ing_Egr_Inven_distribucion_Info model = new in_Ing_Egr_Inven_distribucion_Info();
            if (info_det.dm_cantidad > 0)
            {
                DateTime? lote_fecha_fab;
                DateTime? lote_fecha_vcto;
                string lote_num_lote = "";
                string pr_descripcion = "";
                string unidad_medida = "";
                if (ModelState.IsValid)
                {
                    if (Session["list_productos"] != null)
                    {
                        var list = Session["list_productos"] as List<in_Producto_Info>;
                        var info_ = list.Where(v => v.IdProducto == info_det.IdProducto).FirstOrDefault();
                        lote_fecha_fab = info_.lote_fecha_fab;
                        lote_fecha_vcto = info_.lote_fecha_vcto;
                        lote_num_lote = info_.lote_num_lote;
                        pr_descripcion = info_.pr_descripcion;
                        unidad_medida = info_.IdUnidadMedida;

                        info_det.lote_fecha_fab = lote_fecha_fab;
                        info_det.lote_fecha_vcto = lote_fecha_vcto;
                        info_det.lote_num_lote = lote_num_lote;
                        info_det.pr_descripcion = pr_descripcion;
                        info_det.IdUnidadMedida = unidad_medida;



                    }
                    // si existe
                    var pro = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession)).Where(v => v.IdProducto == info_det.IdProducto).FirstOrDefault();
                    double? cantidad_x_distibuir = lst_x_distribuir_lis.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession)).Where(v => v.IdProducto_padre == Convert.ToDecimal(SessionFixed.IdProducto_padre_dist)).FirstOrDefault().can_total;

                    if (pro == null)
                    {
                        if(cantidad_x_distibuir==null)
                        {
                            cantidad_x_distibuir = 0;
                        }
                        else
                        {
                            if (cantidad_x_distibuir < info_det.dm_cantidad)
                                info_det.dm_cantidad = Convert.ToDouble( cantidad_x_distibuir);
                        }
                        List_in_Ing_Egr_Inven_det.AddRow(info_det);
                    }
                    else
                    {
                        pro.dm_cantidad = pro.dm_cantidad + info_det.dm_cantidad;
                        List_in_Ing_Egr_Inven_det.UpdateRow(pro);
                    }

                    // actualizar lista distribuidas
                    lst_x_distribuir = lst_x_distribuir_lis.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession)).Where(v => v.IdProducto_padre == Convert.ToDecimal(SessionFixed.IdProducto_padre_dist)).ToList();
                    if (lst_x_distribuir != null)
                        foreach (var item in lst_x_distribuir)
                        {
                            item.can_distribuida = model.lst_distribuido.Sum(v => v.dm_cantidad);
                            item.can_x_distribuir = item.can_total - item.can_distribuida;
                        }
                    lst_x_distribuir_lis.set_list(lst_x_distribuir, Convert.ToDecimal(SessionFixed.IdTransaccionSession));
                    if (SessionFixed.IdProducto_padre_dist != null)
                        IdProducto_padre = Convert.ToDecimal(SessionFixed.IdProducto_padre_dist);
                    cargar_combos_detalle(IdProducto_padre);
                    ViewBag.producto = Session["NombreProducto"];

                }
            }
            model.lst_distribuido = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            return PartialView("_GridViewPartial_distribucion_det", model.lst_distribuido);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] in_Ing_Egr_Inven_distribucion_Info info_det)
        {
            in_Ing_Egr_Inven_distribucion_Info model = new in_Ing_Egr_Inven_distribucion_Info();
            if (info_det.dm_cantidad > 0)
            {
                DateTime? lote_fecha_fab;
                DateTime? lote_fecha_vcto;
                string lote_num_lote = "";
                string pr_descripcion = "";
                string unidad_medida = "";
                if (ModelState.IsValid)
                {
                    if (Session["list_productos"] != null)
                    {
                        var list = Session["list_productos"] as List<in_Producto_Info>;
                        var info_ = list.Where(v => v.IdProducto == info_det.IdProducto).FirstOrDefault();
                        lote_fecha_fab = info_.lote_fecha_fab;
                        lote_fecha_vcto = info_.lote_fecha_vcto;
                        lote_num_lote = info_.lote_num_lote;
                        pr_descripcion = info_.pr_descripcion;
                        unidad_medida = info_.IdUnidadMedida;

                        info_det.lote_fecha_fab = lote_fecha_fab;
                        info_det.lote_fecha_vcto = lote_fecha_vcto;
                        info_det.lote_num_lote = lote_num_lote;
                        info_det.pr_descripcion = pr_descripcion;
                        info_det.IdUnidadMedida = unidad_medida;



                    }
                    double? cantidad_x_distibuir = lst_x_distribuir_lis.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession)).Where(v => v.IdProducto_padre == Convert.ToDecimal(SessionFixed.IdProducto_padre_dist)).FirstOrDefault().can_total;

                    
                        if (cantidad_x_distibuir == null)
                        {
                            cantidad_x_distibuir = 0;
                        }
                        else
                        {
                            if (cantidad_x_distibuir < info_det.dm_cantidad)
                                info_det.dm_cantidad = Convert.ToDouble(cantidad_x_distibuir);
                        }
                        List_in_Ing_Egr_Inven_det.UpdateRow(info_det);
                    
                    // actualizar lista distribuidas
                    List<in_Ing_Egr_Inven_distribucion_Info> lst_x_distribuir;
                    lst_x_distribuir = lst_x_distribuir_lis.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession)).Where(v => v.IdProducto_padre == Convert.ToDecimal(SessionFixed.IdProducto_padre_dist)).ToList();
                    foreach (var item in lst_x_distribuir)
                    {
                        item.can_distribuida = model.lst_distribuido.Sum(v => v.dm_cantidad);
                        item.can_x_distribuir = item.can_total - item.can_distribuida;

                    }
                    lst_x_distribuir_lis.set_list(lst_x_distribuir, Convert.ToDecimal(SessionFixed.IdTransaccionSession));
                    if (SessionFixed.IdProducto_padre_dist != null)
                        IdProducto_padre = Convert.ToDecimal(SessionFixed.IdProducto_padre_dist);
                    cargar_combos_detalle(IdProducto_padre);
                }
            }
            model.lst_distribuido = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            return PartialView("_GridViewPartial_distribucion_det", model.lst_distribuido);
        }

        public ActionResult EditingDelete(int secuencia_distribucion)
        {
            List_in_Ing_Egr_Inven_det.DeleteRow(secuencia_distribucion);
            in_Ing_Egr_Inven_distribucion_Info model = new in_Ing_Egr_Inven_distribucion_Info();
            model.lst_distribuido = List_in_Ing_Egr_Inven_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            // actualizar lista distribuidas
            lst_x_distribuir = lst_x_distribuir_lis.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            foreach (var item in lst_x_distribuir)
            {
                item.can_distribuida = model.lst_distribuido.Sum(v => v.dm_cantidad);
                item.can_x_distribuir = item.can_total - item.can_distribuida;

            }
          lst_x_distribuir_lis.set_list( lst_x_distribuir,Convert.ToDecimal(SessionFixed.IdTransaccionSession));


            if (SessionFixed.IdProducto_padre_dist != null)
                IdProducto_padre = Convert.ToDecimal(SessionFixed.IdProducto_padre_dist);
            cargar_combos_detalle(IdProducto_padre);
            return PartialView("_GridViewPartial_distribucion_det", model.lst_distribuido);
        }
        #endregion

        #region Funciones Json

        public JsonResult Mostrar(decimal IdProducto_padre = 0)
        {
            var info_producto = lst_x_distribuir_lis.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession)).Where(v => v.IdProducto_padre == Convert.ToDecimal(SessionFixed.IdProducto_padre_dist)).FirstOrDefault();
            if (info_producto != null)
               Session["NombreProducto"]= info_producto.pr_descripcion+" total: "+info_producto.can_total+" Distribuida: "+info_producto.can_distribuida+" Por distribuir: "+info_producto.can_x_distribuir;
            SessionFixed.IdProducto_padre_dist =IdProducto_padre.ToString();
           
            cargar_combos_detalle(IdProducto_padre);

            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    #region clase personalizada
    public class in_Ing_Egr_Inven_distribucion_lst
    {
        string variable = "in_Ing_Egr_Inven_distribucion_Info";
        public List<in_Ing_Egr_Inven_distribucion_Info> get_list(decimal IdTransaccionSession)//Convert.ToDecimal(SesIdTransaccionSessionsionFixed.)
        {
            if (HttpContext.Current.Session[variable+IdTransaccionSession.ToString()] == null)
            {
                List<in_Ing_Egr_Inven_distribucion_Info> list = new List<in_Ing_Egr_Inven_distribucion_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_Ing_Egr_Inven_distribucion_Info>)HttpContext.Current.Session[variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_Ing_Egr_Inven_distribucion_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(in_Ing_Egr_Inven_distribucion_Info info_det)
        {
            List<in_Ing_Egr_Inven_distribucion_Info> list = get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            info_det.secuencia_distribucion = list.Count == 0 ? 1 : list.Max(q => q.secuencia_distribucion) + 1;
            info_det.IdProducto = info_det.IdProducto;
            info_det.IdUnidadMedida = info_det.IdUnidadMedida;
            info_det.mv_costo = info_det.mv_costo;
            info_det.dm_cantidad = info_det.dm_cantidad;

            list.Add(info_det);
        }

        public void UpdateRow(in_Ing_Egr_Inven_distribucion_Info info_det)
        {
            in_Ing_Egr_Inven_distribucion_Info edited_info = get_list(Convert.ToDecimal( SessionFixed.IdTransaccionSession)).Where(m => m.secuencia_distribucion == info_det.secuencia_distribucion).First();

            edited_info.IdProducto = info_det.IdProducto;
            edited_info.IdUnidadMedida = info_det.IdUnidadMedida;
            edited_info.mv_costo = info_det.mv_costo;
            edited_info.dm_cantidad = info_det.dm_cantidad;

        }

        public void DeleteRow(int secuencia_distribucion)
        {
            List<in_Ing_Egr_Inven_distribucion_Info> list = get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            list.Remove(list.Where(m => m.secuencia_distribucion == secuencia_distribucion).First());
        }
    }

  public class lst_x_distribuir
    {
        string variable = "lst_x_distribuir";
        public List<in_Ing_Egr_Inven_distribucion_Info> get_list(decimal IdTransaccionSession)//Convert.ToDecimal(SesIdTransaccionSessionsionFixed.)
        {
            if (HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_Ing_Egr_Inven_distribucion_Info> list = new List<in_Ing_Egr_Inven_distribucion_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_Ing_Egr_Inven_distribucion_Info>)HttpContext.Current.Session[variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_Ing_Egr_Inven_distribucion_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }

    }
    #endregion

}