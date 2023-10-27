using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Data;
using Shared.Model;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace Service;

public class DataService
{
    private PostsContext db { get; }

    public DataService(PostsContext db)
    {
        this.db = db;
    }

    //Seeder noget nyt data i databasen hvis det er nødvendigt.
    public void SeedData()
    {
        Posts posts = db.Post.FirstOrDefault()!;

        if (posts == null)
        {
            var post1 = new Posts { Name = "SejeReje43", Text = "AITA for dumping water on a fish?", Header = "AITA", DownVotes = 4, Upvotes = 1};
            var post2 = new Posts { Name = "TivoliTommy", Text = "Hjælp til arbejde", Header = "HJÆLP", DownVotes = 7, Upvotes = 10 };

            //kommentarer til post1
            var comment1 = new Comments { Name = "FlotteHans", Text = "Yes, YATH", DownVotes = 13, Upvotes = 1 };
            var comment2 = new Comments { Name = "GrimmeGert", Text = "Ja for søren du er!", DownVotes = 10, Upvotes = 82034 };

            //kommentarer til post2
            var comment3 = new Comments { Name = "FrækkeFrederik", Text = "Gør det selv :)" };

            post1.Comments.Add(comment1);
            post1.Comments.Add(comment2);

            post2.Comments.Add(comment3);

            db.Post.Add(post1);
            db.Post.Add(post2);
        }

        db.SaveChanges();
    }

    public List<Posts> GetPosts()
    {
        return db.Post.OrderByDescending(post => post.DateTime).ToList();
    }

    public Posts GetSinglePost(int id)
    {
        return db.Post.Include(b => b.Comments).FirstOrDefault(a => a.PostsId == id);
    }

    public string CreatePost(string text, string name, string header)
    {
        db.Post.Add(new Posts(text, name, header));
        db.SaveChanges();
        return "Post created";
    }

    public string createComment(string name, string text, int postsId)
    {
        Posts posts = db.Post.FirstOrDefault(a => a.PostsId == postsId);
        posts.Comments.Add(new Comments { Name = name, Text = text });
        db.Update(posts);
        db.SaveChanges();
        return "Comment created";
    }

    public string upvotePost(int postsId)
    {
        Posts posts = db.Post.FirstOrDefault(a => a.PostsId == postsId);

        if (posts != null)
        {
            posts.Upvotes++;
            db.Update(posts);
            db.SaveChanges();
            return "Post upvoted!";
        }

        return "Post not found!";
    }

    public string downvotePost(int postsId)
    {
        Posts posts = db.Post.FirstOrDefault(a => a.PostsId == postsId);

        if (posts != null)
        {
            posts.DownVotes++;
            db.Update(posts);
            db.SaveChanges();
            return "Post downvoted!";
        }

        return "Post not found!";
    }

    public string UpvoteComment(int commentsId)
    {
        Comments comments = db.Comments.FirstOrDefault(a => a.CommentsId == commentsId);

        if (comments != null)
        {
            comments.Upvotes++;
            db.Update(comments);
            db.SaveChanges();
            return "Post downvoted!";
        }

        return "Post not found!";
    }

    public string DownvoteComment(int commentsId)
    {
        Comments comments = db.Comments.FirstOrDefault(a => a.CommentsId == commentsId);

        if (comments != null)
        {
            comments.DownVotes++;
            db.Update(comments);
            db.SaveChanges();
            return "Post downvoted!";
        }

        return "Post not found!";
    }
}

