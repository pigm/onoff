using System;
using Android.App;
using Firebase.Iid;
using Android.Util;
namespace BancoSecurityOnOff.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        static string refreshedToken;
        const string TAG = "MyFirebaseIIDService";

        public override void OnTokenRefresh()
        {
            try
            {
                refreshedToken = FirebaseInstanceId.Instance.Token;
                Log.Debug(TAG, "Refreshed token: " + refreshedToken);
                if (refreshedToken == null)
                { 
                    refreshedToken = FirebaseInstanceId.Instance.Token;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exeption tokenFCM: " + ex);
            }
         
            
        }

        public static string SendRegistrationToServer()
        {
            string tokenFCM = string.Empty;
            tokenFCM = refreshedToken;
            return tokenFCM;
        }
    }
}
