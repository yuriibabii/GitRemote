using GitRemote.DI;
using GitRemote.Droid.DependencyServices;
using GitRemote.Services;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Auth;
using Xamarin.Forms;
using Account = Xamarin.Auth.Account;
using Application = Android.App.Application;

[assembly: Dependency(typeof(SecuredDataProvider))]
namespace GitRemote.Droid.DependencyServices
{
    public class SecuredDataProvider : ISecuredDataProvider
    {
        private readonly AccountStore _accountStore;

        public SecuredDataProvider()
        {
            _accountStore = AccountStore.Create(Application.Context);
        }

        public void Store(string userId, string providerName, Dictionary<string, string> data)
        {
            var account = new Account(userId, data);
            _accountStore.SaveAsync(account, providerName);
        }

        public void Clear(string userName)
        {
            var accounts = _accountStore.FindAccountsForService(ConstantsService.ProviderName);
            var account = accounts.First(a => a.Username == userName);
            _accountStore.Delete(account, ConstantsService.ProviderName);
        }

        Account ISecuredDataProvider.Retreive(string providerName, string userName)
        {
            var account = _accountStore.FindAccountsForService(providerName)
                    .FirstOrDefault(acc => acc.Username == userName);
            return account;
        }

        public List<Account> RetreiveAll(string providerName)
        {
            return _accountStore.FindAccountsForService(providerName) as List<Account>;
        }
    }
}