using System;
namespace BancoSecurityOnOff.Droid.Bean
{
    public class SetEnabledRequest
    {
        public ParametriaLog logHeader { get; set; }
        public String pan { get; set; } //0005523002200004854
        public String estado { get; set; } //1
    }
}
