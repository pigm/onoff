
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
    [Activity(Label = "TerminoYCondicionesActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar",
	          ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)] 
	
    public class TerminoYCondicionesActivity : Activity
    {
        TextView lblTitulo;
        TextView lblCondiciones, lblServicios, lblPolíticaPrivacidadConﬁdencialidad, lblAccesoMedianteBiometría, lblModoDeUso, lblResponsabilidad;
        TextView lblCondicionesParrafo, lblServiciosParrafo, lblPolíticaPrivacidadConﬁdencialidadParrafo, lblAccesoMedianteBiometríaParrafo, lblModoDeUsoParrafo, lblResponsabilidadParrafo; 
        ImageView lblNoAceptoApp;
        Button btnAceptaTerminos; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TerminoYCondiciones);
            lblCondiciones = FindViewById<TextView>(Resource.Id.lblCondiciones);
            lblServicios = FindViewById<TextView>(Resource.Id.lblServicios);
            lblPolíticaPrivacidadConﬁdencialidad = FindViewById<TextView>(Resource.Id.lblPolíticaPrivacidadConﬁdencialidad);
            lblAccesoMedianteBiometría = FindViewById<TextView>(Resource.Id.lblAccesoMedianteBiometría);
            lblModoDeUso = FindViewById<TextView>(Resource.Id.lblModoDeUso);
            lblResponsabilidad = FindViewById<TextView>(Resource.Id.lblResponsabilidad);

            lblCondicionesParrafo = FindViewById<TextView>(Resource.Id.lblCondicionesParrafo);
            lblServiciosParrafo = FindViewById<TextView>(Resource.Id.lblServiciosParrafo);
            lblPolíticaPrivacidadConﬁdencialidadParrafo = FindViewById<TextView>(Resource.Id.lblPolíticaPrivacidadConﬁdencialidadParrafo);
            lblAccesoMedianteBiometríaParrafo = FindViewById<TextView>(Resource.Id.lblAccesoMedianteBiometríaParrafo);
            lblModoDeUsoParrafo = FindViewById<TextView>(Resource.Id.lblModoDeUsoParrafo);
            lblResponsabilidadParrafo = FindViewById<TextView>(Resource.Id.lblResponsabilidadParrafo);

            lblTitulo = FindViewById<TextView>(Resource.Id.lblTitulo);
            btnAceptaTerminos = FindViewById<Button>(Resource.Id.btnAceptaTerminos);
            btnAceptaTerminos.Click += BtnAceptaTerminos_Click;

            lblNoAceptoApp = FindViewById<ImageView>(Resource.Id.lblNoAceptoApp);
            lblNoAceptoApp.Click += LblNoAceptoApp_Click;

            var fontRegular = Typeface.CreateFromAsset(Assets, "fonts/titillium_web/TitilliumWeb-Regular.ttf");
            var fontSemiBold = Typeface.CreateFromAsset(Assets, "fonts/titillium_web/TitilliumWeb-SemiBold.ttf");
            lblTitulo.Typeface = fontRegular;

            lblCondiciones.Typeface = fontSemiBold;
            lblServicios.Typeface = fontSemiBold;
            lblPolíticaPrivacidadConﬁdencialidad.Typeface = fontSemiBold;
            lblAccesoMedianteBiometría.Typeface = fontSemiBold;
            lblModoDeUso.Typeface = fontSemiBold;
            lblResponsabilidad.Typeface = fontSemiBold;

            lblCondicionesParrafo.Typeface = fontRegular;
            lblServiciosParrafo.Typeface = fontRegular;
            lblPolíticaPrivacidadConﬁdencialidadParrafo.Typeface = fontRegular;
            lblAccesoMedianteBiometríaParrafo.Typeface = fontRegular;
            lblModoDeUsoParrafo.Typeface = fontRegular;
            lblResponsabilidadParrafo.Typeface = fontRegular;
           
            btnAceptaTerminos.Typeface = fontSemiBold;
          
        }

        private void LblNoAceptoApp_Click(object sender, EventArgs e)
        {
            Intent intentLogin = new Intent(this, typeof(LoginActivity));
            StartActivity(intentLogin);
        }

        private void BtnAceptaTerminos_Click(object sender, EventArgs e)
        {
            Intent intentLLamada = new Intent(this, typeof(AutenticacionPorSMSActivity));
            StartActivity(intentLLamada);
        }

        public void principalView()
        {
            Intent i = new Intent(this, typeof(LoginActivity));
            StartActivity(i);
        }

        public override void OnBackPressed()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Banco Security");
            builder.SetMessage("¿Está seguro que desea comenzar nuevamente?");
            builder.SetPositiveButton("Aceptar", delegate { principalView(); });
            builder.SetNegativeButton("Cancelar", delegate { });
            builder.Show();
        }

    }
}
