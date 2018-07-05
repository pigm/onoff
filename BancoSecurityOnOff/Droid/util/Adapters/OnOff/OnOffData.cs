using System;
using System.Json;
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
    public class OnOffData
    {
        public int Id { get; set; }
        public string TipoProducto { get; set; }
        public int ImageId { get; set; }
        public string Pan { get; set; }
        public string CodigoTipoProducto { get; set; }
        public string GlosaProducto { get; set; }
        public string EstadoNotificacionProducto { get; set; }
        public string MontoPesos { get; set; }
        public string MontoDolar { get; set; }
        public string NombreTarjeta { get; set; }

        public JsonValue estadoTarjetaCredito { get; set; } 
        public JsonValue estadoTarjetaDebito { get; set; } 
    }
}
