using System.Collections.Generic;
using Xamarin.Auth;

namespace GitRemote.DI
{
    public interface ISecuredDataProvider
    {
        /// <summary>
        /// Stores account in memory.
        /// </summary>
        /// <param name="userId">Unique name of account(login, userName, ..)</param>
        /// <param name="providerName">Unique name, that helps to find accounts for application(ConstantsService.ProviderName)</param>
        /// <param name="data">Unique id for token(Note) and token</param>
        void Store(string userId, string providerName, Dictionary<string, string> data);

        /// <summary>
        /// Clears account from storage.
        /// </summary>
        /// <param name="userName">Unique name of account(login, userName, ..)</param>
        void Clear(string userName);

        /// <summary>
        /// Retreives account from storage.
        /// </summary>
        /// <param name="providerName">Unique name, that helps to find accounts for application(ConstantsService.ProviderName)</param>
        /// <param name="userName">Unique name of account(login, userName, ..)</param>
        /// <returns></returns>
        Account Retreive(string providerName, string userName);

        /// <summary>
        /// Retrieves list of accounts.
        /// </summary>
        /// <param name="providerName">Unique name, that helps to find accounts for application(ConstantsService.ProviderName)</param>
        /// <returns>List of Account</returns>
        List<Account> RetreiveAll(string providerName);
    }
}
