using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.Banco;
using Core.Erp.Info.Banco;
using Core.Erp.Bus.General;
using Core.Erp.Info.Helps;
using Core.Erp.Bus.Contabilidad;
using DevExpress.Web.Mvc;
using Core.Erp.Info.CuentasPorPagar;

namespace Core.Erp.Web.Areas.Banco.Controllers
{
    public class ArchivoBancarioController : Controller
    {
        #region Variables
        ba_Archivo_Transferencia_Bus bus_archivo = new ba_Archivo_Transferencia_Bus();
        ba_Archivo_Transferencia_Det_Bus bus_archivo_det = new ba_Archivo_Transferencia_Det_Bus();
        ba_Archivo_Transferencia_Det_List List_det = new ba_Archivo_Transferencia_Det_List();
        ba_Archivo_Transferencia_Det_List_op Lst_det_op = new ba_Archivo_Transferencia_Det_List_op();

        tb_banco_procesos_bancarios_x_empresa_Bus bus_procesos_bancarios = new tb_banco_procesos_bancarios_x_empresa_Bus();
        ba_Banco_Cuenta_Bus bus_cuentas_bancarias = new ba_Banco_Cuenta_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        string mensaje = string.Empty;
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();

        #endregion
        #region Index
        public ActionResult Index()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_archivo_bancario()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var model = bus_archivo.GetList(IdEmpresa, true);
            return PartialView("_GridViewPartial_archivo_bancario", model);
        }
        #endregion
        #region Metodos
        private bool validar(ba_Archivo_Transferencia_Info i_validar, ref string msg)
        {
            if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.Fecha, cl_enumeradores.eModulo.BANCO, i_validar.IdSucursal, ref msg))
            {
                return false;
            }

            var pro = bus_procesos_bancarios.get_info(i_validar.IdEmpresa, i_validar.IdProceso_bancario);
            i_validar.Cod_Empresa = pro.Codigo_Empresa;

            return true;

        }
        private void cargar_combos()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            
            var lst_cuenta_bancarias = bus_cuentas_bancarias.get_list(IdEmpresa, Convert.ToInt32(SessionFixed.IdSucursal), false);
            ViewBag.lst_cuenta_bancarias = lst_cuenta_bancarias;

            var lst_proceso = bus_procesos_bancarios.get_list(IdEmpresa, false);
            ViewBag.lst_proceso = lst_proceso;

            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, false);
            ViewBag.lst_sucursal = lst_sucursal;
            
        }

        #endregion
        #region Acciones

        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            ba_Archivo_Transferencia_Info model = new ba_Archivo_Transferencia_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                Fecha = DateTime.Now,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                Lst_det = new List<ba_Archivo_Transferencia_Det_Info>()
            };
            List_det.set_list(model.Lst_det, model.IdTransaccionSession);
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(ba_Archivo_Transferencia_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario;
            model.Lst_det = List_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos();
                return View(model);
            }
            if (!bus_archivo.GuardarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Modificar(int IdEmpresa = 0, decimal IdArchivo = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            ba_Archivo_Transferencia_Info model = bus_archivo.GetInfo(IdEmpresa, IdArchivo);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            model.Lst_det = bus_archivo_det.GetList(model.IdEmpresa, model.IdArchivo);
            List_det.set_list(model.Lst_det, model.IdTransaccionSession);
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ba_Archivo_Transferencia_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            model.Lst_det = List_det.get_list(model.IdTransaccionSession);
            if (!bus_archivo.ModificarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Anular(int IdEmpresa = 0, decimal IdArchivo = 0)
        {
            ba_Archivo_Transferencia_Info model = bus_archivo.GetInfo(IdEmpresa, IdArchivo);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ba_Archivo_Transferencia_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_archivo.AnularDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Detalle
        private void cargar_combos_Detalle()
        {
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_archivo_bancario_det()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            cargar_combos_Detalle();
            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_archivo_bancario_det", model);
        }
        public ActionResult GridViewPartial_archivo_bancario_det_op()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            cargar_combos_Detalle();
            var model = Lst_det_op.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_archivo_bancario_det_op", model);
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult EditingAddNew(string IDs = "", decimal IdTransaccionSession = 0, int IdEmpresa = 0)
        {
            if (IDs != "")
            {
                var Lst_det = Lst_det_op.get_list(IdTransaccionSession);
                string[] array = IDs.Split(',');
                foreach (var item in array)
                {
                    var info_det = Lst_det.Where(q => q.IdOrdenPago == Convert.ToInt32(item)).FirstOrDefault();
                    if (info_det != null)
                    {
                        List_det.AddRow(info_det, IdTransaccionSession);
                    }
                }
            }
            var model = List_det.get_list(IdTransaccionSession);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ba_Archivo_Transferencia_Det_Info info_det)
        {

            if (ModelState.IsValid)
                List_det.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_Detalle();
            return PartialView("_GridViewPartial_archivo_bancario_det", model);
        }
        public ActionResult EditingDelete(int Secuencia)
        {
            List_det.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = List_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_Detalle();
            return PartialView("_GridViewPartial_archivo_bancario_det", model);
        }
        #endregion

        public JsonResult GetListPorCruzar(int IdEmpresa = 0, decimal IdTransaccionSession = 0)
        {
            var lst = bus_archivo_det.get_list_con_saldo(IdEmpresa, 0, "", 0, "", "", Convert.ToInt32(SessionFixed.IdSucursal), false);
            Lst_det_op.set_list(lst, IdTransaccionSession);
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

    }
    public class ba_Archivo_Transferencia_Det_List
    {
        string Variable = "ba_Archivo_Transferencia_Det_Info";
        public List<ba_Archivo_Transferencia_Det_Info> get_list(decimal IdTransaccionSession)
        {

            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ba_Archivo_Transferencia_Det_Info> list = new List<ba_Archivo_Transferencia_Det_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ba_Archivo_Transferencia_Det_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ba_Archivo_Transferencia_Det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(ba_Archivo_Transferencia_Det_Info info_det, decimal IdTransaccionSession)
        {
            List<ba_Archivo_Transferencia_Det_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            if (list.Where(q => q.IdEmpresa == info_det.IdEmpresa).Count() == 0)
                list.Add(info_det);
        }

        public void UpdateRow(ba_Archivo_Transferencia_Det_Info info_det, decimal IdTransaccionSession)
        {
            ba_Archivo_Transferencia_Det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            if (edited_info != null)
            {
                edited_info.Valor = info_det.Valor;
            }
        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<ba_Archivo_Transferencia_Det_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.Secuencia == Secuencia).First());
        }
    }

    public class ba_Archivo_Transferencia_Det_List_op
    {
        string Variable = "ba_Archivo_Transferencia_Det_Info_op";
        public List<ba_Archivo_Transferencia_Det_Info> get_list(decimal IdTransaccionSession)
        {

            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<ba_Archivo_Transferencia_Det_Info> list = new List<ba_Archivo_Transferencia_Det_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<ba_Archivo_Transferencia_Det_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<ba_Archivo_Transferencia_Det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
    }