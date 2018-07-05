using System;
namespace BancoSecurityOnOff.Droid.Bean
{
    public class AutenticarClaveVozRequest
    {
        public ParametriaLog logHeader { get; set; }
        public String strGrupoIDG { get; set; }// security
        public String strIdMecanismo { get; set; }// OTP
        public String strRutCliente { get; set; } //168392036
        public String strRutUsuario { get; set; } //168392036
        public String strPassword { get; set; } 
        public String codigoTransaccion { get; set; }// 730
        public String strSessionId1 { get; set; } //0
        public String strSessionId2 { get; set; } //0
        public String idLog { get; set; } //0
        public String strIP { get; set; } //190.243.31.62
    }
}
