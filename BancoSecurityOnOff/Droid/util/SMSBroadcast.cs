using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Telephony;
using Android.Util;

namespace BancoSecurityOnOff.Droid.Util
{
    // Clase encargada de escuchar evento SMS 

    [BroadcastReceiver(Enabled = true, Label = "SMS Receiver")]
    public class SMSBroadcast : BroadcastReceiver
    {
        private const string IntentAction = "android.provider.Telephony.SMS_RECEIVED";
        AutenticacionPorSMSActivity actividadSMS;

        public void setActividadSMS(AutenticacionPorSMSActivity actividadSMS)
        {
            this.actividadSMS = actividadSMS;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action != IntentAction)
                {
                    return;
                }
                var bundle = intent.Extras;
                if (bundle == null)
                {
                    return;
                }
                var pdus = bundle.Get("pdus");
                var castedPdus = JNIEnv.GetArray<Java.Lang.Object>(pdus.Handle);
                // var msgs = new SmsMessage[castedPdus.Length - 1];
                for (var i = 0; i < castedPdus.Length; i++)
                {
                    var bytes = new byte[JNIEnv.GetArrayLength(castedPdus[i].Handle)];
                    JNIEnv.CopyArray(castedPdus[i].Handle, bytes);
                    // msgs[i] = 
                    SmsMessage sms = SmsMessage.CreateFromPdu(bytes);

                    if (sms.DisplayMessageBody != null && sms.DisplayMessageBody.ToUpper().StartsWith("ESTIMADO"))
                    {
                        string mensaje = sms.DisplayMessageBody.ToUpper();

                       // string verificationCode = msgs[i].DisplayMessageBody.Split(':')[1].Split('.')[0];
                        actividadSMS.smsRecibido(mensaje);
                       /* Intent otpIntent = new Intent(Application.Context, typeof());
                        otpIntent.PutExtra("verificationCode", verificationCode.Trim());
                        otpIntent.PutExtra("fromsms", "OK");
                        otpIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.SingleTop);
                        context.StartActivity(otpIntent);
                        */
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("ON_OFF_ANDROID_ERROR:", ex.Message);
            }
        }
    }
}
