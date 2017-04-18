using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitRemote.Common
{
    public class Enums
    {
        public enum ActionTypes
        {
            Opened,
            Created,
            Added,
            Forked,
            Starred,
            Made
        }

        public enum RepositoriesTypes
        {
            Private,
            Public,
            Fork
        }
    }
}
