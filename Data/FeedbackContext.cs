using Microsoft.EntityFrameworkCore;
using NewsPortal.Models;

namespace NewsPortal.Data
{
    public class FeedbackContext : DbContext
    {
        public FeedbackContext(DbContextOptions<FeedbackContext> options) : base(options) { }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
