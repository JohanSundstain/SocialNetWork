using SocialNetWork.Model;
using SocialNetWork.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SocialNetWork.Items
{


    public class UserImage
    {

        public BitmapImage _image { get; set; }
        public String _date { get; set; }
        
        public UserImage(BitmapImage image,String date)
        {
            _image = image;
            _date = date;
        }
    }

}
