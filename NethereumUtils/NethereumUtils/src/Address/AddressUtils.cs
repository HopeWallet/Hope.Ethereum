namespace NethereumUtils.Standard
{
    /// <summary>
    /// Class which has some utility methods needed for ethereum contracts.
    /// </summary>
    public static class AddressUtils
    {
        private const int ADDRESS_LENGTH = 42;
        private const string CORRECT_ADDRESS_START = "0x";

        /// <summary>
        /// Checks if the input string is a valid ethereum address.
        /// </summary>
        /// <param name="address"> The address to check. </param>
        /// <returns> True if the <see langword="string"/> is a valid ethereum address. </returns>
        public static bool IsValidEthereumAddress(string address)
        {
            return !string.IsNullOrEmpty(address) && CorrectAddressLength(address) && CorrectAddressBeginning(address) && CorrectAddressCharacters(address);
        }

        /// <summary>
        /// Checks if the input <see langword="string"/> is of correct length to be an ethereum address.
        /// </summary>
        /// <param name="address"> The address to check. </param>
        /// <returns> True if the string is of correct length. </returns>
        public static bool CorrectAddressLength(string address)
        {
            return address.Length == ADDRESS_LENGTH;
        }

        /// <summary>
        /// Checks if the input <see langword="string"/> has the correct beginning characters to be an ethereum address.
        /// </summary>
        /// <param name="address"> The address to check. </param>
        /// <returns> True if the <see langword="string"/> starts with '0x'. </returns>
        private static bool CorrectAddressBeginning(string address)
        {
            return address.StartsWith(CORRECT_ADDRESS_START);
        }

        /// <summary>
        /// Checks if every <see langword="char"/> in the <see langword="string"/> except for the 0x start is a hexadecimal character.
        /// </summary>
        /// <param name="address"> The address to check. </param>
        /// <returns> True if the <see langword="string"/> has all hex characters. </returns>
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