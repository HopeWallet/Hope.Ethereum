using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NethereumUtils.Contracts
{
    public abstract class EthereumContract
    {
        protected EthereumContract(string mainnetAddress)
        {

        }

        protected EthereumContract(string mainnetAddress, string rinkebyAddress)
        {

        }
    }
}
