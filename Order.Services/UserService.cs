using EndWerk.Project.Data;
using Order.Object;

namespace Order.Services
{
    public class UserService
    {
        private Repository _repository;

        public UserService(Repository repository)
        {
            this._repository = repository;
        }

        public List<User> GetUsers()
        {
            return _repository.Users.ToList();
           
        }

        public User GetUser(string id)
        {
            return _repository.Users.FirstOrDefault(c => c.Id == id);

        }


        public User UpdataOrCreateUser(User user)
        {
            if (user.Id == null)
            {
                _repository.Users.Add(user);
            }
            else
            {
                _repository.Attach(user);
                var e = _repository.ChangeTracker.Entries().FirstOrDefault(e => e.Entity == user);
                e.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            _repository.SaveChanges();
            return user;
        }

        public void DeleteUser(string id) 
        {

            var user = GetUser(id);
            if (user != null)
            {
                _repository.Users.Remove(user);
                _repository.SaveChanges();
            }
            else 
            {
                return;
            }

        }
    } 
}