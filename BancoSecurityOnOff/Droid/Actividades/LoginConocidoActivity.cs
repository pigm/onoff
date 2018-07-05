using System;
using System.Json;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using BancoSecurityOnOff.Utilidades.Firmas;
using Android.Hardware.Fingerprints;
using Android.Support.V4.App;
using Android;
using BancoSecurityOnOff.Droid.WebServiceSecurity.HelperFinger;
using Javax.Crypto;
using Java.Security;
using Android.Security.Keystore;
using BancoSecurityOnOff.Utilidades.SQLiteDataBase;
using System.IO;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Telephony;
using Android.Views;
using Android.Graphics.Drawables;


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
    [Activity(Label = "LoginConocidoActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar",
	          ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)] 
	
    public class LoginConocidoActivity : Android.Support.V7.App.AppCompatActivity
    {
  
        TextView txtUsernameConocido;
        EditText txtClaveConocido;
        Button btnIniciaSesionConocido;
        TextView lblMensajeConocido;
        ImageView btnProblemasClaveConocido;
        TextInputLayout txtInputClaveConocido;
        View lineTxtClaveConocido;
        AnimationDrawable frameAnimation;
        ImageView btnAyudaLoginConocido;
        string rutConocido;
        string rutDesdeConfirmacion;
        string rutDefinitivo;

        string nombreConocido;
        string nombreDesdeConfirmacion;
        string nombreTitulo;
        string[] nombreUsuario;
        string primerNombreUsuario = string.Empty;
        string segundoNombreUsuario = string.Empty;

        private KeyStore keyStore;
        private Cipher cipher;
        private String KEY_NAME = null;
        ParametriaLogUtil parametriaLogUtil;
        string primerNombreFormateado;
        string segundoNombreFormateado;
        DialogoLoadingBcoSecurityActivity dialogoLoadingBcoSecurityActivity;
        /// <summary>
        /// Constantes de la clase
        /// </summary>
        private const string rutaFuenteTitiliumRegular = "fonts/titillium_web/TitilliumWeb-Regular.ttf";
        private const string rutaFuenteTitiliumSemiBold = "fonts/titillium_web/TitilliumWeb-SemiBold.ttf";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LoginConocido);
            nombreConocido = MainActivity.returnNombreConsultaEnrolado();
            nombreDesdeConfirmacion = ConfirmacionEnroladoActivity.returnNombreConsultaEnroladoConocido();

            nombreTitulo = nombreConocido;
            if (string.IsNullOrEmpty(nombreTitulo))
            {
                nombreTitulo = nombreDesdeConfirmacion;
            }

            string nombreLogin = nombreTitulo;

            nombreUsuario = nombreLogin.Split(' ');
            primerNombreUsuario = nombreUsuario[0].ToLower();
            segundoNombreUsuario = nombreUsuario[1].ToLower();

            parametriaLogUtil = new ParametriaLogUtil();
            string primeraLetraPrimerNombre = primerNombreUsuario.Substring(0, 1).ToUpper();
            string restoPrimernombreFormateado = primerNombreUsuario.Substring(1, primerNombreUsuario.Length - 1);
            primerNombreFormateado = primeraLetraPrimerNombre + restoPrimernombreFormateado;
            if (!string.IsNullOrEmpty(segundoNombreUsuario))
            {
                string primeraLetraSegundoNombre = segundoNombreUsuario.Substring(0, 1).ToUpper();
                string restoSegundonombreFormateado = segundoNombreUsuario.Substring(1, segundoNombreUsuario.Length - 1);
                segundoNombreFormateado = primeraLetraSegundoNombre + restoSegundonombreFormateado;
            }
            string nombreUserFormateado = primerNombreFormateado + " " +segundoNombreFormateado;

            rutConocido = MainActivity.returnRutConsultaEnrolado();
            rutDesdeConfirmacion = ConfirmacionEnroladoActivity.returnRutConsultaEnroladoConocido();
            rutDefinitivo = rutConocido;
            if (string.IsNullOrEmpty(rutDefinitivo))
            {
                rutDefinitivo = rutDesdeConfirmacion;
            }


            String nombreArchivo = "app-bco_security.sqlite";
            String rutaCarpeta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            String ruta = System.IO.Path.Combine(rutaCarpeta, nombreArchivo);
            KEY_NAME = DatabaseHelper.pass(ruta);

            KeyguardManager keyguardManager = (KeyguardManager)GetSystemService(KeyguardService);
            FingerprintManager fingerprintManager = (FingerprintManager)GetSystemService(FingerprintService);

            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.UseFingerprint) != (int)Android.Content.PM.Permission.Granted)
            {
                return;
            }

            if (!fingerprintManager.IsHardwareDetected)
            {
                //Toast.MakeText(ApplicationContext, "Lector de huella no habilitado", ToastLength.Long).Show();
            }
            else
            {

                if (!fingerprintManager.HasEnrolledFingerprints)
                {
                    //Toast.MakeText(ApplicationContext, "Debes enrolar tu huella a la app", ToastLength.Long).Show();
                }
                else
                {
                    if (!keyguardManager.IsKeyguardSecure)
                    {
                        Toast.MakeText(ApplicationContext, "No tienes habilitada la configuracion del scaner dactilar", ToastLength.Short).Show();
                    }
                    else
                    {
                        GenKey();

                    }

                    if (CipherInit())
                    {

                        FingerprintManager.CryptoObject cryptoObject = new FingerprintManager.CryptoObject(cipher);
                        FingerprintHandlerLogin helperLogin = new FingerprintHandlerLogin(this);
                        helperLogin.StartAuthentication(fingerprintManager, cryptoObject);
                    }
                }
            }

            txtUsernameConocido = FindViewById<TextView>(Resource.Id.txtUsernameConocido);
            txtUsernameConocido.Text = "Hola, " + nombreUserFormateado;
            txtClaveConocido = FindViewById<EditText>(Resource.Id.txtClaveConocido);
            lblMensajeConocido = FindViewById<TextView>(Resource.Id.lblMensajeConocido);
            txtInputClaveConocido = FindViewById<TextInputLayout>(Resource.Id.txtInputClaveConocido);

            btnIniciaSesionConocido = FindViewById<Button>(Resource.Id.btnIniciaSesionConocido);
            btnIniciaSesionConocido.Click += BtnIniciaSesionConocido_ClickAsync;

            btnProblemasClaveConocido = FindViewById<ImageView>(Resource.Id.btnProblemasClaveConocido);
            btnProblemasClaveConocido.Click += BtnProblemasClaveConocido_Click;
            txtClaveConocido.TextChanged += TxtClaveConocido_TextChanged;
            btnAyudaLoginConocido = FindViewById<ImageView>(Resource.Id.btnAyudaLoginConocido);


            var fontRegular = Typeface.CreateFromAsset(Assets, rutaFuenteTitiliumRegular);
            var fontSemiBold = Typeface.CreateFromAsset(Assets, rutaFuenteTitiliumSemiBold);
            txtUsernameConocido.Typeface = fontRegular;
            btnIniciaSesionConocido.Typeface = fontSemiBold;
            txtClaveConocido.Typeface = fontRegular;
            txtInputClaveConocido.Typeface = fontRegular;
            lblMensajeConocido.Typeface = fontRegular;
            lineTxtClaveConocido = FindViewById<View>(Resource.Id.lineTxtClaveConocido);
            dialogoLoadingBcoSecurityActivity = new DialogoLoadingBcoSecurityActivity(this);
        }

        void TxtClaveConocido_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (txtClaveConocido.RequestFocus())
            {
                lineTxtClaveConocido.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.security_line_sin_validacion);
                lblMensajeConocido.Text = string.Empty;
            }
        }



        private void BtnProblemasClaveConocido_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(RecuperarClaveActivity));
            StartActivity(i);
        }

        private async void BtnIniciaSesionConocido_ClickAsync(object sender, EventArgs e)
        {
            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            if (string.IsNullOrEmpty(txtClaveConocido.Text))
            {
                lblMensajeConocido.Text = "Clave incorrecta";
                lineTxtClaveConocido.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            }
            else if (txtClaveConocido.Text.Trim().Length < 8)
            {
                lineTxtClaveConocido.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                lblMensajeConocido.Text = "Clave incorrecta";
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            }
            else if (txtClaveConocido.Text.Trim().Length > 8)
            {
                lineTxtClaveConocido.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                lblMensajeConocido.Text = "Clave incorrecta";
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            }
            else
            {
                lblMensajeConocido.Text = string.Empty;
                try
                {
                    lineTxtClaveConocido.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.green);
                    string imei = this.getIMEI();
                    string idDispositivoLogLogin = parametriaLogUtil.getIdDispositivoParaLog(imei);
                    string ipDisposiivoLogLogin = ParametriaLogUtil.GetIpLocal();
                    JsonValue jsonResponseAccessToken = await WebServiceSecurity.ServiciosSecurity.CallRESTaccessToken();
                    JsonValue jt = jsonResponseAccessToken["access_token"];                

                    JsonValue jsonResponseLogin = await WebServiceSecurity.ServiciosSecurity.CallRESTlogin(jt, rutDefinitivo.Trim(), txtClaveConocido.Text, SecurityEndpoints.userTypeWS_LOGIN, SecurityEndpoints.channelNameWS_LOGIN, idDispositivoLogLogin, ipDisposiivoLogLogin);
                    string jl = jsonResponseLogin["statusCode"];

                    if (jl.Equals("Success"))
                    {
                        string operationResult = jsonResponseLogin["operationResult"];
                        if (operationResult.Equals("Success_1"))
                        {
                            Intent i = new Intent(this, typeof(HomeActivity));
                            StartActivity(i);
                        }else if(operationResult.Equals("ErrorInvalidUser_4") 
                                 || operationResult.Equals("ErrorUserDeleted_7")
                                 || operationResult.Equals("ErrorGenericError_8")
                                 || operationResult.Equals("ErrorDocumentNumberWithInvalidFormat_9")
                                 || operationResult.Equals("ErrorUserAndPasswordDoNotMatch_10")
                                 || operationResult.Equals("InvalidChannel_11")
                                 || operationResult.Equals("InvalidPassword_12")
                                 || operationResult.Equals("NotMatchUserAndUserType_13")
                                 || operationResult.Equals("ErrorCredentialNotAvailable_14")
                                 || operationResult.Equals("SuccessPendingValidatePIN_15")
                                ){
                         
                            txtClaveConocido.Text = string.Empty;
                            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                            lineTxtClaveConocido.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                            lblMensajeConocido.Text = "Estimado cliente, usuario ingresado es invalido. Por favor ingréselo nuevamente.";
                        }else if(operationResult.Equals("ErrorUserBlockedByPassword_5")
                                 || operationResult.Equals("ErrorUserBlockedByAdministrator_6")
                                 || operationResult.Equals("ErrorUserBlockedBySecurityQuestion_16")
                                 || operationResult.Equals("ErrorUserBlockedByAttemptsPinActivation_17")
                                ){
                            txtClaveConocido.Text = string.Empty;
                            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                            lineTxtClaveConocido.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                            lblMensajeConocido.Text = "Estimado Cliente, su clave se encuentra bloqueada. Por favor contáctese con el Servicio Atención Clientes Security +56 2 2222 2222.";
                        }
                        else{
                            ExceptionGeneric();
                        }
                     

                    }
                    else
                    {
                        DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                        lineTxtClaveConocido.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                        lblMensajeConocido.Text = "Error, debes ingresar tus credenciales de Banco Security";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    ExceptionGeneric();
                }
            }
        }

        public string returnNameFingerPrint()
        {
            String fingerprint = Android.OS.Build.Fingerprint;
            return "Fingerprint " + fingerprint;
        }

        private bool CipherInit()
        {
            try
            {
                cipher = Cipher.GetInstance(KeyProperties.KeyAlgorithmAes + "/" + KeyProperties.BlockModeCbc + "/" + KeyProperties.EncryptionPaddingPkcs7);
                keyStore.Load(null);
                IKey key = (IKey)keyStore.GetKey(KEY_NAME, null);
                cipher.Init(CipherMode.EncryptMode, key);
                return true;
            }
            catch (Exception)
            {
                ExceptionGeneric();
                return false;
            }
        }

        private void GenKey()
        {
            keyStore = KeyStore.GetInstance("AndroidKeyStore");
            KeyGenerator keyGenerator = null;
            keyGenerator = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, "AndroidKeyStore");
            keyStore.Load(null);
            keyGenerator.Init(new KeyGenParameterSpec.Builder(KEY_NAME, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt).SetBlockModes(KeyProperties.BlockModeCbc).SetUserAuthenticationRequired(true).SetEncryptionPaddings(KeyProperties.EncryptionPaddingPkcs7).Build());
            keyGenerator.GenerateKey();
        }

        public void ExceptionGeneric()
        {
            DialogoErrorActivity.mostrarViewErrorLoginEnrolado(this);
        }

        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

		public override void OnBackPressed()
		{
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Banco Security");
            builder.SetMessage("¿Está seguro que desea salir de la aplicación?");
            builder.SetPositiveButton("Aceptar", delegate { ExitAppSecurityGO(); });
            builder.SetNegativeButton("Cancelar", delegate { });
            builder.Show();
		}

        public void ExitAppSecurityGO()
        {
            Intent intent = new Intent(Intent.ActionMain);
            intent.AddCategory(Intent.CategoryHome);
            intent.SetFlags(ActivityFlags.NewTask);
            StartActivity(intent);
        }

        public String getIMEI()
        {
            TelephonyManager imei;
            imei = (TelephonyManager)GetSystemService(Context.TelephonyService);
            return imei.DeviceId;
        }
	}
}
