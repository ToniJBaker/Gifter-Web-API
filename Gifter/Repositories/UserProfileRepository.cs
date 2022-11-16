using Gifter.Models;
using Gifter.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Gifter.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT Id AS 'UserId', [Name], Email, ImageUrl, Bio, DateCreated
                            FROM UserProfile";

                    var reader = cmd.ExecuteReader();

                    var userProfiles = new List<UserProfile>();
                    while (reader.Read())
                    {
                        userProfiles.Add(new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "UserId"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                            Bio = DbUtils.GetString(reader, "Bio"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),

                        });
                    }

                    reader.Close();

                    return userProfiles;
                }
            }
        }
        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT Id, Name, Email, ImageUrl, Bio, DateCreated FROM UserProfile WHERE Email = @email";
                    cmd.Parameters.AddWithValue("@email", email);



                    var reader = cmd.ExecuteReader();

                    UserProfile user = null;
                    if (reader.Read())
                    {
                        user = new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "iD"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl")
                        };
                    }

                    reader.Close();

                    return user;
                }
            }
        }

        public List<UserProfile> GetAllWithPosts()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT up.Id AS 'UserProfileId', up.[Name], up.Email, up.ImageUrl AS 'UserProfileImageUrl', up.Bio, up.DateCreated AS 'UserProfileDateCreated',
                         
                          p.Id AS 'PostId', p.Title, p.ImageUrl AS 'PostImageUrl', p.Caption,         p.UserProfileId AS 'PostUserProfileId', p.DateCreated AS 'PostDateCreated',

                          c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId

                          FROM UserProfile up
                          LEFT JOIN Post p ON up.Id = p.UserProfileId
                            LEFT JOIN Comment c ON p.Id = c.PostId
                            ";

                    var reader = cmd.ExecuteReader();
                    var posts = new List<Post>();
                    var userProfiles = new List<UserProfile>();
                    while (reader.Read())
                    {
                        var userProfileId = DbUtils.GetInt(reader, "UserProfileId");

                        var existingUserProfile = userProfiles.FirstOrDefault(up => up.Id == userProfileId);
                        
                        //userProfiles.Add(new UserProfile()
                        if(existingUserProfile ==null)
                        {
                            existingUserProfile = new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "UserProfileId"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                                Bio = DbUtils.GetString(reader, "Bio"),
                                DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                                Posts = new List<Post>()
                            };
                            
                            userProfiles.Add(existingUserProfile);
                        }

                        var postId = DbUtils.GetInt(reader, "PostId");
                        var existingPost = existingUserProfile.Posts.FirstOrDefault(p => p.Id == postId);
                        
                       if (existingPost == null)
                        {
                            existingPost = new Post()
                            {
                                Id = DbUtils.GetInt(reader, "PostId"),
                                Title = DbUtils.GetString(reader, "Title"),
                                Caption = DbUtils.GetString(reader, "Caption"),
                                DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                                UserProfileId = DbUtils.GetInt(reader, "PostUserProfileId"),
                                Comments = new List<Comment>()
                            };
                            existingUserProfile.Posts.Add(existingPost);
                        }
                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            existingPost.Comments.Add(new Comment()
                            {
                                Id = DbUtils.GetInt(reader, "CommentId"),
                                Message = DbUtils.GetString(reader, "Message"),
                                PostId = postId,
                                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
                            });
                        }
                        
                    }

                    reader.Close();

                    return userProfiles;
                }
            }
        }
        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT Id AS 'UserId', [Name], Email,  ImageUrl, Bio, DateCreated
                            FROM UserProfile
                           WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    UserProfile singleUserProfile = null;
                    if (reader.Read())
                    {
                        singleUserProfile = new UserProfile()
                        {
                            Id = id,
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            ImageUrl = DbUtils.GetString(reader, "ImageUrl"),
                            Bio = DbUtils.GetString(reader, "Bio"),
                            DateCreated = DbUtils.GetDateTime(reader, "DateCreated"),
                        };
                    }

                    reader.Close();

                    return singleUserProfile;
                }
            }
        }

        public UserProfile GetByIdWithPosts(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Id AS 'UserProfileId', up.[Name], up.Email, up.ImageUrl AS 'UserProfileImageUrl', up.Bio, up.DateCreated AS 'UserProfileDateCreated',
                         
                          p.Id AS 'PostId', p.Title, p.ImageUrl AS 'PostImageUrl', p.Caption,         p.UserProfileId AS 'PostUserProfileId', p.DateCreated AS 'PostDateCreated',

                          c.Id AS CommentId, c.Message, c.UserProfileId AS CommentUserProfileId, c.PostId AS CommentPostId

                          FROM UserProfile up
                          LEFT JOIN Post p ON up.Id = p.UserProfileId
                            LEFT JOIN Comment c ON p.Id = c.PostId
                           WHERE up.Id = @Id";

                    DbUtils.AddParameter(cmd, "@Id", id);

                    var reader = cmd.ExecuteReader();

                    UserProfile singleUserProfile = null;
                    while (reader.Read())
                    {
                        if (singleUserProfile == null)
                        {
                            singleUserProfile = new UserProfile()
                            {
                                Id = id,
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                                Bio = DbUtils.GetString(reader, "Bio"),
                                DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                                Posts = new List<Post>()
                            };
                        }
                        
                        var postId = DbUtils.GetInt(reader, "PostId");
                        var existingPost = singleUserProfile.Posts.FirstOrDefault(p => p.Id == postId);

                        if (existingPost == null)
                        {
                            existingPost = new Post()
                            {
                                Id = DbUtils.GetInt(reader, "PostId"),
                                Title = DbUtils.GetString(reader, "Title"),
                                Caption = DbUtils.GetString(reader, "Caption"),
                                DateCreated = DbUtils.GetDateTime(reader, "PostDateCreated"),
                                ImageUrl = DbUtils.GetString(reader, "PostImageUrl"),
                                UserProfileId = DbUtils.GetInt(reader, "PostUserProfileId"),
                                Comments = new List<Comment>()
                            };
                            singleUserProfile.Posts.Add(existingPost);
                        }
                        if (DbUtils.IsNotDbNull(reader, "CommentId"))
                        {
                            existingPost.Comments.Add(new Comment()
                            {
                                Id = DbUtils.GetInt(reader, "CommentId"),
                                Message = DbUtils.GetString(reader, "Message"),
                                PostId = postId,
                                UserProfileId = DbUtils.GetInt(reader, "CommentUserProfileId")
                            });
                        }

                    }

                    reader.Close();

                    return singleUserProfile;
                }
            }
        }

        public void Add(UserProfile singleUserProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfile ([Name], Email, DateCreated, ImageUrl, Bio)
                        OUTPUT INSERTED.ID
                        VALUES (@Name, @Email, @DateCreated, @ImageUrl, @Bio)";

                    DbUtils.AddParameter(cmd, "@Name", singleUserProfile.Name);
                    DbUtils.AddParameter(cmd, "@Email", singleUserProfile.Email);
                    DbUtils.AddParameter(cmd, "@DateCreated", DateTime.Now);
                    DbUtils.AddParameter(cmd, "@ImageUrl", singleUserProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@Bio", singleUserProfile.Bio);

                    singleUserProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(UserProfile singleUserProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                           SET [Name] = @Name,
                               Email = @Email,
                               DateCreated = @DateCreated,
                               ImageUrl = @ImageUrl,
                               Bio = @Bio
                         WHERE Id = @Id";

                    DbUtils.AddParameter(cmd, "@Name", singleUserProfile.Name);
                    DbUtils.AddParameter(cmd, "@Email", singleUserProfile.Email);
                    DbUtils.AddParameter(cmd, "@DateCreated", singleUserProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", singleUserProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@Bio", singleUserProfile.Bio);
                    DbUtils.AddParameter(cmd, "@Id", singleUserProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserProfile WHERE Id = @Id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }


}
