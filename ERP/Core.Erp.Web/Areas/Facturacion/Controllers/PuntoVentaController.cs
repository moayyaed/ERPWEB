using Core.Erp.Info.Facturacion;
using Core.Erp.Bus.Facturacion;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.General;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.Caja;
using Core.Erp.Info.General;
using Core.Erp.Info.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;
using Core.Info.Facturacion;
using Core.Bus.Facturacion;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    [SessionTimeout]
    public class PuntoVentaController : Controller
    {
        #region Index

        fa_PuntoVta_Bus bus_punto = new fa_PuntoVta_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        fa_PuntoVta_List Lista_PuntoVenta = new fa_PuntoVta_List();
        string mensaje = string.Empty;
        fa_PuntoVta_x_seg_usuario_List Lista_PtoVta_Usuario = new fa_PuntoVta_x_seg_usuario_List();
        seg_usuario_Bus bus_usuario = new seg_usuario_Bus();
        fa_PuntoVta_x_seg_usuario_Bus bus_punto_usuario = new fa_PuntoVta_x_seg_usuario_Bus();

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "PuntoVenta", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal =  Convert.ToInt32(SessionFixed.IdSucursal),
                CodDocumentoTipo = ""
            };
            cargar_combos_consulta();
            var lst = bus_punto.get_list(model.IdEmpresa, model.IdSucursal, model.CodDocumentoTipo, true);
            Lista_PuntoVenta.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "PuntoVenta", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            var lst = bus_punto.get_list(model.IdEmpresa, model.IdSucursal, model.CodDocumentoTipo, true);
            Lista_PuntoVenta.set_list(lst, model.IdTransaccionSession);
            cargar_combos_consulta();
            return View(model);
        }
        private void cargar_combos_consulta()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            tb_sis_Documento_Tipo_Bus bus_tipo = new tb_sis_Documento_Tipo_Bus();
            var lst_doc = bus_tipo.get_list(false);
            lst_doc.Add(new tb_sis_Documento_Tipo_Info
            {
                codDocumentoTipo = "",
                descripcion = "Todos"
            });
            ViewBag.lst_doc = lst_doc;

            tb_sucursal_Bus bus_suc = new tb_sucursal_Bus();
            var lst_sucursal = bus_suc.get_list(IdEmpresa, false);
            lst_sucursal.Add(new tb_sucursal_Info
            {
                IdSucursal = 0,
                Su_Descripcion = "Todos"
            });
            ViewBag.lst_sucursal = lst_sucursal;
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_puntoventa(bool Nuevo = false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //ViewBag.IdEmpresa = IdEmpresa;
            //ViewBag.IdSucursal = IdSucursal;
            //ViewBag.CodDocumentoTipo = CodDocumentoTipo;
            //List<fa_PuntoVta_Info> model = bus_punto.get_list(IdEmpresa, IdSucursal, CodDocumentoTipo, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_PuntoVenta.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_puntoventa", model);
        }
        private void cargar_combos( fa_PuntoVta_Info model)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.get_list(model.IdEmpresa, false);
            ViewBag.lst_sucursal = lst_sucursal;

            tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
            var lst_bodega = bus_bodega.get_list(model.IdEmpresa, model.IdSucursal, false);
            ViewBag.lst_bodega = lst_bodega;

            Dictionary<string, string> lst_signos = new Dictionary<string, string>();
            lst_signos.Add("-", "-");
            lst_signos.Add("+", "+");
            ViewBag.lst_signos = lst_signos;

            caj_Caja_Bus bus_caja = new caj_Caja_Bus();
            var lst_caja = bus_caja.get_list(model.IdEmpresa, false);
            ViewBag.lst_caja = lst_caja;

            tb_sis_Documento_Tipo_Bus bus_doc = new tb_sis_Documento_Tipo_Bus();
            var lst_doc = bus_doc.get_list(false);
            ViewBag.lst_doc = lst_doc;

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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "PuntoVenta", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_PuntoVta_Info model = new fa_PuntoVta_Info
            {
                IdEmpresa = IdEmpresa,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                lst_usuarios = new List<fa_PuntoVta_x_seg_usuario_Info>()
            };
            cargar_combos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(fa_PuntoVta_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            model.lst_usuarios = Lista_PtoVta_Usuario.get_list(model.IdTransaccionSession);
            if (!bus_punto.guardarDB(model))
            {
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdPuntoVta = model.IdPuntoVta, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdSucursal = 0, int IdPuntoVta = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            fa_PuntoVta_Info model = bus_punto.get_info(IdEmpresa, IdSucursal, IdPuntoVta);
            model.lst_usuarios = bus_punto_usuario.get_list(IdEmpresa, IdPuntoVta);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            Lista_PtoVta_Usuario.set_list(model.lst_usuarios, model.IdTransaccionSession);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "PuntoVenta", "Index");
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion


            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(model);
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0 , int IdSucursal = 0, int IdPuntoVta = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "PuntoVenta", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_PuntoVta_Info model = bus_punto.get_info(IdEmpresa,IdSucursal, IdPuntoVta);
            var lst = bus_punto_usuario.get_list(IdEmpresa, IdPuntoVta);
            model.lst_usuarios = new List<fa_PuntoVta_x_seg_usuario_Info>();
            model.lst_usuarios = bus_punto_usuario.get_list(IdEmpresa, IdPuntoVta);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            Lista_PtoVta_Usuario.set_list(model.lst_usuarios, model.IdTransaccionSession);
            if (model == null)
            return RedirectToAction("Index");
                cargar_combos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(fa_PuntoVta_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            model.lst_usuarios = Lista_PtoVta_Usuario.get_list(model.IdTransaccionSession);
            if (!bus_punto.modificarDB(model))
            {
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdPuntoVta = model.IdPuntoVta, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0 , int IdSucursal = 0, int IdPuntoVta = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "PuntoVenta", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            fa_PuntoVta_Info model = bus_punto.get_info(IdEmpresa, IdSucursal, IdPuntoVta);
            model.lst_usuarios = bus_punto_usuario.get_list(IdEmpresa, IdPuntoVta);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
                cargar_combos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(fa_PuntoVta_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
            model.lst_usuarios = Lista_PtoVta_Usuario.get_list(model.IdTransaccionSession);
            if (!bus_punto.anularDB(model))
            {
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Json
        public JsonResult cargar_bodega(int IdEmpresa = 0 , int IdSucursal = 0)
        {
            tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
            var resultado = bus_bodega.get_list(IdEmpresa, IdSucursal, false);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Metodos del detalle
        public ActionResult GridViewPartial_puntoventa_usuario()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            cargar_combos_detalle();
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_PtoVta_Usuario.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_puntoventa_usuario", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] fa_PuntoVta_x_seg_usuario_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            if (info_det != null)
                Lista_PtoVta_Usuario.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = Lista_PtoVta_Usuario.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            cargar_combos_detalle();
            return PartialView("_GridViewPartial_puntoventa_usuario", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] fa_PuntoVta_x_seg_usuario_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            if (info_det != null)
                Lista_PtoVta_Usuario.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            var model = Lista_PtoVta_Usuario.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_puntoventa_usuario", model);
        }

        public ActionResult EditingDelete(int Secuencia)
        {
            Lista_PtoVta_Usuario.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = Lista_PtoVta_Usuario.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();

            return PartialView("_GridViewPartial_puntoventa_usuario", model);
        }

        private void cargar_combos_detalle()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            var lst_responsable = bus_usuario.get_list(false);
            ViewBag.lst_responsable = lst_responsable;
        }

        private bool Validar(fa_PuntoVta_Info i_validar, ref string msg)
        {
            i_validar.lst_usuarios = Lista_PtoVta_Usuario.get_list(i_validar.IdTransaccionSession);

            foreach (var item1 in i_validar.lst_usuarios)
            {
                var contador = 0;
                foreach (var item2 in i_validar.lst_usuarios)
                {
                    if (item1.IdUsuario == item2.IdUsuario)
                    {
                        contador++;
                    }

                    if (contador > 1)
                    {
                        mensaje = "Existen usuarios repetidos en el detalle";
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion        
    }

    public class fa_PuntoVta_List
    {
        string Variable = "fa_PuntoVta_Info";
        public List<fa_PuntoVta_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_PuntoVta_Info> list = new List<fa_PuntoVta_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_PuntoVta_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_PuntoVta_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class fa_PuntoVta_x_seg_usuario_List
    {
        string Variable = "fa_PuntoVta_x_seg_usuario_Info";
        public List<fa_PuntoVta_x_seg_usuario_Info> get_list(decimal IdTransaccionSession)
        {

            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_PuntoVta_x_seg_usuario_Info> list = new List<fa_PuntoVta_x_seg_usuario_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_PuntoVta_x_seg_usuario_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_PuntoVta_x_seg_usuario_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(fa_PuntoVta_x_seg_usuario_Info info_det, decimal IdTransaccionSession)
        {
            List<fa_PuntoVta_x_seg_usuario_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            info_det.IdUsuario = info_det.IdUsuario;

            list.Add(info_det);
        }

        public void UpdateRow(fa_PuntoVta_x_seg_usuario_Info info_det, decimal IdTransaccionSession)
        {
            fa_PuntoVta_x_seg_usuario_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).First();
            edited_info.IdUsuario = info_det.IdUsuario;
        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<fa_PuntoVta_x_seg_usuario_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.Secuencia == Secuencia).FirstOrDefault());
        }
    }
}