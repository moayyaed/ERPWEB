using Core.Bus.General;
using Core.Erp.Bus.CuentasPorCobrar;
using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.General;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.CuentasPorCobrar;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Areas.ActivoFijo.Controllers;
using Core.Erp.Web.Helps;
using Core.Info.General;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.CuentasPorCobrar.Controllers
{
    public class SeguimientoCarteraController : Controller
    {
        #region Variables
        cxc_SeguimientoCartera_Bus bus_seguimiento = new cxc_SeguimientoCartera_Bus();
        fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
        string mensaje = string.Empty;
        string mensajeInfo = string.Empty;
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        cxc_SeguimientoCartera_List Lista_Seguimiento = new cxc_SeguimientoCartera_List();
        cxc_SeguimientoCartera_x_Alumno_List Lista_Seguimiento_x_Alumno = new cxc_SeguimientoCartera_x_Alumno_List();
        tb_ColaCorreo_Bus bus_cola_correo = new tb_ColaCorreo_Bus();
        cxc_cobro_Bus bus_cobros = new cxc_cobro_Bus();
        cxc_Parametro_Bus bus_parametros = new cxc_Parametro_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        public static UploadedFile file { get; set; }
        public static byte[] imagen { get; set; }
        public static byte[] seguimiento_foto { get; set; }
        #endregion

        #region Combos bajo demanada
        public ActionResult Cmb_Cliente()
        {
            cxc_SeguimientoCartera_Info model = new cxc_SeguimientoCartera_Info();
            return PartialView("_CmbCliente", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda_cliente(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda_cliente(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }
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

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdCliente = 0,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now
            };

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "SeguimientoCartera", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            var Lista = bus_seguimiento.getList(model.IdEmpresa, model.IdCliente, true, model.fecha_ini, model.fecha_fin);
            Lista_Seguimiento.set_list(Lista, model.IdTransaccionSession);

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "SeguimientoCartera", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            var Lista = bus_seguimiento.getList(model.IdEmpresa, model.IdCliente, true, model.fecha_ini, model.fecha_fin);
            Lista_Seguimiento.set_list(Lista, model.IdTransaccionSession);

            return View(model);
        }
        #endregion

        #region Grids
        [ValidateInput(false)]
        public ActionResult GridViewPartial_SeguimientoCartera(DateTime? Fecha_ini, DateTime? Fecha_fin, int IdAlumno = 0, bool Nuevo = false, bool Modificar = false, bool Anular = false)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.Fecha_ini = Fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(Fecha_ini);
            ViewBag.Fecha_fin = Fecha_fin == null ? DateTime.Now.Date : Convert.ToDateTime(Fecha_fin);

            ViewBag.Nuevo = Nuevo;
            ViewBag.Modificar = Modificar;
            ViewBag.Anular = Anular;

            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<cxc_SeguimientoCartera_Info> model = Lista_Seguimiento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_SeguimientoCartera", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_SeguimientoCarteraDet()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<cxc_SeguimientoCartera_Info> model = Lista_Seguimiento_x_Alumno.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_SeguimientoCarteraDet", model);
        }
        #endregion

        #region Json
        public JsonResult GetInfoCliente(int IdEmpresa = 0, decimal IdCliente = 0)
        {
            var info = bus_cliente.get_info(IdEmpresa, IdCliente);
            var lst_Saldos = bus_cobros.get_Saldo(IdEmpresa, IdCliente);
            info.Saldo = (lst_Saldos.Count == 0 ? 0 : Convert.ToDouble(lst_Saldos.Sum(q=>q.Saldo)));
            var Saldo = (lst_Saldos.Count == 0 ? 0 : Convert.ToDouble(lst_Saldos.Sum(q => q.Saldo)));

            var infoSeguimiento = new cxc_SeguimientoCartera_Info
            {
                IdEmpresa=IdEmpresa,
                IdCliente=IdCliente,
                Saldo = Saldo.ToString("C2"),
                Direccion = (info == null ? "" : info.Direccion),
                Celular = (info == null ? "" : info.Celular),
                Telefono = (info == null ? "" : info.Telefono),
                Correo = (info == null ? "" : info.Correo)
            };

            return Json(infoSeguimiento, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetList_x_Cliente(decimal IdTransaccionSession = 0, int IdEmpresa = 0, decimal IdCliente = 0)
        {
            var lst = bus_seguimiento.getList_x_Alumno(IdEmpresa, IdCliente);

            Lista_Seguimiento_x_Alumno.set_list(lst, IdTransaccionSession);

            return Json(lst.Count, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EnviarCorreo(int IdSeguimiento = 0)
        {
            string resultado = string.Empty;
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            var model = bus_seguimiento.get_info(IdEmpresa, IdSeguimiento);
            var info_cliente = bus_cliente.get_info(model.IdEmpresa, model.IdCliente);

            if (model != null)
            {
                var info_correo = new tb_ColaCorreo_Info
                {
                    IdEmpresa = IdEmpresa,
                    Asunto = "SEGUIMIENTO DE COBRANZA",
                    Cuerpo = model.Observacion,
                    Destinatarios = (info_cliente == null ? "" : info_cliente.Correo),
                    Codigo = "",
                    Parametros = IdEmpresa.ToString() + ";" + model.IdCliente.ToString(),
                    IdUsuarioCreacion = SessionFixed.IdUsuario
                };

                if (bus_cola_correo.GuardarDB(info_correo))
                {
                    model.IdUsuarioModificacion = SessionFixed.IdUsuario;
                    bus_seguimiento.EnviarCorreoDB(model);

                    resultado = "El correo fue enviado.";
                }
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Funciones imagen alumno
        public JsonResult nombre_imagen(decimal IdSeguimiento = 0)
        {
            try
            {
                if (IdSeguimiento == 0)
                    IdSeguimiento = bus_seguimiento.getId(Convert.ToInt32(SessionFixed.IdEmpresa));
                SessionFixed.NombreImagenSeguimiento = IdSeguimiento.ToString("000000");
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult get_imagen_general()
        {
            byte[] model = empresa_imagen.seguimiento_foto;
            if (model == null)
                model = new byte[0];
            return PartialView("_Empresa_imagen", model);
        }
        public class empresa_imagen
        {
            public static byte[] seguimiento_foto { get; set; }
            public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
            {
                AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png" },
                MaxFileSize = 4000000
            };
            public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
            {

                if (e.UploadedFile.IsValid)
                {
                    seguimiento_foto = e.UploadedFile.FileBytes;
                }
            }
        }
        public JsonResult actualizar_div()
        {
            return Json(SessionFixed.NombreImagenSeguimiento, JsonRequestBehavior.AllowGet);
        }
        public string UploadDirectory = "~/Content/imagenes/seguimiento/";
        public ActionResult DragAndDropImageUpload([ModelBinder(typeof(DragAndDropSupportDemoBinder))]IEnumerable<UploadedFile> ucDragAndDrop)
        {

            try
            {
                //Extract Image File Name.
                string fileName = System.IO.Path.GetFileName(ucDragAndDrop.FirstOrDefault().FileName);
                var IdEmpresa = Convert.ToString(SessionFixed.IdEmpresa).PadLeft(3, '0');
                //Set the Image File Path.
                UploadDirectory = UploadDirectory + IdEmpresa + SessionFixed.NombreImagenSeguimiento + ".jpg";
                imagen = ucDragAndDrop.FirstOrDefault().FileBytes;
                //Save the Image File in Folder.
                ucDragAndDrop.FirstOrDefault().SaveAs(Server.MapPath(UploadDirectory));
                SessionFixed.NombreImagenSeguimiento = UploadDirectory;

                file = ucDragAndDrop.FirstOrDefault();
                return Json(ucDragAndDrop.FirstOrDefault().FileBytes, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return View();
            }

        }

        #endregion

        #region Metodos
        private bool validar(cxc_SeguimientoCartera_Info info, ref string msg)
        {
            if (info.IdCliente == 0)
            {
                msg = "Debe seleccionar al estudiante";
                return false;
            }

            return true;
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
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "SeguimientoCartera", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            cxc_SeguimientoCartera_Info model = new cxc_SeguimientoCartera_Info
            {
                IdEmpresa = IdEmpresa,
                Fecha = DateTime.Now.Date,
                lst_det = new List<cxc_SeguimientoCartera_Info>(),
                seguimiento_foto = new byte[0],
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };

            Lista_Seguimiento_x_Alumno.set_list(model.lst_det, model.IdTransaccionSession);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(cxc_SeguimientoCartera_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario.ToString();
            var ListaDetalle = Lista_Seguimiento_x_Alumno.get_list(model.IdTransaccionSession);
            model.lst_det = ListaDetalle.ToList();

            if (model.seguimiento_foto == null)
                model.seguimiento_foto = new byte[0];

            if (!validar(model, ref mensaje))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                Lista_Seguimiento_x_Alumno.set_list(Lista_Seguimiento_x_Alumno.get_list(model.IdTransaccionSession), model.IdTransaccionSession);
                ViewBag.mensaje = mensaje;
                return View(model);
            }

            if (!bus_seguimiento.guardarDB(model))
            {
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                Lista_Seguimiento_x_Alumno.set_list(Lista_Seguimiento_x_Alumno.get_list(model.IdTransaccionSession), model.IdTransaccionSession);
                ViewBag.mensaje = "No se ha podido guardar el registro";

                return View(model);
            };

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSeguimiento = model.IdSeguimiento, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdSeguimiento = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            cxc_SeguimientoCartera_Info model = bus_seguimiento.get_info(IdEmpresa, IdSeguimiento);
            if (model == null)
                return RedirectToAction("Index");

            if (model.seguimiento_foto == null)
                model.seguimiento_foto = new byte[0];

            try
            {

                model.seguimiento_foto = System.IO.File.ReadAllBytes(Server.MapPath(UploadDirectory) + model.IdEmpresa.ToString("000") + model.IdSeguimiento.ToString("000000") + ".jpg");
                if (model.seguimiento_foto == null)
                    model.seguimiento_foto = new byte[0];
            }
            catch (Exception)
            {

                model.seguimiento_foto = new byte[0];
            }

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "SeguimientoCartera", "Index");
            if (model.Estado == false)
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion
            var infoCliente = bus_cliente.get_info(model.IdEmpresa, model.IdCliente);
            var lst_Saldos = bus_cobros.get_Saldo(model.IdEmpresa, model.IdCliente);
            var Saldo = (lst_Saldos.Count == 0 ? 0 : Convert.ToDouble(lst_Saldos.Sum(q => q.Saldo)));
            model.Saldo = Saldo.ToString("C2");
            model.Direccion = (infoCliente == null ? "" : infoCliente.Direccion);
            model.Celular = (infoCliente == null ? "" : infoCliente.Celular);
            model.Telefono = (infoCliente == null ? "" : infoCliente.Telefono);
            model.Correo = (infoCliente == null ? "" : infoCliente.Correo);

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdSeguimiento = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            cxc_SeguimientoCartera_Info model = bus_seguimiento.get_info(IdEmpresa, IdSeguimiento);
            if (model == null)
                return RedirectToAction("Index");

            var infoCliente = bus_cliente.get_info(model.IdEmpresa, model.IdCliente);
            var lst_Saldos = bus_cobros.get_Saldo(model.IdEmpresa, model.IdCliente);
            var Saldo = (lst_Saldos.Count == 0 ? 0 : Convert.ToDouble(lst_Saldos.Sum(q => q.Saldo)));
            model.Saldo = Saldo.ToString("C2");
            model.Direccion = (infoCliente == null ? "" : infoCliente.Direccion);
            model.Celular = (infoCliente == null ? "" : infoCliente.Celular);
            model.Telefono = (infoCliente == null ? "" : infoCliente.Telefono);
            model.Correo = (infoCliente == null ? "" : infoCliente.Correo);

            if (model.seguimiento_foto == null)
                model.seguimiento_foto = new byte[0];

            try
            {

                model.seguimiento_foto = System.IO.File.ReadAllBytes(Server.MapPath(UploadDirectory) + model.IdEmpresa.ToString("000") + model.IdSeguimiento.ToString("000000") + ".jpg");
                if (model.seguimiento_foto == null)
                    model.seguimiento_foto = new byte[0];
            }
            catch (Exception)
            {

                model.seguimiento_foto = new byte[0];
            }

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "CuentasPorCobrar", "SeguimientoCartera", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(cxc_SeguimientoCartera_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;

            if (model.seguimiento_foto == null)
                model.seguimiento_foto = new byte[0];

            if (!bus_seguimiento.anularDB(model))
            {
                ViewBag.mensaje = "No se ha podido anular el registro";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                return View(model);
            };

            return RedirectToAction("Index");
        }
        #endregion
    }

    public class cxc_SeguimientoCartera_List
    {
        string Variable = "cxc_SeguimientoCartera_Info";
        public List<cxc_SeguimientoCartera_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<cxc_SeguimientoCartera_Info> list = new List<cxc_SeguimientoCartera_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cxc_SeguimientoCartera_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cxc_SeguimientoCartera_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class cxc_SeguimientoCartera_x_Alumno_List
    {
        string Variable = "cxc_SeguimientoCartera_x_Alumno_Info";
        public List<cxc_SeguimientoCartera_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<cxc_SeguimientoCartera_Info> list = new List<cxc_SeguimientoCartera_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cxc_SeguimientoCartera_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cxc_SeguimientoCartera_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}