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
            if (_context.Feedbacks.Any(f => f.PostId == feedback.PostId))
            {
                return BadRequest(new { message = "Ya existe feedback para este post." });
            }
            feedback.Fecha = DateTime.UtcNow;
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
    }
}
