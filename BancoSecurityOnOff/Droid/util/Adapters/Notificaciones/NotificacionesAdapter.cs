using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using BancoSecurityOnOff.Droid.Bean;
using BancoSecurityOnOff.Droid.Notificaciones;
using static Android.App.ActionBar;

namespace BancoSecurityOnOff.Droid.Resources.NotificacionesAdapter
{
    public class NotificacionesAdapter : BaseAdapter<Notificacion>
    {
        Activity _context;
        string horaFormateada;
        string horaNotificacion;
        string minutoNotificacion;
        string[] hora;
        List<Notificacion> _notificaciones;
        NotificacionesUtil notificacionesUtil;
        LinearLayout llFondoItemNotificacion;
        PreferenciasNotificacionesLeidas preferenciasNotificacionesLeidas;
        private const string rutaFuenteTitiliumRegular = "fonts/titillium_web/TitilliumWeb-Regular.ttf";
        private const string rutaFuenteTitiliumSemiBold = "fonts/titillium_web/TitilliumWeb-SemiBold.ttf";

        public NotificacionesAdapter(Activity context, List<Notificacion> notificaciones)
        {
            _context = context;
            _notificaciones = notificaciones;
            horaFormateada = string.Empty;
            horaNotificacion = string.Empty;
            minutoNotificacion = string.Empty;
            notificacionesUtil = new NotificacionesUtil(_context);
            preferenciasNotificacionesLeidas = new PreferenciasNotificacionesLeidas();
        }

        public override Notificacion this[int position] => _notificaciones[position];

		public override int Count => _notificaciones.Count;

        public override long GetItemId(int position)
        {
            return this[position].id;
        }

        public Notificacion GetNotificacion(int position)
        {
            return this[position];
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = this[position];
            var fontRegular = Typeface.CreateFromAsset(_context.Assets, rutaFuenteTitiliumRegular);
            var fontSemiBold = Typeface.CreateFromAsset(_context.Assets, rutaFuenteTitiliumSemiBold);

            if (!item.esFecha)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.NotificacionesRow, null);
                string horaFormatoCorrecto = this.formateadorHora(item.hora);
                convertView.FindViewById<TextView>(Resource.Id.horaNotificacion).Text = horaFormatoCorrecto;

                if (item.estado)
                {
                    convertView.FindViewById<TextView>(Resource.Id.contenidoNotificacion).Text = item.mensajeNotificacion;
                    convertView.FindViewById<TextView>(Resource.Id.contenidoNotificacion).Typeface = fontRegular;
                    convertView.FindViewById<TextView>(Resource.Id.contenidoNotificacion).LayoutParameters.Height = LayoutParams.WrapContent;
                    convertView.FindViewById<ImageView>(Resource.Id.chevron).SetImageResource(Resource.Drawable.exp2);
                    convertView.FindViewById<ImageView>(Resource.Id.ic_noLeida).Visibility = ViewStates.Invisible;
                    convertView.FindViewById<TextView>(Resource.Id.horaNotificacion).Typeface = fontRegular;
                    convertView.FindViewById<LinearLayout>(Resource.Id.llFondoItemNotificacion).SetBackgroundColor(Color.Rgb(255, 255, 255));
                }
                else
                {
                    convertView.FindViewById<LinearLayout>(Resource.Id.llFondoItemNotificacion).SetBackgroundColor(Color.Rgb(245, 245, 245));
                    convertView.FindViewById<TextView>(Resource.Id.contenidoNotificacion).Text = item.mensajeNotificacionCorto;
                    convertView.FindViewById<TextView>(Resource.Id.contenidoNotificacion).Typeface = fontSemiBold;
                    convertView.FindViewById<TextView>(Resource.Id.contenidoNotificacion).LayoutParameters.Height = LayoutParams.WrapContent;
                    convertView.FindViewById<ImageView>(Resource.Id.chevron).SetImageResource(Resource.Drawable.exp1);
                    convertView.FindViewById<ImageView>(Resource.Id.ic_noLeida).Visibility = ViewStates.Visible;
                    List<string> listadoDeNotificacionesLeidas = preferenciasNotificacionesLeidas.obneterListadoDeNotificacionesLeidas(_context);
                    convertView.FindViewById<TextView>(Resource.Id.horaNotificacion).Typeface = fontSemiBold;
                    foreach (var notificacionLeida in listadoDeNotificacionesLeidas)
                    {
                        if (item.idNotificacion.Equals(notificacionLeida))
                        {
                            convertView.FindViewById<ImageView>(Resource.Id.ic_noLeida).Visibility = ViewStates.Invisible;
                            convertView.FindViewById<TextView>(Resource.Id.contenidoNotificacion).Typeface = fontRegular;
                            convertView.FindViewById<TextView>(Resource.Id.horaNotificacion).Typeface = fontRegular;
                            convertView.FindViewById<LinearLayout>(Resource.Id.llFondoItemNotificacion).SetBackgroundColor(Color.Rgb(255, 255, 255));
                        }
                    }
                }




            }
            else
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.FechaRow, null);
                convertView.FindViewById<TextView>(Resource.Id.lblFecha).Text = item.mensajeNotificacion;
                convertView.FindViewById<TextView>(Resource.Id.lblFecha).Typeface = fontSemiBold;
            }
            return convertView;
        }


        public string formateadorHora(string itemHoraParam){
            
            string horaMinFormateada = itemHoraParam;

            hora = horaMinFormateada.Split(':');
            horaNotificacion = hora[0];
            if (horaMinFormateada.Contains(":"))
            {
                minutoNotificacion = hora[1];
            }else{
                minutoNotificacion = "00";
            }


            if (horaMinFormateada.Contains("p. m."))
            {
                if (horaNotificacion.Equals(" 12"))
                {
                    horaNotificacion = "12";
                }
                else if (horaNotificacion.Equals(" 1"))
                {
                    horaNotificacion = "13";
                }
                else if (horaNotificacion.Equals(" 2"))
                {
                    horaNotificacion = "14";
                }
                else if (horaNotificacion.Equals(" 3"))
                {
                    horaNotificacion = "15";
                }
                else if (horaNotificacion.Equals(" 4"))
                {
                    horaNotificacion = "16";
                }
                else if (horaNotificacion.Equals(" 5"))
                {
                    horaNotificacion = "17";
                }
                else if (horaNotificacion.Equals(" 6"))
                {
                    horaNotificacion = "18";
                }
                else if (horaNotificacion.Equals(" 7"))
                {
                    horaNotificacion = "19";
                }
                else if (horaNotificacion.Equals(" 8"))
                {
                    horaNotificacion = "20";
                }
                else if (horaNotificacion.Equals(" 9"))
                {
                    horaNotificacion = "21";
                }
                else if (horaNotificacion.Equals(" 10"))
                {
                    horaNotificacion = "22";
                }
                else if (horaNotificacion.Equals(" 11"))
                {
                    horaNotificacion = "23";
                }
            }
            else if (horaMinFormateada.Contains("a. m."))
            {
                if (horaNotificacion.Equals(" 12"))
                {
                    horaNotificacion = "00";
                }
                else if (horaNotificacion.Equals(" 1"))
                {
                    horaNotificacion = "01";
                }
                else if (horaNotificacion.Equals(" 2"))
                {
                    horaNotificacion = "02";
                }
                else if (horaNotificacion.Equals(" 3"))
                {
                    horaNotificacion = "03";
                }
                else if (horaNotificacion.Equals(" 4"))
                {
                    horaNotificacion = "04";
                }
                else if (horaNotificacion.Equals(" 5"))
                {
                    horaNotificacion = "05";
                }
                else if (horaNotificacion.Equals(" 6"))
                {
                    horaNotificacion = "06";
                }
                else if (horaNotificacion.Equals(" 7"))
                {
                    horaNotificacion = "07";
                }
                else if (horaNotificacion.Equals(" 8"))
                {
                    horaNotificacion = "08";
                }
                else if (horaNotificacion.Equals(" 9"))
                {
                    horaNotificacion = "09";
                }
                else if (horaNotificacion.Equals(" 10"))
                {
                    horaNotificacion = "10";
                }
                else if (horaNotificacion.Equals(" 11"))
                {
                    horaNotificacion = "11";
                }
            }

            string horaMinutosFormateado = horaNotificacion + ":" + minutoNotificacion;
            return horaMinutosFormateado;
        }
    }
}