using System;
using System.Collections.Generic;
using System.Json;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Text.Style;
using BancoSecurityOnOff.Utilidades.Firmas;
using Android.Util;
using BancoSecurityOnOff.Utilidades.SQLiteDataBase;
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
    [Activity(Label = "AutenticacionPorVozActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar",
              ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AutenticacionPorSMSActivity : Activity
    {
        TextView lblMsjClaveVoz;
        EditText txtClaveVoz;
        EditText txtClaveVozSegundo;
        EditText txtClaveVozTercero;
        EditText txtClaveVozCuarto;
        EditText txtClaveVozQuinto;
        EditText txtClaveVozSexto;
        TextView txtInstruccionAutenticacion;
        TextView lblCodigoSubTitulo;
        Button btnAutenticarClaveVoz;
        TextView btnReLLamada;
        string rut;
        List<Contenido> tokenFCM;
        string idDispositivoTokenFCM;
        SMSBroadcast smsReceiver;
        ParametriaLogUtil parametriaLogUtil;
        Typeface fontRegular;
        Typeface fontSemiBold;
        DialogoLoadingBcoSecurityActivity dialogoLoadingBcoSecurityActivity;
        View lineTxtClaveVoz;
        View lineTxtClaveVozSegundo;
        View lineTxtClaveVozTercero;
        View lineTxtClaveVozCuarto;
        View lineTxtClaveVozQuinto;
        View lineTxtClaveVozSexto;

        const string relleno = "                                   ";
        const string TEXTO_NO_RECIBI_SMS = "No recibí el SMS, volver a enviar";
        const string TEXTO_NO_TITULO_SMS = "Para activar tu Security On,\ningresa acá el código de verificación que te acabamos de enviar vía SMS.";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            registrarListenerSMS();
            SetContentView(Resource.Layout.AutenticacionPorSMS);
            lblMsjClaveVoz = FindViewById<TextView>(Resource.Id.lblMsjClaveVoz);
            txtClaveVoz = FindViewById<EditText>(Resource.Id.txtClaveVoz);
            txtClaveVozSegundo = FindViewById<EditText>(Resource.Id.txtClaveVozSegundo);
            txtClaveVozTercero = FindViewById<EditText>(Resource.Id.txtClaveVozTercero);
            txtClaveVozCuarto = FindViewById<EditText>(Resource.Id.txtClaveVozCuarto);
            txtClaveVozQuinto = FindViewById<EditText>(Resource.Id.txtClaveVozQuinto);
            txtClaveVozSexto = FindViewById<EditText>(Resource.Id.txtClaveVozSexto);
            lineTxtClaveVoz = FindViewById<View>(Resource.Id.lineTxtClaveVoz);
            lineTxtClaveVozSegundo = FindViewById<View>(Resource.Id.lineTxtClaveVozSegundo);
            lineTxtClaveVozTercero = FindViewById<View>(Resource.Id.lineTxtClaveVozTercero);
            lineTxtClaveVozCuarto = FindViewById<View>(Resource.Id.lineTxtClaveVozCuarto);
            lineTxtClaveVozQuinto = FindViewById<View>(Resource.Id.lineTxtClaveVozQuinto);
            lineTxtClaveVozSexto = FindViewById<View>(Resource.Id.lineTxtClaveVozSexto);
            txtClaveVoz.TextChanged += TxtClaveVoz_TextChanged;
            txtClaveVozSegundo.TextChanged += TxtClaveVozSegundo_TextChanged;
            txtClaveVozTercero.TextChanged += TxtClaveVozTercero_TextChanged;
            txtClaveVozCuarto.TextChanged += TxtClaveVozCuarto_TextChanged;
            txtClaveVozQuinto.TextChanged += TxtClaveVozQuinto_TextChanged;
            txtClaveVozSexto.TextChanged += TxtClaveVozSexto_TextChanged;
            txtInstruccionAutenticacion = FindViewById<TextView>(Resource.Id.txtInstruccionAutenticacion);
            lblCodigoSubTitulo = FindViewById<TextView>(Resource.Id.lblCodigoSubTitulo);
            btnReLLamada = FindViewById<TextView>(Resource.Id.btnReLLamada);
            btnReLLamada.Click += BtnReLLamada_ClickAsync;
            btnAutenticarClaveVoz = FindViewById<Button>(Resource.Id.btnAutenticarClaveVoz);
            btnAutenticarClaveVoz.Click += BtnAutenticarClaveVoz_ClickAsync;
            fontRegular = Typeface.CreateFromAsset(Assets, ConstantesSecurity.rutaFuenteTitiliumRegular);
            fontSemiBold = Typeface.CreateFromAsset(Assets, ConstantesSecurity.rutaFuenteTitiliumSemiBold);
            lblCodigoSubTitulo.Typeface = fontRegular;
            lblMsjClaveVoz.Typeface = fontRegular;
            txtClaveVoz.Typeface = fontRegular;
            txtClaveVozSegundo.Typeface = fontRegular;
            txtClaveVozTercero.Typeface = fontRegular;
            txtClaveVozCuarto.Typeface = fontRegular;
            txtClaveVozQuinto.Typeface = fontRegular;
            txtClaveVozSexto.Typeface = fontRegular;
            btnAutenticarClaveVoz.Typeface = fontSemiBold;
            btnAutenticarClaveVoz.Enabled = false;
            formatearTextos();
            if (!btnAutenticarClaveVoz.Enabled)
            {
                btnAutenticarClaveVoz.SetBackgroundResource(Resource.Drawable.cssBoton);
            }
            parametriaLogUtil = new ParametriaLogUtil();
            dialogoLoadingBcoSecurityActivity = new DialogoLoadingBcoSecurityActivity(this);
            obtenerSMS();
        }

        void formatearTextos()
        {
            btnReLLamada.Typeface = fontRegular;
            SpannableString contenido = new SpannableString(TEXTO_NO_RECIBI_SMS);
            contenido.SetSpan(new UnderlineSpan(), 0, contenido.Length(), 0);
            btnReLLamada.TextFormatted = contenido;


            SpannableString spanString = new SpannableString(TEXTO_NO_TITULO_SMS);
            spanString.SetSpan(new CustomTypefaceSpan(fontRegular), 0, 5, SpanTypes.ExclusiveExclusive);
            spanString.SetSpan(new CustomTypefaceSpan(fontSemiBold), 6, 27, SpanTypes.ExclusiveExclusive);
            spanString.SetSpan(new CustomTypefaceSpan(fontRegular), 27, spanString.Length(), SpanTypes.ExclusiveExclusive);
            txtInstruccionAutenticacion.TextFormatted = spanString;
        }

        public bool habiltarBotonContinuar()
        {
            bool habilitado = false;
            if (txtClaveVoz.Text.Trim().Length == 1 && txtClaveVozSegundo.Text.Trim().Length == 1 && txtClaveVozTercero.Text.Trim().Length == 1 && txtClaveVozCuarto.Text.Trim().Length == 1 && txtClaveVozQuinto.Text.Trim().Length == 1 && txtClaveVozSexto.Text.Trim().Length == 1)
            {
                habilitado = true;
            }
            return habilitado;
        }

        async void obtenerSMS()
        {
            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            try
            {
                this.cleanEditTextClaveVoz();
                JsonValue jsonResponseAccessToken = await WebServiceSecurity.ServiciosSecurity.CallRESTaccessToken();
                JsonValue token = jsonResponseAccessToken["access_token"];
                JsonValue smsResponse = await WebServiceSecurity.ServiciosSecurity.CallRESTSMS(token, LoginActivity.returnRut(), parametriaLogUtil.getIdDispositivoParaLog(UtilAndroid.getIMEI(this)), ParametriaLogUtil.GetIpLocal());
                string statusCodeSMS = smsResponse["statusCode"];

                if (statusCodeSMS != null && statusCodeSMS.Equals(ConstantesSecurity.ESTADO_SUCCESS)){// sin respuesta al success 
                }
                else{
                    DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                    DialogoErrorActivity.mostrarViewErrorLogin(this);
                }
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            }
            catch (Exception x){
                Log.Error("", x.Message);
                DialogoErrorActivity.mostrarViewErrorLogin(this);
            }
        }

        private void BtnReLLamada_ClickAsync(object sender, EventArgs e){
            obtenerSMS();
        }

        private async void BtnAutenticarClaveVoz_ClickAsync(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtClaveVoz.Text))
            {
                lblMsjClaveVoz.Text = "Código no coincide, ingréselo nuevamente";
                pintarLineaEditText(lineTxtClaveVoz, lineTxtClaveVozSegundo, lineTxtClaveVozTercero, lineTxtClaveVozCuarto, lineTxtClaveVozQuinto, lineTxtClaveVozSexto, Resource.Color.red);
            }
            else
            {
                dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
                try
                {
                    String nombreArchivo = "app-bco_security_enrolamiento.sqlite";
                    String rutaCarpeta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    String ruta = System.IO.Path.Combine(rutaCarpeta, nombreArchivo);
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
                    string codigoAutenticar = txtClaveVoz.Text + txtClaveVozSegundo.Text + txtClaveVozTercero.Text + txtClaveVozCuarto.Text + txtClaveVozQuinto.Text + txtClaveVozSexto.Text;
                    JsonValue jsonResponseAccessToken = await WebServiceSecurity.ServiciosSecurity.CallRESTaccessToken();
                    JsonValue jt = jsonResponseAccessToken["access_token"];
                    JsonValue jsonResponseAutenticarClaveVoz = await WebServiceSecurity.ServiciosSecurity.CallRESTAutenticarClaveVoz(jt, SecurityEndpoints.A_GRUPOIDG, SecurityEndpoints.A_STR_IDMECANISMO, LoginActivity.returnRut(), LoginActivity.returnRut(), codigoAutenticar, SecurityEndpoints.A_CODIGOTRANSACCION, SecurityEndpoints.A_STR_SESSIONID1, SecurityEndpoints.A_STR_SESSIONID2, SecurityEndpoints.A_IDLOG, SecurityEndpoints.A_STR_IP, parametriaLogUtil.getIdDispositivoParaLog(UtilAndroid.getIMEI(this)), ParametriaLogUtil.GetIpLocal());
                    JsonValue jcv = jsonResponseAutenticarClaveVoz["statusCode"];
                    string status = jcv.ToString();

                    if (status.Equals("0")){
                        pintarLineaEditText(lineTxtClaveVoz, lineTxtClaveVozSegundo, lineTxtClaveVozTercero, lineTxtClaveVozCuarto, lineTxtClaveVozQuinto, lineTxtClaveVozSexto, Resource.Color.green);
                        lblMsjClaveVoz.Text = string.Empty;
                        JsonValue jsonResponseAccessTokenn = await WebServiceSecurity.ServiciosSecurity.CallRESTaccessToken();
                        JsonValue jtt = jsonResponseAccessTokenn["access_token"];
                        JsonValue jsonResponseEnrolarDispositivo = await WebServiceSecurity.ServiciosSecurity.CallRESTEnrolarDispositivo(jtt, idDispositivoConsultaEnrolado, LoginActivity.returnRut(), SecurityEndpoints.ED_SISTEMA, SecurityEndpoints.ED_CANAL, SecurityEndpoints.ED_ESTADO, SecurityEndpoints.ED_SISTEMA_ANDROID, getDispositivo(), getNameModel(), parametriaLogUtil.getIdDispositivoParaLog(UtilAndroid.getIMEI(this)), ParametriaLogUtil.GetIpLocal());
                        string jed = jsonResponseEnrolarDispositivo["statusCode"];
                        if (jed.Equals("Success")){
                            Intent i = new Intent(this, typeof(FingerPrintAuthActivity));
                            StartActivity(i);
                        }else{
                            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                            pintarLineaEditText(lineTxtClaveVoz, lineTxtClaveVozSegundo, lineTxtClaveVozTercero, lineTxtClaveVozCuarto, lineTxtClaveVozQuinto, lineTxtClaveVozSexto, Resource.Color.red);
                            lblMsjClaveVoz.Text = "Operación sin exito";
                        }
                    }else{
                        DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                        pintarLineaEditText(lineTxtClaveVoz, lineTxtClaveVozSegundo, lineTxtClaveVozTercero, lineTxtClaveVozCuarto, lineTxtClaveVozQuinto, lineTxtClaveVozSexto, Resource.Color.red);
                        lblMsjClaveVoz.Text = "Código no coincide, ingréselo nuevamente";
                    }
                }catch (Exception x){
                    Log.Error("", x.Message);
                    DialogoErrorActivity.mostrarViewErrorLogin(this);
                }
            }
        }
        public String getNameModel(){
            return Build.Brand + " " + Build.Model;
        }

        public String getDispositivo(){
            return "Dispositivo " + Build.Brand + ", Android";
        }

        public override void OnBackPressed()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Banco Security");
            builder.SetMessage("¿Está seguro que desea comenzar nuevamente?");
            builder.SetPositiveButton("Aceptar", delegate { 
                Intent i = new Intent(this, typeof(LoginActivity));
                StartActivity(i);
            });
            builder.SetNegativeButton("Cancelar", delegate { });
            builder.Show();
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e){
            switch (keyCode){
                case Keycode.Del:
                    if (txtClaveVozSexto.Text.Length == 0){
                        txtClaveVozQuinto.RequestFocus();
                    }
                    if (txtClaveVozQuinto.Text.Length == 0){
                        txtClaveVozCuarto.RequestFocus();
                    }
                    if (txtClaveVozCuarto.Text.Length == 0){
                        txtClaveVozTercero.RequestFocus();
                    }
                    if (txtClaveVozTercero.Text.Length == 0){
                        txtClaveVozSegundo.RequestFocus();
                    }
                    if (txtClaveVozSegundo.Text.Length == 0){
                        txtClaveVoz.RequestFocus();
                    }
                    return true;
                default:
                    return base.OnKeyUp(keyCode, e);
            }
        }

        void TxtClaveVoz_TextChanged(object sender, TextChangedEventArgs e)
        {
            cambioFocoEditText(txtClaveVoz.Text, txtClaveVozSegundo);
            habilitarDesabilitarBoton();
            resetEditTextBorrado(txtClaveVoz.Text, txtClaveVozSegundo.Text, txtClaveVozTercero.Text, txtClaveVozCuarto.Text, txtClaveVozQuinto.Text, txtClaveVozSexto.Text);
        }

        void TxtClaveVozSegundo_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            cambioFocoEditText(txtClaveVozSegundo.Text, txtClaveVozTercero);
            habilitarDesabilitarBoton();
            resetEditTextBorrado(txtClaveVoz.Text, txtClaveVozSegundo.Text, txtClaveVozTercero.Text, txtClaveVozCuarto.Text, txtClaveVozQuinto.Text, txtClaveVozSexto.Text);
        }

        void TxtClaveVozTercero_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            cambioFocoEditText(txtClaveVozTercero.Text, txtClaveVozCuarto);
            habilitarDesabilitarBoton();
            resetEditTextBorrado(txtClaveVoz.Text, txtClaveVozSegundo.Text, txtClaveVozTercero.Text, txtClaveVozCuarto.Text, txtClaveVozQuinto.Text, txtClaveVozSexto.Text);
        }

        void TxtClaveVozCuarto_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            cambioFocoEditText(txtClaveVozCuarto.Text, txtClaveVozQuinto);
            habilitarDesabilitarBoton();
            resetEditTextBorrado(txtClaveVoz.Text, txtClaveVozSegundo.Text, txtClaveVozTercero.Text, txtClaveVozCuarto.Text, txtClaveVozQuinto.Text, txtClaveVozSexto.Text);
        }

        void TxtClaveVozQuinto_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            cambioFocoEditText(txtClaveVozQuinto.Text, txtClaveVozSexto);
            habilitarDesabilitarBoton();
            resetEditTextBorrado(txtClaveVoz.Text, txtClaveVozSegundo.Text, txtClaveVozTercero.Text, txtClaveVozCuarto.Text, txtClaveVozQuinto.Text, txtClaveVozSexto.Text);
        }

        void TxtClaveVozSexto_TextChanged(object sender, Android.Text.TextChangedEventArgs e){
            habilitarDesabilitarBoton();
            resetEditTextBorrado(txtClaveVoz.Text, txtClaveVozSegundo.Text, txtClaveVozTercero.Text, txtClaveVozCuarto.Text, txtClaveVozQuinto.Text, txtClaveVozSexto.Text);
        }

        public void habilitarDesabilitarBoton(){
            if (habiltarBotonContinuar()){
                btnAutenticarClaveVoz.Enabled = true;
                btnAutenticarClaveVoz.SetBackgroundResource(Resource.Drawable.animaBoton);
            }else{
                btnAutenticarClaveVoz.Enabled = false;
                btnAutenticarClaveVoz.SetBackgroundResource(Resource.Drawable.cssBoton);
            }
        }

        public void cambioFocoEditText(string txtPreFoco, EditText txtPostFoco){
            if (txtPreFoco.Length == 1){
                txtPostFoco.RequestFocus();
            }
        }

        private void cleanEditTextClaveVoz(){
            txtClaveVoz.Text = string.Empty;
            txtClaveVozSegundo.Text = string.Empty;
            txtClaveVozTercero.Text = string.Empty;
            txtClaveVozCuarto.Text = string.Empty;
            txtClaveVozQuinto.Text = string.Empty;
            txtClaveVozSexto.Text = string.Empty;
            lblMsjClaveVoz.Text = string.Empty;
            txtClaveVoz.RequestFocus();
            pintarLineaEditText(lineTxtClaveVoz, lineTxtClaveVozSegundo, lineTxtClaveVozTercero, lineTxtClaveVozCuarto, lineTxtClaveVozQuinto, lineTxtClaveVozSexto, Resource.Color.security_on_off_edittextclavevozsinerror);
        }

        private void cleanEditTextClaveSmsBorrado(){
            txtClaveVoz.RequestFocus();
            pintarLineaEditText(lineTxtClaveVoz, lineTxtClaveVozSegundo, lineTxtClaveVozTercero, lineTxtClaveVozCuarto, lineTxtClaveVozQuinto, lineTxtClaveVozSexto, Resource.Color.security_on_off_edittextclavevozsinerror);
            lblMsjClaveVoz.Text = string.Empty;
        }

        void registrarListenerSMS(){
            try{
                // Registering the broad cast receiver to detect sms from local phone
                if (null == this.smsReceiver){
                    smsReceiver = new SMSBroadcast();
                    smsReceiver.setActividadSMS(this);
                    this.RegisterReceiver(this.smsReceiver, new IntentFilter("android.provider.Telephony.SMS_RECEIVED"));
                }
            }catch (Exception ex){
                Log.Error("", ex.Message);
                DialogoErrorActivity.mostrarViewErrorLogin(this);
            }
        }

        public void smsRecibido(string mensaje)
        {
            // Estimado JAVIER PATRICIO JOFRE LALLEMAND la clave es 226743
            try{
                string codigo = mensaje.Substring(mensaje.Length - 6); // ojo , el valor -6 indica que el mensaje siempre sera de largo 6 y estara al final del mensaje.
                char[] listadoCodigos = codigo.ToCharArray();
                txtClaveVoz.Text = listadoCodigos[0].ToString();
                txtClaveVozSegundo.Text = listadoCodigos[1].ToString();
                txtClaveVozTercero.Text = listadoCodigos[2].ToString();
                txtClaveVozCuarto.Text = listadoCodigos[3].ToString();
                txtClaveVozQuinto.Text = listadoCodigos[4].ToString();
                txtClaveVozSexto.Text = listadoCodigos[5].ToString();
            }catch (Exception e){
                Log.Error("ON_OFF_ANDROID_ERROR_SMS", e.Message);
                DialogoErrorActivity.mostrarViewErrorLogin(this);
            }
        }

        public void resetEditTextBorrado(string txt1, string txt2, string txt3, string txt4, string txt5, string txt6){
            if(string.IsNullOrEmpty(txt1) && string.IsNullOrEmpty(txt2) && string.IsNullOrEmpty(txt3) && string.IsNullOrEmpty(txt4) && string.IsNullOrEmpty(txt5) && string.IsNullOrEmpty(txt6)){
                cleanEditTextClaveSmsBorrado();
            }
        }

        public void pintarLineaEditText(View txt1, View txt2, View txt3, View txt4, View txt5, View txt6, int color){
            txt1.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, color);
            txt2.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, color);
            txt3.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, color);
            txt4.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, color);
            txt5.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, color);
            txt6.BackgroundTintList = Android.Support.V4.Content.ContextCompat.GetColorStateList(ApplicationContext, color);
        }
    }
}