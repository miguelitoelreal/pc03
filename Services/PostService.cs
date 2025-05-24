using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;

namespace NewsPortal.Services
{
    public class PostDto
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string? title { get; set; }
        public string? body { get; set; }
    }

    public class UserDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        // Puedes agregar más campos si lo necesitas
    }

    public class CommentDto
    {
        public int postId { get; set; }
        public int id { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public string? body { get; set; }
    }

    public class EnrichedPostDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public UserDto? Author { get; set; }
        public List<CommentDto> Comments { get; set; } = new();
    }

    public class PostService
    {
        private readonly HttpClient _httpClient;
        public PostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PostDto>> GetPostsAsync()
        {
            var posts = await _httpClient.GetFromJsonAsync<List<PostDto>>("https://jsonplaceholder.typicode.com/posts");
            return posts ?? new List<PostDto>();
        }

        public async Task<List<EnrichedPostDto>> GetEnrichedPostsAsync()
        {
            var posts = await _httpClient.GetFromJsonAsync<List<PostDto>>("https://jsonplaceholder.typicode.com/posts");
            var users = await _httpClient.GetFromJsonAsync<List<UserDto>>("https://jsonplaceholder.typicode.com/users");
            var comments = await _httpClient.GetFromJsonAsync<List<CommentDto>>("https://jsonplaceholder.typicode.com/comments");

            var enriched = posts?.Select(post => new EnrichedPostDto
            {
                Id = post.id,
                Title = post.title,
                Body = post.body,
                Author = users?.FirstOrDefault(u => u.id == post.userId),
                Comments = comments?.Where(c => c.postId == post.id).ToList() ?? new List<CommentDto>()
            }).ToList() ?? new List<EnrichedPostDto>();

            return enriched;
        }

        public async Task<EnrichedPostDto?> GetEnrichedPostByIdAsync(int id)
        {
            var post = await _httpClient.GetFromJsonAsync<PostDto>($"https://jsonplaceholder.typicode.com/posts/{id}");
            if (post == null) return null;
            var user = await _httpClient.GetFromJsonAsync<UserDto>($"https://jsonplaceholder.typicode.com/users/{post.userId}");
            var comments = await _httpClient.GetFromJsonAsync<List<CommentDto>>($"https://jsonplaceholder.typicode.com/comments?postId={id}");
            return new EnrichedPostDto
            {
                Id = post.id,
                Title = post.title,
                Body = post.body,
                Author = user,
                Comments = comments ?? new List<CommentDto>()
            };
        }
    }
}
