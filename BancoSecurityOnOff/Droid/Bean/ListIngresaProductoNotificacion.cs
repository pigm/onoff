using System;
using System.Collections.Generic;

namespace BancoSecurityOnOff.Droid.Bean
{
    public class ListIngresaProductoNotificacion
    {
        public List<IngresarProductosNotificacionesRequest> notificacion { get; set; }

        public ListIngresaProductoNotificacion(List<IngresarProductosNotificacionesRequest> listado)
        {
            notificacion = listado;
        }

        public ListIngresaProductoNotificacion()
        {

        }
    }
}
