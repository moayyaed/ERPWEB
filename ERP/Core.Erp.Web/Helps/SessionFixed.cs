﻿using System.Web;

namespace Core.Erp.Web.Helps
{
    public interface ISessionValueProvider
    {
        string TipoPersona { get; set; }
        string IdEmpresa { get; set; }
        string IdUsuario { get; set; }
        string NomEmpresa { get; set; }
        string IdProducto_padre_dist { get; set; }
        string IdSucursal { get; set; }
        string IdEntidad { get; set; }
        string em_direccion { get; set; }
        string IdTransaccionSession { get; set; }
        string IdTransaccionSessionActual { get; set; }
        string IdNivelDescuento { get; set; }
        string NombreImagen { get; set; }
        string EsSuperAdmin { get; set; }
        string IdCaja { get; set; }
        string Ruc { get; set; }
        string IdDivision { get; set; }
        string IdArea { get; set; }
        string IdDivision_IC { get; set; }
        string Idtipo_cliente { get; set; }
        string IdSucursalInv { get; set; } //Para filtrar producto por bodega
        string IdBodegaInv { get; set; } //Para filtrar producto por bodega
        string IdPuntoGrupo { get; set; }
        string IdCiudad { get; set; }
        string IdNomina_Tipo { get; set; }
        string IdNomina_TipoLiqui { get; set; }
        string NombreImagenSeguimiento { get; set; }
    }

    public static class SessionFixed
    {
        private static ISessionValueProvider _sessionValueProvider;
        public static void SetSessionValueProvider(ISessionValueProvider provider)
        {
            _sessionValueProvider = provider;
        }

        public static string TipoPersona
        {
            get { return _sessionValueProvider.TipoPersona; }
            set { _sessionValueProvider.TipoPersona = value; }
        }
        public static string NombreImagen
        {
            get { return _sessionValueProvider.NombreImagen; }
            set { _sessionValueProvider.NombreImagen = value; }
        }
        public static string IdEmpresa
        {
            get { return _sessionValueProvider.IdEmpresa; }
            set { _sessionValueProvider.IdEmpresa = value; }
        }
        public static string NomEmpresa
        {
            get { return _sessionValueProvider.NomEmpresa; }
            set { _sessionValueProvider.NomEmpresa = value; }
        }
        public static string IdUsuario
        {
            get { return _sessionValueProvider.IdUsuario; }
            set { _sessionValueProvider.IdUsuario = value; }
        }

        public static string IdProducto_padre_dist
        {
            get { return _sessionValueProvider.IdProducto_padre_dist; }
            set { _sessionValueProvider.IdProducto_padre_dist = value; }

        }
        public static string IdNivelDescuento
        {
            get { return _sessionValueProvider.IdNivelDescuento; }
            set { _sessionValueProvider.IdNivelDescuento = value; }

        }
        public static string IdSucursal
        {
            get { return _sessionValueProvider.IdSucursal; }
            set { _sessionValueProvider.IdSucursal = value; }
        }
        public static string IdEntidad
        {
            get { return _sessionValueProvider.IdEntidad; }
            set { _sessionValueProvider.IdEntidad = value; }
        }
        public static string em_direccion
        {
            get { return _sessionValueProvider.em_direccion; }
            set { _sessionValueProvider.em_direccion = value; }
        }
        public static string IdTransaccionSession
        {
            get { return _sessionValueProvider.IdTransaccionSession; }
            set { _sessionValueProvider.IdTransaccionSession = value; }
        }
        public static string IdTransaccionSessionActual
        {
            get { return _sessionValueProvider.IdTransaccionSessionActual; }
            set { _sessionValueProvider.IdTransaccionSessionActual = value; }
        }
        public static string EsSuperAdmin
        {
            get { return _sessionValueProvider.EsSuperAdmin; }
            set { _sessionValueProvider.EsSuperAdmin = value; }
        }
        public static string IdCaja
        {
            get { return _sessionValueProvider.IdCaja; }
            set { _sessionValueProvider.IdCaja = value; }
        }
        public static string Ruc
        {
            get { return _sessionValueProvider.Ruc; }
            set { _sessionValueProvider.Ruc = value; }
        }
        
        public static string IdDivision
        {
            get { return _sessionValueProvider.IdDivision; }
            set { _sessionValueProvider.IdDivision = value; }
        }

        public static string IdArea
        {
            get { return _sessionValueProvider.IdArea; }
            set { _sessionValueProvider.IdArea = value; }
        }

        public static string IdDivision_IC
        {
            get { return _sessionValueProvider.IdDivision_IC; }
            set { _sessionValueProvider.IdDivision_IC = value; }
        }

        public static string Idtipo_cliente
        {
            get { return _sessionValueProvider.Idtipo_cliente; }
            set { _sessionValueProvider.Idtipo_cliente = value; }
        }

        public static string IdSucursalInv
        {
            get { return _sessionValueProvider.IdSucursalInv; }
            set { _sessionValueProvider.IdSucursalInv = value; }
        }

        public static string IdBodegaInv
        {
            get { return _sessionValueProvider.IdBodegaInv; }
            set { _sessionValueProvider.IdBodegaInv = value; }
        }

        public static string IdPuntoGrupo
        {
            get { return _sessionValueProvider.IdPuntoGrupo; }
            set { _sessionValueProvider.IdPuntoGrupo = value; }
        }

        public static string IdCiudad
        {
            get { return _sessionValueProvider.IdCiudad; }
            set { _sessionValueProvider.IdCiudad = value; }
        }

        public static string IdNomina_Tipo
        {
            get { return _sessionValueProvider.IdNomina_Tipo; }
            set { _sessionValueProvider.IdNomina_Tipo = value; }
        }

        public static string IdNomina_TipoLiqui
        {
            get { return _sessionValueProvider.IdNomina_TipoLiqui; }
            set { _sessionValueProvider.IdNomina_TipoLiqui = value; }
        }

        public static string NombreImagenSeguimiento
        {
            get { return _sessionValueProvider.NombreImagenSeguimiento; }
            set { _sessionValueProvider.NombreImagenSeguimiento = value; }
        }
        
    }

    public class WebSessionValueProvider : ISessionValueProvider
    {
        private const string _IdTipoPersona = "Fx_PERSONA";
        private const string _IdUsuario = "Fx_IdUsuario";
        private const string _IdEmpresa = "Fx_IdEmpresa";
        private const string _NomEmpresa = "Fx_FIXED";
        private const string _IdProducto_padre_dist = "Fx_IdProducto_padre_dist";
        private const string _IdEntidad = "Fx_IdEntidadParam";
        private const string _IdSucursal = "Fx_IdSucursal";
        private const string _em_direccion = "Fx_em_direccion";
        private const string _IdTransaccionSession = "Fx_IdTransaccionSesssion";
        private const string _IdTransaccionSessionActual = "Fx_IdTransaccionSessionActual";
        private const string _IdNivelDescuento = "Fx_IdNivelDescuento";
        private const string _NombreImagen = "Fx_NombreImagen";
        private const string _EsSuperAdmin = "Fx_EsSuperAdmin";
        private const string _IdCaja = "Fx_IdCaja";
        private const string _Ruc = "Fx_Ruc";
        private const string _IdDivision = "Fx_IdDivision";
        private const string _IdArea = "Fx_IdArea";
        private const string _IdDivision_IC = "Fx_IdDivision_IC";
        private const string _Idtipo_cliente = "Fx_Idtipo_cliente";
        private const string _IdSucursalInv = "Fx_IdSucursalInv";
        private const string _IdBodegaInv = "Fx_IdBodegaInv";
        private const string _IdPuntoGrupo = "Fx_IdPuntoGrupo";
        private const string _IdCiudad = "Fx_IdCiudad";
        private const string _IdNomina_Tipo = "Fx_IdNomina_Tipo";
        private const string _IdNomina_TipoLiqui = "Fx_IdNomina_TipoLiqui";
        private const string _NombreImagenSeguimiento = "Fx_NombreImagenSeguimiento";
        

        public string TipoPersona
        {
            get { return (string)HttpContext.Current.Session[_IdTipoPersona]; }
            set { HttpContext.Current.Session[_IdTipoPersona] = value; }
        }

        public string IdEmpresa
        {
            get { return (string)HttpContext.Current.Session[_IdEmpresa]; }
            set { HttpContext.Current.Session[_IdEmpresa] = value; }
        }
        public string IdUsuario
        {
            get { return (string)HttpContext.Current.Session[_IdUsuario]; }
            set { HttpContext.Current.Session[_IdUsuario] = value; }
        }
        public string NomEmpresa
        {
            get { return (string)HttpContext.Current.Session[_NomEmpresa]; }
            set { HttpContext.Current.Session[_NomEmpresa] = value; }
        }
        public string IdProducto_padre_dist
        {
            get { return (string)HttpContext.Current.Session[_IdProducto_padre_dist]; }
            set { HttpContext.Current.Session[_IdProducto_padre_dist] = value; }
        }
        public string IdSucursal
        {
            get { return (string)HttpContext.Current.Session[_IdSucursal]; }
            set { HttpContext.Current.Session[_IdSucursal] = value; }
        }
        public string IdEntidad
        {
            get { return (string)HttpContext.Current.Session[_IdEntidad]; }
            set { HttpContext.Current.Session[_IdEntidad] = value; }
        }
        public string em_direccion
        {
            get { return (string)HttpContext.Current.Session[_em_direccion]; }
            set { HttpContext.Current.Session[_em_direccion] = value; }
        }
        public string IdTransaccionSession
        {
            get { return (string)HttpContext.Current.Session[_IdTransaccionSession]; }
            set { HttpContext.Current.Session[_IdTransaccionSession] = value; }
        }
        public string IdTransaccionSessionActual
        {
            get { return (string)HttpContext.Current.Session[_IdTransaccionSessionActual]; }
            set { HttpContext.Current.Session[_IdTransaccionSessionActual] = value; }
        }
        public string IdNivelDescuento
        {
            get { return (string)HttpContext.Current.Session[_IdNivelDescuento]; }
            set { HttpContext.Current.Session[_IdNivelDescuento] = value; }
        }
        public string NombreImagen
        {
            get { return (string)HttpContext.Current.Session[_NombreImagen]; }
            set { HttpContext.Current.Session[_NombreImagen] = value; }
        }
        public string EsSuperAdmin
        {
            get { return (string)HttpContext.Current.Session[_EsSuperAdmin]; }
            set { HttpContext.Current.Session[_EsSuperAdmin] = value; }
        }
        public string IdCaja
        {
            get { return (string)HttpContext.Current.Session[_IdCaja]; }
            set { HttpContext.Current.Session[_IdCaja] = value; }
        }
        public string Ruc
        {
            get { return (string)HttpContext.Current.Session[_Ruc]; }
            set { HttpContext.Current.Session[_Ruc] = value; }
        }

        public string IdDivision
        {
            get { return (string)HttpContext.Current.Session[_IdDivision]; }
            set { HttpContext.Current.Session[_IdDivision] = value; }
        }

        public string IdArea
        {
            get { return (string)HttpContext.Current.Session[_IdArea]; }
            set { HttpContext.Current.Session[_IdArea] = value; }
        }

        public string IdDivision_IC
        {
            get { return (string)HttpContext.Current.Session[_IdDivision_IC]; }
            set { HttpContext.Current.Session[_IdDivision_IC] = value; }
        }

        public string Idtipo_cliente
        {
            get { return (string)HttpContext.Current.Session[_Idtipo_cliente]; }
            set { HttpContext.Current.Session[_Idtipo_cliente] = value; }
        }

        public string IdSucursalInv
        {
            get { return (string)HttpContext.Current.Session[_IdSucursalInv]; }
            set { HttpContext.Current.Session[_IdSucursalInv] = value; }
        }

        public string IdBodegaInv
        {
            get { return (string)HttpContext.Current.Session[_IdBodegaInv]; }
            set { HttpContext.Current.Session[_IdBodegaInv] = value; }
        }

        public string IdPuntoGrupo
        {
            get { return (string)HttpContext.Current.Session[_IdPuntoGrupo]; }
            set { HttpContext.Current.Session[_IdPuntoGrupo] = value; }
        }

        public string IdCiudad
        {
            get { return (string)HttpContext.Current.Session[_IdCiudad]; }
            set { HttpContext.Current.Session[_IdCiudad] = value; }
        }

        public string IdNomina_Tipo
        {
            get { return (string)HttpContext.Current.Session[_IdNomina_Tipo]; }
            set { HttpContext.Current.Session[_IdNomina_Tipo] = value; }
        }

        public string IdNomina_TipoLiqui
        {
            get { return (string)HttpContext.Current.Session[_IdNomina_TipoLiqui]; }
            set { HttpContext.Current.Session[_IdNomina_TipoLiqui] = value; }
        }
        
        public string NombreImagenSeguimiento
        {
            get { return (string)HttpContext.Current.Session[_NombreImagenSeguimiento]; }
            set { HttpContext.Current.Session[_NombreImagenSeguimiento] = value; }
        }
    }
}