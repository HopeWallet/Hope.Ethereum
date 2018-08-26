namespace Hope.Ethereum.Utils
{
    /// <summary>
    /// Class which has some utility methods needed for ethereum contracts.
    /// </summary>
    public static class AddressUtils
    {
        private const int ADDRESS_LENGTH = 42;
        private const int HASH_LENGTH = 66;

        private const string CORRECT_ADDRESS_START = "0x";

        /// <summary>
        /// Checks if the input string is a valid ethereum address.
        /// </summary>
        /// <param name="address"> The address to check. </param>
        /// <returns> True if the <see cref="string"/> is a valid ethereum address. </returns>
        public static bool IsValidEthereumAddress(string address)
        {
            return !string.IsNullOrEmpty(address) && CorrectAddressLength(address, ADDRESS_LENGTH) && CorrectAddressBeginning(address) && CorrectAddressCharacters(address);
        }

        /// <summary>
        /// Checks if the input string is a valid ethereum transaction hash.
        /// </summary>
        /// <param name="txHash"> The transaction hash to check. </param>
        /// <returns> True if the <see cref="string"/> is a valid transaction hash. </returns>
        public static bool IsValidTransactionHash(string txHash)
        {
            return !string.IsNullOrEmpty(txHash) && CorrectAddressLength(txHash, HASH_LENGTH) && CorrectAddressBeginning(txHash) && CorrectAddressCharacters(txHash);
        }

        /// <summary>
        /// Checks if the input <see cref="string"/> is of correct length.
        /// </summary>
        /// <param name="address"> The address to check. </param>
        /// <param name="correctLength"> The correct length of the <see cref="string"/>. </param>
        /// <returns> True if the string is of correct length. </returns>
        public static bool CorrectAddressLength(string address, int correctLength)
        {
            return address.Length == correctLength;
        }

        /// <summary>
        /// Checks if the input <see cref="string"/> has the correct beginning characters to be an ethereum address.
        /// </summary>
        /// <param name="address"> The address to check. </param>
        /// <returns> True if the <see langword="string"/> starts with '0x'. </returns>
        private static bool CorrectAddressBeginning(string address)
        {
            return address.StartsWith(CORRECT_ADDRESS_START);
        }

        /// <summary>
        /// Checks if every <see cref="char"/> in the <see cref="string"/> except for the 0x start is a hexadecimal character.
        /// </summary>
        /// <param name="address"> The address to check. </param>
        /// <returns> True if the <see cref="string"/> has all hex characters. </returns>
        private static bool CorrectAddressCharacters(string address)
        {
            for (int i = CORRECT_ADDRESS_START.Length; i < address.Length; i++)
            {
                var c = address[i];

                if (!((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
                {
                    return false;
                }
            }

            return true;
        }
    }
}