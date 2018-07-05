using System;
using System.Collections.Generic;
using Android.Telephony;
using BancoSecurityOnOff.Utilidades.SQLiteDataBase;
using Android.Content;
using System.Net;
using System.Net.Sockets;

namespace BancoSecurityOnOff.Droid
{
    public class ParametriaLogUtil
    {
        List<Contenido> tokenFCM;
        string idDispositivoTokenFCM;
        string idTokenFCM;
        string idDispositivoLogLogin;
        String ruta = string.Empty;
        String nombreArchivo = string.Empty;
        String rutaCarpeta = string.Empty;

        const string relleno = "                                   ";
        public ParametriaLogUtil()
        {
        }

        public string getIdDispositivoParaLog(string imei)
        {
            idTokenFCM = MyFirebaseIIDService.SendRegistrationToServer();
            if (idTokenFCM != null)
            {
                nombreArchivo = "app-bco_security_enrolamiento.sqlite";
                rutaCarpeta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                ruta = System.IO.Path.Combine(rutaCarpeta, nombreArchivo);
                tokenFCM = new List<Contenido>();
                tokenFCM = DatabaseHelper.sqliteTokenFCM(ruta);

                foreach (var token in tokenFCM)
                {
                    if (token.Id == 1)
                    {
                        idDispositivoTokenFCM = token.Password;
                        break;
                    }

                }
                idDispositivoLogLogin = imei + relleno + idDispositivoTokenFCM;
            }else{
                idDispositivoLogLogin = string.Empty;
            }
            return idDispositivoLogLogin;
        }

        public static string GetIpLocal()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
