using Hope.Ethereum;
using Hope.Ethereum.Unity.Promises;
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
        FakeERC20Test();
    }

    private void FakeERC20Test()
    {
        QuerySymbol().OnSuccess(_ => Debug.Log("Success")).OnError(_ => Debug.Log("Error"));
        //ERC20 erc20 = new ERC20("0x0000000000000000000000000000000000000000");
        //erc20.OnInitializationSuccessful(() => Debug.Log("Initialization Successful"));
        //erc20.OnInitializationUnsuccessful(() => Debug.Log("Initialization Unsuccessful"));
    }

    public EthCallPromise<string> QuerySymbol()
    {
        EthCallPromise<string> promise = new EthCallPromise<string>();
        SimpleContractQueries.QueryStringOutput(new ERC20.Queries.Symbol(), "0x0000000000000000000000000000000000000000", null)
                             .OnSuccess(symbol => promise.Build(() => symbol?.Value))
                             .OnError(error => promise.Build(() => "error", () => error));

        return promise;
    }

    private void ERC721Tests()
    {
        // NOTE:
        // Since CryptoKitties was the first draft of ERC721, many functions were not formalized and decided upon.
        // Therefore, some functions will not work and some others will.

        // You can also initialize it with the values initially setup
        // ERC721 cryptokitties = new ERC721("0x06012c8cf97BEaD5deAe237070F9587f8E7A266d", "CryptoKitties", "CK", 0);

        // Here we initialize it without any knowledge of the decimal count, name, or symbol of the address.
        ERC721 cryptokitties = new ERC721("0x06012c8cf97BEaD5deAe237070F9587f8E7A266d");
        cryptokitties.OnInitializationSuccessful(() =>
        {
            cryptokitties.QueryBalanceOf("0x12b353D1a2842D2272aB5A18C6814D69f4296873")
                         .OnSuccess(balance => Debug.Log(balance))
                         .OnError(Debug.Log);
        });
    }

    private void ERC20Tests()
    {
        ERC20 prps = new ERC20("0xd94F2778e2B3913C53637Ae60647598bE588c570", "Purpose", "PRPS", 18);

        prps.QueryBalanceOf("0xa8EF8e0855F84F25666Cc5b37C5aB8cBF9de314F")
            .OnSuccess(balance => Debug.Log(balance))
            .OnError(Debug.Log);
    }

    private void UnknownERC20Tests()
    {
        ERC20 rand = new ERC20("0xe41d2489571d322189246dafa5ebde1f4699f498");
        rand.OnInitializationSuccessful(() => Debug.Log(rand.Name + " => " + rand.Symbol + " => " + rand.Decimals));
    }
}
