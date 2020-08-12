using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.Banco;
using Core.Erp.Info.Banco;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.Banco.Controllers
{
    [SessionTimeout]
    public class TalonarioChequeBancoController : Controller
    {
        #region Variables
        ba_Talonario_cheques_x_banco_Bus bus_talonario = new ba_Talonario_cheques_x_banco_Bus();
        ba_Banco_Cuenta_Bus bus_bco_cuenta = new ba_Banco_Cuenta_Bus();
        ba_Banco_Cuenta_Bus bus_banco = new ba_Banco_Cuenta_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        ba_Talonario_cheques_x_banco_List Lista_TalonarioCheque = new ba_Talonario_cheques_x_banco_List();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TalonarioChequeBanco", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            ba_Talonario_cheques_x_banco_Info model = new ba_Talonario_cheques_x_banco_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal)
            };

            var lst = bus_talonario.get_list(model.IdEmpresa, model.IdSucursal, true);
            Lista_TalonarioCheque.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_talonario_cheque(bool Nuevo = false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //var model = bus_talonario.get_list(IdEmpresa,Convert.ToInt32(SessionFixed.IdSucursal), true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_TalonarioCheque.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_talonario_cheque", model);
        }

        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa, int IdSucursal )
        {
            var lst_banco = bus_banco.get_list(IdEmpresa,IdSucursal, false);
            ViewBag.lst_banco = lst_banco;
        }

        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TalonarioChequeBanco", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            ba_Talonario_cheques_x_banco_Info model = new ba_Talonario_cheques_x_banco_Info
            {
               IdEmpresa = IdEmpresa,               
               Estado_bool = true
            };
            cargar_combos(IdEmpresa, Convert.ToInt32(SessionFixed.IdSucursal));
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(ba_Talonario_cheques_x_banco_Info model)
        {
            decimal documento_inicial = Convert.ToDecimal(model.Num_cheque);
            decimal documento_final = Convert.ToDecimal(model.Documentofinal);
            int length = model.Num_cheque.Length;
            string relleno = string.Empty;
            for (int i = 0; i < length; i++)
            {
                relleno += "0";
            }
            decimal secuencia = documento_inicial;
            for (decimal i = documento_inicial; i < documento_final+1; i++)
            {

                ba_Talonario_cheques_x_banco_Info info = new ba_Talonario_cheques_x_banco_Info
                {
                    IdEmpresa = model.IdEmpresa,
                    IdBanco = model.IdBanco,
                    Num_cheque = secuencia.ToString(relleno),
                    Estado_bool = model.Estado_bool ,
                    Estado= model.Estado_bool == true ? "A" : "I",
                    Usado = model.Usado,
                };
                if (!bus_talonario.guardarDB(info))
                {
                    cargar_combos(model.IdEmpresa, Convert.ToInt32(SessionFixed.IdSucursal));
                    return View(model);
                }
                secuencia++;
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdBanco = model.IdBanco, Num_cheque = model.Num_cheque, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdBanco = 0, string Num_cheque = "", bool Exito = false)
        {
            ba_Talonario_cheques_x_banco_Info model = bus_talonario.get_info(IdEmpresa, IdBanco, Num_cheque);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TalonarioChequeBanco", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(IdEmpresa, Convert.ToInt32(SessionFixed.IdSucursal));
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdBanco = 0, string Num_cheque = "", bool Exito = false)
        {
            ba_Talonario_cheques_x_banco_Info model = bus_talonario.get_info(IdEmpresa, IdBanco, Num_cheque);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Banco", "TalonarioChequeBanco", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(IdEmpresa, Convert.ToInt32(SessionFixed.IdSucursal));
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(ba_Talonario_cheques_x_banco_Info model)
        {
            if(!bus_talonario.modificarDB(model))
            {
                cargar_combos(model.IdEmpresa, Convert.ToInt32(SessionFixed.IdSucursal));
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdBanco = model.IdBanco, Num_cheque = model.Num_cheque, Exito = true });
        }

        #endregion

        #region Json

        public JsonResult get_id(int IdEmpresa = 0, int IdBanco = 0)
        {
            var banco_cuenta = bus_bco_cuenta.get_info(IdEmpresa, IdBanco);
            var Numerocheque = bus_talonario.get_id(IdEmpresa, IdBanco, banco_cuenta.ba_num_digito_cheq);

            return Json(Numerocheque, JsonRequestBehavior.AllowGet);

        }

        public JsonResult get_num_x_bco(int IdEmpresa = 0, int IdBanco= 0)
        {
            var banco_cuenta = bus_bco_cuenta.get_info(IdEmpresa, IdBanco);
            string relleno = string.Empty;
            for (int i = 0; i < banco_cuenta.ba_num_digito_cheq; i++)
            {
                relleno += "0";
            }

            return Json(relleno, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }

    public class ba_Talonario_cheques_x_banco_List
    {
        string Variable = "ba_Talonario_cheques_x_banco_Info";
        public List<ba_Talonario_cheques_x_banco_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ba_Talonario_cheques_x_banco_Info> list = new List<ba_Talonario_cheques_x_banco_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ba_Talonario_cheques_x_banco_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ba_Talonario_cheques_x_banco_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}