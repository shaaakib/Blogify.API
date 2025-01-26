using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blogify.Api.Models.Domain
{
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

    }
}
