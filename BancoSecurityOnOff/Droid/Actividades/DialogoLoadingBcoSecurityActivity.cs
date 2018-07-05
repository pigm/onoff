using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;

namespace BancoSecurityOnOff.Droid
{

    public class DialogoLoadingBcoSecurityActivity{
        static Dialog customDialog;
        static ImageView ivLoader;
        Activity activity;

        public DialogoLoadingBcoSecurityActivity(Activity activity){
            customDialog = null;
            ivLoader = null;
            this.activity = activity;
            customDialog = new Dialog(activity, Resource.Style.Theme_Dialog_Translucent_Loading);
        }

        public void mostrarViewLoadingSecurity(){ 
            customDialog.SetCancelable(false);
            customDialog.SetContentView(Resource.Layout.dialogoLoadingBcoSecurity);
            customDialog.Window.SetStatusBarColor(Color.Transparent);
            customDialog.OwnerActivity = activity;
            ivLoader = customDialog.FindViewById<ImageView>(Resource.Id.gifLoadingBcoSecurity);
            ivLoader.SetBackgroundResource(Resource.Animation.animationloader);
            AnimationDrawable frameAnimation = (AnimationDrawable)ivLoader.Background;
            frameAnimation.Start();
            customDialog.Show();
        }

        public static void ocultarLoadingSecurity(){
            customDialog.Dismiss();
        }
    }
}
