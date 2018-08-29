<p align="center">
  <img src="Hope.Ethereum/Hope_Background.png?raw=true" alt="Hope" align="center" width="785px" height="328px"/>
</p>

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
* .NET Standard Specifics
  * Utilities
    * <a href="#transactionpoller">TransactionPoller</a>
* Unity Specifics
  * Utilities
    * <a href="#promises">Promises</a>
* General
  * Ether 
    * <a href="#ethtransfer">Ether Transfers</a>
    * <a href="#ethbalance">Ether Balances</a>
  * ERC20 Tokens
    * <a href="#erc20-init">ERC20 Token Initialization</a>
    * <a href="#erc20-messages">ERC20 Token Messages</a>
    * <a href="#erc20-queries">ERC20 Token Queries</a>
  * ERC721 Tokens
    * ERC721 Token Initialization
    * ERC721 Token Messages
    * ERC721 Token Queries
  * Utilities
    * Address Utilities
    * Contract Utilities
    * Eth Utilities
    * Gas Utilities
    * Solidity Utilities
    * Wallet Utillities
    
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

## <a id="transactionpoller"></a>Transaction Poller

In .NET Standard, when we send transactions, we will often get the return type of ```Task<TransactionPoller>```. The ```TransactionPoller``` is simply a class which polls for a transaction receipt until we get a result from the Ethereum blockchain. The result will be representative of whether the transaction was successful or unsuccessful on the blockchain.

We can execute some code when we get our receipt that the transaction was successful, or failed to execute.

See the code below.

```c#
TransactionPoller poller = await // Some transaction
poller.OnTransactionSuccessful(() => Console.WriteLine("Transaction Successful!"));
poller.OnTransactionFailure(() => Console.WriteLine("Transaction failed!"));
```

From the example above, you can see that we will write some text to the Console once we get our transaction result. We can easily pass more complex code through the lambda expressions if we need to.

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
string privateKey = "0x215939f9664cc1a2ad9f004abea96286e81e57fc2c21a8204a1462bec915be8f";
string addressTo = "0x0101010101010101010101010101010101010101";
decimal ethAmountToSend = 0.48m;

EthTransactionPromise transactionPromise = EthUtils.SendEther(privateKey, addressTo, ethAmountToSend);

transactionPromise.OnSuccess(_ => Debug.Log("Successfully sent 0.01 ETH"));
transactionPromise.OnError(Debug.Log);
transactionPromise.OnSuccessOrError(() => Debug.Log("Test"));
```

This is a scenario in which we are sending 0.48 Ether to a random address. We set our privateKey, addressTo and ethAmountToSend variables, and send the transaction without an input gas price and gas limit. It returns us our ```Promise```, and we can pass through any code we want which will run if the transaction was successful, or ran into an error. 

```c#
transactionPromise.OnSuccessOrError(() => Debug.Log("Test"));
```

This line will run whether an error occurs or not. You can think of it as like a try, catch, finally block. ```OnSuccess``` is like your try, ```OnError``` is your catch, and ```OnSuccessOrError``` is your finally.


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

We can also easily send Ether from one address to another in Unity as well as .NET Standard.

```c#
string privateKey = "0x215939f9664cc1a2ad9f004abea96286e81e57fc2c21a8204a1462bec915be8f";
string addressTo = "0x0101010101010101010101010101010101010101";
decimal ethAmountToSend = 0.48m;
BigInteger gasPrice = GasUtils.GetFunctionalGasPrice(2.5m);
BigInteger gasLimit = 21000;

EthUtils.SendEther(privateKey, addressTo, ethAmountToSend, gasPrice, gasLimit)
        .OnSuccess(_ => Debug.Log("Successfully sent 0.48 Ether"));
```

As like in the .NET Standard library, we have method overloads for the ```SendEther``` which allow for the sending of Ether without a gas price or gas limit. In this case, the gas price and gas limit will be estimated based on the current network traffic.

## <a id="ethbalance"></a>Ether Balances

### .NET Standard

You can easily check the Ether balance of an address with more utility methods inside the ```EthUtils``` class.

```c#
string address = "0x0000000000000000000000000000000000000000";
decimal balance = await EthUtils.GetEtherBalance(address);
```

This code will query the Ether balance of a given address at the current block number. The balance is converted from the wei amount into the common readable format in Ether.

### Unity

Getting the Ether balance of an address in unity is very similar to the .NET Standard library. The only difference is the return type of the method ```EthUtils.GetEtherBalance```. Oftentimes, you will find this is the one of the most common differences in the libraries

```c#
string address = "0x0000000000000000000000000000000000000000";

// Print out the Ether balance
EthUtils.GetEtherBalance(address).OnSuccess(Debug.Log);
```

## <a id="erc20-init"></a>ERC20 Token Initialization

ERC20 token initialization is made very simple in this library. You can create a new instance of the ```ERC20``` class with knowledge of the token's symbol, name, and decimals, or no knowledge at all.

You can create an instance of a known ERC20 token with the following code.

```c#
ERC20 purpose = new ERC20("0xd94F2778e2B3913C53637Ae60647598bE588c570", "Purpose", "PRPS", 18);
```

This initilization is done instantly and the the instance of the ```ERC20``` class can be used right away since all arguments have been fulfilled in the constructor.

However, since no rinkeby address is input, this instance of ```ERC20``` can only interact with the mainnet PRPS ERC20 token.

You can also initialize an ERC20 token with no knowledge of the name, symbol, or decimals. 

```c#
ERC20 purpose = new ERC20("0xd94F2778e2B3913C53637Ae60647598bE588c570", "0x5831819C84C05DdcD2568dE72963AC9f1e6831b6");
purpose.OnInitializationSuccessful(() => Console.WriteLine(purpose.Name));
purpose.OnInitializationUnsuccessful(() => Console.WriteLine("Failed to initialize token of given addresses"));
```

In this example we initialize a new ERC20 instance with the mainnet and rinkeby addresses, but with no name, symbol, or decimal count. In the ERC20 constructor, it will initialize the Name, Symbol, and Decimals properties based on the input contract addresses. Once it is done, it will call any code that was added to ```purpose.OnInitializationSuccessful``` if it was successfully initialized, or ```purpose.OnInitializationUnsuccessful``` if it was unsuccessfully initialized.

The ERC20 instance is not safe to use until it has called the code in ```OnInitializationSuccessful```. So, if you have code that needs to be executed once the token is initialized, you pass it through the ```OnInitializationSuccessful``` method.

## <a id="erc20-messages"></a>ERC20 Token Messages

The [ERC20 token standard]("https://github.com/ethereum/EIPs/issues/20") has a variety of functions which interact with the token on the blockchain. The ones we are interested in this case are the transfer, transferFrom, and approve functions.

The ```ERC20``` class in the Hope.Ethereum library has implemented the equivalent of these functions. These methods simply call the solidity functions on deployed ERC20 token contracts.

## <a id="erc20-queries"></a>ERC20 Token Queries

## Final Words

This is a library of utility classes that were created for use in the Hope Ethereum wallet. This library won't likely get updated very much unless there are any glaring issues anywhere that haven't been discovered. If you encounter any problems, post an issue and support will gladly be provided!
