using System;
using System.Globalization;

namespace BancoSecurityOnOff.Utilidades
{
    public static class Funcional{
        static string fecha = string.Empty;
        static string meridiano = string.Empty;
        static DateTime fechaNotificacion = DateTime.Now;
        static int largoStr = 0;
        const string stringPunto = ".";
        const string stringEspacio = " ";

        public static string FormatearFecha(string fechaHora){
            if (string.IsNullOrEmpty(fechaHora)){
                //Sin notificaciones por fecha corrupta.
            }
            else{
                fechaHora = fechaHora.Replace(stringPunto, string.Empty);
                largoStr = fechaHora.Length;
                meridiano = fechaHora.Substring(largoStr - 3, 3).Trim();
                fechaHora = fechaHora.Replace(fechaHora.Substring(largoStr - 3, 3), meridiano.Replace(stringEspacio, string.Empty).ToUpper());
                fechaNotificacion = DateTime.Parse(fechaHora, CultureInfo.CurrentCulture);
                fecha = ToShortTimeString(fechaNotificacion);
            }
            return fecha;
        }

        public static string FormatearHora(string fechaHora){
            string hora = string.Empty;
            if (string.IsNullOrEmpty(fechaHora)){
                //sin notificaciones por hora corrupta
            }else{
                fechaHora = fechaHora.Replace(stringPunto, string.Empty);
                largoStr = fechaHora.Length;
                meridiano = fechaHora.Substring(largoStr - 3, 3).Trim();
                fechaHora = fechaHora.Replace(fechaHora.Substring(largoStr - 3, 3), meridiano.Replace(stringEspacio, string.Empty).ToUpper());
                fechaNotificacion = DateTime.Parse(fechaHora, CultureInfo.CurrentCulture);
                fecha = ToShortTimeString(fechaNotificacion);
                hora = fechaNotificacion.ToString().Replace(fecha, string.Empty);
                hora = hora.Replace(meridiano, string.Empty);
            }
            return hora;
        }

        private static string ToShortTimeString(this DateTime dateTime){
            return dateTime.ToString("d", DateTimeFormatInfo.CurrentInfo);
        }
    }
}