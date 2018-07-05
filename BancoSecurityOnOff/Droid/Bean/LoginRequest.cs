using System;
namespace BancoSecurityOnOff.Droid.Bean
{
    public class LoginRequest
    {
        public ParametriaLog logHeader { get; set; }
        public String documentNumber { get; set; }
        public String password { get; set; }
        public String userType { get; set; }
        public String channelName { get; set; }
    }
}
