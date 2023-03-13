using Models;
using Microsoft.EntityFrameworkCore;

namespace DbContexts;


public class BasketPlayersTeamsDb : DbContext
{

    public BasketPlayersTeamsDb(DbContextOptions<BasketPlayersTeamsDb> options)
        : base(options) { }

    public DbSet<BasketballPlayer> Players => Set<BasketballPlayer>();

}
