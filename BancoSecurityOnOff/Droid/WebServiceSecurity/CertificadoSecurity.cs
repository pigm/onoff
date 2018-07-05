using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

/*
   @autor:          PaBlo Garrido
   @cargo:          Ingeniero de Software
   @institucion:    TInet soluciones informaticas
   @modificaciones: 
   @version:        1.0.0.0
   @proyecto:       OnOff
   
*/

namespace BancoSecurityOnOff.Droid.WebServiceSecurity
{
    public static class CertificadoSecurity
    {
        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors){
            return true;
        }
    }
}
