using System.Collections.Generic;
using Xamarin.Auth;

namespace GitRemote.Services
{
    public interface ISecuredDataProvider
    {
        void Store(string userId, string providerName, Dictionary<string, string> data);

        void Clear(string userName);

        Account Retreive(string providerName, string userName);

        List<Account> RetreiveAll(string providerName);
    }
}
