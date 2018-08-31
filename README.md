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
    * <a href="#ethtransfer">Transfers</a>
    * <a href="#ethbalance">Balances</a>
  * Ethereum Tokens
    * <a href="#token-overview">Overview</a>
    * <a href="#token-init">Initialization</a>
    * <a href="#token-messages">Messages</a>
    * <a href="#token-queries">Queries</a>
  * Utilities
    * <a href="#address-utils">Address Utilities</a>
    * <a href="#contract-utils">Contract Utilities</a>
    * <a href="#gas-utils">Gas Utilities</a>
    * <a href="#solidity-utils">Solidity Utilities</a>
    
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
EthUtils.GetEtherBalance(address).OnSuccess(balance => Debug.Log(balance));
```
## <a id="token-overview"></a>Overview

The Hope.Ethereum .NET Standard and Unity libraries both implement code to interact with a variety of Ethereum tokens. The currently implemented tokens are the <b>ERC20</b> and <b>ERC721</b> standards. 

Each token standard derives from a base class of ```Token```, so the code for each token specification is more or less the same. The only differences between tokens lies in the actual functions they implement.

It is best to refer to documents which show the exact functions of each token specification.

- [ERC20 Token Standard](https://github.com/ethereum/EIPs/blob/master/EIPS/eip-20.md)
- [ERC721 Token Standard](https://github.com/ethereum/EIPs/blob/master/EIPS/eip-721.md)

For all examples below, we will use the ```ERC20``` class to demonstrate how to interact with token code through the Hope.Ethereum library. Any tokens listed above can be used in the exact same way. Please refer to the list above for a list of all implemented Ethereum tokens in the Hope.Ethereum library.

## <a id="token-init"></a>Initialization

Ethereum token initialization is made very simple in this library. You can create a new instance of a ```Token``` class with knowledge of the token's symbol, name, and decimals, or no knowledge at all.

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

An instance of ```Token``` initialized without a name, symbol, and decimal count is not safe to use until it has called the code in ```OnInitializationSuccessful```. So, if you have code that needs to be executed once the token is initialized, you pass it through the ```OnInitializationSuccessful``` method.

## <a id="token-messages"></a>Messages

Sometimes we may want to interact with our Ethereum tokens on the blockchain. Perhaps we want to transfer some tokens from one address to another, or approve the transfers for one address. All functions that are implemented in the respective token standard will also be implemented in Hope.Ethereum

Let's take a look at how to transfer some ERC20 tokens.

```c#
string privateKey = "0x215939f9664cc1a2ad9f004abea96286e81e57fc2c21a8204a1462bec915be8f";
string addressTo = "0x0101010101010101010101010101010101010101";
decimal amountToSend = 5;
BigInteger gasPrice = GasUtils.GetFunctionalGasPrice(2.5m);
BigInteger gasLimit = 21000;

ERC20 purpose = new ERC20("0xd94F2778e2B3913C53637Ae60647598bE588c570", "Purpose", "PRPS", 18);
purpose.Transfer(privateKey, addressTo, amountToSend, gasLimit, gasPrive);
```

This is an example of the ERC20 Transfer method in the Hope.Ethereum library. The return value of this method is either of type ```EthTransactionPromise``` or ```Task<TransactionPoller>``` depending on if you are using the Hope.Ethereum.Unity or Hope.Ethereum library.

The other ERC20 token messages are implemented in the same way.

```c#
ERC20 purpose = new ERC20("0xd94F2778e2B3913C53637Ae60647598bE588c570", "Purpose", "PRPS", 18);

// Approve method
// purpose.Approve...

// TransferFrom metho:
// purpose.TransferFrom...
```

## <a id="token-queries"></a>Queries

There is also a variety of data we can query from Ethereum tokens. For a list of query functions, please refer to the respective token standard.

Queries are slightly different in .NET Standard and Unity, so examples for both will be shown below.

### .NET Standard

```c#
ERC20 purpose = new ERC20("0xd94F2778e2B3913C53637Ae60647598bE588c570", "Purpose", "PRPS", 18);

// Query the name of the contract.
string name = await purpose.QueryName();

// Query the balance of the address "0x0101010101010101010101010101010101010101" of the contract.
decimal balance = await purpose.QueryBalanceOf("0x0101010101010101010101010101010101010101");
```

### Unity

```c#
ERC20 purpose = new ERC20("0xd94F2778e2B3913C53637Ae60647598bE588c570", "Purpose", "PRPS", 18);

string name;

// Query the name of the contract.
purpose.QueryName().OnSuccess(tokenName => name = tokenName);

// Query the balance of the address "0x0101010101010101010101010101010101010101" of the contract and print it out.
purpose.QueryBalanceOf("0x0101010101010101010101010101010101010101").OnSuccess(Debug.Log);
```

The reason for the differences between libraries is due to the fact that the return types are different. The Unity return is of type ```EthCallPromise<T>``` while .NET Standard returns a ```Task<T>```. 

## <a id="address-utils"></a>Address Utilities

The ```EthUtils``` class implements a few useful methods for determining whether some code is a valid Ethereum address or transaction hash.

Take a look at the following code.

```c#
// Check if the input text is a valid ethereum address.
bool isEthAddress = AddressUtils.IsValidEthereumAddress("008rwebi341u");

// Check if the input text is a valid transaction hash.
bool isTransactionHash = AddressUtils.IsValidTransactionHash("0x0101010101010101010101010101010101010101");
```

## <a id="contract-utils"></a>Contract Utilities

The ```ContractUtils``` class implements a few methods for generically interacting with Ethereum contracts. It is the core class that is used to drive our Ethereum token library.

For example, the internal code used to query the name from an ethereum token looks like the following.

Firstly, we need a class to mimic our function which we want to execute on the Ethereum contract.

```c#
[Function("name", "string")]
public sealed class Name : FunctionMessage
{
}
```

We can pass an instance of this class into the ```ContractUtils.QueryContract``` to query the name.

```c#
// Our return type is a simple output of type string.
// Once we retrieved our value from either the EthCallPromise or Task<SimpleOutputs.String> we access the 'Value' property for the true value.
// We also do not need to add a sender address to the query.
ContractUtils.QueryContract<Name, SimpleOutputs.String>(new Name(), "0xE41d2489571d322189246DaFA5ebDe1F4699F498", null);
```

The process for sending a message to an Ethereum contract is very similar. We can define our function using a class derived from ```FunctionMessage``` like with the ```Name``` class above.

```c#
[Function("transfer", "bool")]
public sealed class Transfer : FunctionMessage
{
  [Parameter("address", "_to", 1)]
  public string To { get; set; }

  [Parameter("uint256", "_value", 2)]
  public BigInteger Value { get; set; }
}
```

We can then execute this code using the ```ContractUtils.SendContractMessage``` class.

```c#
string privateKey = "0x215939f9664cc1a2ad9f004abea96286e81e57fc2c21a8204a1462bec915be8f";
BigInteger gasPrice = GasUtils.GetFunctionalGasPrice(2.5m);
BigInteger gasLimit = 21000;

Transfer transfer = new Transfer
{
  To = "0x0101010101010101010101010101010101010101",
  Value = SolidityUtils.ConvertToUInt(1.54m, 18) // Send 1.48 of a specific token
};

// Execute the transfer function on the given contract address using the account under the private key.
ContractUtils.SendContractMessage<Transfer>(transfer, "0xE41d2489571d322189246DaFA5ebDe1F4699F498", privateKey, gasPrice, gasLimit);
```

Take a look at the source code for the ```ERC20``` class for more concrete examples on how to execute contract messages and queries.

## <a id="gas-utils"></a>Gas Utilities

The ```GasUtils``` class implements many methods for easily estimating gas prices and gas limits, as well as several others.

You can easily estimate the current gas price using the following code.

```c#
BigInteger gasPrice = await GasUtils.EstimateGasPrice();
```

However, since the return type of the method is either ```EthCallPromise<BigInteger>``` or ```Task<BigInteger>``` the value will be unreadable. It is a functional gas price in the sense that it can be used to send a transaction without any issues, but it not very readable. 

Take a look at the following code for getting the estimated gas price in readable format.

```c#
BigInteger functionalGasPrice = await GasUtils.EstimateGasPrice();
decimal readableGasPrice = GasUtils.GetReadableGasPrice(functionalGasPrice);
```

You can easily convert to and from readable gas prices using the methods ```GasUtils.GetReadableGasPrice``` and ```GasUtils.GetFunctionalGasPrice```.

You can also estimate the gas limit for a transaction using ```GasUtils.EstimateEthGasLimit``` and ```GasUtils.EstimateContractGasLimit```. 

```GasUtils.EstimateEthGasLimit``` is very straight forward, only requiring you to enter the address you are sending the Ether to, as well as the Ether value (in wei) that you are sending. However, the ```GasUtils.EstimateContractGasLimit``` takes in a ```FunctionMessage```, much like the ```ContractUtils.SendContractMessage``` method.

We can estimate the gas limit for a Token transfer function with the following code.

We first define our ```FunctionMessage``` class, which in this case is the ERC20 transfer function.

```c#
[Function("transfer", "bool")]
public sealed class Transfer : FunctionMessage
{
  [Parameter("address", "_to", 1)]
  public string To { get; set; }

  [Parameter("uint256", "_value", 2)]
  public BigInteger Value { get; set; }
}
```

We can now easily estimate the gas limit by filling in the required method parameters.

```c#
Transfer transfer = new Transfer
{
  To = "0x0101010101010101010101010101010101010101",
  Value = SolidityUtils.ConvertToUInt(1.54m, 18) // Send 1.48 of a specific token
};

string contractAddress = "0xE41d2489571d322189246DaFA5ebDe1F4699F498";
string senderAddress = "0xb332feee826bf44a431ea3d65819e31578f30446";

BigInteger gasLimit = await GasUtils.EstimateGasLimit<Transfer>(transfer, contractAddress, senderAddress);
```

Those examples above are using the .NET Standard implementation of the Hope.Ethereum library. The same could be achieved in the Unity version through the use of the ```EthCallPromise``` class as well.

## <a id="solidity-utils"></a>Solidity Utilities

The ```SolidityUtils``` class implements a few methods which are useful for converting values to and from the respective solidity uint values.

If you query a uint value from an Ethereum contract, you can convert it to a readable value using the following code.

```c#
// Convert the uint value '1000000000000000000000' to a readable value using 18 decimal places.
// The standard is 18 decimal places. For example, all ether values on the blockchain are to 18 decimal places.
decimal readableValue = SolidityUtils.ConvertFromUInt(1000000000000000000000, 18);
```

You can also convert a readable value back to the uint value using the following code.

```c#
// Convert Ether value of 2.62 back to a usable solidity value.
BigInteger uintValue = SolidityUtils.ConvertToUInt(2.62m, 18);
```

## Contributing

Contributions are always welcome! Whether it has to do with code refactoring, feature addition, or bugs - all are appreciated!

Create an [issue](https://github.com/HopeWallet/Hope.Ethereum/issues) and create pull requests, and they will all be taken a look at!

## Final Words

This is a library of utility classes that were created for use in the Hope Ethereum wallet. This library won't likely get updated very much unless there are any glaring issues anywhere that haven't been discovered. If you encounter any problems, post an issue and support will gladly be provided!
