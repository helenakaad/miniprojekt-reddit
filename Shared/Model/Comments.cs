using System;
using System.Text.Json.Serialization;

namespace Shared.Model;

public class Comments
{
    [JsonConstructor]
    public Comments(string text, DateTime dateTime, string name, int upvotes, int downvotes)
    {
        this.Text = text;
        this.DateTime = dateTime;
        this.Name = name;
        this.Upvotes = upvotes;
        this.DownVotes = downvotes;
    }

    public Comments(string text, string name)
    {
        this.Text = text;
        this.Name = name;
    }

    public Comments()
    {
    }

    public int CommentsId { get; set; }

    public string Text { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;
    public string Name { get; set; }
    public int Upvotes { get; set; } = 0;
    public int DownVotes { get; set; } = 0;
}

