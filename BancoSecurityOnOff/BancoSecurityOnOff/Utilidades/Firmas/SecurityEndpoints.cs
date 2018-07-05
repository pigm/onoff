using System;

/**
   @autor:          PaBlo Garrido
   @cargo:          Ingeniero de Software
   @institucion:    TInet soluciones informaticas
   @modificaciones: 
   @version:        1.0.0.0
   @proyecto:       OnOff
   
*/
namespace BancoSecurityOnOff.Utilidades.Firmas
{
    public static class SecurityEndpoints
    {
        //URL'S
        public const string URL_RECUPERAR_CLAVE = "https://www.bancosecurity.cl/security/AutenticacionPersonas/ingreso.asp";
        //ENDPOINTS
        public const string URL_ACCESSTOKEN = "https://190.243.30.34:5000/bc-desarrollo/sb/oauth2/token";
        public const string URL_LOGIN = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/Login";
        public const string URL_LLAMAR = "https://190.243.30.34:5000/bc-desarrollo/sb/api/Voz/Llamar";
        public const string URL_SMS = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/NotificarOTP";
        public const string URL_AUTENTICAR_CLAVEVOZ = "https://190.243.30.34:5000/bc-desarrollo/sb/api/Validacion/Autenticar";
        public const string URL_CONSULTAENROLADO = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/ConsultaEnrolado";
        public const string URL_ENROLARDISPOSITIVO = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/EnrolarDispositivo";
        public const string URL_CONSULTAPRODUCTONOTIFICACIONES = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/ConsultaProductosNotificaciones";
        public const string URL_GETCARDDATA = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/GetCardData";
        public const string URL_SETENABLE = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/SetEnabled";
        public const string URL_CHANGEPAYMENTCHANNELS = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/ChangePaymentChannels";
        public const string URL_CHECKCARDSTATUS = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/CheckCardStatus";
        public const string URL_ONOFFCARDSTATUS = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/OnOffCardStatus";
        public const string URL_CONSULTANOTIFICACIONES = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/ConsultaNotificaciones";
        public const string URL_INGRESAPRODUCTONOTIFICACIONES = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/IngresarProductosNotificaciones";
        public const string URL_INGRESANOTIFICACIONES = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/IngresarNotificaciones";
        public const string URL_CONSULTAHISTORIALNOTIFICACIONES = "https://190.243.30.34:5000/bc-desarrollo/sb/onoff/ConsultaHistorialNotificaciones";
        //Parametros unicos al login
        public const string userTypeWS_LOGIN = "User_1";
        public const string channelNameWS_LOGIN = "Banking Personas";
        //Parametros unicos para LLamada telefonica
        public const string STR_GRUPOIDG = "security";
        public const string STR_CODIGOTRANSACCION = "730";
        public const string SESSION_ID1 = "0";
        public const string SESSION_ID2 = "0";
        public const string ID_LOG = "0";
        //Parametros unicos para Autentificacion por Voz
        public const string A_GRUPOIDG = "security";
        public const string A_STR_IDMECANISMO = "OTP";
        public const string A_CODIGOTRANSACCION = "730";
        public const string A_STR_SESSIONID1 = "0";
        public const string A_STR_SESSIONID2 = "0";
        public const string A_IDLOG = "0";
        public const string A_STR_IP = "190.243.31.62";
        //Parametros unicos para consultar enrolado
        public const string SISTEMA_ANDROID = "ANDROID";
        public const string SISTEMA_IOS = "IOS";
        //Parametros unicos para enrolar un dispositivo
        public const string ED_SISTEMA = "@ONOFF";
        public const string ED_CANAL = "@7";
        public const string ED_ESTADO = "1";
        public const string ED_SISTEMA_ANDROID = "ANDROID";
        public const string ED_SISTEMA_IOS = "IOS";
        //Parametros unicos para consulta producto notificaciones
        public const string flagInicio = "S";
        public const string tamanoBloque = "93";
        public const string paginaConsultar = "1";
        public const string numeroRegistros = "0";
        public const string totalPaginas = "0";
        public const string ultimoRegistro = "0";
        //Parametros unicos para OnOffCardStatus
        public const string oceFi =  "0014";
        public const string oceAbaBranch = "04900";
        public const string oceAccountType = "AC";
        //Parametros unicos para CheckCardStatus
        public const string cceFi = "0014";
        public const string cceAbaBranch = "04900";
        public const string cceAccountType = "AC";
    }
}