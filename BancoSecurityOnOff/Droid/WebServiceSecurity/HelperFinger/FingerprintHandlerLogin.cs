using System;
using Android;
using Android.Content;
using Android.Hardware.Fingerprints;
using Android.OS;
using Android.Support.V4.App;
using Android.Widget;

/**
   @autor:          PaBlo Garrido
   @cargo:          Ingeniero de Software
   @institucion:    TInet soluciones informaticas
   @modificaciones: 
   @version:        1.0.0.0
   @proyecto:       OnOff
   
*/

namespace BancoSecurityOnOff.Droid.WebServiceSecurity.HelperFinger
{
    public class FingerprintHandlerLogin : FingerprintManager.AuthenticationCallback
    {
        private Context mainActivity;

        public FingerprintHandlerLogin(Context mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        internal void StartAuthentication(FingerprintManager fingerprintManager, FingerprintManager.CryptoObject cryptoObject)
        {
            CancellationSignal cancellationSignal = new CancellationSignal();
            if (ActivityCompat.CheckSelfPermission(mainActivity, Manifest.Permission.UseFingerprint) != (int)Android.Content.PM.Permission.Granted)
            {
                return;
            }
            else
            {
                fingerprintManager.Authenticate(cryptoObject, cancellationSignal, 0, this, null);
            }

        }

        public override void OnAuthenticationFailed()
        {
            Toast.MakeText(mainActivity, "Tú huella no es valida", ToastLength.Long).Show();
        }

        public override void OnAuthenticationSucceeded(FingerprintManager.AuthenticationResult result)
        {
            Toast.MakeText(mainActivity, "Autenticacion Exitosa", ToastLength.Long).Show();
            mainActivity.StartActivity(new Intent(mainActivity, typeof(HomeActivity)));
        }
    }
}
