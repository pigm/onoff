using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.V7.App;

using Android.Hardware.Fingerprints;
using Android.Support.V4.App;
using Android;
using BancoSecurityOnOff.Droid.WebServiceSecurity.HelperFinger;
using Android.Security.Keystore;
using Javax.Crypto;
using Java.Security;
using System.IO;
using BancoSecurityOnOff.Utilidades.SQLiteDataBase;
using Android.Graphics;

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
    [Activity(Label = "FingerPrintAuthActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar",
	          ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)] 
	
    public class FingerPrintAuthActivity : AppCompatActivity
    {
        ImageView btnOmitirEnrolado;
        TextView lblTituloAsociaHuella;
        TextView lblTextoIndicacionAsociacion1;
        TextView lblTextoIndicacionAsociacion2;
        TextView lblTextoIndicacionAsociacion3;
        TextView lblTextoIndicacionAsociacion4;
        TextView lblTextoIndicacionAsociacion5;
        private KeyStore keyStore;
        private Cipher cipher;
        private String KEY_NAME = LoginActivity.returnPasswordLogin();

        private const string rutaFuenteTitiliumRegular = "fonts/titillium_web/TitilliumWeb-Regular.ttf";
        private const string rutaFuenteTitiliumSemiBold = "fonts/titillium_web/TitilliumWeb-SemiBold.ttf";


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FingerPrintAuth);

            String nombreArchivo = "app-bco_security.sqlite";
            String rutaCarpeta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            String ruta = System.IO.Path.Combine(rutaCarpeta, nombreArchivo);

            var key = new Contenido()
            {
                Password = KEY_NAME
            };

            DatabaseHelper.Insertar(ref key, ruta);

            KeyguardManager keyguardManager = (KeyguardManager)GetSystemService(KeyguardService);
            FingerprintManager fingerprintManager = (FingerprintManager)GetSystemService(FingerprintService);

            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.UseFingerprint) != (int)Android.Content.PM.Permission.Granted)
            {
                return;
            }

            if (!fingerprintManager.IsHardwareDetected)
            {
                Toast.MakeText(ApplicationContext, "Lector de huella no habilitado en tu dispositivo", ToastLength.Long).Show();
            }
            else
            {

                if (!fingerprintManager.HasEnrolledFingerprints)
                {
                    Toast.MakeText(ApplicationContext, "Debes enrolar tu huella del dispositivo", ToastLength.Long).Show();
                }
                else
                {
                    if (!keyguardManager.IsKeyguardSecure)
                    {
                        Toast.MakeText(ApplicationContext, "No tienes habilitada la configuracion del scaner dactilar", ToastLength.Long).Show();
                    }
                    else
                    {
                        GenKey();
                    }

                    if (CipherInit())
                    {
                        FingerprintManager.CryptoObject cryptoObject = new FingerprintManager.CryptoObject(cipher);
                        FingerprintHandler helper = new FingerprintHandler(this);
                        helper.StartAuthentication(fingerprintManager, cryptoObject);
                    }
                }
            }
            lblTituloAsociaHuella = FindViewById<TextView>(Resource.Id.lblTituloAsociaHuella);
            lblTextoIndicacionAsociacion1 = FindViewById<TextView>(Resource.Id.lblTextoIndicacionAsociacion1);
            lblTextoIndicacionAsociacion2 = FindViewById<TextView>(Resource.Id.lblTextoIndicacionAsociacion2);
            lblTextoIndicacionAsociacion3 = FindViewById<TextView>(Resource.Id.lblTextoIndicacionAsociacion3);
            lblTextoIndicacionAsociacion4 = FindViewById<TextView>(Resource.Id.lblTextoIndicacionAsociacion4);
            lblTextoIndicacionAsociacion5 = FindViewById<TextView>(Resource.Id.lblTextoIndicacionAsociacion5);
            btnOmitirEnrolado = FindViewById<ImageView>(Resource.Id.btnOmitirEnrolado);
            btnOmitirEnrolado.Click += BtnOmitirEnrolado_Click;

            var fontRegular = Typeface.CreateFromAsset(Assets, rutaFuenteTitiliumRegular);
            var fontSemiBold = Typeface.CreateFromAsset(Assets, rutaFuenteTitiliumSemiBold);
            lblTituloAsociaHuella.Typeface = fontSemiBold;
            lblTextoIndicacionAsociacion1.Typeface = fontRegular;
            lblTextoIndicacionAsociacion2.Typeface = fontSemiBold;
            lblTextoIndicacionAsociacion3.Typeface = fontRegular;
            lblTextoIndicacionAsociacion4.Typeface = fontRegular;
            lblTextoIndicacionAsociacion5.Typeface = fontRegular;
        }

        private void BtnOmitirEnrolado_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(ConfirmacionEnroladoActivity));
            StartActivity(i);
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
                return false;
            }
        }

        public void principalView()
        {
            Intent i = new Intent(this, typeof(LoginActivity));
            StartActivity(i);
        }

        public override void OnBackPressed()
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle("Banco Security");
            builder.SetMessage("¿Está seguro que desea comenzar nuevamente?");
            builder.SetPositiveButton("Aceptar", delegate { principalView(); });
            builder.SetNegativeButton("Cancelar", delegate { });
            builder.Show();
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
    }
}
