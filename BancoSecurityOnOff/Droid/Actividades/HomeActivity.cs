using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.App;
using Android.Content;
using System;
using Android.Graphics;
using Android.Support.V4.View;
using Android.Text;
using System.Collections.Generic;
using BancoSecurityOnOff.Droid.Bean;
using BancoSecurityOnOff.Droid.Notificaciones;
using BancoSecurityOnOff.Droid.Util;

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
	[Activity(Label = "", Theme = "@style/Theme.AppCompat.Light.NoActionBar"  , ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize |
              Android.Content.PM.ConfigChanges.Orientation,
              ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)] 
    public class HomeActivity : AppCompatActivity
    {
        DrawerLayout drawerLayout;
        ImageView btnBurger;
		ImageView botonCerrarMenu;
        ImageView btnCamapanaNotificaciones;
        ImageView btnCamapanaNotificacionesNoLeida;
        PreferenciasNotificacionesLeidas preferenciasNotificacionesLeidas;
        NotificacionesUtil notificacionesUtil;
        List<Notificacion> listadoFinal;
        RelativeLayout relativeLayoutCampanaNotificacionesContador;
        TextView lblUsuarioMenu;
        DialogoLoadingBcoSecurityActivity dialogoLoadingBcoSecurityActivity;
        static TextView lblContadorNotificacionesNoLeidas;
        string nombreConocido;
        string nombreDesdeConfirmacion;
        string nombreTitulo;
        string[] nombreUsuario;
        string primerNombreUsuario = string.Empty;
        string segundoNombreUsuario = string.Empty;
        string primerNombreFormateado;
        string segundoNombreFormateado;

        private const int cero = 0;
        private const string rutaFuenteTitilium = "fonts/titillium_web/TitilliumWeb-Regular.ttf";
        private const string rutaFuenteTitiliumBold = "fonts/titillium_web/TitilliumWeb-SemiBold.ttf";
        private const string tituloDiago = "Banco Security";
        private const string mensajeSpinnerGenerico = "Cargando datos...";
        private const string mensajeSpinnerCerrarSesion = "Cerrando sesión ...";
        private const string mensajeDialogoOnBackPressed = "¿Está seguro que desea salir de la aplicación?";
        private const string mensajeDialogoOnBackPressedPositiveButton = "Si";
        private const string mensajeDialogoOnBackPressedNegativeButton = "No";
        private const string mensajeDialogoOnBackPressedConfirmacion = "Por favor confirme si desea cerrar su sesión";
        private const string mensajeDialogoOnBackPressedPositiveButtonConfirmacion = "Aceptar";
        private const string mensajeDialogoOnBackPressedNegativeButtonConfirmacion = "Cancelar";

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Home);
            nombreConocido = MainActivity.returnNombreConsultaEnrolado();
            nombreDesdeConfirmacion = ConfirmacionEnroladoActivity.returnNombreConsultaEnroladoConocido();
           // NotificacionesFragment NotificacionesFragment = new NotificacionesFragment();
            nombreTitulo = nombreConocido;
            if (string.IsNullOrEmpty(nombreTitulo))
            {
                nombreTitulo = nombreDesdeConfirmacion;
            }
            string nombreLogin = nombreTitulo;
            nombreUsuario = nombreLogin.Split(' ');
            primerNombreUsuario = nombreUsuario[0].ToLower();
            segundoNombreUsuario = nombreUsuario[1].ToLower();
            string primeraLetraPrimerNombre = primerNombreUsuario.Substring(0, 1).ToUpper();
            string restoPrimernombreFormateado = primerNombreUsuario.Substring(1, primerNombreUsuario.Length - 1);
            primerNombreFormateado = primeraLetraPrimerNombre + restoPrimernombreFormateado;
            if (!string.IsNullOrEmpty(segundoNombreUsuario))
            {
                string primeraLetraSegundoNombre = segundoNombreUsuario.Substring(0, 1).ToUpper();
                string restoSegundonombreFormateado = segundoNombreUsuario.Substring(1, segundoNombreUsuario.Length - 1);
                segundoNombreFormateado = primeraLetraSegundoNombre + restoSegundonombreFormateado;
            }
            string nombreUserFormateado = primerNombreFormateado + " " + segundoNombreFormateado;
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            btnBurger = FindViewById<ImageView>(Resource.Id.btnBurger);
            btnCamapanaNotificaciones = FindViewById<ImageView>(Resource.Id.btnCamapanaNotificaciones);
            btnCamapanaNotificacionesNoLeida = FindViewById<ImageView>(Resource.Id.btnCamapanaNotificacionesNoLeida);
            lblContadorNotificacionesNoLeidas = FindViewById<TextView>(Resource.Id.lblContadorNotificacionesNoLeidas);
            relativeLayoutCampanaNotificacionesContador = FindViewById<RelativeLayout>(Resource.Id.relativeLayoutCampanaNotificacionesContador);
            notificacionesUtil = new NotificacionesUtil(this);
            preferenciasNotificacionesLeidas = new PreferenciasNotificacionesLeidas();
            listadoFinal = await notificacionesUtil.Llamarservicio(this, UtilAndroid.getRut());
            int contadorNotificaciones = preferenciasNotificacionesLeidas.getContadorNotificacionesNoLeidas(this, listadoFinal);
            if (contadorNotificaciones == 0)
            {
                relativeLayoutCampanaNotificacionesContador.Visibility = ViewStates.Gone;
                btnCamapanaNotificaciones.Visibility = ViewStates.Visible;
                btnCamapanaNotificaciones.Click += BtnCamapanaNotificaciones_Click;
            }else{
                lblContadorNotificacionesNoLeidas.Text = contadorNotificaciones+string.Empty;
                relativeLayoutCampanaNotificacionesContador.Visibility = ViewStates.Visible;
                btnCamapanaNotificaciones.Visibility = ViewStates.Gone;
                btnCamapanaNotificacionesNoLeida.Click += BtnCamapanaNotificacionesNoLeida_Click;
            }
            btnBurger.Click += BtnBurger_Click;
            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            View hView = navigationView.GetHeaderView(cero);
            lblUsuarioMenu = hView.FindViewById<TextView>(Resource.Id.lblUsuarioMenu);
            lblUsuarioMenu.Text = nombreUserFormateado;
            var fontRegular = Typeface.CreateFromAsset(Assets, rutaFuenteTitilium);
            var fontBold = Typeface.CreateFromAsset(Assets, rutaFuenteTitiliumBold);
            lblUsuarioMenu.Typeface = fontRegular;
            cambiarFuenteMenu();
            var fragmentOnOff = FragmentManager.BeginTransaction();
            fragmentOnOff.AddToBackStack(null);
            fragmentOnOff.Add(Resource.Id.HomeLayout, new OnOffFragment());
            fragmentOnOff.Commit();
            dialogoLoadingBcoSecurityActivity = new DialogoLoadingBcoSecurityActivity(this);
        }

        private void cambiarFuenteMenu(){
            // cambio tipografia menu!
            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            var fontBold = Typeface.CreateFromAsset(Assets, rutaFuenteTitiliumBold);
            int totalElementoMenu = navigationView.Menu.Size();
            for (int i = 0; i < totalElementoMenu; i++)
            {
                IMenuItem menuItem = navigationView.Menu.GetItem(i);
                if (menuItem != null)
                {
                    string title = menuItem.TitleFormatted.ToString();
                    SpannableString spannableTitle = new SpannableString(title);
                    spannableTitle.SetSpan(new Android.Text.Style.TypefaceSpan(rutaFuenteTitiliumBold), 0, title.Length, 0);
                    menuItem.SetTitle(spannableTitle);
                }
            }
        }

        private void BtnBurger_Click(object sender, EventArgs e)
        {
            drawerLayout.OpenDrawer(GravityCompat.End); 
			if (botonCerrarMenu == null)
            {
                using(botonCerrarMenu = FindViewById<ImageView>(Resource.Id.botonCerrarMenu)){
                    botonCerrarMenu.Click += eventoCerrarMenu;
                }
            }
        }

        private void eventoCerrarMenu(object sender, EventArgs e)
        {
            drawerLayout.CloseDrawer(GravityCompat.End);
        }

        private void BtnCamapanaNotificacionesNoLeida_Click(object sender, EventArgs e)
        {
            var ftNotificaciones = FragmentManager.BeginTransaction();
            ftNotificaciones.AddToBackStack(null);
            ftNotificaciones.Add(Resource.Id.HomeLayout, new NotificacionesFragment());
            ftNotificaciones.Commit();

        }

        private void BtnCamapanaNotificaciones_Click(object sender, EventArgs e)
        {
            var ftNotificaciones = FragmentManager.BeginTransaction();
            ftNotificaciones.AddToBackStack(null);
            ftNotificaciones.Add(Resource.Id.HomeLayout, new NotificacionesFragment());
            ftNotificaciones.Commit(); 

        }

        void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.nav_onoff):
                    using(var fragmentOnOff = FragmentManager.BeginTransaction()){
                        fragmentOnOff.AddToBackStack(null);
                        fragmentOnOff.Add(Resource.Id.HomeLayout, new OnOffFragment());
                        fragmentOnOff.Commit();
                        break;
                    }
                case (Resource.Id.nav_notificaciones):
                    using (var ftNotificaciones = FragmentManager.BeginTransaction())
                    {
                        ftNotificaciones.AddToBackStack(null);
                        ftNotificaciones.Add(Resource.Id.HomeLayout, new NotificacionesFragment());
                        ftNotificaciones.Commit();
                        break;
                    }
                case (Resource.Id.nav_configurarnoti):
                    using (var ftConfigurarNoti = FragmentManager.BeginTransaction())
                    {
                        ftConfigurarNoti.AddToBackStack(null);
                        ftConfigurarNoti.Add(Resource.Id.HomeLayout, new ConfigurarNotificacionesFragment());
                        ftConfigurarNoti.Commit();
                        break; 
                    }
                case (Resource.Id.nav_terminos):
                    using(var ftTerminos = FragmentManager.BeginTransaction()){
                        ftTerminos.AddToBackStack(null);
                        ftTerminos.Add(Resource.Id.HomeLayout, new TerminoYCondicionesFragment());
                        ftTerminos.Commit();
                        break; 
                    }
                //case (Resource.Id.nav_adminhuella):
                //    var ftAdminhuella = FragmentManager.BeginTransaction();
                //    ftAdminhuella.AddToBackStack(null);
                //    ftAdminhuella.Add(Resource.Id.HomeLayout, new FingerPrintAdminFragment());
                //    ftAdminhuella.Commit();
                //    break; 

                case (Resource.Id.nav_cerrarsesion):
                    dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
                    using (Intent intentCerrarSesion = new Intent(this, typeof(LoginConocidoActivity)))
                    {
                        StartActivity(intentCerrarSesion);
                        break;
                    }
            }
            // Close drawer
            drawerLayout.CloseDrawers();
        }
        public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount != cero)
            {
                FragmentManager.PopBackStack();
            }
            else
            {
                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                builder.SetTitle(tituloDiago);
                builder.SetMessage(mensajeDialogoOnBackPressed);
                builder.SetPositiveButton(mensajeDialogoOnBackPressedPositiveButton, delegate {
                    Android.App.AlertDialog.Builder cerrarSesionModal = new Android.App.AlertDialog.Builder(this);
                    cerrarSesionModal.SetTitle(tituloDiago);
                    cerrarSesionModal.SetMessage(mensajeDialogoOnBackPressedConfirmacion);
                    cerrarSesionModal.SetPositiveButton(mensajeDialogoOnBackPressedPositiveButtonConfirmacion, delegate {
                        Intent i = new Intent(this, typeof(LoginConocidoActivity));
                        StartActivity(i);
                    });
                    cerrarSesionModal.SetNegativeButton(mensajeDialogoOnBackPressedNegativeButtonConfirmacion, delegate { });
                    cerrarSesionModal.Show();
                });
                builder.SetNegativeButton(mensajeDialogoOnBackPressedNegativeButton, delegate { });
                builder.Show();

            }
        }
    }
}