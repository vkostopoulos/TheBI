using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDb.Identity
{
    public class ApplicationIdentityContext : IDisposable
    {
        private ApplicationIdentityContext(IMongoCollection<ApplicationUser> users,
            IMongoCollection<AspNet.Identity.MongoDB.IdentityRole> roles)
        {
            Users = users;
            Roles = roles;
        }

        public IMongoCollection<AspNet.Identity.MongoDB.IdentityRole> Roles { get; set; }

        public IMongoCollection<ApplicationUser> Users { get; set; }

        public void Dispose()
        {
        }

        public static ApplicationIdentityContext Create()
        {
            // todo add settings where appropriate to switch server & database in your own application
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("mydb");
            var users = database.GetCollection<ApplicationUser>("users");
            var roles = database.GetCollection<AspNet.Identity.MongoDB.IdentityRole>("roles");
            return new ApplicationIdentityContext(users, roles);
        }

        public Task<List<AspNet.Identity.MongoDB.IdentityRole>> AllRolesAsync()
        {
            return Roles.Find(r => true).ToListAsync();
        }
    }
}