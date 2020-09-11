using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    [SessionTimeout]
    public class VendedorController : Controller
    {
        #region Variables
        fa_Vendedor_Bus bus_vendedor = new fa_Vendedor_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        fa_Vendedor_List Lista_Vendedor = new fa_Vendedor_List();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        #endregion

        #region Index
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Vendedor", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            fa_Vendedor_Info model = new fa_Vendedor_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_vendedor.get_list(model.IdEmpresa, true);
            Lista_Vendedor.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_vendedor(bool Nuevo = false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //var model = bus_vendedor.get_list(IdEmpresa, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Vendedor.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_vendedor", model);
        }

        #endregion

        #region Acciones

        public ActionResult Nuevo(int IdEmpresa = 0 )
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Vendedor", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_Vendedor_Info model = new fa_Vendedor_Info
            {
                IdEmpresa = IdEmpresa
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(fa_Vendedor_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario.ToString();
            if (!bus_vendedor.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdVendedor = model.IdVendedor, Exito = true });
        }
        public ActionResult Consultar(int IdEmpresa = 0, int IdVendedor = 0, bool Exito=false)
        {
            fa_Vendedor_Info model = bus_vendedor.get_info(IdEmpresa, IdVendedor);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Vendedor", "Index");
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
        public ActionResult Modificar(int IdEmpresa = 0 , int IdVendedor = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Vendedor", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_Vendedor_Info model = bus_vendedor.get_info(IdEmpresa, IdVendedor);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(fa_Vendedor_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario.ToString();
            if (!bus_vendedor.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdVendedor = model.IdVendedor, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0, int IdVendedor = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "Vendedor", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            fa_Vendedor_Info model = bus_vendedor.get_info(IdEmpresa, IdVendedor);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(fa_Vendedor_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario.ToString();
            if (!bus_vendedor.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

    }

    public class fa_Vendedor_List
    {
        string Variable = "fa_cliente_tipo_Info";
        public List<fa_Vendedor_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_Vendedor_Info> list = new List<fa_Vendedor_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_Vendedor_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_Vendedor_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}