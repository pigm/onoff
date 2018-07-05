
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BancoSecurityOnOff.Droid
{
    [Activity(Label = "")]
    public class DialogoErrorActivity : Activity
    {
        static Dialog customDialog = null;

        private const string rutaFuenteTitiliumRegular = "fonts/titillium_web/TitilliumWeb-Regular.ttf";
        private const string rutaFuenteTitiliumSemiBold = "fonts/titillium_web/TitilliumWeb-SemiBold.ttf";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.dialogoError);

        }

        public static void mostrarViewError(Activity activity)
        {
            // con este tema personalizado evitamos los bordes por defecto
            customDialog = new Dialog(activity, Resource.Style.Theme_Dialog_Translucent);
            //deshabilitamos el título por defecto
            //customDialog.RequestWindowFeature((int)WindowFeatures.NoTitle);

            //obligamos al usuario a pulsar los botones para cerrarlo
            customDialog.SetCancelable(false);
            //establecemos el contenido de nuestro dialog
            customDialog.SetContentView(Resource.Layout.dialogoError);
            TextView lblTituloDialogoError = customDialog.FindViewById<TextView>(Resource.Id.lblTituloDialogoError);
            TextView lblMensajeErrorParrafoUno = customDialog.FindViewById<TextView>(Resource.Id.lblMensajeErrorParrafoUno);
            TextView lblMensajeErrorParrafoDos = customDialog.FindViewById<TextView>(Resource.Id.lblMensajeErrorParrafoDos);
            Button btnCerrarDialogo =customDialog.FindViewById<Button>(Resource.Id.btnCerrarDialogo);

            Typeface fontRegular = Typeface.CreateFromAsset(activity.Assets, rutaFuenteTitiliumRegular);
            Typeface fontSemiBold = Typeface.CreateFromAsset(activity.Assets, rutaFuenteTitiliumSemiBold);
            lblTituloDialogoError.Typeface = fontSemiBold;
            lblMensajeErrorParrafoUno.Typeface = fontRegular;
            lblMensajeErrorParrafoDos.Typeface = fontRegular;
            btnCerrarDialogo.Typeface = fontRegular;
            btnCerrarDialogo.Click += delegate {
                activity.Finish();
            };
            customDialog.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            //btnCerrar.Click += BtnCerrar_Click;
            customDialog.Show();
        }

        public static void mostrarViewErrorLoginEnrolado(Activity activity)
        {
            // con este tema personalizado evitamos los bordes por defecto
            customDialog = new Dialog(activity, Resource.Style.Theme_Dialog_Translucent);
            //deshabilitamos el título por defecto
            //customDialog.RequestWindowFeature((int)WindowFeatures.NoTitle);

            //obligamos al usuario a pulsar los botones para cerrarlo
            customDialog.SetCancelable(false);
            //establecemos el contenido de nuestro dialog
            customDialog.SetContentView(Resource.Layout.dialogoError);
            TextView lblTituloDialogoError = customDialog.FindViewById<TextView>(Resource.Id.lblTituloDialogoError);
            TextView lblMensajeErrorParrafoUno = customDialog.FindViewById<TextView>(Resource.Id.lblMensajeErrorParrafoUno);
            TextView lblMensajeErrorParrafoDos = customDialog.FindViewById<TextView>(Resource.Id.lblMensajeErrorParrafoDos);
            Button btnCerrarDialogo = customDialog.FindViewById<Button>(Resource.Id.btnCerrarDialogo);

            Typeface fontRegular = Typeface.CreateFromAsset(activity.Assets, rutaFuenteTitiliumRegular);
            Typeface fontSemiBold = Typeface.CreateFromAsset(activity.Assets, rutaFuenteTitiliumSemiBold);
            lblTituloDialogoError.Typeface = fontSemiBold;
            lblMensajeErrorParrafoUno.Typeface = fontRegular;
            lblMensajeErrorParrafoDos.Typeface = fontRegular;
            btnCerrarDialogo.Typeface = fontRegular;
            btnCerrarDialogo.Click += delegate {
                Intent intent = new Intent(Intent.ActionMain);
                intent.AddCategory(Intent.CategoryHome);
                intent.SetFlags(ActivityFlags.NewTask);
                activity.StartActivity(intent);
            };
            customDialog.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            //btnCerrar.Click += BtnCerrar_Click;
            customDialog.Show();
        }

        public static void mostrarViewErrorLogin(Activity activity)
        {
            // con este tema personalizado evitamos los bordes por defecto
            customDialog = new Dialog(activity, Resource.Style.Theme_Dialog_Translucent);
            //deshabilitamos el título por defecto
            //customDialog.RequestWindowFeature((int)WindowFeatures.NoTitle);

            //obligamos al usuario a pulsar los botones para cerrarlo
            customDialog.SetCancelable(false);
            //establecemos el contenido de nuestro dialog
            customDialog.SetContentView(Resource.Layout.dialogoError);
            TextView lblTituloDialogoError = customDialog.FindViewById<TextView>(Resource.Id.lblTituloDialogoError);
            TextView lblMensajeErrorParrafoUno = customDialog.FindViewById<TextView>(Resource.Id.lblMensajeErrorParrafoUno);
            TextView lblMensajeErrorParrafoDos = customDialog.FindViewById<TextView>(Resource.Id.lblMensajeErrorParrafoDos);
            Button btnCerrarDialogo = customDialog.FindViewById<Button>(Resource.Id.btnCerrarDialogo);

            Typeface fontRegular = Typeface.CreateFromAsset(activity.Assets, rutaFuenteTitiliumRegular);
            Typeface fontSemiBold = Typeface.CreateFromAsset(activity.Assets, rutaFuenteTitiliumSemiBold);
            lblTituloDialogoError.Typeface = fontSemiBold;
            lblMensajeErrorParrafoUno.Typeface = fontRegular;
            lblMensajeErrorParrafoDos.Typeface = fontRegular;
            btnCerrarDialogo.Typeface = fontRegular;
            btnCerrarDialogo.Click += delegate {
                Intent i = new Intent(activity, typeof(LoginActivity));
                activity.StartActivity(i);
                 
            };
            customDialog.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            //btnCerrar.Click += BtnCerrar_Click;
            customDialog.Show();
        }

        public static void mostrarViewErrorHome(Activity activity)
        {
            // con este tema personalizado evitamos los bordes por defecto
            customDialog = new Dialog(activity, Resource.Style.Theme_Dialog_Translucent);
            //deshabilitamos el título por defecto
            //customDialog.RequestWindowFeature((int)WindowFeatures.NoTitle);

            //obligamos al usuario a pulsar los botones para cerrarlo
            customDialog.SetCancelable(false);
            //establecemos el contenido de nuestro dialog
            customDialog.SetContentView(Resource.Layout.dialogoError);
            TextView lblTituloDialogoError = customDialog.FindViewById<TextView>(Resource.Id.lblTituloDialogoError);
            TextView lblMensajeErrorParrafoUno = customDialog.FindViewById<TextView>(Resource.Id.lblMensajeErrorParrafoUno);
            TextView lblMensajeErrorParrafoDos = customDialog.FindViewById<TextView>(Resource.Id.lblMensajeErrorParrafoDos);
            Button btnCerrarDialogo = customDialog.FindViewById<Button>(Resource.Id.btnCerrarDialogo);

            Typeface fontRegular = Typeface.CreateFromAsset(activity.Assets, rutaFuenteTitiliumRegular);
            Typeface fontSemiBold = Typeface.CreateFromAsset(activity.Assets, rutaFuenteTitiliumSemiBold);
            lblTituloDialogoError.Typeface = fontSemiBold;
            lblMensajeErrorParrafoUno.Typeface = fontRegular;
            lblMensajeErrorParrafoDos.Typeface = fontRegular;
            btnCerrarDialogo.Typeface = fontRegular;
            btnCerrarDialogo.Click += delegate {
                //Intent intentActividadHome = new Intent(activity, typeof(HomeActivity));
                //activity.StartActivity(intentActividadHome);
                customDialog.Dismiss();
            };
            customDialog.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            //btnCerrar.Click += BtnCerrar_Click;
            customDialog.Show();
        }
    }
}
