
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
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
    public class TerminoYCondicionesFragment : Fragment
    {
        TextView lblTitulo;
        TextView lblCondiciones, lblServicios, lblPolíticaPrivacidadConﬁdencialidad, lblAccesoMedianteBiometría, lblModoDeUso, lblResponsabilidad;
        TextView lblCondicionesParrafo, lblServiciosParrafo, lblPolíticaPrivacidadConﬁdencialidadParrafo, lblAccesoMedianteBiometríaParrafo, lblModoDeUsoParrafo, lblResponsabilidadParrafo; 
        Button btnAceptaTerminos;
        ImageView lblNoAceptoApp;
        LinearLayout llFondoTerminoYcondiciones;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.TerminoYCondiciones, container, false);
            lblTitulo = view.FindViewById<TextView>(Resource.Id.lblTitulo);
            lblCondiciones = view.FindViewById<TextView>(Resource.Id.lblCondiciones);
            lblServicios = view.FindViewById<TextView>(Resource.Id.lblServicios);
            lblPolíticaPrivacidadConﬁdencialidad = view.FindViewById<TextView>(Resource.Id.lblPolíticaPrivacidadConﬁdencialidad);
            lblAccesoMedianteBiometría = view.FindViewById<TextView>(Resource.Id.lblAccesoMedianteBiometría);
            lblModoDeUso = view.FindViewById<TextView>(Resource.Id.lblModoDeUso);
            lblResponsabilidad = view.FindViewById<TextView>(Resource.Id.lblResponsabilidad);
            llFondoTerminoYcondiciones = view.FindViewById<LinearLayout>(Resource.Id.llFondoTerminoYcondiciones);
            lblCondicionesParrafo = view.FindViewById<TextView>(Resource.Id.lblCondicionesParrafo);
            lblServiciosParrafo = view.FindViewById<TextView>(Resource.Id.lblServiciosParrafo);
            lblPolíticaPrivacidadConﬁdencialidadParrafo = view.FindViewById<TextView>(Resource.Id.lblPolíticaPrivacidadConﬁdencialidadParrafo);
            lblAccesoMedianteBiometríaParrafo = view.FindViewById<TextView>(Resource.Id.lblAccesoMedianteBiometríaParrafo);
            lblModoDeUsoParrafo = view.FindViewById<TextView>(Resource.Id.lblModoDeUsoParrafo);
            lblResponsabilidadParrafo = view.FindViewById<TextView>(Resource.Id.lblResponsabilidadParrafo);
            btnAceptaTerminos = view.FindViewById<Button>(Resource.Id.btnAceptaTerminos);
            lblNoAceptoApp = view.FindViewById<ImageView>(Resource.Id.lblNoAceptoApp);

            llFondoTerminoYcondiciones.SetBackgroundColor(Color.Rgb(245, 245, 245));
            var fontRegular = Typeface.CreateFromAsset(Activity.Assets, "fonts/titillium_web/TitilliumWeb-Regular.ttf");
            var fontSemiBold = Typeface.CreateFromAsset(Activity.Assets, "fonts/titillium_web/TitilliumWeb-SemiBold.ttf");
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
           

            btnAceptaTerminos.Visibility = ViewStates.Gone;
            lblNoAceptoApp.Visibility = ViewStates.Gone;

            return view;
        }
    }
}
