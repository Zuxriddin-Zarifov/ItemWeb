using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjir.Zuhriddin.Items.Services
{
    public interface ITokenService
    {
        public string CreateToken(string email);
    }
}
