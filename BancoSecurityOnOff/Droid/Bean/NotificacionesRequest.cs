using System;
namespace BancoSecurityOnOff.Droid.Bean
{
    /// <summary>
    /// Notificaciones request.
    /// clase que contiene el dato requerido para reralizar request a servicio hitorialNotificaciones
    /// </summary>
    public class NotificacionesRequest
    {
        public ParametriaLog logHeader { get; set; }
        //Corresponde al rut que se envia al servicio
        public string rut { get; set; }
    }
}
