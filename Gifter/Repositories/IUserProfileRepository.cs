using Gifter.Models;
using System.Collections.Generic;

namespace Gifter.Repositories
{
    public interface IUserProfileRepository
    {
        void Add(UserProfile singleUserProfile);
        void Delete(int id);
        List<UserProfile> GetAll();
        List<UserProfile> GetAllWithPosts();

        UserProfile GetById(int id);
        UserProfile GetByIdWithPosts(int id);
        UserProfile GetByEmail(string email);

        void Update(UserProfile singleUserProfile);
    }
}