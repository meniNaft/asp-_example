using asp__example.DAL;
using asp__example.Models;
using Microsoft.EntityFrameworkCore;

namespace asp__example.BL
{
    public class BLFriends
    {
        DataContext dataContext;
        public BLFriends(DataContext Context)
        {
            dataContext = Context;
        }


        public List<Friend> GetAll() 
        {
            return dataContext.Friends.ToList();
        }
    }
}
