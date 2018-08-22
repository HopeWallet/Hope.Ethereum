# NethereumUtils

Library of utility classes useful for interacting with the Ethereum blockchain through Nethereum. 

Includes .NET Standard and Unity game engine variants.

## Installation

Since the NethereumUtils library is split up into two core libraries, the installation is slightly different for the two.

### Standard Library

The required dlls for the Standard Library to function are located in the [Nethereum releases](https://github.com/Nethereum/Nethereum/releases). Download either the latest .NET 4.6.1 Nethereum library, or the version used for this library found [here](https://github.com/Nethereum/Nethereum/releases/download/3.0.0-rc1/net461dlls.zip).

Add the dlls to your project reference and you should be good to go.

### Unity Library

The required dlls for the Unity Library to function are located in the [Nethereum releases](https://github.com/Nethereum/Nethereum/releases). Download either the latest Unity .NET 3.5 Nethereum library, or the version used for this library found [here](https://github.com/Nethereum/Nethereum/releases/download/3.0.0-rc1/unitynet35dlls.zip).

Add the dlls to your Unity project in your Plugins folder and the library should be usable.

## Usage

The usage for the NethereumUtils library is split up into different sections. [NethereumUtils](https://github.com/ThatSlyGuy/NethereumUtils/tree/master/NethereumUtils/NethereumUtils) contains the .NET Standard working library, while [NethereumUtils.Unity](https://github.com/ThatSlyGuy/NethereumUtils/tree/master/NethereumUtils/NethereumUtils.Unity) contains the Unity game engine working library.

### Standard Library

This is pretty straight forward. See the [NethereumUtils.Tests](https://github.com/ThatSlyGuy/NethereumUtils/tree/master/NethereumUtils/NethereumUtils.Tests) code for examples.

### Unity Library

In Unity, we unfortunately do not have the luxury of using async and Task returns like in .NET standard. Since Unity uses its own HTTP request system we need to use Coroutines to run our async (not really) code.

However, Coroutines can be quite annoying to work with. When having a wrapper method to delegate the Coroutine calls, code becomes quite cumbersome due to all the Actions you have to pass around through parameters. Having an empty return statement all the time seemed very useless.

To solve these inconveniences, I've created my own simple implementation of js promises. These are the abstract ```Promise<TPromise, TReturn>``` class, and concrete ```EthCallPromise<T>``` and ```EthTransactionPromise```.

Take a look at the following code.

```c#
EthCallPromise<dynamic> ethBalancePromise = EthUtils.GetEtherBalance("0xb332Feee826BF44a431Ea3d65819e31578f30446");
ethBalancePromise.OnSuccess(balance => Debug.Log("Eth balance of " + balance);
ethBalancePromise.OnError(Debug.Log);
```

We can also chain our reactions together to each promise result.

```c#
EthUtils.GetEtherBalance("0xb332Feee826BF44a431Ea3d65819e31578f30446")
        .OnSuccess(balance => Debug.Log("Eth balance of " + balance)
        .OnError(Debug.Log);
```

As you can see, it becomes quite trivial to have some code execute when the Coroutine has finished. Now, we have a way of delegating a method on a successful, or unsuccessful response. This is how we will handle calls to retrieve data from the Ethereum blockchain; with ``` EthCallPromise<T> ```.
  
Sometimes we would like to send a transaction to the Ethereum blockchain, and know whether it went through successfully or not. For this, we use the ```EthTransactionPromise``` class.

Take a look at the following.

```c#
TransactionSignedUnityRequest signedUnityRequest = new TransactionSignedUnityRequest(NetworkProvider.GetNetworkChainUrl(), "0x215939f9664cc1a2ad9f004abea96286e81e57fc2c21a8204a1462bec915be8f", "0xb332Feee826BF44a431Ea3d65819e31578f30446");

EthTransactionPromise transactionPromise = EthUtils.SendEther(signedUnityRequest, // Signed tx request
                                                              new HexBigInteger(new BigInteger(21000)), // Gas limit
                                                              new HexBigInteger(SolidityUtils.ConvertToUInt(2.5m, 18)), // Gas price
                                                              "0xb332Feee826BF44a431Ea3d65819e31578f30446", // Address sending eth
                                                              "0x0101010101010101010101010101010101010101", // Address to send eth to
                                                              0.01m); // Amount of eth to send
transactionPromise.OnSuccess(_ => Debug.Log("Successfully sent 0.01 ETH"));
transactionPromise.OnError(Debug.Log);
transactionPromise.OnSuccessOrError(() => Debug.Log("Test"));
```

This is a scenario in which we are sending 0.01 Ether to a random address. We create our ```TransactionSignedUnityRequest``` with our network, private key, and wallet address, and call the ```EthUtils.SendEther``` method. It returns us our ```Promise```, and we can pass through any code we want which will run if the transaction was successful, or ran into an error. 

```c#
transactionPromise.OnSuccessOrError(() => Debug.Log("Test"));
```

This line will run whether an error occurs or not. You can think of it as like a try, catch, finally block. ```OnSuccess``` is like your try, ```OnError``` is your catch, and ```OnSuccessOrError``` is your finally.

## Final Words

This is a library of utility classes that I created to use with my own Ethereum wallet: Hope Wallet. This library won't likely get updated too much unless there are glaring issues anywhere I haven't noticed yet. Feel free to create some issues if you run into any problems and I will see what I can do!
