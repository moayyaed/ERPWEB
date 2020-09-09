using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Info.Inventario;
using Core.Erp.Bus.Inventario;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    [SessionTimeout]
    public class ProductoTipoController : Controller
    {
        #region Index

        in_ProductoTipo_Bus bus_producto_tipo = new in_ProductoTipo_Bus();
        in_ProductoTipo_List Lista_ProductoTipo = new in_ProductoTipo_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "ProductoTipo", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            in_ProductoTipo_Info model = new in_ProductoTipo_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_producto_tipo.get_list(model.IdEmpresa, true);
            Lista_ProductoTipo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_tipo_producto(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_ProductoTipo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_tipo_producto", model);
        }
        #endregion
        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "ProductoTipo", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            in_ProductoTipo_Info model = new in_ProductoTipo_Info
            {
                IdEmpresa = IdEmpresa
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(in_ProductoTipo_Info model)
        {
            if (!bus_producto_tipo.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdProductoTipo = model.IdProductoTipo, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdProductoTipo = 0, bool Exito=false)
        {
            in_ProductoTipo_Info model = bus_producto_tipo.get_info(IdEmpresa, IdProductoTipo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "ProductoTipo", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0 , int IdProductoTipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "ProductoTipo", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            in_ProductoTipo_Info model = bus_producto_tipo.get_info(IdEmpresa,IdProductoTipo);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(in_ProductoTipo_Info model)
        {
            if (!bus_producto_tipo.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdProductoTipo = model.IdProductoTipo, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0 , int IdProductoTipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "ProductoTipo", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            in_ProductoTipo_Info model = bus_producto_tipo.get_info(IdEmpresa, IdProductoTipo);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(in_ProductoTipo_Info model)
        {
            if (!bus_producto_tipo.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
        #region json
        public JsonResult get_info_producto_tipo( int IdProductoTipo = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            in_ProductoTipo_Bus bus_producto_tipo = new in_ProductoTipo_Bus();
            var resultado = bus_producto_tipo.get_info(IdEmpresa, IdProductoTipo);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class in_ProductoTipo_List
    {
        string Variable = "in_ProductoTipo_Info";
        public List<in_ProductoTipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_ProductoTipo_Info> list = new List<in_ProductoTipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_ProductoTipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_ProductoTipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}