using System;
using Android.App;
using Android.Text;
using Android.Widget;
using Java.Lang;

namespace BancoSecurityOnOff.Droid
{
    public class MascarasTextWatcher : Java.Lang.Object, ITextWatcher
    {
        EditText editText;
        Activity activity;
        string textoFormateado;

        public static int TIPO_RUT = 1;
        public static int TIPO_MONTO_PESOS = 2;
        public static int TIPO_MONTO_USD = 3;
        string[] montoDolar;
        string montoDolarEntero = string.Empty;
        string montoDolarDecimal = string.Empty;
        int tipoFormateador;

        public MascarasTextWatcher(EditText editText, Activity activity,int tipoFormateador)
        {
            this.activity = activity;
            this.editText = editText;
            this.tipoFormateador = tipoFormateador;
        }

        public void AfterTextChanged(IEditable s)
        {
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (tipoFormateador == TIPO_RUT)
            {
                textoFormateado = MascarasEditText.formatearRut(editText.Text);
                editText.RemoveTextChangedListener(this); // removemos el lister para no caer en loop infinito. 
                editText.Text = textoFormateado;
                editText.AddTextChangedListener(this);
                editText.SetSelection(editText.Text.Length);
                // editText.SetRawInputType(InputTypes.NumberVariationNormal);
                //editText.InputType = Android.Text.InputTypes.TextVariationPassword; //| Android.Text.InputTypes.ClassNumber;
                editText.SetCursorVisible(true);
            }
            else if (tipoFormateador == TIPO_MONTO_PESOS)
            {
                textoFormateado = MascarasEditText.formatearMonto(editText.Text);
                editText.RemoveTextChangedListener(this); // removemos el lister para no caer en loop infinito. 
                editText.Text = textoFormateado;
                editText.AddTextChangedListener(this);
                editText.SetSelection(editText.Text.Length);
                // editText.SetRawInputType(InputTypes.NumberVariationNormal);
                //editText.InputType = Android.Text.InputTypes.TextVariationPassword; //| Android.Text.InputTypes.ClassNumber;
                editText.SetCursorVisible(true);
            }
            else if (tipoFormateador == TIPO_MONTO_USD)
            {
                string dolar = editText.Text;
                if (dolar.Equals("0.0"))
                {
                    dolar = "0,00";
                }
                dolar = dolar.Replace(",", "");
                if (dolar.Trim().Length >=3){

                    dolar = dolar.Replace(".0", "");
                    int tam_var = dolar.Length;
                    string decimalDolar = dolar.Substring((tam_var - 2), 2);
                    dolar = dolar.Replace(dolar.Substring((tam_var - 2), 2), "");
                    dolar = dolar + "," + decimalDolar;
                    montoDolar = dolar.Split(',');
                    montoDolarEntero = montoDolar[0];
                    montoDolarDecimal = montoDolar[1];
                    if (!dolar.Contains(","))
                    {
                        montoDolarEntero = dolar;
                    }
                }else if(dolar.Trim().Length == 2){
                    dolar = dolar.Replace(".0", "");
                    dolar = dolar.Replace(",", "");
                    int tam_var = dolar.Length;
                    string decimalDolar = dolar.Substring((tam_var - 1), 1);
                    dolar = dolar.Replace(dolar.Substring((tam_var - 1), 1), "");
                    dolar = dolar + "," + decimalDolar;
                    montoDolar = dolar.Split(',');
                    montoDolarEntero = montoDolar[0];
                    montoDolarDecimal = montoDolar[1];
                    if(!dolar.Contains(",")){
                        montoDolarEntero = dolar;
                    }
                }else if(dolar.Trim().Length == 1){
                    montoDolarDecimal = string.Empty;
                    dolar = dolar.Replace(",", "");
                    dolar = dolar + string.Empty;
                    if (!dolar.Contains(","))
                    {
                        montoDolarEntero = dolar;
                    }

                }else if (dolar.Trim().Length == 0){
                    montoDolarEntero = string.Empty;
                    dolar = dolar + string.Empty;
                    if (!dolar.Contains(","))
                    {
                        montoDolarEntero = dolar;
                    }
                }
                textoFormateado = MascarasEditText.formatearMontoDolar(montoDolarEntero, montoDolarDecimal);
                editText.RemoveTextChangedListener(this); // removemos el lister para no caer en loop infinito. 
                editText.Text = textoFormateado;

                string editTextFormateado = editText.Text;
                string[] div; 
                string montoDolarEnt = string.Empty;
                string montoDolarDec = string.Empty;
                editText.AddTextChangedListener(this);
                div = editTextFormateado.Split(',');
                montoDolarEnt = div[0];
                montoDolarDec = div[1];
                editText.SetSelection(editText.Text.Length);
                if (montoDolarDec.Trim().Length >= 3)
                {
                    editText.SetSelection(editText.Text.Length -3);
                }
                // editText.SetRawInputType(InputTypes.NumberVariationNormal);
                //editText.InputType = Android.Text.InputTypes.TextVariationPassword; //| Android.Text.InputTypes.ClassNumber;
                editText.SetCursorVisible(true);
            }
        }
    }
}
