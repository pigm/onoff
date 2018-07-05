using System;
using Android.App;
using Android.Content;
using Android.Telephony;

namespace BancoSecurityOnOff.Droid.Util
{
    public class UtilAndroid
    {
        static String rutDefinitivo;
        static TelephonyManager imei;

        public UtilAndroid()
        {
            rutDefinitivo = string.Empty;
            imei = null;
        }

        public static string getRut(){
            string rutConocido = MainActivity.returnRutConsultaEnrolado();
            string rutDesdeConfirmacion = ConfirmacionEnroladoActivity.returnRutConsultaEnroladoConocido();
            rutDefinitivo = rutConocido;

            if (string.IsNullOrEmpty(rutDefinitivo))
            {
                rutDefinitivo = rutDesdeConfirmacion;
            }
            return rutDefinitivo.Trim();
        }

        public static string getIMEI(Activity activity)
        {
            imei = (TelephonyManager)activity.GetSystemService(Context.TelephonyService);
            return imei.DeviceId;
        }
    }
}
