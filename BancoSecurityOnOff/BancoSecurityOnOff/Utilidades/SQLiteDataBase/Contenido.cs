using System;
using SQLite;
/*
   @autor:          PaBlo Garrido
   @cargo:          Ingeniero de Software
   @institucion:    TInet soluciones informaticas
   @modificaciones: 
   @version:        1.0.0.0
   @proyecto:       OnOff
   
*/
namespace BancoSecurityOnOff.Utilidades.SQLiteDataBase
{
    public class Contenido{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Password { get; set; }
		public override string ToString(){
            return $"{Password}";
		}
	}
}
