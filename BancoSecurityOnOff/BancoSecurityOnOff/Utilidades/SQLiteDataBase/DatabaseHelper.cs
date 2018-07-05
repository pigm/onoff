using System.Collections.Generic;
using System.Linq;
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
    public class DatabaseHelper{
        public static bool Insertar<T>(ref T data, string ruta_db){   
            using (SQLiteConnection conexion = new SQLiteConnection(ruta_db)){
                conexion.CreateTable<T>();
                if (conexion.Insert(data) > 0){
                    return true;
                }
            }
            return false;
        }

        public static string pass(string ruta_db){
            string contenido = string.Empty;
            using (SQLiteConnection conexion = new SQLiteConnection(ruta_db)){
                contenido = conexion.Table<Contenido>().ToString();
            }
            return contenido;
        }

        public static List<Contenido> sqliteTokenFCM(string ruta_db){
            List<Contenido> contenido = new List<Contenido>();
            using (SQLiteConnection conexion = new SQLiteConnection(ruta_db)){
                contenido = conexion.Table<Contenido>().ToList();
            }
            return contenido;
        }
    }
}
