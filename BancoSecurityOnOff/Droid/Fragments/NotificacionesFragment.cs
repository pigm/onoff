using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using BancoSecurityOnOff.Droid.Bean;
using BancoSecurityOnOff.Utilidades.Firmas;
using System.Globalization;
using BancoSecurityOnOff.Droid.Resources.NotificacionesAdapter;
using BancoSecurityOnOff.Droid.Notificaciones;
using BancoSecurityOnOff.Droid.Util;

namespace BancoSecurityOnOff.Droid
{
    public class NotificacionesFragment : Fragment
    {
        NotificacionesUtil notificacionesUtil;
        PreferenciasNotificacionesLeidas preferenciasNotificacionesLeidas;
        ListView NotificacionesListView;
        static List<Notificacion> listadoFinal;
        const string nombreCultura = "es-ES";
        TextView lblMarcarComoLeida;
        ImageView btnCamapanaNotificaciones;
        ImageView btnCamapanaNotificacionesNoLeida;
        TextView lblContadorNotificacionesNoLeidas;
        RelativeLayout relativeLayoutCampanaNotificacionesContador;
        NotificacionesAdapter adapterHistoricoNotificaciones;
        List<String> listNotificacionLeidas;
        DialogoLoadingBcoSecurityActivity dialogoLoadingBcoSecurityActivity;
         
        public NotificacionesFragment(){
            preferenciasNotificacionesLeidas = new PreferenciasNotificacionesLeidas();
        }

        public string NombreMes(int mes){
            DateTimeFormatInfo dtinfo = new CultureInfo(nombreCultura, false).DateTimeFormat;
            return dtinfo.GetMonthName(mes);
        }

        public override void OnCreate(Bundle savedInstanceState){
            base.OnCreate(savedInstanceState);
        }

        protected void setUpViews(){
            NotificacionesListView = View.FindViewById<ListView>(Resource.Id.notificacionesListView);
        }

        public void setCortarCadenas(string cadena, ref string mitad1, ref string mitad2){
            int mitad = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cadena.Length / 2)));
            mitad1 = cadena.Substring(0, mitad);
            mitad2 = cadena.Substring(mitad, cadena.Length - mitad);
        } 
    

        /// Metodo que envia los rescursos de la lista al adaptador.
		public async override void OnActivityCreated(Bundle savedInstanceState){
            base.OnActivityCreated(savedInstanceState);
            setUpViews();
            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            await notificacionesUtil.Llamarservicio(this.Activity, UtilAndroid.getRut());
            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            var agrupacion = notificacionesUtil.NotificacionesPorFecha();

            listadoFinal = new List<Notificacion>();
            foreach (var grupo in agrupacion)
            {
                var notifFecha = new Notificacion();
                notifFecha.esFecha = true;
                DateTime fecha = DateTime.Parse(grupo.Key, CultureInfo.CurrentCulture);
                var mes = NombreMes(fecha.Month);
                var fechaFormat = fecha.Day + " " + mes; 
                notifFecha.mensajeNotificacion = fechaFormat;

                listadoFinal.Add(notifFecha);
                listadoFinal.AddRange(grupo.ToList());
            }
            adapterHistoricoNotificaciones = new NotificacionesAdapter(Activity, listadoFinal);
            NotificacionesListView.Adapter = adapterHistoricoNotificaciones;

            NotificacionesListView.ItemClick += (sender, e) => {
                Bean.Notificacion notiSelected = adapterHistoricoNotificaciones.GetNotificacion(e.Position);
                foreach (var notificacion in listadoFinal){
                    if (notificacion.idNotificacion == notiSelected.idNotificacion){
                        if (notificacion.estado){
                            notificacion.estado = false;
                        }else{
                            notificacion.estado = true;
                        }

                    }
                }
                listNotificacionLeidas = preferenciasNotificacionesLeidas.obneterListadoDeNotificacionesLeidas(Activity);
                if (listNotificacionLeidas.Count == 0){
                    if (listNotificacionLeidas == null){
                        listNotificacionLeidas.Remove(null);
                    }
                    preferenciasNotificacionesLeidas.guardarPreferencia(Activity, notiSelected.idNotificacion); 
                }else{
                    foreach (string notificacionLeida in listNotificacionLeidas){
                        if (!notificacionLeida.Equals(notiSelected.idNotificacion)){
                            preferenciasNotificacionesLeidas.guardarPreferencia(Activity, notiSelected.idNotificacion);
                        }
                    }
                }

                if (listNotificacionLeidas != null || listNotificacionLeidas.Count != 0){
                    int contadorDeNotificacionesRefrescado = preferenciasNotificacionesLeidas.getContadorNotificacionesNoLeidas(Activity, listadoFinal);
                    lblContadorNotificacionesNoLeidas = Activity.FindViewById<TextView>(Resource.Id.lblContadorNotificacionesNoLeidas);
                    lblContadorNotificacionesNoLeidas.Text = contadorDeNotificacionesRefrescado + string.Empty;
                }
                adapterHistoricoNotificaciones.NotifyDataSetChanged();
            };

            lblMarcarComoLeida.Click += delegate {
                foreach (var notificacion in listadoFinal)
                {
                    listNotificacionLeidas = preferenciasNotificacionesLeidas.obneterListadoDeNotificacionesLeidas(Activity);
                    if (listNotificacionLeidas.Count == 0)
                    {
                        if (listNotificacionLeidas == null)
                        {
                            listNotificacionLeidas.Remove(null);
                        }
                        preferenciasNotificacionesLeidas.guardarPreferencia(Activity, notificacion.idNotificacion);
                    }
                    else
                    {
                        preferenciasNotificacionesLeidas.guardarPreferencia(Activity, notificacion.idNotificacion);
                    }
                    notificacion.estado = true;
                }
  
                relativeLayoutCampanaNotificacionesContador.Visibility = ViewStates.Gone;
                btnCamapanaNotificaciones.Visibility = ViewStates.Visible;

                adapterHistoricoNotificaciones.NotifyDataSetChanged();
            };
        }

        /// Metodo que instancia la vista y recursos de la vista y la devuelve
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            notificacionesUtil = new NotificacionesUtil(this.Activity);
            View view = inflater.Inflate(Resource.Layout.Notificaciones, container, false);
            TextView lblNotificacionesTitulo = view.FindViewById<TextView>(Resource.Id.lblNotificacionesTitulo);
            lblMarcarComoLeida = view.FindViewById<TextView>(Resource.Id.lblMarcarComoLeida);
            btnCamapanaNotificaciones = Activity.FindViewById<ImageView>(Resource.Id.btnCamapanaNotificaciones);
            btnCamapanaNotificacionesNoLeida = Activity.FindViewById<ImageView>(Resource.Id.btnCamapanaNotificacionesNoLeida);
            lblContadorNotificacionesNoLeidas = Activity.FindViewById<TextView>(Resource.Id.lblContadorNotificacionesNoLeidas);
            relativeLayoutCampanaNotificacionesContador = Activity.FindViewById<RelativeLayout>(Resource.Id.relativeLayoutCampanaNotificacionesContador);
            NotificacionesListView = view.FindViewById<ListView>(Resource.Id.notificacionesListView);
            var font = Typeface.CreateFromAsset(Activity.Assets, ConstantesSecurity.fuente);
            lblNotificacionesTitulo.Typeface = font;
            lblMarcarComoLeida.Typeface = font;
            dialogoLoadingBcoSecurityActivity = new DialogoLoadingBcoSecurityActivity(Activity);
            return view;
        }
    }
}
