# Hope.Ethereum

Library of classes used for interacting with the Ethereum blockchain through the Nethereum code library. Contains simple classes and utility methods for interacting with ERC20 and ERC721 tokens, sending Ether, and several other Ethereum related bits and pieces.

Includes .NET Standard and Unity game engine variants.

## Installation

Since the Hope.Ethereum library is split up into two core libraries, the installation is slightly different for the two.

### .NET Standard Library

The required dlls for the Standard Library to function are located in the [Hope.Ethereum releases](https://github.com/HopeWallet/Hope.Ethereum/releases). Download the latest Hope.Ethereum zip file, and add all required dlls to your project reference.

If you already have Nethereum in your project, simply add only the Hope.Ethereum dll.

### Unity Library

The required dlls for the Unity Library to function are located in the [Hope.Ethereum releases](https://github.com/HopeWallet/Hope.Ethereum/releases). Download the latest Hope.Ethereum.Unity zip file, and add all required dlls to your Unity project's plugins folder.

If you already have Nethereum in your project, only add the Hope.Ethereum.Unity dll to the plugins folder.

## Table of Contents
* Introduction
  * <a href="#comparison">.NET Standard vs Unity</a>
  * <a href="#network">Ethereum Network Provider</a>
* General
  * Ether 
    * <a href="#ethtransfer">Ether Transfers</a>
    * <a href="#ethbalance">Ether Balances</a>
  * ERC20 Tokens
    * ERC20 Token Initialization
    * ERC20 Token Transfers
    * ERC20 Token Balances
    * Other ERC20 Token Methods
  * ERC721 Tokens
    * ERC721 Token Initialization
    * ERC721 Token Transfers
    * ERC721 Token Balances
    * Other ERC721 Token Methods
  * Utilities
    * Address Utilities
    * Contract Utilities
    * Eth Utilities
    * Gas Utilities
    * Solidity Utilities
    * Wallet Utillities
* .NET Standard Specifics
  * Utilities
    * TransactionPoller
* Unity Specifics
  * Utilities
    * Promises
    * Transaction Utilities
    
## <a id="comparison"></a>.NET Standard vs Unity

The class library for .NET Standard and Unity have been split into different sections for very good reasons. The primary one has to do with the fact that Unity is driven by Coroutines, while .NET Standard is driven through asynchronous methods and Tasks.

With .NET Standard, returning queried results from the Ethereum blockchain is quite simple. You can have an async method with a Task return containing your return type as the generic argument. 

The .NET Standard code for getting the Ether balance of an address is as follows.

```c#
decimal balance;

string address = "0x0000000000000000000000000000000000000000";
balance = await EthUtils.GetEtherBalance(address);
```

You would simply run this code in an async method and get the balance very easily.

With Unity, Coroutines cannot have a return type that is not of type IEnumerator. This can make things rather difficult and annoying when you want to return some result at the end of a Coroutine.

For this we have developed <a href="#promises">Promises</a>. You can think of these as an extremely simplified version of Javascript promises. By utilising an ```EthCallPromise```, we can get our Ether balance easily.

The Unity code for getting the Ether balance of an address is as follows.

```c#
decimal balance;
string address = "0x0000000000000000000000000000000000000000";

// If the transaction is successful (OnSuccess), we set our balance variable to the ethBalance result in the lambda expression.
EthUtils.GetEtherBalance(address).OnSuccess(ethBalance => balance = ethBalance);
```

These Promises create a very easy and intuitive way to get some results from the Ethereum blockchain without the hindrances that come with Coroutines. You can read more about this in the <a href="#promises">Promises</a> section.

## <a id="network"></a>Ethereum Network Provider

All code in the Hope.Ethereum and Hope.Ethereum.Unity libraries are driven by the ``` NetworkProvider ``` class. This class is used to provide the required information to the utility classes so that all interaction on the Ethereum blockchain can be executed smoothly and effortlessly. 

By default, the ``` NetworkProvider ``` is set to use the Ethereum mainnet as our core driver. However, if you want to use a different network, you can easily do so.

You can swap in between the Mainnet and Rinkeby chains easily. Any others will result in the network being set to the Rinkeby network.

```c#
// Swap to the rinkeby network
NetworkProvider.SwitchNetworkChain(Chain.Rinkeby);
```

It is important that you set the ```NetworkProvider``` to the correct network before calling any utility methods. If you are calling ```EthUtils.SendEther``` on the mainnet when you only have Ether on rinkeby, this will result in an error being thrown.

## <a id="ethtransfer"></a>Ether Transfers

### .NET Standard

You can easily send Ether from one address to another using the Hope.Ethereum library. The .NET Standard code for doing this is as follows:

```c#
decimal readableEthAmount = 0.0000000000001m;
decimal readableGasPrice = 5.24m;

BigInteger gasLimit = 75000;
BigInteger gasPrice = GasUtils.GetFunctionalGasPrice(readableGasPrice);
string privateKey = "0x215939f9664cc1a2ad9f004abea96286e81e57fc2c21a8204a1462bec915be8f";

(await EthUtils.SendEther(privateKey, "0x5831819C84C05DdcD2568dE72963AC9f7e2833b6", readableEthAmount, gasPrice))
              .OnTransactionSuccessful(() => Console.WriteLine("Transaction successful!"))
              .OnTransactionFailure(() => Console.WriteLine("Transaction failed!"));
```

Oftentimes we may find it easier to use a decimal readable gas price in gwei like ``` decimal readableGasPrice = 5.24m ```. Hope.Ethereum makes it very easy to use this, and then convert it to a gas price usable by the ``` SendEther ``` method. 

The ``` SendEther ``` method takes in the Ether amount in Ether, NOT wei. However, if you already have the amount of Ether you want to send in wei, you can convert it to the Ether amount, and then send that. 

See below for an example of the following.

```c#
BigInteger etherAmountInWei = 100000000000000000000;
decimal etherAmount = SolidityUtils.ConvertFromUInt(etherAmountInWei, 18);

// Send ether...
```

The ``` SendEther ``` method has optional overloads which allow for no gas price or gas limit to be input into the method. If this is used, the gas price and gas limit will be estimated accordingly and then sent.

### Unity

We can also easily send Ether from one address to another 

## <a id="ethbalance"></a>Ether Balances

You can easily check the Ether balance of an address with more utility methods inside the ```EthUtils``` class.

```c#
string address = "0x0000000000000000000000000000000000000000";
decimal balance = await EthUtils.GetEtherBalance(address);
```

This code will query the Ether balance of a given address at the current block number. The balance is converted from the wei amount into the common readable format in Ether.

#### ERC20 Tokens

#### ERC721 Tokens

#### Ethereum Utils

## <a id="promises"></a>Promises

In Unity, we unfortunately do not have the luxury of using async and Task returns like in .NET standard. Since Unity uses its own HTTP request system we need to use Coroutines to run our async (not really) code.

However, Coroutines can be quite annoying to work with. When having a wrapper method to delegate the Coroutine calls, code becomes quite cumbersome due to all the Actions you have to pass around through parameters. Having an empty return statement all the time seemed very useless.

To solve these inconveniences, I've created my own simple implementation of Javascript promises. These are the abstract ```Promise<TPromise, TReturn>``` class, and concrete ```EthCallPromise<T>``` and ```EthTransactionPromise```.

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

This is a library of utility classes that were created for use in the Hope Ethereum wallet. This library won't likely get updated very much unless there are any glaring issues anywhere that haven't been discovered. If you encounter any problems, post an issue and support will gladly be provided!
