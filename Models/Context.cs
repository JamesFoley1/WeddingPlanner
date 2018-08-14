using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Models {

    public class Context : DbContext {
        //base() calls the parent class' constructor passing the "options" paramater along
        public Context(DbContextOptions<Context> options) : base(options) {}
        public DbSet<User> users {get;set;}
        public DbSet<Wedding> weddings  {get;set;}
        public DbSet<Rsvp> rsvps {get;set;}
    }
}