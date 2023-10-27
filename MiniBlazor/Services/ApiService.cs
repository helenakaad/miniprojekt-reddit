using System;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using MiniBlazor.Pages;
using Shared.Model;

namespace Services;

public class ApiService
{
    private readonly HttpClient http;
    private readonly IConfiguration configuration;
    private readonly string baseAPI = "https://localhost:7038/api/";

    public ApiService(HttpClient http)
    {
        this.http = http;
    }

    public async Task<List<Posts>> GetPosts()
    {
        string url = $"{baseAPI}posts/";
        return await http.GetFromJsonAsync<List<Posts>>(url);
    }

    public async Task<Posts> GetSinglePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/";
        return await http.GetFromJsonAsync<Posts>(url);
    }

    public async Task<Posts> CreatePost(string header, string name, string text)
    {
        string url = $"{baseAPI}posts/newPost/";

        // Post JSON to API, save the HttpResponseMessage
        HttpResponseMessage msg = await http.PostAsJsonAsync(url, new { header, name, text });

        // Get the JSON string from the response
        string json = msg.Content.ReadAsStringAsync().Result;

        // Deserialize the JSON string to a Posts object
        Posts? newPost = JsonSerializer.Deserialize<Posts>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Ignore case when matching JSON properties to C# properties 
        });

        // Return the new post
        return newPost;
    }

    public async Task<Comments> CreateComment(string text, string name, int id)
    {
        string url = $"{baseAPI}posts/{id}/newComment/";

        HttpResponseMessage msg = await http.PostAsJsonAsync(url, new { name, text });

        string json = msg.Content.ReadAsStringAsync().Result;

        Comments? newComment = JsonSerializer.Deserialize<Comments>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Return the new comment
        return newComment;
    }

    public async Task<Posts> UpvotePosts(int id)
    {
        string url = $"{baseAPI}posts/{id}/upvote/";
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, id);
        string json = msg.Content.ReadAsStringAsync().Result;
        Posts? updatedPost = JsonSerializer.Deserialize<Posts>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        // Return the updated post (vote increased)
        return updatedPost;
    }

    public async Task<Posts> DownvotePosts(int id)
    {
        string url = $"{baseAPI}posts/{id}/downvote/";
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, id);
        string json = msg.Content.ReadAsStringAsync().Result;
        Posts? updatedPost = JsonSerializer.Deserialize<Posts>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        // Return the updated post (vote increased)
        return updatedPost;
    }

    public async Task<Comments> UpvoteComments(int id)
    {
        string url = $"{baseAPI}posts/{id}/upvoteC/";
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, id);
        string json = msg.Content.ReadAsStringAsync().Result;
        Comments? updatedComment = JsonSerializer.Deserialize<Comments>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        // Return the updated comment (vote increased)
        return updatedComment;
    }

    public async Task<Comments> DownvoteComments(int id)
    {
        string url = $"{baseAPI}posts/{id}/downvoteC/";
        HttpResponseMessage msg = await http.PutAsJsonAsync(url, id);
        string json = msg.Content.ReadAsStringAsync().Result;
        Comments? updatedComment = JsonSerializer.Deserialize<Comments>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Return the updated post (vote increased)
        return updatedComment;
    }
}
