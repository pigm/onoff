using System;
namespace BancoSecurityOnOff.Droid.Bean
{
    public class IngresarProductosNotificacionesRequest
    {
        public String rut { get; set; } 
        public String numeroProducto { get; set; } 
        public String codigoTipoProducto { get; set; } 
        public String glosaProducto { get; set; } 
        public String estadoNotificacionProducto { get; set; } 
        public String montoPesos { get; set; } 
        public String montoDolar { get; set; } 
    }
}
