using System;
using System.Json;

namespace BancoSecurityOnOff.Droid.Bean
{
    /// <summary>
    /// Notificacion.
    /// Clase entidad que representa una notificacion extraida del servicio
    /// </summary>
    public class Notificacion
    {
        //Corresponde al Id de la Notificacion
        public long id { get; set; }
        //Corresponde al campo en el que se corrobora si hay fecha o no
        public bool esFecha { get; set; }
        //Correspon de al estado de la notificacion
        public bool estado { get; set; }
        //Corresponde a una version corta del mensaje
        public string mensajeNotificacionCorto { get; set; }
        //Corresponde al mensaje completo de la notificacion
        public string mensajeNotificacion { get; set; }
        //Corresponde a la imagen expandida de la fila
        public int imagenOn { get; set; }
        //Corresponde a la imagen retraida de la fila
        public int imagenOff { get; set; }
        //Corresponde a la fecha de emision de la notificacion
        public string fecha { get; set; }
        //Corresponde a la hora de emision de la notificacion
        public string hora { get; set; }
        public string idNotificacion{ get; set; }

    }
}
