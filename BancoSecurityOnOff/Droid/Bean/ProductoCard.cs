using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BancoSecurityOnOff.Droid.Bean
{
    public class ProductoCard
    {
     
        //public String numeroProducto { get; set; }              //000125663701
        //public String codigoTipoProducto { get; set; }          //E011
        //public String tipoProducto { get; set; }                //CUENTA CORRIENTE
        //public String glosaProducto { get; set; }               //CTA CTE SIN INTERESES PERSONAS NATURALES PESO 
        //public String estadoNotificacionProducto { get; set; }  // 1
        //public String montoPesos { get; set; }                  //1000
        //public String montoDolar { get; set; }                  //2

        [JsonProperty("ProductosNotificaciones")]
        public List<Producto> productosNotificaciones { get; set; }

        public ProductoCard()
        {
            productosNotificaciones = new List<Producto>();
        }
    }
}
