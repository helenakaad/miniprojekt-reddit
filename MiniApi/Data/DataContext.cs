using Microsoft.EntityFrameworkCore;
using Shared.Model;

namespace Data;

public class PostsContext : DbContext
{
    public DbSet<Posts> Post => Set<Posts>();
    public DbSet<Comments> Comments => Set<Comments>();

    public PostsContext(DbContextOptions<PostsContext> options)
        : base(options)
    {
    }
}
