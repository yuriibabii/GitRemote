using GitRemote.DI;
using GitRemote.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace GitRemote.GitHub
{
    public class UserManager
    {
        private readonly ObservableCollection<string> _users;
        public static List<string> AddedUsers = new List<string>();
        public static List<string> DeletedUsers = new List<string>();
        private static string _lastUser = GetLastUserFromStorage();

        public UserManager(ISecuredDataProvider securedDataProvider)
        {
            _users = new ObservableCollection<string>(securedDataProvider.RetreiveAll(ConstantsService.ProviderName)
                                                                         .Select(acc => acc.Username)
                                                                         .Reverse());
        }

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
            Application.Current.Properties["_lastUser"] = _lastUser;
        }

        public static string GetLastUser()
        {
            return _lastUser;
        }

        public static string GetLastUserFromStorage()
        {

            if ( Application.Current.Properties.ContainsKey("_lastUser") )
                return Application.Current.Properties["_lastUser"] as string;

            return string.Empty;
        }
        #endregion
    }


}
