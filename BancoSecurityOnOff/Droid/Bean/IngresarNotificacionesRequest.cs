using System;
namespace BancoSecurityOnOff.Droid.Bean
{
    public class IngresarNotificacionesRequest
    {
        public String rut { get; set; }
        public String estadoNotificacion { get; set; } 
        public String codigoNotificacion { get; set; } 
        public String delivery { get; set; } 
    }
}
