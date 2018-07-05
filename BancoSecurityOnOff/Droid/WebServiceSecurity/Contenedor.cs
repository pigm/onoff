using System;
using System.Collections.Generic;

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
    public class Contenedor<T>
    {
        public List<T> Contenido { get; set; }
        public string MensajeError { get; set; }
       
        public Contenedor()
        {
            Contenido = new List<T>();
        }
    }
}
