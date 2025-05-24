using Microsoft.AspNetCore.Mvc;
using NewsPortal.Data;
using NewsPortal.Models;

namespace NewsPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackContext _context;
        public FeedbackController(FeedbackContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Guarda feedback del usuario. No permite múltiples votos por postId y solo acepta 'like' o 'dislike'.
        /// </summary>
        /// <param name="feedback">Feedback a guardar</param>
        /// <returns>200 OK si se guarda, 400 si ya existe o es inválido</returns>
        [HttpPost]
        public IActionResult Post([FromBody] Feedback feedback)
        {
            if (feedback.Sentimiento != "like" && feedback.Sentimiento != "dislike")
            {
                return BadRequest(new { message = "Sentimiento inválido." });
            }
            // Identificador único por usuario/navegador (usando una cookie simple)
            string userKey = Request.Cookies["userKey"];
            if (string.IsNullOrEmpty(userKey))
            {
                userKey = Guid.NewGuid().ToString();
                Response.Cookies.Append("userKey", userKey, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            }
            // Solo un feedback por postId y userKey
            bool yaExiste = _context.Feedbacks.Any(f => f.PostId == feedback.PostId && f.UserKey == userKey);
            if (yaExiste)
            {
                return BadRequest(new { message = "Ya enviaste feedback para este post." });
            }
            feedback.Fecha = DateTime.UtcNow;
            feedback.UserKey = userKey;
            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Devuelve todos los registros de feedback.
        /// </summary>
        /// <returns>Lista de feedback</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Feedbacks.ToList());
        }

        /// <summary>
        /// Endpoint de prueba para Render: responde pong
        /// </summary>
        [HttpGet("/api/ping")]
        public IActionResult Ping()
        {
            return Ok(new { message = "pong" });
        }

        /// <summary>
        /// Obtiene la cantidad de likes y dislikes para un post específico.
        /// </summary>
        /// <param name="postId">ID del post</param>
        /// <returns>200 OK con la cantidad de likes y dislikes</returns>
        [HttpGet("/api/feedback/post/{postId}")]
        public IActionResult GetFeedbackForPost(int postId)
        {
            var feedbacks = _context.Feedbacks.Where(f => f.PostId == postId).ToList();
            var likes = feedbacks.Count(f => f.Sentimiento == "like");
            var dislikes = feedbacks.Count(f => f.Sentimiento == "dislike");
            return Ok(new { postId, likes, dislikes });
        }
    }
}
