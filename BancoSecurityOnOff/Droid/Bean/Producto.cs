using System;
using Newtonsoft.Json;

namespace BancoSecurityOnOff.Droid.Bean
{
    public class Producto
    {
     
            [JsonProperty("numeroProducto")]
            public string NumeroProducto;

            [JsonProperty("codigoTipoProducto")]
            public string CodigoTipoProducto;

            [JsonProperty("tipoProducto")]
            public string TipoProducto;

            [JsonProperty("glosaProducto")]
            public string GlosaProducto;

            [JsonProperty("estadoNotificacionProducto")]
            public string EstadoNotificacionProducto;

            [JsonProperty("montoPesos")]
            public string MontoPesos;

            [JsonProperty("montoDolar")]
            public string MontoDolar;


    }
}
