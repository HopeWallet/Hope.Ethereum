using Hope.Ethereum.Unity.Tokens;
using Hope.Ethereum.Unity.Utils;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        NetworkProvider.SwitchNetworkChain(Nethereum.Signer.Chain.MainNet);

        UnknownERC20Tests();
        ERC20Tests();
        ERC721Tests();
    }

    private void ERC721Tests()
    {
        // NOTE:
        // Since CryptoKitties was the first draft of ERC721, many functions were not formalized and decided upon.
        // Therefore, some functions will not work and some others will.
        ERC721 cryptokitties = new ERC721("0x06012c8cf97BEaD5deAe237070F9587f8E7A266d", "CryptoKitties", "CK", 0);

        cryptokitties.QueryBalanceOf("0x12b353D1a2842D2272aB5A18C6814D69f4296873")
                     .OnSuccess(balance => Debug.Log(SolidityUtils.ConvertFromUInt(balance.Value, cryptokitties.Decimals.Value)))
                     .OnError(Debug.Log);
    }

    private void ERC20Tests()
    {
        ERC20 prps = new ERC20("0xd94F2778e2B3913C53637Ae60647598bE588c570", "Purpose", "PRPS", 18);

        prps.QueryBalanceOf("0xa8EF8e0855F84F25666Cc5b37C5aB8cBF9de314F")
            .OnSuccess(balance => Debug.Log(SolidityUtils.ConvertFromUInt(balance.Value, prps.Decimals.Value)))
            .OnError(Debug.Log);
    }

    private void UnknownERC20Tests()
    {
        ERC20 rand = new ERC20("0xe41d2489571d322189246dafa5ebde1f4699f498");
        rand.OnInitializationSuccessful(() => Debug.Log(rand.Name + " => " + rand.Symbol + " => " + rand.Decimals));
    }
}
