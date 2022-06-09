using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMA.Common
{
    public class GeneratePassword
    {
        public static string Generar()
        {
            Random rdn = new Random();
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%$#@";
            int longitud = caracteres.Length;
            char letra;
            int longitudClave = 8;
            string claveAleatoria = string.Empty;
            for (int i = 0; i < longitudClave; i++)
            {
                letra = caracteres[rdn.Next(longitud)];
                claveAleatoria += letra.ToString();
            }
            return claveAleatoria;
        }
    }
}
