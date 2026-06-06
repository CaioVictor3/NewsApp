namespace NewsApp.Application.Helpers
{
    public static class StringUtils
    {
        public static string CapitalizarPrimeiraLetra(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return texto;

            texto = texto.Trim().ToLower();

            var resultado = new System.Text.StringBuilder(texto.Length);

            bool capitalizarProximo = true;

            foreach (char c in texto)
            {
                if (capitalizarProximo && char.IsLetter(c))
                {
                    resultado.Append(char.ToUpper(c));
                    capitalizarProximo = false;
                }
                else
                {
                    resultado.Append(c);
                }

                if (c == '.')
                    capitalizarProximo = true;
            }

            return resultado.ToString();
        }
    }
}
