using SocialNetWork.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetWork.Repository
{
    public interface IRepository
    {
        Task<Users> AddUser(Users user);

        Task<Users> GetUsersById(long id);

        Task<Users> GetUsersByMailAndPassword(string mail, string password);

        Task<Images> GetUserAvatarById(long? id);

        Task<Users> ChangeUser(Users user);

        Task<Users> ChangeAvatar(Users user);

        Task<List<Message>> GetMessages();

        Task SendMessage(long user_id, string content);

        Task <List<Images>> GetAllUserImages(Users user);
    }
}
