using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetWork.Model;

namespace SocialNetWork.Repository
{
    public class RepositoryIm : IRepository
    {
        private Entities _dbContext;
        

        public RepositoryIm(Entities dbContext)
        {
            this._dbContext = dbContext;
        }

        private IQueryable<Images> AllImages()
        {
            return _dbContext.Images.AsQueryable();
        }



        public async Task<Users> AddUser(Users user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<Users> ChangeAvatar(Users user)
        {
            Users _user = _dbContext.Users.Where(c => c.Id == user.Id).FirstOrDefault();
            _user.Images = user.Images;
            _user.Images1.Add(_user.Images);
            await _dbContext.SaveChangesAsync();
            return _user;

        }

        public async Task<Users> ChangeUser(Users user)
        {
            Users _user = _dbContext.Users.Where(c=> c.Id == user.Id).FirstOrDefault();
            _user.Name = user.Name;
            _user.Surname = user.Surname;
            _user.Birthday = user.Birthday;
            _user.Email = user.Email;
            _user.Password = user.Password;
            await _dbContext.SaveChangesAsync();
            return _user;
        }

        public async Task<Images> GetUserAvatarById(long? id)
        {
            return await Task.Run(() =>
            {
                return _dbContext.Images.SingleOrDefault(c => c.Id == id);
            });
        }

        public async Task<Users> GetUsersById(long id)
        {
            return await Task.Run(() =>
            {
                return _dbContext.Users.SingleOrDefault(c => c.Id == id);
            });
        }

        public async Task<Users> GetUsersByMailAndPassword(string mail, string password)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(c => c.Email == mail && c.Password == password);
        }

        public async Task<List<Images>> GetAllUserImages(Users user)
        {
            return await Task.Run(() => {
                return user.Images1.ToList();
            });
        }

        public async Task<List<Message>> GetMessages()
        {
            return await _dbContext.Messages.ToListAsync();
        }

        public async Task SendMessage(long user_id, string content)
        {
            Message message = new Message
            {
                IdUser = user_id,
                Content = content
            };
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
        }
    }
}
