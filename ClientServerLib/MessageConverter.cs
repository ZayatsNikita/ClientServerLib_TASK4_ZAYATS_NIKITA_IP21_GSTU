using System;
using System.Text;

namespace ClientServerLib
{
    public static class MessageConverter
    {

        static string[] rusLetters = new string[]{"а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й",
          "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц",
          "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };
        static string[] engLetters = new string[]{ "a", "b", "v", "g", "d", "e", "e", "zj", "z", "i", "j",
          "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kx", "c",
          "ch", "sh", "shch", "", "y", "", "ea", "yu", "ya" };


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
                int indexInRussianAlfabet = IndexOf(rusLetters, Char.ToLower(message[index]));
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

        /// <summary>
        /// Method that checks whether the array contains a string equal to the specified one.
        /// </summary>
        /// <param name="array">Array for seach.</param>
        /// <param name="forSeach">A string whose existence you want to check.</param>
        /// <returns>The index of a string in the specified array, if there is no such string, is returned minus 1.</returns>
        public static int IndexOf(this string[] array, char forSeach)
        {
            if (array == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                int length = array.Length;
                for (int index = 0; index < length; index++)
                {
                    if (forSeach == array[index][0])
                    {
                        return index;
                    }
                }
                return -1;
            }
        }
    }
}
