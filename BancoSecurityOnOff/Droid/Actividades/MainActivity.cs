using System;
using System.Collections.Generic;
using System.Json;
using System.Net;
using System.Net.Security;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Net;
using Android.OS;
using Android.Telephony;
using Android.Widget;
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
	[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true,  NoHistory =true, 
              ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)] 
	
    public class MainActivity : Activity
    {
        static string jceNombre;
        static string jceRut;
        List<Contenido> tokenFCM;

        string idDispositivoTokenFCM;
        string idTokenFCM;
        String nombreArchivo = string.Empty;
        String rutaCarpeta = string.Empty;
        String ruta = string.Empty;
        MyFirebaseIIDService myFirebaseIIDService;
        ParametriaLogUtil parametriaLogUtil;

        const string relleno = "                                   ";
        const string RUT = "RUT NO SE CONOCE EN EL SPLASH";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(WebServiceSecurity.CertificadoSecurity.ValidateServerCertificate);
            base.OnCreate(savedInstanceState);
            if(!IsPlayServicesAvailable()){
                DialogoErrorActivity.mostrarViewError(this);
            }
             nombreArchivo = "app-bco_security_enrolamiento.sqlite";
             rutaCarpeta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
             ruta = System.IO.Path.Combine(rutaCarpeta, nombreArchivo);
             myFirebaseIIDService = new MyFirebaseIIDService();
             myFirebaseIIDService.OnTokenRefresh();
             parametriaLogUtil = new ParametriaLogUtil();
        }

        private bool isNetDisponible()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(Context.ConnectivityService);
            NetworkInfo actNetInfo = connectivityManager.ActiveNetworkInfo;
            return (actNetInfo != null && actNetInfo.IsConnected);
        }

		protected override void OnResume()
		{
            //DialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity(this);
            base.OnResume();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(WebServiceSecurity.CertificadoSecurity.ValidateServerCertificate);

                if (isNetDisponible())
                {
                    try
                    {
                       
                        Task startupWork = new Task(() =>
                        {
                            Task.Delay(3000);
                        });

                            startupWork.ContinueWith(async t => {
                            try
                             {
                              
                                JsonValue jsonResponseAccessToken = await WebServiceSecurity.ServiciosSecurity.CallRESTaccessToken();
                                JsonValue jt = jsonResponseAccessToken["access_token"];

                                idTokenFCM = MyFirebaseIIDService.SendRegistrationToServer();

                                var key = new Contenido()
                                {
                                    Password = idTokenFCM
                                };
                                string imei = this.getIMEI();
                                string idDispositivoLogConsultaEnrolado;
                                if (idTokenFCM != null)
                                {
                                    DatabaseHelper.Insertar(ref key, ruta);
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
                                        
                                }
                                else{
                                    idDispositivoTokenFCM = string.Empty;
                                }
                                idDispositivoLogConsultaEnrolado = parametriaLogUtil.getIdDispositivoParaLog(imei);
                                string ipDisposiivoLogConsultaEnrolado = ParametriaLogUtil.GetIpLocal();
                                string idDispositivoConsultaEnrolado = imei + relleno + idDispositivoTokenFCM;
                                       
                                JsonValue jsonResponseConsultaEnrrolado = await WebServiceSecurity.ServiciosSecurity.CallRESTConsultaEnrrolado(jt, idDispositivoConsultaEnrolado, SecurityEndpoints.SISTEMA_ANDROID, RUT, idDispositivoConsultaEnrolado, ipDisposiivoLogConsultaEnrolado);
                                string jce = jsonResponseConsultaEnrrolado["statusCode"];
                                if (jce.Equals("BusinessException"))
                                {
                                    Intent iLogin = new Intent(this, typeof(LoginActivity));
                                    StartActivity(iLogin);
                                    return;
                                }

                                jceNombre = jsonResponseConsultaEnrrolado["nombre"];
                                jceRut = jsonResponseConsultaEnrrolado["rut"];

                                if (jce.Equals("Success"))
                                {
                                    Intent iLoginConocido = new Intent(this, typeof(LoginConocidoActivity));
                                    StartActivity(iLoginConocido);
                                }
                              
                            }
                            catch (Exception xe)
                            {
                                Console.WriteLine(xe.ToString());
                                ExceptionGeneric();
                            }
                        }, TaskScheduler.FromCurrentSynchronizationContext());

                        startupWork.Start();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        ExceptionGeneric();
                    }
                }
                else
                {
                    ExceptionNetwork();
                }
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                Console.WriteLine("Sin conexion  a internet: "+ex);
                ExceptionNetwork();
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                ExceptionGeneric();
            }
        }

		protected override void OnStop()
		{
            base.OnStop();
		}

		protected override void OnDestroy()
		{
            base.OnDestroy();
		}
		protected override void OnPause()
		{
            base.OnPause();
		}

		protected override void OnStart()
		{
            base.OnStart();
		}

		protected override void OnRestart()
		{
			base.OnRestart();
		}

		public override void OnBackPressed(){
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Banco Security");
            builder.SetMessage("¿Está seguro que desea salir de la aplicación?");
            builder.SetPositiveButton("Aceptar", delegate { base.OnBackPressed(); });
            builder.SetNegativeButton("Cancelar", delegate { Finish(); });
            builder.Show();
		}

		public static string returnNombreConsultaEnrolado(){
            string nombre = jceNombre;
            return nombre;
        }

        public static string returnRutConsultaEnrolado(){
            string rut = jceRut;
            return rut; 
        }
        public String getIMEI()
        {
            TelephonyManager imei;
            imei = (TelephonyManager)GetSystemService(Context.TelephonyService);
            return imei.DeviceId;
        }

        public void ExceptionNetwork()
        {
            DialogoErrorActivity.mostrarViewError(this);
        }

        public void ExceptionGeneric()
        {

            DialogoErrorActivity.mostrarViewError(this);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode)){
                    // msgText.Text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                   // msgText.Text = "This device is not supported";
                    Finish();
                }
                return false;
            }
            else
            {
                return true;
            }
        }
	}
}