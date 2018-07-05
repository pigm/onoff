using System;
using Android.Widget;
using Android.App;
using Android.Content;
using Android.Support.V4.View;
using Android.Views;
using System.Collections.Generic;
using System.Json;
using BancoSecurityOnOff.Utilidades.Firmas;
using Android.Graphics;
using BancoSecurityOnOff.Droid.Constantes;
using BancoSecurityOnOff.Droid.Util;

/**
   @autor:          PaBlo Garrido
   @cargo:          Ingeniero de Software
   @institucion:    TInet soluciones informaticas
   @modificaciones: 
   @version:        1.0.0.0
   @proyecto:       OnOff
   
*/
namespace BancoSecurityOnOff.Droid.Resources.OnOffAdapter
{
	public class OnOffAdapter : PagerAdapter
    {
        Activity activity;
        List<OnOffData> datosTarjeta;
		List<View> listadoItemsView = new List<View>();
        LayoutInflater layoutInflater;

		TextView lblCanalComprasPresenciales;
		TextView lblCanalComprasPorInternet;
		TextView lblCanalGirosCajerosAutomaticos;
		TextView lblCanalComprasViaTelefonica;
        string rutConocido;
        string rutDesdeConfirmacion;
        string rutDefinitivo;
        ParametriaLogUtil parametriaLogUtil;
        DialogoLoadingBcoSecurityActivity dialogoLoadingBcoSecurityActivity;

        ViewPager componenteCarrusel;

        public OnOffAdapter(Activity activity, List<OnOffData> datosTarjeta,ViewPager componenteCarrusel)
        {
            this.activity = activity;
			this.datosTarjeta = datosTarjeta;
            this.componenteCarrusel = componenteCarrusel;
            dialogoLoadingBcoSecurityActivity = new DialogoLoadingBcoSecurityActivity(activity);
        }

        public override int Count
        {
            get
            {
				return datosTarjeta.Count;
            }
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object objectvalue)
        {
            return view == objectvalue;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
		{
			View itemView = obtenerVista(position);
			if(itemView != null){ 
				container.AddView(itemView);
				return itemView;
			}
			layoutInflater = (LayoutInflater) activity.ApplicationContext.GetSystemService(Context.LayoutInflaterService);
			itemView = layoutInflater.Inflate(Resource.Layout.OnOffTarjetaCredito, container, false);
            ImageView cardImage = (ImageView)itemView.FindViewById<ImageView>(Resource.Id.imgCardBcoSecurity);
			Switch swtNameCard = itemView.FindViewById<Switch>(Resource.Id.swtNameCard);
			TextView nameCard = itemView.FindViewById<TextView>(Resource.Id.lblNameCard);
            TextView statusCard = itemView.FindViewById<TextView>(Resource.Id.lblStatusCard);
            TextView lblInfo = itemView.FindViewById<TextView>(Resource.Id.lblInfo);
			TextView numeroTarjeta = itemView.FindViewById<TextView>(Resource.Id.numero_tarjeta);
            TextView nombreTarjeta = itemView.FindViewById<TextView>(Resource.Id.nombre_tarjeta);

            lblCanalComprasPresenciales = itemView.FindViewById<TextView>(Resource.Id.lblCanalComprasPresenciales);
            lblCanalComprasPorInternet = itemView.FindViewById<TextView>(Resource.Id.lblCanalComprasPorInternet);
            lblCanalGirosCajerosAutomaticos = itemView.FindViewById<TextView>(Resource.Id.lblCanalGirosCajerosAutomaticos);
            lblCanalComprasViaTelefonica = itemView.FindViewById<TextView>(Resource.Id.lblCanalComprasViaTelefonica);

            var fontBold = Typeface.CreateFromAsset(activity.Assets,ConstantesOnOff.rutaFuenteTitiliumBold);
            lblCanalComprasPresenciales.Typeface = fontBold;
            lblCanalComprasPorInternet.Typeface = fontBold;
            lblCanalGirosCajerosAutomaticos.Typeface = fontBold;
            lblCanalComprasViaTelefonica.Typeface = fontBold;
            parametriaLogUtil =new ParametriaLogUtil();
            LinearLayout panelCanales = (LinearLayout) itemView.FindViewById<LinearLayout>(Resource.Id.panelCanales);

            cardImage.SetBackgroundResource(datosTarjeta[position].ImageId);
            string tipoTarjeta = datosTarjeta[position].TipoProducto;
			string pan = datosTarjeta[position].Pan;
            string nombreTarjetaVista = datosTarjeta[position].NombreTarjeta;
            string glosaProducto = datosTarjeta[position].GlosaProducto;
            setearNumeroTarjetaFormateado(pan, numeroTarjeta, tipoTarjeta, nombreTarjeta, nombreTarjetaVista);

            if (tipoTarjeta.Equals(ConstantesOnOff.TIPO_TARJETA_DEBITO))
            {
                nameCard.Text = ConstantesOnOff.TEXTO_DEBITO;
				panelCanales.Visibility = ViewStates.Gone;
				itemView = consultarEstadoTarjetaDebito(position, itemView,swtNameCard);
            }
            else if (tipoTarjeta.Equals(ConstantesOnOff.TIPO_TARJETA_CREDITO))
            {
                if (glosaProducto.Equals(ConstantesOnOff.GLOSA_MASTERCARD_BLACK))
                {
                    nameCard.Text = ConstantesOnOff.TEXTO_MASTERCARD_BLACK;
                }else if (glosaProducto.Equals(ConstantesOnOff.GLOSA_MASTERCARD_ONE))
                {
                    nameCard.Text = ConstantesOnOff.TEXTO_MASTERCARD_ONE;
                }else{
                    nameCard.Text = ConstantesOnOff.TEXTO_CREDITO;
                }
                panelCanales.Visibility = ViewStates.Visible;
				itemView = consultarEstadoTarjetaCredito(position, itemView,swtNameCard);
				Switch swtCanalComprasPresenciales = itemView.FindViewById<Switch>(Resource.Id.swtCanalComprasPresenciales);
                Switch swtCanalComprasPorInternet = itemView.FindViewById<Switch>(Resource.Id.swtCanalComprasPorInternet);
                Switch swtCanaLGirosCajerosAutomaticos = itemView.FindViewById<Switch>(Resource.Id.swtCanaLGirosCajerosAutomaticos);
                Switch swtCanaLComprasViaTelefonica = itemView.FindViewById<Switch>(Resource.Id.swtCanaLComprasViaTelefonica);
				swtCanalComprasPresenciales.SetTag(Resource.Id.swtCanalComprasPresenciales, position);
				swtCanalComprasPorInternet.SetTag(Resource.Id.swtCanalComprasPorInternet, position);
				swtCanaLGirosCajerosAutomaticos.SetTag(Resource.Id.swtCanaLGirosCajerosAutomaticos, position);
				swtCanaLComprasViaTelefonica.SetTag(Resource.Id.swtCanaLComprasViaTelefonica, position);
            }
			string positionString = position.ToString();
			swtNameCard.SetTag(Resource.Id.swtNameCard,position);
			swtNameCard.CheckedChange  += eventoCambioEstadoOnOFF;
			guardarVista(itemView, position);
			iniciarIndicadorPagina(datosTarjeta.Count, position);
			container.AddView(itemView);
            ImageView flechaCarruselAnterior = (ImageView)itemView.FindViewById<ImageView>(Resource.Id.flechaCarruselAnterior);
            ImageView flechaCarruselSiguiente = (ImageView)itemView.FindViewById<ImageView>(Resource.Id.flechaCarruselSiguiente);
            flechaCarruselAnterior.Click += eventoFlechaCarruselAnterior;
            flechaCarruselSiguiente.Click += eventoFlechaCarruselSiguiente;
            validarEstadoFlechas(position);
            if(tipoTarjeta.Equals(ConstantesOnOff.TIPO_TARJETA_CREDITO)){
                cambiarEstadoPanelCanales(position, statusCard.Text);
            }
            return itemView;
		}

        void eventoFlechaCarruselAnterior(object sender, EventArgs e)
        {
            int indice = componenteCarrusel.CurrentItem - 1;
            if(indice < 0 ){
                indice = 0;
            }

            cambiarPaginaCarrusel(indice);
        }

        void eventoFlechaCarruselSiguiente(object sender, EventArgs e)
        {
            int indice = componenteCarrusel.CurrentItem + 1;
            if (indice <= componenteCarrusel.ChildCount )
            {
                cambiarPaginaCarrusel(indice);
            }
        }

        void cambiarPaginaCarrusel(int indice){
            componenteCarrusel.SetCurrentItem(indice, true);
        }

        // faltan metodos para estado icono flecha ( activado , desactivado ).
        void validarEstadoFlechas(int position){
            View itemView = obtenerVista(position);
            ImageView flechaCarruselAnterior = (ImageView)itemView.FindViewById<ImageView>(Resource.Id.flechaCarruselAnterior);
            ImageView flechaCarruselSiguiente = (ImageView)itemView.FindViewById<ImageView>(Resource.Id.flechaCarruselSiguiente);
            int indiceActual = position;//componenteCarrusel.CurrentItem;
            int indiceAnterior = indiceActual - 1;
            int indiceSiguiente = indiceActual + 1;
            int totalElementos = datosTarjeta.Count;
            if(indiceAnterior < 0 ){
                flechaCarruselAnterior.SetImageResource(Resource.Drawable.flechacarruselizquerdadesactiva);
            }else{
                flechaCarruselAnterior.SetImageResource(Resource.Drawable.flechaCarruselIzquierda);
            }

            if (indiceSiguiente >= totalElementos)
            {
                flechaCarruselSiguiente.SetImageResource(Resource.Drawable.flechacarruselderechadesactiva);
            }
            else
            {
                flechaCarruselSiguiente.SetImageResource(Resource.Drawable.flechaCarruselDerecha);
            }
        }

        private void setearNumeroTarjetaFormateado(string pan,TextView textView,string tipoTarjeta, TextView nombreTarjeta, string nombreTarjetaVista){
			if(pan != null && pan.Length < 4){
				textView.Text = "";
				return;
			}
            var font = Typeface.CreateFromAsset(activity.Assets, ConstantesOnOff.rutaFuenteTitiliumBold);
			textView.Typeface = font;
            nombreTarjeta.Typeface = font;
			int inicio = pan.Length - 4;
			string ultimosCuatroNumeros = pan.Substring(inicio,4);
			string tarjetaMascara = "XXXX XXXX XXXX " + ultimosCuatroNumeros;
			textView.Text = tarjetaMascara;
            nombreTarjeta.Text = nombreTarjetaVista;
            if (tipoTarjeta.Equals(ConstantesOnOff.TIPO_TARJETA_DEBITO))
            {
				textView.SetTextColor(textView.Context.Resources.GetColor(Resource.Color.security_plomo_tarjeta));
                nombreTarjeta.SetTextColor(textView.Context.Resources.GetColor(Resource.Color.security_plomo_tarjeta));
            }
			else{
				textView.SetTextColor(textView.Context.Resources.GetColor(Resource.Color.white));
                nombreTarjeta.SetTextColor(textView.Context.Resources.GetColor(Resource.Color.white));
            }
		}

		private void guardarVista(View item,int position){
			listadoItemsView.Insert(position, item);
		}

		private View obtenerVista(int position){
			bool validador = false;
			for (int i = 0; i < listadoItemsView.Count; i++){
				if(i == position){
					validador = true;
					break;
				}
			}
			if(validador){
				View vista = listadoItemsView[position];	
                return vista;	
			}
			return null;
		}

        public View consultarEstadoTarjetaCredito(int position, View itemView,Switch switchEstadoTarjeta)
        {
            try
            {
                OnOffData detalleTarjeta = (OnOffData) datosTarjeta[position];
                TextView nameCard = itemView.FindViewById<TextView>(Resource.Id.lblNameCard);
                TextView statusCard = itemView.FindViewById<TextView>(Resource.Id.lblStatusCard);
                TextView lblInfo = itemView.FindViewById<TextView>(Resource.Id.lblInfo);
                JsonValue jsonResponseAccessToken = WebServiceSecurity.ServiciosSecurity.CallRESTToken();
                if (!string.IsNullOrEmpty(jsonResponseAccessToken.ToString()))
                {
                    JsonValue token = jsonResponseAccessToken["access_token"];
                    JsonValue jsonResponseCheckCardStatus = detalleTarjeta.estadoTarjetaCredito;
                    string jsonResponseCheckCardStatus_StatusCode = jsonResponseCheckCardStatus["statusCode"];
                    string estadoTarjetaCredito = jsonResponseCheckCardStatus["estado"];
                    if (estadoTarjetaCredito.Equals(ConstantesOnOff.ESTADO_ACTIVADO_1))
                    {
                        switchEstadoTarjeta.Checked = true;
                        statusCard.Text = ConstantesOnOff.TEXTO_ESTADO_ON;
                        statusCard.SetTextColor(statusCard.Context.Resources.GetColor(Resource.Color.security_on_off_activado));
                        lblInfo.Text = ConstantesOnOff.GLOSA_CREDITO_ON;
                    }
                    else
                    {
                        switchEstadoTarjeta.Checked = false;
                        statusCard.Text = ConstantesOnOff.TEXTO_ESTADO_OFF;
                        statusCard.SetTextColor(statusCard.Context.Resources.GetColor(Resource.Color.security_on_off_desactivado));
                        lblInfo.Text = ConstantesOnOff.GLOSA_CREDITO_OFF;
                    }
                    var font = Typeface.CreateFromAsset(activity.Assets, ConstantesOnOff.rutaFuenteTitilium);
                    lblInfo.Typeface = font;
                    statusCard.Typeface = font;
                    nameCard.Typeface = font;
                }
                else
                {
                    DialogoErrorActivity.mostrarViewErrorHome(activity);
                }
            }
            catch (System.Exception x)
            {
                Console.WriteLine(x);
                DialogoErrorActivity.mostrarViewErrorHome(activity);
            }
            return itemView;
		}

        private void cambiarEstadoSwitchCanales(JsonValue jsonResponseCheckCardStatus,View itemView){ // estado visual
			string estadoPos = jsonResponseCheckCardStatus["estadoPos"];
			string estadoEcom = jsonResponseCheckCardStatus["estadoEcom"];
			string estadoAtm = jsonResponseCheckCardStatus["estadoAtm"];
			string estadoMoto = jsonResponseCheckCardStatus["estadoMoto"];
			bool validadorPersona = estadoPos.Equals("1");
			bool validadorCompraInternet = estadoEcom.Equals("1") ;
			bool validadorCajeros = estadoAtm.Equals("1");
			bool validadorTelefono = estadoMoto.Equals("1");
			Switch swtCanalComprasPresenciales = itemView.FindViewById<Switch>(Resource.Id.swtCanalComprasPresenciales);
			Switch swtCanalComprasPorInternet = itemView.FindViewById<Switch>(Resource.Id.swtCanalComprasPorInternet);
			Switch swtCanaLGirosCajerosAutomaticos = itemView.FindViewById<Switch>(Resource.Id.swtCanaLGirosCajerosAutomaticos);
			Switch swtCanaLComprasViaTelefonica = itemView.FindViewById<Switch>(Resource.Id.swtCanaLComprasViaTelefonica);
			cambiarEstadoSwitch(validadorPersona,swtCanalComprasPresenciales);
			cambiarEstadoSwitch(validadorCompraInternet,swtCanalComprasPorInternet);
			cambiarEstadoSwitch(validadorCajeros,swtCanaLGirosCajerosAutomaticos);
			cambiarEstadoSwitch(validadorTelefono,swtCanaLComprasViaTelefonica);
		}

		private void cambiarEstadoSwitch(bool estado , Switch switchModificar){
		    switchModificar.Checked = estado;
		}

        private View consultarEstadoTarjetaDebito(int position, View itemView, Switch swtNameCard)
        {
            try
            {
                JsonValue jsonResponseAccessToken = WebServiceSecurity.ServiciosSecurity.CallRESTToken();
                OnOffData detalleTarjeta = (OnOffData) datosTarjeta[position];
                if (!string.IsNullOrEmpty(jsonResponseAccessToken.ToString()))
                {
                    JsonValue token = jsonResponseAccessToken["access_token"];
                    TextView nameCard = itemView.FindViewById<TextView>(Resource.Id.lblNameCard);
                    TextView statusCard = itemView.FindViewById<TextView>(Resource.Id.lblStatusCard);
                    TextView lblInfo = itemView.FindViewById<TextView>(Resource.Id.lblInfo);

                    string pan = datosTarjeta[position].Pan;
                    int contadorFinal = pan.Length - 3;
                    pan = pan.Substring(3, contadorFinal); // para redbank no se envia los ceros del comienzo.
                    JsonValue jsonResponseCheckCardStatus = detalleTarjeta.estadoTarjetaDebito;
                    string estadoTarjetaDebito = jsonResponseCheckCardStatus["statusDescription"];

                    if (estadoTarjetaDebito.Equals(ConstantesOnOff.ESTADO_ACTIVADO_ON))
                    {
                        swtNameCard.Checked = true;
                        statusCard.Text = ConstantesOnOff.TEXTO_ESTADO_ON;
                        statusCard.SetTextColor(statusCard.Context.Resources.GetColor(Resource.Color.security_on_off_activado));
                        lblInfo.Text = ConstantesOnOff.GLOSA_DEBITO_ON;
                    }
                    else
                    {
                        swtNameCard.Checked = false;
                        statusCard.Text = ConstantesOnOff.TEXTO_ESTADO_OFF;
                        statusCard.SetTextColor(statusCard.Context.Resources.GetColor(Resource.Color.security_on_off_desactivado));
                        lblInfo.Text = ConstantesOnOff.GLOSA_DEBITO_OFF;
                    }
                    var font = Typeface.CreateFromAsset(activity.Assets, ConstantesOnOff.rutaFuenteTitilium);
                    lblInfo.Typeface = font;
                    statusCard.Typeface = font;
                    nameCard.Typeface = font;
                }
                else
                {
                    DialogoErrorActivity.mostrarViewErrorHome(activity);
                }
            }
            catch (System.Exception se)
            {
                Console.WriteLine(se);
                DialogoErrorActivity.mostrarViewErrorHome(activity);
            }
            return itemView;
		}

        private async void eventoCambioEstadoOnOFF(object sender, EventArgs e)
        {
			try
            {
                Switch btnEvento = (Switch)sender;
                int positionActual = (int)btnEvento.GetTag(Resource.Id.swtNameCard);
                string tipoTarjeta = datosTarjeta[positionActual].TipoProducto;
                string pan = datosTarjeta[positionActual].Pan;
                bool estadoTarjeta = btnEvento.Checked;
                string estadoTarjetaRequest = estadoTarjeta ? "1" : "0";

                JsonValue jsonResponseAccessToken = await WebServiceSecurity.ServiciosSecurity.CallRESTaccessToken();
                if (!string.IsNullOrEmpty(jsonResponseAccessToken.ToString()))
                {
                    JsonValue token = jsonResponseAccessToken["access_token"];
                    if (tipoTarjeta.Equals(ConstantesOnOff.TIPO_TARJETA_DEBITO))
                    {
                        guardarEstadoTarjetaDebito(positionActual, estadoTarjeta, estadoTarjetaRequest, token, pan);
                    }
                    else if (tipoTarjeta.Equals(ConstantesOnOff.TIPO_TARJETA_CREDITO))
                    {
                        guardarEstadoTarjetaCredito(positionActual, estadoTarjeta, estadoTarjetaRequest, token, pan);
                    }
                }
                else
                {
                    DialogoErrorActivity.mostrarViewErrorHome(activity);
                }
            }
            catch (System.Exception x)
            {
                Console.WriteLine(x);
                DialogoErrorActivity.mostrarViewErrorHome(activity);
			}
        }

        // GUARDAR ESTADOS TARJETA
		private async void guardarEstadoTarjetaCredito(int position,bool estadoTarjeta,string estadoTarjetaRequest, string token, string pan){
            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            View view = listadoItemsView[position];
            TextView statusCard = view.FindViewById<TextView>(Resource.Id.lblStatusCard);
            TextView lblInfo = view.FindViewById<TextView>(Resource.Id.lblInfo);

			try
			{
                JsonValue jsonResponseOnOffCardStatus = await WebServiceSecurity.ServiciosSecurity.CallRESTSetEnable(token, pan, estadoTarjetaRequest, UtilAndroid.getRut(), parametriaLogUtil.getIdDispositivoParaLog(UtilAndroid.getIMEI(activity)), ParametriaLogUtil.GetIpLocal());
                if (!string.IsNullOrEmpty(jsonResponseOnOffCardStatus.ToString()))
                {
                    string codigoRespuesta = jsonResponseOnOffCardStatus["statusCode"];

                    if (codigoRespuesta.Equals(ConstantesOnOff.TEXTO_ESTADO_SUCCESS) && estadoTarjeta)
                    {
                        statusCard.Text = ConstantesOnOff.TEXTO_ESTADO_ON;
                        statusCard.SetTextColor(statusCard.Context.Resources.GetColor(Resource.Color.green));
                        lblInfo.Text = ConstantesOnOff.GLOSA_CREDITO_ON;
                        OnOffData elementoActual = datosTarjeta[position];
                        elementoActual.estadoTarjetaCredito["estado"] = ConstantesOnOff.ESTADO_ACTIVADO_1;
                        cambiarEstadoPanelCanales(position, statusCard.Text);
                        DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                    }
                    else if (codigoRespuesta.Equals(ConstantesOnOff.TEXTO_ESTADO_SUCCESS) && !estadoTarjeta)
                    {
                        statusCard.Text = ConstantesOnOff.TEXTO_ESTADO_OFF;
                        statusCard.SetTextColor(statusCard.Context.Resources.GetColor(Resource.Color.red));
                        lblInfo.Text = ConstantesOnOff.GLOSA_CREDITO_OFF;
                        OnOffData elementoActual = datosTarjeta[position];
                        elementoActual.estadoTarjetaCredito["estado"] = ConstantesOnOff.ESTADO_DESACTIVADO_0;
                        cambiarEstadoPanelCanales(position, statusCard.Text);
                        DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                    }
                    else
                    {
                        DialogoErrorActivity.mostrarViewErrorHome(activity);
                    }

                }else{
                    DialogoErrorActivity.mostrarViewErrorHome(activity);
                }
            }
            catch (System.Net.WebException ex)
            {
                Console.WriteLine(ex);
                DialogoErrorActivity.mostrarViewErrorHome(activity);
            }
            catch (ArgumentException ax)
            {
                Console.WriteLine(ax);
                DialogoErrorActivity.mostrarViewErrorHome(activity);
            }
		}

		private async void guardarEstadoTarjetaDebito(int position,bool estadoTarjeta,string estadoTarjetaRequest,string token,string pan)
        {
            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            View view = listadoItemsView[position];
            TextView statusCard = view.FindViewById<TextView>(Resource.Id.lblStatusCard);
            TextView lblInfo = view.FindViewById<TextView>(Resource.Id.lblInfo);
            int contadorFinal = pan.Length - 3;
            pan = pan.Substring(3, contadorFinal); // para redbank no se envia los ceros del comienzo.
            try
            {
                JsonValue jsonResponseOnOffCardStatus = await WebServiceSecurity.ServiciosSecurity.CallRESTOnOffCardStatus
                                                                                (token, SecurityEndpoints.oceFi, SecurityEndpoints.oceAbaBranch, pan, SecurityEndpoints.oceAccountType, estadoTarjetaRequest, UtilAndroid.getRut(), parametriaLogUtil.getIdDispositivoParaLog(UtilAndroid.getIMEI(activity)), ParametriaLogUtil.GetIpLocal());
                if (!string.IsNullOrEmpty(jsonResponseOnOffCardStatus.ToString()))
                {
                    string codigoRespuesta = jsonResponseOnOffCardStatus["statusCode"];
                    if (codigoRespuesta.Equals(ConstantesOnOff.TEXTO_ESTADO_SUCCESS) && estadoTarjeta)
                    {
                        statusCard.Text = ConstantesOnOff.TEXTO_ESTADO_ON;
                        statusCard.SetTextColor(statusCard.Context.Resources.GetColor(Resource.Color.green));
                        lblInfo.Text = ConstantesOnOff.GLOSA_DEBITO_ON;
                        DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();

                    }
                    else if (codigoRespuesta.Equals(ConstantesOnOff.TEXTO_ESTADO_SUCCESS) && !estadoTarjeta)
                    {
                        statusCard.Text = ConstantesOnOff.TEXTO_ESTADO_OFF;
                        statusCard.SetTextColor(statusCard.Context.Resources.GetColor(Resource.Color.red));
                        lblInfo.Text = ConstantesOnOff.GLOSA_DEBITO_OFF;
                        DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                    }
                    else
                    {
                        DialogoErrorActivity.mostrarViewErrorHome(activity);
                    }
                }
                else
                {
                    DialogoErrorActivity.mostrarViewErrorHome(activity);
                }
            }
            catch (System.Exception x)
            {
                Console.WriteLine(x);
                DialogoErrorActivity.mostrarViewError(activity);
            }
		}

		private void eventoCanalPresencial(object sender, EventArgs e)
        {
            Switch btnEvento = (Switch)sender;
            int positionActual = (int)btnEvento.GetTag(Resource.Id.swtCanalComprasPresenciales);
            View view = listadoItemsView[positionActual];
			eventoCanalGenerico(view, positionActual);
        }
        // EVENTOS SWITCH CANALES
		private void eventoCanalInternet(object sender, EventArgs e)
		{
			Switch btnEvento = (Switch)sender;
			int positionActual = (int)btnEvento.GetTag(Resource.Id.swtCanalComprasPorInternet);
            View view = listadoItemsView[positionActual];
            eventoCanalGenerico(view, positionActual);
		}

		private void eventoCanalAutomatico(object sender, EventArgs e)
        {
			Switch btnEvento = (Switch)sender;
			int positionActual = (int)btnEvento.GetTag(Resource.Id.swtCanaLGirosCajerosAutomaticos);
            View view = listadoItemsView[positionActual];
            eventoCanalGenerico(view, positionActual);
        }

		private void eventoCanalTelefonica(object sender, EventArgs e)
        {
			Switch btnEvento = (Switch)sender;
			int positionActual = (int)btnEvento.GetTag(Resource.Id.swtCanaLComprasViaTelefonica);
            View view = listadoItemsView[positionActual];
            eventoCanalGenerico(view, positionActual);
        }

        private async void eventoCanalGenerico(View view,int positionActual){
			try
            {
                Switch swtCanalComprasPresenciales = view.FindViewById<Switch>(Resource.Id.swtCanalComprasPresenciales);
                Switch swtCanalComprasPorInternet = view.FindViewById<Switch>(Resource.Id.swtCanalComprasPorInternet);
                Switch swtCanaLGirosCajerosAutomaticos = view.FindViewById<Switch>(Resource.Id.swtCanaLGirosCajerosAutomaticos);
                Switch swtCanaLComprasViaTelefonica = view.FindViewById<Switch>(Resource.Id.swtCanaLComprasViaTelefonica);

                string pan = datosTarjeta[positionActual].Pan;
                bool cpcEstadoPos = swtCanalComprasPresenciales.Checked;
                bool cpcEstadoAtm = swtCanalComprasPorInternet.Checked;
                bool cpcEstadoEcom = swtCanaLGirosCajerosAutomaticos.Checked;
                bool cpcEstadoMoto = swtCanaLComprasViaTelefonica.Checked;

                string cpcEstadoPosRequest = swtCanalComprasPresenciales.Checked ? "1" : "0";
                string cpcEstadoEcomRequest = swtCanalComprasPorInternet.Checked ? "1" : "0";
                string cpcEstadoAtmRequest = swtCanaLGirosCajerosAutomaticos.Checked ? "1" : "0";
                string cpcEstadoMotoRequest = swtCanaLComprasViaTelefonica.Checked ? "1" : "0";

                JsonValue jsonResponseAccessToken = await WebServiceSecurity.ServiciosSecurity.CallRESTaccessToken();
                if (!string.IsNullOrEmpty(jsonResponseAccessToken.ToString()))
                {
                    JsonValue token = jsonResponseAccessToken["access_token"];
                    guardarDatosCanales(token, pan, cpcEstadoPosRequest, cpcEstadoAtmRequest, cpcEstadoEcomRequest, cpcEstadoMotoRequest,positionActual);
                }
                else
                {
                    DialogoErrorActivity.mostrarViewErrorHome(activity);
                }
            }
            catch (System.Exception x)
            {
                Console.WriteLine(x);
                DialogoErrorActivity.mostrarViewErrorHome(activity);
			}
		}

        // GUARDAR ESTADO CANALES
        private async void guardarDatosCanales(string token, string pan,string cpcEstadoPos,string cpcEstadoAtm,string cpcEstadoEcom,string cpcEstadoMoto,int positionActual)
        {
            try
            {
                JsonValue jsonResponseOnOffCardStatus = await WebServiceSecurity.ServiciosSecurity.CallRESTChangePaymentChannels
                                                                                (token, pan, cpcEstadoPos, cpcEstadoAtm, cpcEstadoEcom, cpcEstadoMoto, UtilAndroid.getRut(), parametriaLogUtil.getIdDispositivoParaLog(UtilAndroid.getIMEI(activity)), ParametriaLogUtil.GetIpLocal());
                if (!string.IsNullOrEmpty(jsonResponseOnOffCardStatus.ToString()))
                {
                    string codigoRespuesta = jsonResponseOnOffCardStatus["statusCode"];

                    if (codigoRespuesta.Equals(ConstantesOnOff.TEXTO_ESTADO_SUCCESS))
                    {
                        OnOffData elementoActual = datosTarjeta[positionActual];
                        elementoActual.estadoTarjetaCredito["estadoPos"] = cpcEstadoPos;
                        elementoActual.estadoTarjetaCredito["estadoEcom"] = cpcEstadoEcom;
                        elementoActual.estadoTarjetaCredito["estadoAtm"] = cpcEstadoAtm;
                        elementoActual.estadoTarjetaCredito["estadoMoto"] = cpcEstadoMoto;
                    }
                    else
                    {
                        DialogoErrorActivity.mostrarViewErrorHome(activity);
                    }
                }else{
                    DialogoErrorActivity.mostrarViewErrorHome(activity);
                }
     
            }
            catch (System.Exception x)
            {
                Console.WriteLine(x);
                DialogoErrorActivity.mostrarViewError(activity);
            }
        }

        public void cambiarEstadoPanelCanales(int position, string estadoGeneral)
		{
            OnOffData elementoActual = datosTarjeta[position];
            string estadoTarjetaCredito = elementoActual.estadoTarjetaCredito["estado"];
            bool estado = false;

            if (estadoTarjetaCredito.Equals(ConstantesOnOff.ESTADO_ACTIVADO_1)){
                estado = true;
            }

            View view = obtenerVista(position);
            cambiarEstadoSwitchCanales(elementoActual.estadoTarjetaCredito, view);

            try
            {
                using(Switch swtCanalComprasPresenciales = view.FindViewById<Switch>(Resource.Id.swtCanalComprasPresenciales)){
                    swtCanalComprasPresenciales.CheckedChange += null;
                    swtCanalComprasPresenciales.Enabled = estado;
                    setColorSwitchCanalesBloqued(swtCanalComprasPresenciales, estado);
                    swtCanalComprasPresenciales.CheckedChange += eventoCanalPresencial;
                }

                using(Switch swtCanalComprasPorInternet = view.FindViewById<Switch>(Resource.Id.swtCanalComprasPorInternet)){
                    swtCanalComprasPorInternet.CheckedChange += null;
                    swtCanalComprasPorInternet.Enabled = estado;
                    setColorSwitchCanalesBloqued(swtCanalComprasPorInternet, estado);
                    swtCanalComprasPorInternet.CheckedChange += eventoCanalInternet;
                }

                using(Switch swtCanaLGirosCajerosAutomaticos = view.FindViewById<Switch>(Resource.Id.swtCanaLGirosCajerosAutomaticos)){
                    swtCanaLGirosCajerosAutomaticos.CheckedChange += null;
                    swtCanaLGirosCajerosAutomaticos.Enabled = estado;
                    setColorSwitchCanalesBloqued(swtCanaLGirosCajerosAutomaticos, estado);
                    swtCanaLGirosCajerosAutomaticos.CheckedChange += eventoCanalAutomatico;
                }

                using(Switch swtCanaLComprasViaTelefonica = view.FindViewById<Switch>(Resource.Id.swtCanaLComprasViaTelefonica)){
                    swtCanaLComprasViaTelefonica.CheckedChange += null;
                    swtCanaLComprasViaTelefonica.Enabled = estado;
                    setColorSwitchCanalesBloqued(swtCanaLComprasViaTelefonica, estado);
                    swtCanaLComprasViaTelefonica.CheckedChange += eventoCanalTelefonica;
                }
            }
            catch (System.Exception x)
            {
                Console.WriteLine(x);
                DialogoErrorActivity.mostrarViewErrorHome(activity);
            }
        }

        public void setColorSwitchCanalesBloqued(Switch swt, bool estado){
            if (!estado)
            {
                swt.SetTrackResource(Resource.Drawable.switch_track_selector_bloqued);
            }else{
                swt.SetTrackResource(Resource.Drawable.switch_track_selector);
            }
        }

		public void mostrarMensaje(string titulo,string mensaje)
        {
			AlertDialog.Builder builder = new AlertDialog.Builder(this.activity);
			builder.SetTitle(titulo);
			builder.SetMessage(mensaje);
            builder.Show();
        }

        public override void DestroyItem(View container, int position, Java.Lang.Object @object)
		{
            ((ViewPager) container).RemoveView((View) @object);
		}

		public void iniciarIndicadorPagina(int cantidadTotal,int paginaActual){
			View vista = listadoItemsView[paginaActual];

            LinearLayout contenedorIndicadores = vista.FindViewById<LinearLayout>(Resource.Id.contenedor_indicadores_carrusel);
            contenedorIndicadores.RemoveAllViews();

			for (int i = 0; i < cantidadTotal; i++)
			{
                ImageView imagenPaginador = new ImageView(activity.ApplicationContext);

				if (paginaActual == i)
                {
                    imagenPaginador.SetImageResource(Resource.Drawable.circulo_morado);
                }else{
                    imagenPaginador.SetImageResource(Resource.Drawable.circulo_plomo);                    
                }

                if(i != 0){
                    imagenPaginador.SetPadding(20, 0, 0, 0);
                }
                contenedorIndicadores.AddView(imagenPaginador);
			}
		}
	}
}