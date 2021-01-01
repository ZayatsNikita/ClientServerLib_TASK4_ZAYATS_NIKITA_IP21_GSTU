using System;

namespace ClientServerLib
{
    /// <summary>
    /// Extension methods that are used in the library.
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Method that checks whether the array contains a string equal to the specified one.
        /// </summary>
        /// <param name="array">Array for seach.</param>
        /// <param name="forSeach">A string whose existence you want to check.</param>
        /// <returns>The index of a string in the specified array, if there is no such string, is returned minus 1.</returns>
        private static int IndexOf(this string[] array, char forSeach)
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
