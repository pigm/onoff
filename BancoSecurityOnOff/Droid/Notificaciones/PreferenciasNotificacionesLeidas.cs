using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Preferences;
using BancoSecurityOnOff.Droid.Bean;
namespace BancoSecurityOnOff.Droid.Notificaciones
{
    public class PreferenciasNotificacionesLeidas{

        List<string> listaNotificacionesLeidas;
        const string PREFERENCIA_TOTAL_NOTIFICACIONES_LEIDAS = "PREFERENCIA_TOTAL_NOTIFICACIONES_LEIDAS";

        public PreferenciasNotificacionesLeidas(){
            listaNotificacionesLeidas = new List<string>();
        }

        public int getContadorNotificacionesNoLeidas(Activity activity ,List<Notificacion> listaNotificaciones)
        {
            if(listaNotificaciones != null && listaNotificaciones.Count != 0){
                return listaNotificaciones.Count - obtenerContadorNotificacionesNoLeidas(activity, listaNotificaciones);    
            }else{
                return 0;
            }
        }

        public int obtenerContadorNotificacionesNoLeidas(Activity activity, List<Notificacion> listaNotificaciones){
            int notificacionLeida = 0;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(activity);
            string preferenciaObtieneNotificacionesLeidas = prefs.GetString(PREFERENCIA_TOTAL_NOTIFICACIONES_LEIDAS, null);

            if (preferenciaObtieneNotificacionesLeidas == null)
            {
                return notificacionLeida;//listaNotificaciones.Count;// si no existe la preferencia , se envia el total 
            }else{
                List<String> listadoNotificacionesLeidas = obneterListadoDeNotificacionesLeidas(activity);
                foreach(var notificacionServicio in listaNotificaciones){
                    foreach(var idNotificacionLeida in listadoNotificacionesLeidas){
                        if (string.IsNullOrEmpty(notificacionServicio.idNotificacion))
                        {

                        }else{
                            if (notificacionServicio.idNotificacion.Equals(idNotificacionLeida))
                            {
                                notificacionLeida++;
                                break;
                            }
                        }
                    
                    }

                }
                return notificacionLeida;
            }

        }


        public string listarPreferencia(Activity activity,string listadoGuardado)
        {
            //listadoGuardado.ToCharArray
            return string.Empty;
        }

        public void guardarPreferencia(Activity activity, string idNotificacion)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(activity);
            ISharedPreferencesEditor editor = prefs.Edit();
                    List<String> listadoNotificacionesLeidas = obneterListadoDeNotificacionesLeidas(activity);
                    listadoNotificacionesLeidas.Add(idNotificacion);
                    string stringGuardar = string.Join(",", listadoNotificacionesLeidas);
                    editor.PutString(PREFERENCIA_TOTAL_NOTIFICACIONES_LEIDAS, stringGuardar);
                    editor.Apply();        // applies changes asynchronously on newer APIs
        }

        public List<String> obneterListadoDeNotificacionesLeidas(Activity activity)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(activity);
                string preferenciaObtieneNotificacionesLeidas = prefs.GetString(PREFERENCIA_TOTAL_NOTIFICACIONES_LEIDAS, null);
                if (preferenciaObtieneNotificacionesLeidas != null)
                {
                    listaNotificacionesLeidas = new List<string>(preferenciaObtieneNotificacionesLeidas.Split(',').Select(s => s));
                }
                return listaNotificacionesLeidas;
        }

        public bool notificacionesLeidas(Activity activity, string notificacionLeida){
            bool leida = false;
            List<String> listaLeidas = obneterListadoDeNotificacionesLeidas(activity);
            foreach (var notificacionLeidaEnLista in listaLeidas){
                if (notificacionLeida.Equals(notificacionLeidaEnLista)){
                    return true;
                }else{
                    return false;
                }
            }
            return leida;
        }
    }
}
