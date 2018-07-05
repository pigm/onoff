
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

namespace BancoSecurityOnOff.Droid
{
    /// <summary>
    /// Clase FingerPrintAdminFragment
    /// </summary>
    public class FingerPrintAdminFragment : Fragment
    {
        /// <summary>
        /// Variables de la clase
        /// </summary>
        TextView lblTituloEstadoHuella;
        TextView lblInfoEstadoHuella;
        TextView lblEstadoUsoHuella;
        Switch swtEstadoHuella;

        /// <summary>
        /// Constantes de la clase
        /// </summary>
        private const string rutaFuenteTitilium = "fonts/titillium_web/TitilliumWeb-Light.ttf";
        private const string estadoUsoHuellaActivado = "Uso de huella: activado";
        private const string estadoUsoHuellaDesactivado = "Uso de huella: desactivado";
        private const string infoEstadoHuellaActivado = "Todas las huellas digitales almacenadas en este dispositivo están activadas para iniciar sesión en la Aplicación On-Off.";
        private const string infoEstadoHuellaDesactivado = "Todas las huellas digitales almacenadas en este dispositivo están desactivadas para iniciar sesión en la Aplicación On-Off.";

        /// <summary>
        /// Metodo onCreate del fragment FingerPrintAdminFragment
        /// </summary>
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        /// <summary>
        /// Metodo onCreateView del fragment FingerPrintAdminFragment
        /// </summary>
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FingerPrintAdmin, container, false);
            lblTituloEstadoHuella = view.FindViewById<TextView>(Resource.Id.lblTituloEstadoHuella);
            lblInfoEstadoHuella = view.FindViewById<TextView>(Resource.Id.lblInfoEstadoHuella);
            lblEstadoUsoHuella = view.FindViewById<TextView>(Resource.Id.lblEstadoUsoHuella);
            swtEstadoHuella = view.FindViewById<Switch>(Resource.Id.swtEstadoHuella);

            swtEstadoHuella.CheckedChange += ChangeStatusFinger;

            var font = Typeface.CreateFromAsset(Activity.Assets, rutaFuenteTitilium);
            lblTituloEstadoHuella.Typeface = font;
            lblInfoEstadoHuella.Typeface = font;
            lblEstadoUsoHuella.Typeface = font;

            return view;
        }

        private void ChangeStatusFinger(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (swtEstadoHuella.Checked == true)
            {
                lblEstadoUsoHuella.Text = estadoUsoHuellaActivado;
                lblEstadoUsoHuella.SetTextColor(lblEstadoUsoHuella.Context.Resources.GetColor(Resource.Color.green));
                lblInfoEstadoHuella.Text = infoEstadoHuellaActivado;
            }
            else
            {
                lblEstadoUsoHuella.Text = estadoUsoHuellaDesactivado;
                lblEstadoUsoHuella.SetTextColor(lblEstadoUsoHuella.Context.Resources.GetColor(Resource.Color.gray));
                lblInfoEstadoHuella.Text = infoEstadoHuellaDesactivado;
            }
        }
    }
}
