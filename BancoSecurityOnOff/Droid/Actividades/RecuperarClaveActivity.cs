
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using BancoSecurityOnOff.Utilidades.Firmas;

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
    [Activity(Label = "RecuperarClaveActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar",
	          ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)] 
	
    public class RecuperarClaveActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RecuperarClave);
            string url = SecurityEndpoints.URL_RECUPERAR_CLAVE;
            var webView = FindViewById<WebView>(Resource.Id.webViewRecuperarClave);
            WebSettings webSettings = webView.Settings;
            webSettings.JavaScriptEnabled = true;
            webView.LoadUrl(url);
        }
    }
}
