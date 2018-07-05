using System;
/**
   @autor:          PaBlo Garrido
   @cargo:          Ingeniero de Software
   @institucion:    TInet soluciones informaticas
   @modificaciones: 
   @version:        1.0.0.0
   @proyecto:       OnOff
   
*/
namespace BancoSecurityOnOff.Droid.Resources.NotificacionesAdapter
{
    public class NotificacionesData
    {
        public int Id { get; set; }
        public string EstadoNotificacion { get; set; }
        public string CodigoNotificacion { get; set; }
        public string Notificacion { get; set; }
        public string Delivery { get; set; }
    }
}
