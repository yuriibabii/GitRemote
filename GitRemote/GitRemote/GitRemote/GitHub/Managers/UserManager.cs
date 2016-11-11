using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GitRemote.DI;
using GitRemote.Services;
using Xamarin.Forms;

namespace GitRemote.GitHub.Managers
{
    public class UserManager
    {
        private readonly ObservableCollection<string> _users;
        public static List<string> AddedUsers = new List<string>();
        public static List<string> DeletedUsers = new List<string>();
        private static string _lastUser = GetLastUserFromStorage();

        public UserManager(ISecuredDataProvider securedDataProvider)
        {
            //Takes a list of accounts names from storage and reverses it to suit display in ListView
            _users = new ObservableCollection<string>(securedDataProvider.RetreiveAll(ConstantsService.ProviderName)
                                                                         .Select(acc => acc.Username)
                                                                         .Reverse());
        }

        /// <summary>
        /// Gets users(accounts) names from local storage of users and corrects changes with it.
        /// </summary>
        /// <returns>ObservableCollection of users names</returns>
        public ObservableCollection<string> GetAllUsers()
        {
            foreach ( var addedUser in AddedUsers )
            {
                _users.Insert(0, addedUser);
                AddedUsers.Remove(addedUser);
            }

            foreach ( var deletedUser in DeletedUsers )
            {
                _users.Remove(deletedUser);
                DeletedUsers.Remove(deletedUser);
            }

            return _users;
        }



        #region LastUser
        public static void SetLastUser(string userName)
        {
            _lastUser = userName;
        }

        public static void SaveLastUser()
        {
            Application.Current.Properties["LastUser"] = _lastUser;
        }

        public static string GetLastUser()
        {
            return _lastUser;
        }

        public static string GetLastUserFromStorage()
        {
            if ( Application.Current.Properties.ContainsKey("LastUser") )
                return Application.Current.Properties["LastUser"] as string;

            return string.Empty;
        }
        #endregion
    }


}
