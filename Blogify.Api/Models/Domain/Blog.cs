using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogify.Api.Models.Domain
{
    public class Blog
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
