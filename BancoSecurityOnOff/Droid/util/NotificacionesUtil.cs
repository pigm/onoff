using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Telephony;
using BancoSecurityOnOff.Droid.Bean;
using BancoSecurityOnOff.Droid.Util;
using BancoSecurityOnOff.Droid.WebServiceSecurity;
using BancoSecurityOnOff.Utilidades;
using Newtonsoft.Json.Linq;

namespace BancoSecurityOnOff.Droid
{
    public class NotificacionesUtil
    {
        Activity activity;
        List<Notificacion> notificaciones;
        ParametriaLogUtil parametriaLogUtil;
        string mitad1;
        string mitad2;
        string fechaHora;
        string fecha;
        string hora;
        string cadena;
        string runSinDV;
        string id;
        int mitad;
        
        const string codigoError = "faultcode";
        const string mensajeError = "message";
        const string indexAccessToken = "access_token";
        const string idHistorialNotificacion = "idNotificacion";
        const string indexRespuestaNotificaciones = "Notificaciones";
        const string cuerpoNotificacion = "mensajeNotificacion";
        const string IndexFechaHora = "fecha";
        const string stringPunto = ".";
        const string stringVacio = "";
        const string stringEspacio = " ";

        public NotificacionesUtil(Activity activity)
        {
            this.activity = activity;
            notificaciones = new List<Notificacion>();
            parametriaLogUtil = new ParametriaLogUtil();
            this.mitad1 = string.Empty;
            this.mitad2 = string.Empty;
            this.fechaHora = string.Empty;
            this.fecha = string.Empty;
            this.hora = string.Empty;
            this.cadena = string.Empty;
            this.runSinDV = string.Empty;
            this.id = string.Empty;
            this.mitad = 0;
        }

        public void setCortarCadenas(string cadena, ref string mitad1, ref string mitad2)
        {
            mitad = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cadena.Length / 2)));
            mitad1 = cadena.Substring(0, mitad);
            mitad2 = cadena.Substring(mitad, cadena.Length - mitad);
        } 

        public async Task<List<Bean.Notificacion>> Llamarservicio(Activity activity, string run)
        {
            JsonValue jsonResponseAccessToken = ServiciosSecurity.CallRESTToken();
            JsonValue jt = jsonResponseAccessToken[indexAccessToken];

            runSinDV = this.rutSinDV(run);
            JsonValue respuesta = await ServiciosSecurity.CallRESTConsultaHistorialNotificaciones(jt, runSinDV, run, parametriaLogUtil.getIdDispositivoParaLog(UtilAndroid.getIMEI(activity)), ParametriaLogUtil.GetIpLocal());

            if (Convert.ToString(JObject.Parse(respuesta.ToString())[codigoError]).Equals("1"))
            {
                //mostrar mensaje en la vista de notificaciones
                return null;
            }

            JsonValue notificacionMatriz = respuesta[indexRespuestaNotificaciones];
            JArray arreglo = JArray.Parse(notificacionMatriz.ToString());
            List<JToken> otherResults = arreglo.Children().ToList();

            int contadorNotificaciones = 0;
            foreach (var item in otherResults)
            {
                fechaHora = Convert.ToString(JObject.Parse(item.ToString())[IndexFechaHora]);
                fecha = Funcional.FormatearFecha(fechaHora);
                hora = Funcional.FormatearHora(fechaHora);
                cadena = Convert.ToString(JObject.Parse(item.ToString())[cuerpoNotificacion]);
                setCortarCadenas(cadena, ref mitad1, ref mitad2);

                notificaciones.Add(new Notificacion
                {
                    idNotificacion = Convert.ToString(JObject.Parse(item.ToString())[idHistorialNotificacion]),
                    id = contadorNotificaciones,
                    hora = hora,
                    fecha = fecha,
                    mensajeNotificacion = Convert.ToString(JObject.Parse(item.ToString())[cuerpoNotificacion]),
                    mensajeNotificacionCorto = mitad1 + " ....",
                });
                contadorNotificaciones++;
            }
            return notificaciones;
        }

        public string rutSinDV(string run){
            if (run.Trim().Length == 9)
            {
                run = run.Substring(0, 8);
            }
            else if (run.Trim().Length == 8)
            {
                run = run.Substring(0, 7);
            }
            return run;
        }

        public IEnumerable<IGrouping<string, Notificacion>> NotificacionesPorFecha()
        {
            var agrupacion = from Not in notificaciones group Not by Not.fecha into grupo select grupo;
            return agrupacion;
        }
    }
}
