using System;
namespace BancoSecurityOnOff.Droid.Bean
{
    public class ConsultaProductosNotificacionesRequest
    {
        public ParametriaLog logHeader { get; set; }
        public String flagInicio { get; set; }//S
        public String tamanoBloque { get; set; } //5
        public String paginaConsultar { get; set; }//1
        public String numeroRegistros { get; set; }//5
        public String totalPaginas { get; set; }//0
        public String ultimoRegistro { get; set; }//0
        public String  rut { get; set; }//1343560w22"
    }
}
