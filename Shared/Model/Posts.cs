using System;
using System.Text.Json.Serialization;
namespace Shared.Model;

public class Posts
{
    [JsonConstructor]
    public Posts(string text, string name, string header, DateTime dateTime, int upvotes, int downvotes, List<Comments> comments)
    {
        this.Text = text;
        this.Name = name;
        this.Header = header;
        this.DateTime = dateTime;
        this.Upvotes = upvotes;
        this.DownVotes = downvotes;
        this.Comments = comments;
    }

    public Posts(string text, string name, string header)
    {
        this.Text = text;
        this.Name = name;
        this.Header = header;
    }

    public Posts(int upvotes)
    {
        this.Upvotes = upvotes;
    }

    public Posts()
    {
    }

    public int PostsId { get; set; }

    public string Text { get; set; }
    public string Header { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
    public string Name { get; set; }
    public int Upvotes { get; set; } = 0;
    public int DownVotes { get; set; } = 0;
    public List<Comments> Comments { get; set; } = new List<Comments>();
}
