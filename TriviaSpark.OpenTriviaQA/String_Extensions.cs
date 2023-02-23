namespace OpenTriviaQA
{
    public static class String_Extensions
    {
        /// <summary>
        /// Computes a deterministic hash code for a given string. If the input string is null, returns -1.
        /// </summary>
        /// <param name="str">The string to hash</param>
        /// <returns>The hash code for the input string</returns>
        public static int GetDeterministicHashCode(this string str)
        {
            // If the input string is null, return -1.
            if (str == null)
            {
                return -1;
            }

            // Compute the hash code using the djb2 algorithm.
            unchecked
            {
                var hash1 = (5381 << 16) + 5381;
                var hash2 = hash1;
                for (var i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                    {
                        break;
                    }
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }
                return hash1 + (hash2 * 1566083941);
            }
        }

    }
}
