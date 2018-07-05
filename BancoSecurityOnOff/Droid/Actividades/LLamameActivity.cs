using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using BancoSecurityOnOff.Utilidades.Firmas;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Telephony;
using BancoSecurityOnOff.Droid.Util;

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
    /// <summary>
    /// Clase LLamameActivity
    /// </summary>
    [Activity(Label = "LLamameActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar",
	          ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)] 
	
    public class LLamameActivity : Activity
    {
        TextView txtParrafo1, txtParrafo2, txtParrafo3, txtParrafo4;
        Button btnLlamame;
        string rut;
        ParametriaLogUtil parametriaLogUtil;
        DialogoLoadingBcoSecurityActivity dialogoLoadingBcoSecurityActivity;

        private const string rutaFuenteTitiliumRegular = "fonts/titillium_web/TitilliumWeb-Regular.ttf";
        private const string rutaFuenteTitiliumSemiBold = "fonts/titillium_web/TitilliumWeb-SemiBold.ttf";
        private const string tituloDialogo = "Banco Security";
        private const string mensajeProgressDialogo = "LLamando ...";
        private const string tituloErrorDialogo = "Error";
        private const string btnAceptarDialogo = "Aceptar";
        private const string btnCancelarDialogo = "Cancelar";
        private const string mensajeDialogoOnBackPresed = "¿Está seguro que desea salir de la aplicación?";
        private const string mensajeDialogoErrorGenerico = "Se ha producido un error. Por favor intente más tarde.";
        private const string varAccesstoken = "access_token";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LLamame);
            btnLlamame = FindViewById<Button>(Resource.Id.btnLlamame);
            txtParrafo1 = FindViewById<TextView>(Resource.Id.txtParrafo1);
            txtParrafo2 = FindViewById<TextView>(Resource.Id.txtParrafo2);
            txtParrafo3 = FindViewById<TextView>(Resource.Id.txtParrafo3);
            txtParrafo4 = FindViewById<TextView>(Resource.Id.txtParrafo4);
            btnLlamame.Click += BtnLlamame_ClickAsync;

            var fontRegular = Typeface.CreateFromAsset(Assets, rutaFuenteTitiliumRegular);
            var fontSemiBold = Typeface.CreateFromAsset(Assets, rutaFuenteTitiliumSemiBold);
           
            btnLlamame.Typeface = fontSemiBold;
            txtParrafo1.Typeface = fontRegular;
            txtParrafo2.Typeface = fontSemiBold;
            txtParrafo3.Typeface = fontRegular;
            txtParrafo4.Typeface = fontRegular;
            parametriaLogUtil = new ParametriaLogUtil();
            dialogoLoadingBcoSecurityActivity = new DialogoLoadingBcoSecurityActivity(this);
        }

        private async void BtnLlamame_ClickAsync(object sender, EventArgs e)
        {
            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            try
            {
                rut = LoginActivity.returnRut();
                JsonValue jsonResponseAccessToken = await WebServiceSecurity.ServiciosSecurity.CallRESTaccessToken();
                JsonValue jt = jsonResponseAccessToken[varAccesstoken];
                JsonValue jsonResponseLLamar = await WebServiceSecurity.ServiciosSecurity.CallRESTLLamar(jt, SecurityEndpoints.STR_GRUPOIDG, rut, rut, SecurityEndpoints.STR_CODIGOTRANSACCION, SecurityEndpoints.SESSION_ID1, SecurityEndpoints.SESSION_ID2, SecurityEndpoints.ID_LOG, parametriaLogUtil.getIdDispositivoParaLog(UtilAndroid.getIMEI(this)), ParametriaLogUtil.GetIpLocal());         
                JsonValue statusCodeLlamada = jsonResponseLLamar["statusCode"];

                if (statusCodeLlamada == 0){
                    Intent i = new Intent(this, typeof(AutenticacionPorVozActivity));
                    StartActivity(i);
                }else{
                    DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                    DialogoErrorActivity.mostrarViewErrorLogin(this);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                DialogoErrorActivity.mostrarViewErrorLogin(this);
            }
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
        public String getIMEI()
        {
            TelephonyManager imei;
            imei = (TelephonyManager)GetSystemService(Context.TelephonyService);
            return imei.DeviceId;
        }
    }
}
