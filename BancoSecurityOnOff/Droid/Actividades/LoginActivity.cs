using Android.App;
using Android.Widget;
using Android.OS;
using BancoSecurityOnOff.Utilidades.Firmas;
using System.Json;
using System.Net;
using System;
using System.Net.Security;
using Android.Content;
using Android.Telephony;
using Android.Graphics;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using System.Collections.Generic;
using BancoSecurityOnOff.Utilidades.SQLiteDataBase;
using System.Net.Sockets;
using Android.Views;

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
    [Activity(Label = "LoginActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar",
	          ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)] 
    
    public class LoginActivity : Android.Support.V7.App.AppCompatActivity
    {
        /// <summary>
        /// Variables de la clase
        /// </summary>
        static EditText txtUsername;
        static EditText txtClave;
        Button btnIniciaSesion;
        ImageView btnProblemasClave;
        TextView lblMensaje;
        ProgressDialog progress;
        TextInputLayout txtInputUsername;
        TextInputLayout txtInputClave;
        ParametriaLogUtil parametriaLogUtil;
        string idTokenFCM;
        String nombreArchivo = string.Empty;
        String rutaCarpeta = string.Empty;
        String ruta = string.Empty;
        MascarasTextWatcher mascarasTextWatcher;
        DialogoLoadingBcoSecurityActivity dialogoLoadingBcoSecurityActivity;
        View lineTxtUsername;
        View lineTxtClave;
        /// <summary>
        /// metodo onCreate de la vista
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(WebServiceSecurity.CertificadoSecurity.ValidateServerCertificate);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            txtUsername = FindViewById<EditText>(Resource.Id.txtUsername);
            txtClave = FindViewById<EditText>(Resource.Id.txtClave);
            lblMensaje = FindViewById<TextView>(Resource.Id.lblMensaje);
            txtInputUsername = FindViewById<TextInputLayout>(Resource.Id.txtInputUsername);
            txtInputClave = FindViewById<TextInputLayout>(Resource.Id.txtInputPassword);
            btnIniciaSesion = FindViewById<Button>(Resource.Id.btnIniciaSesion);

            btnIniciaSesion.Click += BtnIniciaSesion_Click;
            btnProblemasClave = FindViewById<ImageView>(Resource.Id.btnProblemasClave);
            btnProblemasClave.Click += BtnProblemasClave_Click;
            txtUsername.ImeOptions = Android.Views.InputMethods.ImeAction.Next;
            txtClave.ImeOptions = Android.Views.InputMethods.ImeAction.Done;

            var fontRegular = Typeface.CreateFromAsset(Assets, "fonts/titillium_web/TitilliumWeb-Regular.ttf");
            var fontSemiBold = Typeface.CreateFromAsset(Assets, "fonts/titillium_web/TitilliumWeb-SemiBold.ttf");
            txtUsername.Typeface = fontRegular;
            btnIniciaSesion.Typeface = fontSemiBold;
            txtClave.Typeface = fontRegular;
            txtInputClave.Typeface = fontRegular;
            lblMensaje.Typeface = fontRegular;
            txtInputUsername.Typeface = fontRegular;

            parametriaLogUtil = new ParametriaLogUtil();
            nombreArchivo = "app-bco_security_enrolamiento.sqlite";
            rutaCarpeta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            ruta = System.IO.Path.Combine(rutaCarpeta, nombreArchivo);

            mascarasTextWatcher = new MascarasTextWatcher(txtUsername, this,MascarasTextWatcher.TIPO_RUT);
            txtUsername.AddTextChangedListener(mascarasTextWatcher);
            txtUsername.TextChanged += TxtUsername_TextChanged;
            txtClave.TextChanged += TxtClave_TextChanged;
            lineTxtUsername = FindViewById<View>(Resource.Id.lineTxtUsername);
            lineTxtClave = FindViewById<View>(Resource.Id.lineTxtClave);
            dialogoLoadingBcoSecurityActivity = new DialogoLoadingBcoSecurityActivity(this);
        }

        void TxtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUsername.RequestFocus() && validarRut(txtUsername.Text))
            {
                lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.green);
            }
            else if (txtUsername.RequestFocus() && string.IsNullOrEmpty(txtUsername.Text))
            {
                lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.security_line_sin_validacion);
            }
            else
            {
                lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
            }
        }

        void TxtClave_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtClave.RequestFocus())
            {
                lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.security_line_sin_validacion);
                lblMensaje.Text = string.Empty;
            }
        }

         
        /// <summary>
        /// Metodo el cual contiene el evento click del boton el cual accede
        /// a la vista del banco cuando hay problemas de clave
        /// </summary>
        private void BtnProblemasClave_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(RecuperarClaveActivity));
            StartActivity(i);
        }

        /// <summary>
        /// metodo el cual contiene el evento click del boton ingresar.
        /// </summary>
        private async void BtnIniciaSesion_Click(object sender, EventArgs e)
        {
            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            if (string.IsNullOrEmpty(txtUsername.Text) && string.IsNullOrEmpty(txtClave.Text))
            {
                lblMensaje.Text = "Error al ingresar los datos";
                lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            }
            else if (validarRut(txtUsername.Text) == false && txtUsername.Text.Trim().Length >=1 && txtClave.Text.Trim().Length == 8)
            {
                lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.green);
                lblMensaje.Text = "Error al ingresar los datos";
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            }
      
            else if (txtClave.Text.Trim().Length >= 1 && txtClave.Text.Trim().Length < 8 && validarRut(txtUsername.Text) == true)
            {
                lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.green);
                lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                lblMensaje.Text = "Clave incorrecta";
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            }
            else if (string.IsNullOrEmpty(txtClave.Text) && validarRut(txtUsername.Text) == true)
            {
                lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.green);
                lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                lblMensaje.Text = "Clave incorrecta";
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            }
            else if (string.IsNullOrEmpty(txtUsername.Text) && txtClave.Text.Trim().Length == 8)
            {
                lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.green);

                lblMensaje.Text = "Error al ingresar los datos";
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            }
            else if (txtClave.Text.Trim().Length <= 8 && !validarRut(txtUsername.Text))
            {
                lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                lblMensaje.Text = "Error al ingresar los datos";
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            }

            else if (txtClave.Text.Trim().Length < 8 && validarRut(txtUsername.Text))
            {
                lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                lblMensaje.Text = "Clave incorrecta";
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            }
            else
            {
                lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.green);
                lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.green);
                //lblMensaje.Text = "Cargando datos...";

                try
                {
                    idTokenFCM = MyFirebaseIIDService.SendRegistrationToServer();
                    var key = new Contenido()
                    {
                        Password = idTokenFCM
                    };

                    if (idTokenFCM != null)
                    {
                        DatabaseHelper.Insertar(ref key, ruta);
                    }
                    string imei = this.getIMEI();
                    string idDispositivoLogLogin = parametriaLogUtil.getIdDispositivoParaLog(imei);
                    string ipDisposiivoLogLogin = ParametriaLogUtil.GetIpLocal();
                    JsonValue jsonResponseAccessToken = await WebServiceSecurity.ServiciosSecurity.CallRESTaccessToken();
                    JsonValue jt = jsonResponseAccessToken["access_token"];

                    string rutServicio = MascarasEditText.limpiaRut(txtUsername.Text);
                    JsonValue jsonResponseLogin = await WebServiceSecurity.ServiciosSecurity.CallRESTlogin(jt, rutServicio, txtClave.Text, SecurityEndpoints.userTypeWS_LOGIN, SecurityEndpoints.channelNameWS_LOGIN, idDispositivoLogLogin, ipDisposiivoLogLogin);
                    string jl = jsonResponseLogin["statusCode"];
                    if (jl.Equals("Success"))
                    {
                        string operationResult = jsonResponseLogin["operationResult"];
                        if (operationResult.Equals("Success_1"))
                        {
                            Intent i = new Intent(this, typeof(TerminoYCondicionesActivity));
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
                            CleaningEntryLogin();
                            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                            lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                            lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                            lblMensaje.Text = "Estimado cliente, usuario ingresado es invalido. Por favor ingréselo nuevamente.";
                        }else if(operationResult.Equals("ErrorUserBlockedByPassword_5")
                                 || operationResult.Equals("ErrorUserBlockedByAdministrator_6")
                                 || operationResult.Equals("ErrorUserBlockedBySecurityQuestion_16")
                                 || operationResult.Equals("ErrorUserBlockedByAttemptsPinActivation_17")
                                ){
                            CleaningEntryLogin();
                            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                            lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                            lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                            lblMensaje.Text = "Estimado Cliente, su clave se encuentra bloqueada. Por favor contáctese con el Servicio Atención Clientes Security +56 2 2222 2222.";
                        }
                        else{
                            ExceptionGeneric();
                        }
                    }
                    else
                    {
                        lineTxtUsername.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                        lineTxtClave.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.red);
                        lblMensaje.Text = "Error al ingresar los datos";
                        DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    ExceptionGeneric();
                }

            }

       }

        /// <summary>
        /// Metodo el cual contiene el algoritmo para validar el rut
        /// </summary>
        public bool validarRut(string rut)
        {
            
            bool validacion = false;
            try
            {
                rut = rut.Replace(".", "");
                rut = rut.Replace("-", "");
                int rutAux = int.Parse(rut.Substring(0, rut.Length - 1));

                char dv = char.Parse(rut.Substring(rut.Length - 1, 1));

                int m = 0, s = 1;
                for (; rutAux != 0; rutAux /= 10)
                {
                    s = (s + rutAux % 10 * (9 - m++ % 6)) % 11;
                }
                if (dv == (char)(s != 0 ? s + 47 : 75))
                {
                    validacion = true;
                }
                if (dv == (char)(s != 0 ? s + 47 : 107))
                {
                    validacion = true;
                }
            }
            catch (Exception)
            {
                validacion = false;
            }
            return validacion;
        }

        /// <summary>
        /// Metodo que retorna el rut
        /// </summary>
        public static string returnRut()
        {
            string rut = MascarasEditText.limpiaRut(txtUsername.Text);

            return rut;
        }

        public override void OnBackPressed()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Banco Security");
            builder.SetMessage("¿Está seguro que desea salir de la aplicación?");
            builder.SetPositiveButton("Aceptar", delegate { base.OnBackPressed(); });
            builder.SetNegativeButton("Cancelar", delegate { });
            builder.Show();
        }

        public void ExceptionGeneric()
        {
            DialogoErrorActivity.mostrarViewErrorLogin(this);
        }

        /// <summary>
        /// Metodo que retorna la password
        /// </summary>
        public static string returnPasswordLogin()
        {
            string clave = txtClave.Text;

            return clave;
        }

        private void CleaningEntryLogin()
        {
            //txtUsername.Text = "";
            txtClave.Text = "";
        }

        public String getIMEI()
        {
            TelephonyManager imei;
            imei = (TelephonyManager)GetSystemService(Context.TelephonyService);
            return imei.DeviceId;
        }

    }
}