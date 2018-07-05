using System;
namespace BancoSecurityOnOff.Droid.Bean
{
    public class ChangePaymentChannelsRequest
    {
        public ParametriaLog logHeader { get; set; }
        public String pan { get; set; }// 0005523002200004854
        public String estadoPos { get; set; }// 1
        public String estadoAtm { get; set; }// 1
        public String estadoEcom { get; set; }// 1
        public String estadoMoto { get; set; }// 1
    }
}
