using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using BancoSecurityOnOff.Droid.Bean;
using BancoSecurityOnOff.Droid.Resources.OnOffAdapter;
using BancoSecurityOnOff.Droid.Resources.NotificacionesAdapter;
using BancoSecurityOnOff.Utilidades.Firmas;
using Newtonsoft.Json.Linq;
using static Android.App.ActionBar;
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
    /// <summary>
    /// Clase Fragment Admin de Notificaciones
    /// </summary>
    public class ConfigurarNotificacionesFragment : Fragment
    {
        /// <summary>
        /// variables de la clase
        /// </summary>
        TextInputLayout txtInputMontoPesos;
        TextInputLayout txtInputMontoDolar;
        TextView lblTituloConfiguracionNoti;
        TextView lblSubTituloPanelUno;
        TextView lblSubTituloPanelDos;
        TextView lblSubTituloPanelTres;
        TextView lblSubTituloPanelCuatro;
        TextView lblSubTituloPanelCinco;
        EditText txtMontoPeso;
        EditText txtMontoDolar;
        EditText txtMontoDolarEntero;
        EditText txtMontoDolarDecimal;

        Button btnConfirmarMontosDolarPeso;
        TextView lblConfiguracionMonto;
        TextView lblCincoAppOnOff;
        TextView lblCincoCorreoElectronico;
        TextView lblCincoSms;
        LinearLayout linearLayoutPanelConfiguracionMontos;
        LinearLayout llMensajeExitosoMontos;
        LinearLayout llPanelUno;
        LinearLayout llPanelDos;

        List<Switch> switchVista = new List<Switch>();
        string rutConocido;
        string rutDesdeConfirmacion;
        string rutDefinitivo;
        Switch swtCuentasTarjetas;
        Switch swtTipoNotificacion;
        Switch swtMontoMinino;

        //Delivery
        Switch swtDeliveryAppOnOff;
        Switch swtDeliveryCorreo;
        Switch swtDeliverySMS;

        // listado a enviar a servicio , solo con productos activados.
        List<OnOffData> listadoProductosActivados = new List<OnOffData>();
        List<OnOffData> listadoProductosDesActivados = new List<OnOffData>();
        List<IngresarProductosNotificacionesRequest> lstIngresaProdNoti;

        // listado a enviar a servicio , solo con notificaciones activadas.
        List<NotificacionesData> listadoNotificaciones = new List<NotificacionesData>();
        List<NotificacionesData> listadoNotificacionesDesActivados = new List<NotificacionesData>();
        List<IngresarNotificacionesRequest> lstIngresaNotificaciones;
        ParametriaLogUtil parametriaLogUtil;
        IngresarProductosNotificacionesRequest productoVacio;
        IngresarNotificacionesRequest notificacionVacio;
        MascarasTextWatcher mascarasTextWatcherMontoPeso;
        MascarasTextWatcher mascarasTextWatcherMontoDolar;
        DialogoLoadingBcoSecurityActivity dialogoLoadingBcoSecurityActivity;
        /// <summary>
        /// Constantes de la clase
        /// </summary>
        private const string rutaFuenteTitiliumRegular = "fonts/titillium_web/TitilliumWeb-Regular.ttf";
        private const string rutaFuenteTitiliumSemiBold = "fonts/titillium_web/TitilliumWeb-SemiBold.ttf";
        private const string encabezadoTarjetaDebito = "Tarjeta Débito Nº xxxx xxxx xxxx ";
        private const string encabezadoTarjetaCreditoOne = "MasterCard One Nº xxxx xxxx xxxx ";
        private const string encabezadoTarjetaCreditoBlack = "MasterCard Black Nº xxxx xxxx xxxx ";
        private const string encabezadoCuentaCorriente = "Cuenta Corriente Nº ";
        private const string tarDebito = "TARJETA DEBITO";
        private const string cuentaCorriente = "CUENTA CORRIENTE";
        private const string tarCredito = "TARJETA CREDITO";
        private const string tcBlack = "MASTER BLACK";
        private const string tcOne = "MASTER GOLD";
        private const string ceroIntString = "0";
        private const string ceroDoubleString = "0.0";
        private const int cero = 0;
        private const string unoIntString = "1";
        private const int cuatro = 4;
        private const int cinco = 5;
        private const int seis = 6;
        private const int siete = 7;
        private const int quince = 15;
        private const int veinte = 20;
        private const int seicientos = 600;
        private const int setecientosVeinte = 720;
        private const int ochocientosVeinte = 820;
        private const int novecientosVeinte = 920;
        private const string ceroCeroCero = "000";
        private const string ceroCeroUno = "001";
        private const string ceroCeroDos = "002";
        private const string ceroCerotres = "003";
        private const string ceroCeroCuatro = "004";
        private const string ceroCeroCinco = "005";
        private const string ceroCeroSeis = "006";
        private const string ceroCeroSiete = "007";
        private const string indicadorNumeroPrint = " Nº ";
        private const string tituloErrorDialogo = "Error";
        private const string btnAceptarDialogo = "Aceptar";
        private const string mensajeErrorCargaDatos = "Se ha producido un error. No se han guardado sus cambios, Por favor intente más tarde.";
        private const string mensajeGenericError = "Se ha producido un error. Por favor intente más tarde.";
        private const string responseSuccess = "Success";
        private const string varStatusCode = "statusCode";
        private const string varAccesstoken = "access_token";
        private const string varProductosNotificaciones = "ProductosNotificaciones";
        private const string varConsultaProdNotiNumeroProducto = "numeroProducto";
        private const string varConsultaProdNotiCodigoTipoProducto = "codigoTipoProducto";
        private const string varConsultaProdNotiTipoProducto = "tipoProducto";
        private const string varConsultaProdNotiGlosaProducto = "glosaProducto";
        private const string varConsultaProdNotiEstadoNotificacionProducto = "estadoNotificacionProducto";
        private const string varConsultaProdNotiMontoPesos = "montoPesos";
        private const string varConsultaProdNotiMontoDolar = "montoDolar";
        private const string TEF = "TEF";
        private const string TC = "TC";
        private const string TD = "TD";
        private const string PAG = "PAG";
        private const string movimientosCuentaCorriente = "Movimientos Cuenta Corriente";
        private const string movimientosTarjetaCredito = "Movimientos Tarjeta Crédito";
        private const string movimientosTarjetaDebito = "Movimientos Tarjeta Débito";
        private const string pagoServicios = "Pago Servicios (Servipag)";
        private const string varConsultaNotiNotificacion = "Notificacion";
        private const string varConsultaNotiEstadoNotificacion = "estadoNotificacion";
        private const string varConsultaNotiCodigoNotificacion = "codigoNotificacion";
        private const string varConsultaNotinotificacion = "notificacion";
        private const string varConsultaNotiDelivery = "delivery";
        //End Constantes

        int cont1 = cero;
        int cont2 = cero;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            listadoProductosActivados = new List<OnOffData>();
            listadoNotificaciones = new List<NotificacionesData>();
            View view = inflater.Inflate(Resource.Layout.ConfigurarNotificaciones, container, false);
            txtInputMontoPesos = view.FindViewById<TextInputLayout>(Resource.Id.txtInputMontoPeso);
            txtInputMontoDolar = view.FindViewById<TextInputLayout>(Resource.Id.txtInputMontoDolar);
            lblTituloConfiguracionNoti = view.FindViewById<TextView>(Resource.Id.lblTituloConfiguracionNoti);
            lblSubTituloPanelUno = view.FindViewById<TextView>(Resource.Id.lblSubTituloPanelUno);
            lblSubTituloPanelDos = view.FindViewById<TextView>(Resource.Id.lblSubTituloPanelDos);
            lblSubTituloPanelCuatro = view.FindViewById<TextView>(Resource.Id.lblSubTituloPanelCuatro);
            lblSubTituloPanelCinco = view.FindViewById<TextView>(Resource.Id.lblSubTituloPanelCinco);
            txtMontoPeso = view.FindViewById<EditText>(Resource.Id.txtMontoPeso);
            txtMontoDolar = view.FindViewById<EditText>(Resource.Id.txtMontoDolar);
            btnConfirmarMontosDolarPeso = view.FindViewById<Button>(Resource.Id.btnConfirmarMontosDolarPeso);
            lblConfiguracionMonto = view.FindViewById<TextView>(Resource.Id.lblConfiguracionMonto);
            lblCincoAppOnOff = view.FindViewById<TextView>(Resource.Id.lblCincoAppOnOff);
            lblCincoCorreoElectronico = view.FindViewById<TextView>(Resource.Id.lblCincoCorreoElectronico);
            lblCincoSms = view.FindViewById<TextView>(Resource.Id.lblCincoSms);
            swtDeliveryAppOnOff = view.FindViewById<Switch>(Resource.Id.swtCincoAppOnOff);
            swtDeliveryCorreo = view.FindViewById<Switch>(Resource.Id.swtCincoCorreoElectronico);
            swtDeliverySMS = view.FindViewById<Switch>(Resource.Id.swtCincoSms);
            swtMontoMinino = view.FindViewById<Switch>(Resource.Id.swtConfiguracionMonto);
            linearLayoutPanelConfiguracionMontos = view.FindViewById<LinearLayout>(Resource.Id.llCuatroDos);
            llMensajeExitosoMontos = view.FindViewById<LinearLayout>(Resource.Id.llMensajeExitosoMontos);
            dialogoLoadingBcoSecurityActivity = new DialogoLoadingBcoSecurityActivity(Activity);
            btnConfirmarMontosDolarPeso.Click += BtnConfirmarMontosDolarPeso_Click;
            swtMontoMinino.CheckedChange += SwtMontoMinino_CheckedChange;
            swtDeliveryAppOnOff.Click += SwtDeliveryAppOnOff_Click;
            swtDeliveryCorreo.Click += SwtDeliveryCorreo_Click;
            swtDeliverySMS.Click += SwtDeliverySMS_Click;
            txtMontoPeso.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
            txtMontoDolar.ImeOptions = Android.Views.InputMethods.ImeAction.Done;

            parametriaLogUtil = new ParametriaLogUtil();
            Typeface fontRegular = Typeface.CreateFromAsset(Activity.Assets, rutaFuenteTitiliumRegular);
            Typeface fontSemiBold = Typeface.CreateFromAsset(Activity.Assets, rutaFuenteTitiliumSemiBold);
            txtInputMontoDolar.Typeface = fontRegular;
            txtInputMontoPesos.Typeface = fontRegular;
            lblTituloConfiguracionNoti.Typeface = fontRegular;
            lblSubTituloPanelUno.Typeface = fontSemiBold;
            lblSubTituloPanelDos.Typeface = fontSemiBold;
            lblSubTituloPanelCuatro.Typeface = fontSemiBold;
            lblSubTituloPanelCinco.Typeface = fontSemiBold;
            txtMontoPeso.Typeface = fontRegular;
            txtMontoDolar.Typeface = fontRegular;
            btnConfirmarMontosDolarPeso.Typeface = fontSemiBold;
            lblConfiguracionMonto.Typeface = fontRegular;
            lblCincoAppOnOff.Typeface = fontSemiBold;
            lblCincoCorreoElectronico.Typeface = fontSemiBold;
            lblCincoSms.Typeface = fontSemiBold;

            mascarasTextWatcherMontoPeso = new MascarasTextWatcher(txtMontoPeso, this.Activity,MascarasTextWatcher.TIPO_MONTO_PESOS);
            txtMontoPeso.AddTextChangedListener(mascarasTextWatcherMontoPeso);

            //mascarasTextWatcherMontoDolar = new MascarasTextWatcher(txtMontoDolar, this.Activity, MascarasTextWatcher.TIPO_MONTO_USD);
            //txtMontoDolar.AddTextChangedListener(mascarasTextWatcherMontoDolar);

            llPanelUno = view.FindViewById<LinearLayout>(Resource.Id.layoutPanelUno);
            llPanelDos = view.FindViewById<LinearLayout>(Resource.Id.layoutPanelDos);
            lstIngresaProdNoti = new List<IngresarProductosNotificacionesRequest>();
            lstIngresaNotificaciones = new List<IngresarNotificacionesRequest>();

			LinearLayout contenedorTarjeta = view.FindViewById<LinearLayout>(Resource.Id.contenedorTarjeta);
			LinearLayout contenedorNotificaciones = view.FindViewById<LinearLayout>(Resource.Id.contenedorNotificaciones);
            rutConocido = MainActivity.returnRutConsultaEnrolado();
            rutDesdeConfirmacion = ConfirmacionEnroladoActivity.returnRutConsultaEnroladoConocido();
            rutDefinitivo = rutConocido;
            if (string.IsNullOrEmpty(rutDefinitivo))
            {
                rutDefinitivo = rutDesdeConfirmacion;
            }

            ConsultaProductoNotificaciones(fontRegular, contenedorTarjeta, contenedorNotificaciones);

            productoVacio = new IngresarProductosNotificacionesRequest
            {
                rut = rutDefinitivo,
                numeroProducto = string.Empty,
                codigoTipoProducto = string.Empty,
                glosaProducto = string.Empty,
                estadoNotificacionProducto = ceroIntString,
                montoPesos = txtMontoPeso.Text,
                montoDolar = txtMontoDolar.Text
            };

            notificacionVacio = new IngresarNotificacionesRequest
            {
                rut = rutDefinitivo,
                estadoNotificacion = string.Empty,
                codigoNotificacion = string.Empty,
                delivery = ceroIntString
            };

            return view;
        }

        void TxtMontoPeso_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (OnKeyUp(Keycode.Num1, null))
            {
                string texto = MascarasEditText.formatearMonto(txtMontoPeso.Text);
                txtMontoPeso.Text = texto;
                return;
            }
        }
 
        private async void ConsultaProductoNotificaciones(Typeface fontRegular, LinearLayout contenedorTarjeta, LinearLayout contenedorNotificaciones){

            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            try
            {
                JsonValue jsonResponseAccessToken = WebServiceSecurity.ServiciosSecurity.CallRESTToken();
                if (!string.IsNullOrEmpty(jsonResponseAccessToken.ToString()))
                {
                    JsonValue jt = jsonResponseAccessToken[varAccesstoken];
                    string imei = this.getIMEI();
                    string idDispositivoLogConsultaProductoNotificaciones = parametriaLogUtil.getIdDispositivoParaLog(imei);
                    string ipDisposiivoLogConsultaProductoNotificaciones = ParametriaLogUtil.GetIpLocal();
                    JsonValue jsonResponseConsultaProductoNotificaciones = await WebServiceSecurity.ServiciosSecurity.CallRESTConsultaProductoNotificaciones(jt, SecurityEndpoints.flagInicio, SecurityEndpoints.tamanoBloque, SecurityEndpoints.paginaConsultar, SecurityEndpoints.numeroRegistros, SecurityEndpoints.totalPaginas, SecurityEndpoints.ultimoRegistro, rutDefinitivo, idDispositivoLogConsultaProductoNotificaciones, ipDisposiivoLogConsultaProductoNotificaciones);
                    if (!string.IsNullOrEmpty(jsonResponseConsultaProductoNotificaciones.ToString()))
                    {
                        string jcpnStatus = jsonResponseConsultaProductoNotificaciones["statusCode"];
                        if (jcpnStatus.Equals("Success"))
                        {
                            JsonValue productosBcoSecurity = jsonResponseConsultaProductoNotificaciones[varProductosNotificaciones];

                            JArray arreglo = JArray.Parse(productosBcoSecurity.ToString());
                            ProductoCard pc = new ProductoCard();
                            List<JToken> otherResults = arreglo.Children().ToList();
                            calculaMonto(otherResults);
                            construirSwitchProductos(otherResults, fontRegular, contenedorTarjeta);


                            llPanelUno.LayoutParameters.Height = LayoutParams.WrapContent;

                            JsonValue jsonResponseNotificaciones = WebServiceSecurity.ServiciosSecurity.CallRESTConsultaNotificaciones(jt, rutDefinitivo, idDispositivoLogConsultaProductoNotificaciones, ipDisposiivoLogConsultaProductoNotificaciones);
                            JsonValue notificacionesBcoSecurity = jsonResponseNotificaciones[varConsultaNotiNotificacion];
                            JArray arregloNotificacion = JArray.Parse(notificacionesBcoSecurity.ToString());
                            List<JToken> otherResultsNoti = arregloNotificacion.Children().ToList();

                            construirSwitchNotificaciones(otherResultsNoti, fontRegular, contenedorNotificaciones);
                            llPanelDos.LayoutParameters.Height = LayoutParams.WrapContent;
                            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();

                        }
                        else
                        {
                            DialogoErrorActivity.mostrarViewErrorHome(Activity);
                        }
                    }
                    else
                    {
                        DialogoErrorActivity.mostrarViewErrorLoginEnrolado(Activity);
                    }
                }else{
                    DialogoErrorActivity.mostrarViewErrorLoginEnrolado(Activity);
                }
            }
            catch (Exception){
                DialogoErrorActivity.mostrarViewErrorHome(Activity);
            }
        }

		private void SwtDeliverySMS_Click(object sender, EventArgs e)
        {
            string codigoDelivery = codigoDeliveryNotificaciones(swtDeliveryAppOnOff.Checked, swtDeliveryCorreo.Checked, swtDeliverySMS.Checked);
            foreach (var notificacion in lstIngresaNotificaciones)
            {
                notificacion.delivery = codigoDelivery;
            }
            guardarNotificacionesSeleccionados();
        }

        private void SwtDeliveryCorreo_Click(object sender, EventArgs e)
        {
            string codigoDelivery = codigoDeliveryNotificaciones(swtDeliveryAppOnOff.Checked, swtDeliveryCorreo.Checked, swtDeliverySMS.Checked);
            foreach (var notificacion in lstIngresaNotificaciones)
            {
                notificacion.delivery = codigoDelivery;
            }
            guardarNotificacionesSeleccionados();
        }

        private void SwtDeliveryAppOnOff_Click(object sender, EventArgs e)
        {
            string codigoDelivery = codigoDeliveryNotificaciones(swtDeliveryAppOnOff.Checked, swtDeliveryCorreo.Checked, swtDeliverySMS.Checked);
            foreach (var notificacion in lstIngresaNotificaciones)
            {
                notificacion.delivery = codigoDelivery;
            }
            guardarNotificacionesSeleccionados();
        }

        private void BtnConfirmarMontosDolarPeso_Click(object sender, EventArgs e)
        {
            string montoPesosSinFormato = MascarasEditText.limpiarMontoPesos(txtMontoPeso.Text);
            string montoDolar = txtMontoDolar.Text.Replace(",", ".");
           // string dolarSinComa = montoDolar.;
            string montoDolarSinFormato = MascarasEditText.limpiarMontoPesos(montoDolar);
            foreach (var producto in lstIngresaProdNoti)
            {
                producto.montoPesos = montoPesosSinFormato;
                producto.montoDolar = montoDolarSinFormato;
            }
            guardarProductosDesdeMontos();
        }

        private void SwtMontoMinino_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (swtMontoMinino.Checked)
            {
                linearLayoutPanelConfiguracionMontos.Visibility = ViewStates.Visible;
                btnConfirmarMontosDolarPeso.Visibility = ViewStates.Visible;
                llMensajeExitosoMontos.Visibility = ViewStates.Gone;
            }
            else{
                linearLayoutPanelConfiguracionMontos.Visibility = ViewStates.Gone;
            }
        }

       

        private void construirSwitchNotificaciones(List<JToken> otherResultsNoti, Typeface font, LinearLayout contenedorLayout){
			string tipoNotificacionNombre = string.Empty;
			
			foreach (var auxNoti in otherResultsNoti)
            {
                string estadoNotificacion = Convert.ToString(JObject.Parse(auxNoti.ToString())[varConsultaNotiEstadoNotificacion]).Trim();
                string codigoNotificacion = Convert.ToString(JObject.Parse(auxNoti.ToString())[varConsultaNotiCodigoNotificacion]).Trim();
                string notificacion = Convert.ToString(JObject.Parse(auxNoti.ToString())[varConsultaNotinotificacion]).Trim();
                string delivery = Convert.ToString(JObject.Parse(auxNoti.ToString())[varConsultaNotiDelivery]).Trim();

                NotificacionesData objetoNotificaciones = new NotificacionesData()
                {
                    Id = cont2,
                    EstadoNotificacion = estadoNotificacion,
                    CodigoNotificacion = codigoNotificacion,
                    Notificacion = notificacion,
                    Delivery = delivery
                };

                using(LinearLayout filaSwitch = new LinearLayout(this.Activity)){
                    filaSwitch.Orientation = Orientation.Horizontal;
                    filaSwitch.SetPadding(cero, veinte, cero, veinte);

                    TextView etiqueta = new TextView(this.Activity);
                    etiqueta.Typeface = font;
                    etiqueta.Text = obtenerTipoTarjeta(codigoNotificacion, notificacion);

                    swtTipoNotificacion = new Switch(this.Activity);
                    swtTipoNotificacion.SetTrackResource(Resource.Drawable.switch_track_selector);
                    swtTipoNotificacion.ThumbDrawable.SetColorFilter(Color.Rgb(245, 245, 245), PorterDuff.Mode.Multiply);

                    if (estadoNotificacion.Equals(ceroIntString))
                    {
                        swtTipoNotificacion.Checked = false;
                    }
                    else
                    {
                        swtTipoNotificacion.Checked = true;
                    }

                    filaSwitch.AddView(swtTipoNotificacion, cero);
                    filaSwitch.AddView(etiqueta, 1);
                    contenedorLayout.AddView(filaSwitch, cont2);

                    cont2++;
                    swtTipoNotificacion.Id = cont2;
                }

                if (delivery.Equals(ceroCeroCero))
                {
                    swtDeliveryAppOnOff.Checked = false;
                    swtDeliveryCorreo.Checked = false;
                    swtDeliverySMS.Checked = false;
                }
                if (delivery.Equals(ceroCeroUno))
                {
                    swtDeliveryAppOnOff.Checked = false;
                    swtDeliveryCorreo.Checked = false;
                    swtDeliverySMS.Checked = true;
                }
                if (delivery.Equals(ceroCeroDos))
                {
                    swtDeliveryAppOnOff.Checked = false;
                    swtDeliveryCorreo.Checked = true;
                    swtDeliverySMS.Checked = false;
                }
                if (delivery.Equals(ceroCerotres))
                {
                    swtDeliveryAppOnOff.Checked = false;
                    swtDeliveryCorreo.Checked = true;
                    swtDeliverySMS.Checked = true;
                }
                if (delivery.Equals(ceroCeroCuatro))
                {
                    swtDeliveryAppOnOff.Checked = true;
                    swtDeliveryCorreo.Checked = false;
                    swtDeliverySMS.Checked = false;
                }
                if (delivery.Equals(ceroCeroCinco))
                {
                    swtDeliveryAppOnOff.Checked = true;
                    swtDeliveryCorreo.Checked = false;
                    swtDeliverySMS.Checked = true;
                }
                if (delivery.Equals(ceroCeroSeis))
                {
                    swtDeliveryAppOnOff.Checked = true;
                    swtDeliveryCorreo.Checked = true;
                    swtDeliverySMS.Checked = false;
                }
                if (delivery.Equals(ceroCeroSiete))
                {
                    swtDeliveryAppOnOff.Checked = true;
                    swtDeliveryCorreo.Checked = true;
                    swtDeliverySMS.Checked = true;
                }

                // switch activado / agregar a lista de activados. 
                if (estadoNotificacion.Equals(unoIntString))
                {
                    listadoNotificaciones.Add(new NotificacionesData()
                    {
                        Id = cont2,
                        EstadoNotificacion = estadoNotificacion,
                        CodigoNotificacion = codigoNotificacion,
                        Notificacion = notificacion,
                        Delivery = delivery
                    });

                    lstIngresaNotificaciones.Add(new IngresarNotificacionesRequest
                    {
                        rut = rutDefinitivo,
                        estadoNotificacion = unoIntString,
                        codigoNotificacion = codigoNotificacion,
                        delivery = delivery
                    });
                }
                else
                {
                    listadoNotificaciones.Add(new NotificacionesData()
                    {
                        Id = cont2,
                        EstadoNotificacion = estadoNotificacion,
                        CodigoNotificacion = codigoNotificacion,
                        Notificacion = notificacion,
                        Delivery = delivery
                    });
                    lstIngresaNotificaciones.Add(new IngresarNotificacionesRequest
                    {
                        rut = rutDefinitivo,
                        estadoNotificacion = ceroIntString,
                        codigoNotificacion = codigoNotificacion,
                        delivery = delivery
                    });
                }
                swtTipoNotificacion.CheckedChange += OnCheckedChangeTipoNotificacion;
            }
		}

        private String obtenerTipoTarjeta(String codigoNotificacion, String notificacion){
			
            String tipoNotificacionNombre = string.Empty;

			if (codigoNotificacion.Equals(TEF)){
                tipoNotificacionNombre = notificacion;
			}else if(codigoNotificacion.Equals(TC)){
                tipoNotificacionNombre = notificacion;
			}else if (codigoNotificacion.Equals(TD)){
                tipoNotificacionNombre = notificacion;
			}else if (codigoNotificacion.Equals(PAG)){
                tipoNotificacionNombre = notificacion;
            }

			return tipoNotificacionNombre;
		}

		private void construirSwitchProductos(List<JToken> otherResults,Typeface font,LinearLayout contenedorLayout){
			foreach (var items in otherResults)
            {
                string numeroProducto = Convert.ToString(JObject.Parse(items.ToString())[varConsultaProdNotiNumeroProducto]).Trim();
                string codigoTipoProducto = Convert.ToString(JObject.Parse(items.ToString())[varConsultaProdNotiCodigoTipoProducto]).Trim();
                string tipoProducto = Convert.ToString(JObject.Parse(items.ToString())[varConsultaProdNotiTipoProducto]).Trim();
                string glosaProducto = Convert.ToString(JObject.Parse(items.ToString())[varConsultaProdNotiGlosaProducto]).Trim();
                string estadoNotificacionProducto = Convert.ToString(JObject.Parse(items.ToString())[varConsultaProdNotiEstadoNotificacionProducto]);
                string montoPesos = Convert.ToString(JObject.Parse(items.ToString())[varConsultaProdNotiMontoPesos]);
                string montoDolar = Convert.ToString(JObject.Parse(items.ToString())[varConsultaProdNotiMontoDolar]);
				
                using(LinearLayout filaSwitch = new LinearLayout(this.Activity)){
                    filaSwitch.Orientation = Orientation.Horizontal;
                    filaSwitch.SetPadding(0, 20, 0, 20);

                    using(TextView etiqueta = new TextView(this.Activity)){
                        etiqueta.Typeface = font;
                        swtCuentasTarjetas = new Switch(this.Activity);
                        swtCuentasTarjetas.SetTrackResource(Resource.Drawable.switch_track_selector);
                        swtCuentasTarjetas.ThumbDrawable.SetColorFilter(Color.Rgb(245,245,245), PorterDuff.Mode.Multiply);
                        if (tipoProducto.Equals(tarDebito))
                        {
                            etiqueta.Text = encabezadoTarjetaDebito + numeroProducto.Substring(quince, cuatro);
                        }
                        else
                        {
                            if (tipoProducto.Equals(cuentaCorriente))
                            {
                                etiqueta.Text = encabezadoCuentaCorriente + numeroProducto;
                            }
                            if (tipoProducto.Equals(tarCredito))
                            {
                                if (glosaProducto.Equals(tcBlack))
                                {
                                    etiqueta.Text = encabezadoTarjetaCreditoBlack + numeroProducto.Substring(quince, cuatro);
                                }
                                else if (glosaProducto.Equals(tcOne))
                                {
                                    etiqueta.Text = encabezadoTarjetaCreditoOne + numeroProducto.Substring(quince, cuatro);
                                }


                            }
                        }

                        swtCuentasTarjetas.Id = cont1;

                        if (estadoNotificacionProducto.Equals(ceroIntString))
                        {
                            swtCuentasTarjetas.Checked = false;
                        }
                        else
                        {
                            swtCuentasTarjetas.Checked = true;
                        }

                        switchVista.Add(swtCuentasTarjetas);

                        filaSwitch.AddView(swtCuentasTarjetas, 0);
                        filaSwitch.AddView(etiqueta, 1);
                    }

                    contenedorLayout.AddView(filaSwitch, cont1);
                }

                // switch activado / agregar a lista de activados. 
                if (estadoNotificacionProducto.Equals(unoIntString))
                {
                    listadoProductosActivados.Add(new OnOffData()
                    {
                        Id = cont1,
                        Pan = numeroProducto,
                        ImageId = cero,
                        TipoProducto = tipoProducto,
                        CodigoTipoProducto = codigoTipoProducto,
                        GlosaProducto = glosaProducto,
                        EstadoNotificacionProducto = estadoNotificacionProducto,
                        MontoPesos = montoPesos,
                        MontoDolar = montoDolar
                    });

                    lstIngresaProdNoti.Add(new IngresarProductosNotificacionesRequest
                    {
                        rut = rutDefinitivo,
                        numeroProducto = numeroProducto,
                        codigoTipoProducto = codigoTipoProducto,
                        glosaProducto = glosaProducto,
                        estadoNotificacionProducto = unoIntString,
                        montoPesos = txtMontoPeso.Text,
                        montoDolar = txtMontoDolar.Text
                    });
                }else{
                    listadoProductosDesActivados.Add(new OnOffData()
                     {
                         Id = cont1,
                         Pan = numeroProducto,
                         ImageId = cero,
                         TipoProducto = tipoProducto,
                         CodigoTipoProducto = codigoTipoProducto,
                         GlosaProducto = glosaProducto,
                         EstadoNotificacionProducto = estadoNotificacionProducto,
                         MontoPesos = montoPesos,
                         MontoDolar = montoDolar
                     });
                }
                cont1++;
                swtCuentasTarjetas.CheckedChange += OnCheckedChange;
            }
		}

        List<String> listadoMontoPesos;
        List<String> listadoMontoDolares;

        public void calculaMonto(List<JToken> listaConsultaProductos){
            listadoMontoPesos = new List<string>();
            listadoMontoDolares = new List<string>();
            string pesos = string.Empty;
            string dolares = string.Empty;

            foreach (var producto in listaConsultaProductos)
            {
                string montoPesos = Convert.ToString(JObject.Parse(producto.ToString())[varConsultaProdNotiMontoPesos]);
                string montoDolar = Convert.ToString(JObject.Parse(producto.ToString())[varConsultaProdNotiMontoDolar]);
                if (!montoPesos.Equals(ceroIntString) && !montoPesos.Equals(ceroDoubleString))
                {
                    pesos = montoPesos;
                    listadoMontoPesos.Add(pesos);
                }else{
                    pesos = ceroDoubleString;
                    listadoMontoPesos.Add(pesos);
                }

                if (!montoDolar.Equals(ceroIntString) && !montoDolar.Equals(ceroDoubleString))
                {
                    dolares = montoDolar;
                    listadoMontoDolares.Add(dolares);
                }else{
                    dolares = ceroDoubleString;
                    listadoMontoDolares.Add(dolares);
                }
            }

            foreach (var auxMontoPesos in listadoMontoPesos)
            {
                if (!auxMontoPesos.Equals(ceroIntString) && !auxMontoPesos.Equals(ceroDoubleString))
                {
                    txtMontoPeso.Text = auxMontoPesos.Replace(".0","");
                    break;
                }
                else
                {
                    txtMontoPeso.Text = "0";
                }
            }

            foreach (var auxMontoDolares in listadoMontoDolares)
            {
                if (!auxMontoDolares.Equals(ceroIntString) && !auxMontoDolares.Equals(ceroDoubleString))
                {
                    txtMontoDolar.Text =  auxMontoDolares;
                    break;
                }
                else
                {
                    txtMontoDolar.Text = ceroDoubleString;
    
                }
            }
        }

        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            using(Switch btn = (Switch)sender){
                insertaInformacionProductos(btn);
            }
        }

        private void OnCheckedChangeTipoNotificacion(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            using(Switch btnSwtTipoNotificacion = (Switch)sender){
                insertaInformacionEnNotificaciones(btnSwtTipoNotificacion);
            }
        }

        private string codigoDeliveryNotificaciones(bool swtAppOnOff, bool swtCorreo, bool swtSms){
            string codigoDelivery = ceroCeroCero;
            if (!swtAppOnOff && !swtCorreo && swtSms)
            {
                codigoDelivery = ceroCeroUno;
            }else if (!swtAppOnOff && swtCorreo && !swtSms)
            {
                codigoDelivery = ceroCeroDos;
            }else if (!swtAppOnOff && swtCorreo && swtSms)
            {
                codigoDelivery = ceroCerotres;
            }else if (swtAppOnOff && !swtCorreo && !swtSms)
            {
                codigoDelivery = ceroCeroCuatro;
            }else if (swtAppOnOff && !swtCorreo && swtSms)
            {
                codigoDelivery = ceroCeroCinco;
            }else if (swtAppOnOff && swtCorreo && !swtSms)
            {
                codigoDelivery = ceroCeroSeis;
            }else if (swtAppOnOff && swtCorreo && swtSms){
                codigoDelivery = ceroCeroSiete;
            }
            return codigoDelivery;
        }
        private void insertaInformacionProductos(Switch switchSeleccionado)
        {
            bool estadoSwitch = switchSeleccionado.Checked;

            if(estadoSwitch){
                bool validadorProductoAgregado = false;
                foreach (var producto in listadoProductosActivados)
                {
                    if (switchSeleccionado.Id == producto.Id)
                    {
                        validadorProductoAgregado = true;
                    }
                }

                if(!validadorProductoAgregado){
                    OnOffData nuevoProductoActivado = null;
                    foreach (var producto in listadoProductosDesActivados)
                    {
                        if (switchSeleccionado.Id == producto.Id)
                        {
                            nuevoProductoActivado = producto;
                        }
                    }

                    lstIngresaProdNoti.Add(new IngresarProductosNotificacionesRequest
                    {
                        rut = rutDefinitivo,
                        numeroProducto = nuevoProductoActivado.Pan,
                        codigoTipoProducto = nuevoProductoActivado.CodigoTipoProducto,
                        glosaProducto = nuevoProductoActivado.GlosaProducto,
                        estadoNotificacionProducto = unoIntString,
                        montoPesos = txtMontoPeso.Text,
                        montoDolar = txtMontoDolar.Text
                    });

                    listadoProductosActivados.Add(nuevoProductoActivado);
                    listadoProductosDesActivados.Remove(nuevoProductoActivado);

                }
            }else{
                bool validadorProductoAgregado = false;
                OnOffData productoDesSeleccionado = null;
                foreach (var producto in listadoProductosActivados)
                {
                    if (switchSeleccionado.Id == producto.Id)
                    {
                        productoDesSeleccionado = producto;
                    }
                }

                if (productoDesSeleccionado != null)
                {
                    listadoProductosActivados.Remove(productoDesSeleccionado);
                    listadoProductosDesActivados.Add(productoDesSeleccionado);
                    IngresarProductosNotificacionesRequest productoEliminar = null;
                    foreach (var producto in lstIngresaProdNoti)
                    {
                        if (productoDesSeleccionado.Pan == producto.numeroProducto)
                        {
                            productoEliminar = producto;
                        }
                    }
                    lstIngresaProdNoti.Remove(productoEliminar);
                }
            }
            validarEnvioVacioProducto();
            guardarProductosSeleccionados();
        }

        public void validarEnvioVacioProducto(){
            if (lstIngresaProdNoti.Count == cero){
                lstIngresaProdNoti.Add(productoVacio);
            }
            else{
                lstIngresaProdNoti.Remove(productoVacio);
            }
        }

        public void validarEnvioVacioNotificacion(){   
            if (lstIngresaNotificaciones.Count == cero)
            {
                lstIngresaNotificaciones.Add(notificacionVacio);
            }
            else
            {
                lstIngresaNotificaciones.Remove(notificacionVacio);
            }
        }

        public async void guardarProductosSeleccionados(){
            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            try{
                JsonValue jsonResponseAccessToken = WebServiceSecurity.ServiciosSecurity.CallRESTToken();
                if (!string.IsNullOrEmpty(jsonResponseAccessToken.ToString()))
                {
                    JsonValue jt = jsonResponseAccessToken[varAccesstoken];
                    rutConocido = MainActivity.returnRutConsultaEnrolado();
                    rutDesdeConfirmacion = ConfirmacionEnroladoActivity.returnRutConsultaEnroladoConocido();
                    rutDefinitivo = rutConocido;
                    if (string.IsNullOrEmpty(rutDefinitivo))
                    {
                        rutDefinitivo = rutDesdeConfirmacion;
                    }
                    string imei = this.getIMEI();
                    string idDispositivoLogIngresaProductoNotificaciones = parametriaLogUtil.getIdDispositivoParaLog(imei);
                    string ipDisposiivoLogIngresaProductoNotificaciones = ParametriaLogUtil.GetIpLocal();
                    JsonValue jsonResponseIngresaProductoNotificaciones = await WebServiceSecurity.ServiciosSecurity.CallRESTInsertaProductoNotificaciones(jt, lstIngresaProdNoti, rutDefinitivo, idDispositivoLogIngresaProductoNotificaciones, ipDisposiivoLogIngresaProductoNotificaciones);
                    if (!string.IsNullOrEmpty(jsonResponseIngresaProductoNotificaciones.ToString()))
                    {
                        string jsonStatusResponseIngresaProductoNotificaciones = jsonResponseIngresaProductoNotificaciones[varStatusCode];
                        if (jsonStatusResponseIngresaProductoNotificaciones.Equals(responseSuccess))
                        {
                            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                        }
                        else
                        {
                            ErrorCargaDatos();
                        }
                    }else{
                        DialogoErrorActivity.mostrarViewErrorLoginEnrolado(Activity);
                    }
                }else{
                    DialogoErrorActivity.mostrarViewErrorLoginEnrolado(Activity);
                }

            }
                catch (Exception ex)
            {
                Console.WriteLine("Ex: " + ex.Message);
                ExceptionGeneric();
            }
        }

        public async void guardarProductosDesdeMontos()
        {
            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            try
            {
                JsonValue jsonResponseAccessToken = WebServiceSecurity.ServiciosSecurity.CallRESTToken();
                if (!string.IsNullOrEmpty(jsonResponseAccessToken.ToString()))
                {
                    JsonValue jt = jsonResponseAccessToken[varAccesstoken];
                    rutConocido = MainActivity.returnRutConsultaEnrolado();
                    rutDesdeConfirmacion = ConfirmacionEnroladoActivity.returnRutConsultaEnroladoConocido();
                    rutDefinitivo = rutConocido;
                    if (string.IsNullOrEmpty(rutDefinitivo))
                    {
                        rutDefinitivo = rutDesdeConfirmacion;
                    }
                    string imei = this.getIMEI();
                    string idDispositivoLogIngresaProductoNotificaciones = parametriaLogUtil.getIdDispositivoParaLog(imei);
                    string ipDisposiivoLogIngresaProductoNotificaciones = ParametriaLogUtil.GetIpLocal();
                    JsonValue jsonResponseIngresaProductoNotificaciones = await WebServiceSecurity.ServiciosSecurity.CallRESTInsertaProductoNotificaciones(jt, lstIngresaProdNoti, rutDefinitivo, idDispositivoLogIngresaProductoNotificaciones, ipDisposiivoLogIngresaProductoNotificaciones);
                    if (!string.IsNullOrEmpty(jsonResponseIngresaProductoNotificaciones.ToString()))
                    {
                        string jsonStatusResponseIngresaProductoNotificaciones = jsonResponseIngresaProductoNotificaciones[varStatusCode];
                        if (jsonStatusResponseIngresaProductoNotificaciones.Equals(responseSuccess))
                        {
                            btnConfirmarMontosDolarPeso.Visibility = ViewStates.Gone;
                            llMensajeExitosoMontos.Visibility = ViewStates.Visible;
                            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                        }
                        else
                        {
                            ErrorCargaDatos();
                        }
                    }else{
                        DialogoErrorActivity.mostrarViewErrorLoginEnrolado(Activity);
                    }
                }else{
                    DialogoErrorActivity.mostrarViewErrorLoginEnrolado(Activity);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex: " + ex.Message);
                ExceptionGeneric();
            }
        }

        /// <summary>
        /// Metodo insertaInformacion es el encargado de la invocacion 
        /// al servicio InsertaNotificaciones.
        /// </summary>
        private void insertaInformacionEnNotificaciones(Switch switchSeleccionadoNotificaciones)
        {
            //1) caso switch true // validar si el switch seleccionado se encuentra en el listado.
            // si no se encuentra , agregarlo .
            string codigoDelivery = codigoDeliveryNotificaciones(swtDeliveryAppOnOff.Checked, swtDeliveryCorreo.Checked, swtDeliverySMS.Checked);
            if (switchSeleccionadoNotificaciones.Checked)
            {
                // validar si switch se encuentra en la lista 
                foreach (var notificaciones in listadoNotificaciones)
                {
                    if (switchSeleccionadoNotificaciones.Id == notificaciones.Id)
                    {
                        lstIngresaNotificaciones.Add(new IngresarNotificacionesRequest
                        {
                            rut = rutDefinitivo,
                            estadoNotificacion = unoIntString,
                            codigoNotificacion = notificaciones.CodigoNotificacion,
                            delivery = codigoDelivery
                        });
                    }
                }
            }
            else
            {
                foreach (var notificaciones in listadoNotificaciones)
                {
                    if (switchSeleccionadoNotificaciones.Id == notificaciones.Id)
                    {
                        lstIngresaNotificaciones.Add(new IngresarNotificacionesRequest
                        {
                            rut = rutDefinitivo,
                            estadoNotificacion = ceroIntString,
                            codigoNotificacion = notificaciones.CodigoNotificacion,
                            delivery = codigoDelivery
                        });
                    }
                }
            }
            guardarNotificacionesSeleccionados();
        }

        public async void guardarNotificacionesSeleccionados()
        {
            dialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity();
            try
            {
                JsonValue jsonResponseAccessToken = WebServiceSecurity.ServiciosSecurity.CallRESTToken();
                if (!string.IsNullOrEmpty(jsonResponseAccessToken.ToString()))
                {
                    JsonValue jt = jsonResponseAccessToken[varAccesstoken];
                    JsonValue jsonResponseIngresaNotificaciones = await WebServiceSecurity.ServiciosSecurity.CallRESTInsertaNotificaciones(jt, lstIngresaNotificaciones, UtilAndroid.getRut(), parametriaLogUtil.getIdDispositivoParaLog(UtilAndroid.getIMEI(Activity)), ParametriaLogUtil.GetIpLocal());
                    if (!string.IsNullOrEmpty(jsonResponseIngresaNotificaciones.ToString()))
                    {
                        string jsonStatusResponseIngresaProductoNotificaciones = jsonResponseIngresaNotificaciones[varStatusCode];
                        if (jsonStatusResponseIngresaProductoNotificaciones.Equals(responseSuccess))
                        {
                            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                        }
                        else
                        {
                            ErrorCargaDatos();
                        }
                    }else{
                        DialogoErrorActivity.mostrarViewErrorLoginEnrolado(Activity);
                    }
                }else{
                    DialogoErrorActivity.mostrarViewErrorLoginEnrolado(Activity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ExceptionGeneric();
            }
        }
        public void ExceptionGeneric()
        {
            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            DialogoErrorActivity.mostrarViewErrorHome(this.Activity);
        }
        public void ErrorCargaDatos()
        {
            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
            DialogoErrorActivity.mostrarViewErrorHome(this.Activity);
        }

        public String getIMEI()
        {
            using(TelephonyManager imei = (TelephonyManager)Activity.GetSystemService(Context.TelephonyService)){
                return imei.DeviceId;
            }
        }

        public bool OnKeyUp(Keycode keyCode, KeyEvent e)
        {
            bool formateado = false;
            if (keyCode == Keycode.Num1)
            {
                formateado = true;
            }
            return formateado;
        }
    }
}