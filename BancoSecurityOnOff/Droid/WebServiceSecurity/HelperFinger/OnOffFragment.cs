using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Json;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using BancoSecurityOnOff.Droid.Bean;
using BancoSecurityOnOff.Droid.Resources.ViewPageHelper;
using BancoSecurityOnOff.Droid.Resources.ViewPageHelper.Transformer;
using BancoSecurityOnOff.Utilidades.Firmas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Android.Telephony;

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
    public class OnOffFragment: Android.App.Fragment
    {
		private const string rutaFuenteTitilium = "fonts/titillium_web/TitilliumWeb-Regular.ttf";
        private const string TEXTO_ESTADO_SUCCESS = "Success";
        ViewPager componenteCarrusel;
		OnOffAdapter adapterCarrusel;

        string rutConocido;
        string rutDesdeConfirmacion;
        string rutDefinitivo;
        ParametriaLogUtil parametriaLogUtil;
		TextView titulocarrusel;
		LoadingUtil loading;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState){
            DialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity(Activity);
            View view = inflater.Inflate(Resource.Layout.OnOff, container, false);
            base.OnCreate(savedInstanceState);
			titulocarrusel = view.FindViewById<TextView>(Resource.Id.titulocarrusel);
			componenteCarrusel = view.FindViewById<ViewPager>(Resource.Id.componente_carrusel);
			var font = Typeface.CreateFromAsset(this.Activity.Assets, rutaFuenteTitilium);
			titulocarrusel.Typeface = font;
            parametriaLogUtil = new ParametriaLogUtil();
			generarPaginasCarrusel();

            return view;
        }

		private async void generarPaginasCarrusel()
        {
            List<OnOffData> lstResult = new List<OnOffData>();
            rutConocido = MainActivity.returnRutConsultaEnrolado();
            rutDesdeConfirmacion = ConfirmacionEnroladoActivity.returnRutConsultaEnroladoConocido();
            rutDefinitivo = rutConocido;

            string idDispositivoLog = parametriaLogUtil.getIdDispositivoParaLog(getIMEI());
            string ipDispositivoLog= ParametriaLogUtil.GetIpLocal();

			if (string.IsNullOrEmpty(rutDefinitivo)){
                rutDefinitivo = rutDesdeConfirmacion;
			}
           // DialogoLoadingBcoSecurityActivity.mostrarViewLoadingSecurity(Activity);
			try{
				JsonValue jsonResponseAccessToken = await WebServiceSecurity.ServiciosSecurity.CallRESTaccessToken();
                if (!string.IsNullOrEmpty(jsonResponseAccessToken.ToString()))
                {
                    JsonValue token = jsonResponseAccessToken["access_token"];
                    string imei = this.getIMEI();
                    string idDispositivoLogConsultaProductoNotificaciones = parametriaLogUtil.getIdDispositivoParaLog(imei);
                    string ipDisposiivoLogConsultaProductoNotificaciones = ParametriaLogUtil.GetIpLocal();
                    JsonValue jsonResponseConsultaProductoNotificaciones = await WebServiceSecurity.ServiciosSecurity.CallRESTConsultaProductoNotificaciones(token, SecurityEndpoints.flagInicio, SecurityEndpoints.tamanoBloque, SecurityEndpoints.paginaConsultar, SecurityEndpoints.numeroRegistros, SecurityEndpoints.totalPaginas, SecurityEndpoints.ultimoRegistro, rutDefinitivo, idDispositivoLogConsultaProductoNotificaciones, ipDisposiivoLogConsultaProductoNotificaciones);
                    if (!string.IsNullOrEmpty(jsonResponseConsultaProductoNotificaciones.ToString())){
                        string jcpnStatus = jsonResponseConsultaProductoNotificaciones["statusCode"];

                        if (jcpnStatus.Equals("Success"))
                        {
                            JsonValue productosBcoSecurity = jsonResponseConsultaProductoNotificaciones["ProductosNotificaciones"];
                            int tarjetaProducto = 0;
                            JArray arregloProductos = JArray.Parse(productosBcoSecurity.ToString());
                            List<JToken> listadoProductosBco = arregloProductos.Children().ToList();
                            ProductoCard pc = new ProductoCard();

                            foreach (var items in listadoProductosBco)
                            {

                                string numeroProducto = Convert.ToString(JObject.Parse(items.ToString())["numeroProducto"]).Trim();
                                string codigoTipoProducto = Convert.ToString(JObject.Parse(items.ToString())["codigoTipoProducto"]).Trim();
                                string tipoProducto = Convert.ToString(JObject.Parse(items.ToString())["tipoProducto"]).Trim();
                                string glosaProducto = Convert.ToString(JObject.Parse(items.ToString())["glosaProducto"]).Trim();
                                string estadoNotificacionProducto = Convert.ToString(JObject.Parse(items.ToString())["estadoNotificacionProducto"]);
                                string montoPesos = Convert.ToString(JObject.Parse(items.ToString())["montoPesos"]);
                                string montoDolar = Convert.ToString(JObject.Parse(items.ToString())["montoDolar"]);
                                string nombreTarjeta = Convert.ToString(JObject.Parse(items.ToString())["nombreTarjeta"]).Trim();
                                JsonValue estadoTarjetaCredito = null;
                                JsonValue estadoTarjetaDebito = null;

                                bool validadorAgregar = false;

                                if (tipoProducto.Equals("TARJETA DEBITO")){
                                    tarjetaProducto = Resource.Drawable.td;//imagen
                                    string pan = numeroProducto;
                                    int contadorFinal = pan.Length - 3;
                                    pan = pan.Substring(3, contadorFinal); // para redbank no se envia los ceros del comienzo.
                                    //(JsonValue token, string cceFi, string cceAbaBranch, string ccePan, string cceAccountType, string run, string idDispositivoApp, string ipOrigenApp)

                                    estadoTarjetaDebito = WebServiceSecurity.ServiciosSecurity.CallRESTCheckCardStatus(token, SecurityEndpoints.cceFi, SecurityEndpoints.cceAbaBranch, pan, 
                                                                                                                       SecurityEndpoints.cceAccountType, rutDefinitivo, 
                                                                                                                       idDispositivoLog,
                                                                                                                       ipDispositivoLog);


                                    string codigoEstadoDebito = estadoTarjetaDebito["statusCode"];
                                    if (codigoEstadoDebito != null && codigoEstadoDebito.Equals(TEXTO_ESTADO_SUCCESS))
                                    {
                                        validadorAgregar = true;
                                    }

                                }else if(tipoProducto.Equals("TARJETA CREDITO")){
                                    if (glosaProducto.Equals("MASTER BLACK"))
                                    {
                                        tarjetaProducto = Resource.Drawable.tcb;//imagen
                                    }
                                    else if (glosaProducto.Equals("MASTER GOLD"))
                                    {
                                        tarjetaProducto = Resource.Drawable.tc;//imagen
                                    }
                                    estadoTarjetaCredito = WebServiceSecurity.ServiciosSecurity.CallRESTGetCardData(token, rutDefinitivo, numeroProducto,
                                                                                                                                     idDispositivoLog, ipDispositivoLog);

                                    string codigoEstadoCredito = estadoTarjetaCredito["statusCode"];
                                    if(codigoEstadoCredito != null && codigoEstadoCredito.Equals(TEXTO_ESTADO_SUCCESS)){
                                        validadorAgregar = true;
                                    }
                                }

                                if(validadorAgregar == true){
                                    lstResult.Add(new OnOffData()
                                    {
                                        Pan = numeroProducto,
                                        ImageId = tarjetaProducto,
                                        TipoProducto = tipoProducto,
                                        CodigoTipoProducto = codigoTipoProducto,
                                        GlosaProducto = glosaProducto,
                                        EstadoNotificacionProducto = estadoNotificacionProducto,
                                        MontoPesos = montoPesos,
                                        MontoDolar = montoDolar,
                                        NombreTarjeta = nombreTarjeta,
                                        estadoTarjetaCredito = estadoTarjetaCredito,
                                        estadoTarjetaDebito = estadoTarjetaDebito
                                    });
                                }
                            }
                        }
                        else{
                            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                            DialogoErrorActivity.mostrarViewErrorHome(this.Activity);
                        }
                    }else{
                        DialogoErrorActivity.mostrarViewErrorLoginEnrolado(this.Activity);
                    }
                }
                else{
                    DialogoErrorActivity.mostrarViewErrorLoginEnrolado(this.Activity);
                }


            }
            catch (System.Exception ex)
            {
                DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
                DialogoErrorActivity.mostrarViewErrorHome(this.Activity);
				Console.WriteLine(ex.Message);
            }

			adapterCarrusel = new OnOffAdapter(this.Activity, lstResult);
            componenteCarrusel.Adapter = adapterCarrusel;
            DialogoLoadingBcoSecurityActivity.ocultarLoadingSecurity();
		//componenteCarrusel.Invalidate();
        }

        public static List<string> InvalidJsonElements;
        public static IList<T> DeserializeToList<T>(string jsonString)
        {
            ProductoCard productoCard = new ProductoCard();
            InvalidJsonElements = null;
            var array = JArray.Parse(jsonString);
            IList<T> objectsList = new List<T>();
            foreach (var item in array)
            {
                try
                {
                    objectsList.Add(item.ToObject<T>());
                }
                catch (Exception ex)
                {
                    InvalidJsonElements = InvalidJsonElements ?? new List<string>();
                    InvalidJsonElements.Add(item.ToString());
                }

            }
            return objectsList;
        }

        public String getIMEI()
        {
            TelephonyManager imei;
            imei = (TelephonyManager)Activity.GetSystemService(Context.TelephonyService);
            return imei.DeviceId;
        }
    }
}
