using System;
namespace BancoSecurityOnOff.Droid.Bean
{
    public class EnrolarDispositivoRequest
    {
        public ParametriaLog logHeader { get; set; }
        public String idDispositivo { get; set; }// 0123456789
        public String rut { get; set; } //19
        public String sistema { get; set; }// @ONOFF
        public String canal { get; set; }// PERSONA
        public String estado { get; set; } //1
        public String sistemaOperativo { get; set; }// ANDROID
        public String alias { get; set; } //
        public String nombreDispositivo { get; set; }// ej: Samsumg G935F
    }
}
