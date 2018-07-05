using System;
using System.Collections.Generic;

namespace BancoSecurityOnOff.Droid.Bean
{
    public class ListIngresaNotificacion
    {
        public List<IngresarNotificacionesRequest> Notificacion { get; set; }

        public ListIngresaNotificacion(List<IngresarNotificacionesRequest> listado)
        {
            Notificacion = listado;
        }

        public ListIngresaNotificacion()
        {

        }
    }
}
