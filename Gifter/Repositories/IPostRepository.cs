using Gifter.Models;
using System;
using System.Collections.Generic;

namespace Gifter.Repositories
{
    public interface IPostRepository
    {
        
        
        List<Post> GetAll();
        List<Post> GetAllWithComments();
        List<Post> Search(string criterion, bool sortDescending);
        List<Post> Hottest(DateTime criterion, bool sortDescending);

        Post GetById(int id);
        Post GetByIdWithComments(int id);

        void Add(Post post);
        void Delete(int id);
        void Update(Post post);
    }
}