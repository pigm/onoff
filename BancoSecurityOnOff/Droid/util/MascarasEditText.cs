using System;
namespace BancoSecurityOnOff.Droid
{
    public class MascarasEditText
    {
        public static string formatearMonto(string monto)
        {
            string montoPeso;
            if (monto == null || monto.Equals(""))
            {
                montoPeso = string.Empty;
            }else{
                monto = limpiarMontoPesos(monto);
           
                long num = Convert.ToInt64(monto);
                monto = String.Format("{0:N0}", num);

                montoPeso = monto.Replace(",", ".");
            }
            return montoPeso;
        }

        public static string formatearMontoDolar(string monto, string decimalDolar)
        {
            string montoDolar;
            if (monto == null || monto.Equals(""))
            {
                montoDolar = string.Empty;
            }
             else
            {
                monto = limpiarMontoPesos(monto);

                long num = Convert.ToInt64(monto);
                monto = String.Format("{0:N0}", num);

                montoDolar = monto.Replace(",", ".");
            }
            return montoDolar+","+decimalDolar;
        }

        public static string limpiarMontoPesos(string monto)
        {
            monto = monto.Replace(".", "");
            return monto;
        }

        public static string formatearRut(string rut){
            if (rut.Length > 3)
            {
                rut = obtenerRutFormateado(rut);
            }
            else
            {
                rut = limpiaRut(rut);
            }

            return rut;
        }

        public static string obtenerRutFormateado(string rut)
        {
            int cont = 0;
            string format;
            if (rut.Length == 0)
            {
                return "";
            }
            else
            {
                rut = limpiaRut(rut);
                format = "-" + rut.Substring(rut.Length - 1);
                for (int i = rut.Length - 2; i >= 0; i--)
                {
                    format = rut.Substring(i, 1) + format;
                    cont++;
                    if (cont == 3 && i != 0)
                    {
                        format = "." + format;
                        cont = 0;
                    }
                }
                return format;
            }
        }

        public static string limpiaRut(string _rut, int range = 0)
        {
            _rut = _rut.Replace(".", "").Replace("-", "");

            if (range == 1)
            {
                _rut = _rut.Remove(_rut.Length - 1);
            }
            return _rut;
        }
    }
}
