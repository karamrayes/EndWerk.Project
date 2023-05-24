using EndWerk.Project.Data;
using Microsoft.EntityFrameworkCore;
using Order.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Services
{
    public class MessageServices
    {
        private Repository _repository;

        public MessageServices(Repository repository)
        {
            this._repository = repository;
        }

        public List<Message> GetMessages()
        {
            return _repository.Messages.Include(m => m.User).OrderBy(m => m.Type).ToList();

        }

        public Message GetMessage(int id)
        {
            return _repository.Messages.Include(m => m.User).FirstOrDefault(c => c.BerichtId == id);

        }


        public Message UpdataOrCreateMessage(Message message)
        {
            try
            {
                if (message.BerichtId == 0)
                {
                    _repository.Messages.Add(message);
                }
                else
                {
                    _repository.Attach(message);
                    var e = _repository.ChangeTracker.Entries().FirstOrDefault(e => e.Entity == message);
                    e.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                _repository.SaveChanges();
                return message;
            }
            catch (Exception ex)
            {
                return null;
                
            }
            
        }

        public bool DeleteMessage(int id)
        {

            var message = GetMessage(id);
            if (message != null)
            {
                _repository.Messages.Remove(message);
                _repository.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
