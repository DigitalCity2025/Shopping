using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.DAL.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Shopping.DAL.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            Guid guid = new("0995b684-1486-4f9e-8ed1-7a51b59c1e7d");
            Guid guid2 = new("62adfe32-b51c-4fde-9179-b36b64328c1a");

            builder.HasData([
                new User { Id = 42, Email = "lykhun@gmail.com", Username = "khun", Salt = guid, Role = "Admin",
                    Password = Encoding.UTF8.GetString(
                        SHA512.HashData(
                            Encoding.UTF8.GetBytes("1234" + guid)
                        )
                    ) } ,

                new User { Id = 43, Email = "ayoub@gmail.com", Username = "ayoub", Salt = guid2, Role = "Noob",
                    Password = Encoding.UTF8.GetString(
                        SHA512.HashData(
                            Encoding.UTF8.GetBytes("1234" + guid2)
                        )
                    ) } ,
            ]);
        }
    }
}
