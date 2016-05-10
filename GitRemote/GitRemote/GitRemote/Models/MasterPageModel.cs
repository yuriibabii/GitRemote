using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitRemote.Models
{
    class MasterPageModel
    {
        private string _masterProfileImage = "ic_account_circle_white_24dp.png";

        public string MasterProfileImage
        {
            get { return _masterProfileImage; }
            set { _masterProfileImage = value; }
        }

        private string _masterProfileName = "GitRemote";

        public string MasterProfileName
        {
            get { return _masterProfileName; }
            set { _masterProfileName = value; }
        }
    }

    public class MasterPageItem
    {
        private string _masterItemImage;

        public string MasterItemImage
        {
            get { return _masterItemImage; }
            set { _masterItemImage = value; }
        }

        private string _masterItemName;

        public string MasterItemName
        {
            get { return _masterItemName; }
            set { _masterItemName = value; }
        }
    }
}
