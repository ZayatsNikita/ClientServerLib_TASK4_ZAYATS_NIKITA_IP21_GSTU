using System;
using System.Text;

namespace ClientServerLib
{
    /// <summary>
    /// The class that is used to convert strings.
    /// </summary>
    public static class MessageConverter
    {

        static string[] rusLetters = new string[]{"а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й",
          "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц",
          "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };
        static string[] engLetters = new string[]{ "a", "b", "v", "g", "d", "e", "e", "zj", "z", "i", "j",
          "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kx", "c",
          "ch", "sh", "shch", "", "y", "", "ea", "yu", "ya" };


        /// <summary>
        /// A method that replaces all Russian letters in a string with their transliterated counterparts.
        /// </summary>
        /// <param name="message">String to be processed.</param>
        /// <returns>The processed message</returns>
        public static string ConvertRussianToTranslit(string message)
        {
            if (message == null)
            {
                throw new NullReferenceException();
            }
            StringBuilder builder = new StringBuilder();

            int length = message.Length;
            bool IsBig;
            string forAdd;
            for (int index = 0; index < length; index++)
            {
                IsBig = Char.IsUpper(message[index]);
                int indexInRussianAlfabet = rusLetters.IndexOf(Char.ToLower(message[index]));
                if (indexInRussianAlfabet != -1)
                {
                    forAdd = engLetters[indexInRussianAlfabet];
                    if (IsBig && forAdd.Length != string.Empty.Length)
                    {
                        forAdd = engLetters[indexInRussianAlfabet].Replace(engLetters[indexInRussianAlfabet][0], Char.ToUpper(engLetters[indexInRussianAlfabet][0]));
                    }
                    builder.Append(forAdd);
                }
                else
                {
                    builder.Append(message[index]);
                }
            }
            return builder.ToString();

        }

       
    }
}
