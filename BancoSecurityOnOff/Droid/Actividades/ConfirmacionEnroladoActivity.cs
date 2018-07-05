using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using BancoSecurityOnOff.Droid.Util;
using BancoSecurityOnOff.Utilidades.Firmas;
using BancoSecurityOnOff.Utilidades.SQLiteDataBase;

/**
   @autor:          PaBlo Garrido
   @cargo:          Ingeniero de Software
   @institucion:    TInet soluciones informaticas
   @modificaciones: 
   @version:        1.0.0.0
   @proyecto:       OnOff
   
*/

namespace BancoSecurityOnOff.Droid
{
    [Activity(Label = "ConfirmacionEnroladoActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar",
	          ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)] 
    public class ConfirmacionEnroladoActivity : Activity
    {
        TextView lblIndicacion;
        TextView lblIndicacion1;
        TextView lblIndicacion2;
        TextView lblIndicacion3;
        Button btnIngresarLoginC;
        static string jceNombre;
        static string jceRut;
        List<Contenido> tokenFCM;
        string idDispositivoTokenFCM;
        ParametriaLogUtil parametriaLogUtil;
        DialogoLoadingBcoSecurityActivity dialogoLoadingBcoSecurityActivity;

        private const string rutaFuenteTitiliumRegular = "fonts/titillium_web/TitilliumWeb-Regular.ttf";
        private const string rutaFuenteTitiliumSemiBold = "fonts/titillium_web/TitilliumWeb-SemiBold.ttf";
        private const string tituloErrorDialogo = "Error";
        private const string btnAceptarDialogo = "Aceptar";
        const string relleno = "                                   ";
        private const string mensajeProgress = "Cargando On-Off...";
        private const string mensajeGenericError = "Se ha producido un error. Por favor intente más tarde.";
        private const string responseSuccess = "Success";
        private const string responseBusinessException = "BusinessException";
        private const string varStatusCode = "statusCode";
        private const string varAccesstoken = "access_token";
        private const string varConsultaEnroladoNombre = "nombre";
        private const string varConsultaEnroladoRut = "rut";
        private const string tituloDialogoSpinner = "Banco Security";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ConfirmacionEnrolado);
            lblIndicacion = FindViewById<TextView>(Resource.Id.lblIndicacion);
            lblIndicacion1 = FindViewById<TextView>(Resource.Id.lblIndicacion1);
            lblIndicacion2 = FindViewById<TextView>(Resource.Id.lblIndicacion2);
            lblIndicacion3 = FindViewById<TextView>(Resource.Id.lblIndicacion3);
            btnIngresarLoginC = FindViewById<Button>(Resource.Id.btnIngresarLoginC);
            btnIngresarLoginC.Click += btnIngresarLoginC_ClickAsync;

            var fontRegular = Typeface.CreateFromAsset(Assets, rutaFuenteTitiliumRegular);
            var fontSemiBold = Typeface.CreateFromAsset(Assets, rutaFuenteTitiliumSemiBold);
            btnIngresarLoginC.Typeface = fontSemiBold;
            lblIndicacion.Typeface = fontSemiBold;
            lblIndicacion1.Typeface = fontRegular;
            lblIndicacion2.Typeface = fontRegular;
            lblIndicacion3.Typeface = fontSemiBold;
            parametriaLogUtil = new ParametriaLogUtil();
            dialogoLoadingBcoSecurityActivity = new DialogoLoadingBcoSecurityActivity(this);
        }

        private async void btnIngresarLoginC_ClickAsync(object sender, EventArgs e)
        {
            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            try
            {
                String nombreArchivo = "app-bco_security_enrolamiento.sqlite";
                String rutaCarpeta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                String ruta = System.IO.Path.Combine(rutaCarpeta, nombreArchivo);

                tokenFCM = new List<Contenido>();
                tokenFCM = DatabaseHelper.sqliteTokenFCM(ruta);

                foreach (var token in tokenFCM)
                {
                    if (token.Id == 1)
                    {
                        idDispositivoTokenFCM = token.Password;
                        break;
                    }

                }
                string idDispositivoConsultaEnrolado = UtilAndroid.getIMEI(this) + relleno + idDispositivoTokenFCM;
                string rut = LoginActivity.returnRut();

                JsonValue jsonResponseAccessToken = await WebServiceSecurity.ServiciosSecurity.CallRESTaccessToken();
                JsonValue jt = jsonResponseAccessToken[varAccesstoken];
                JsonValue jsonResponseConsultaEnrrolado = await WebServiceSecurity.ServiciosSecurity.CallRESTConsultaEnrrolado(jt, idDispositivoConsultaEnrolado, SecurityEndpoints.SISTEMA_ANDROID, rut, idDispositivoConsultaEnrolado, ParametriaLogUtil.GetIpLocal());
                string jce = jsonResponseConsultaEnrrolado[varStatusCode];
                if (jce.Equals(responseBusinessException)){
                    DialogoErrorActivity.mostrarViewErrorLogin(this);
                    return;
                }
                jceNombre = jsonResponseConsultaEnrrolado[varConsultaEnroladoNombre];
                jceRut = jsonResponseConsultaEnrrolado[varConsultaEnroladoRut];

                if (jce.Equals(responseSuccess))
                {
                    Intent iLoginConocido = new Intent(this, typeof(LoginConocidoActivity));
                    StartActivity(iLoginConocido);
                    DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                }
            }
            catch (Exception xe)
            {
                Console.WriteLine(xe);
                ExceptionGeneric();
            }
        }

        public static string returnNombreConsultaEnroladoConocido(){
            return jceNombre;
        }

        public static string returnRutConsultaEnroladoConocido(){
            return jceRut;
        }

        public void ExceptionGeneric()
        {
            DialogoErrorActivity.mostrarViewErrorLogin(this);
        }

        public void principalView()
        {
            Intent i = new Intent(this, typeof(LoginActivity));
            StartActivity(i);
        }

        public override void OnBackPressed()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Banco Security");
            builder.SetMessage("¿Está seguro que desea comenzar nuevamente?");
            builder.SetPositiveButton("Aceptar", delegate { principalView(); });
            builder.SetNegativeButton("Cancelar", delegate { });
            builder.Show();
        }

    }
}
