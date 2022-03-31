using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace H2M2chat.Models
{
    public class Comment
    {
       
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CommentId { get; set; }
        [Required]
        [ForeignKey("Topic")]
        public Guid TopicId { get; set; }

        [Required]
        [ForeignKey("Comment")]
        public Guid ParentId { get; set; }


        [Required]
        public string? Message { get; set; }

        public string? Creator { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd,hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; } = DateTime.Now;
        public virtual Topic? Topic { get; set; }

        public  List<Comment> SubComments = new List<Comment>();
    }
}