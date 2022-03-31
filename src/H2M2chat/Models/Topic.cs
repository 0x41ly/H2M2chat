using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace H2M2chat.Models
{
    public class Topic
    {
       

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TopicId { get; set; } 
     
        [Required]
        public string Title { get; set; } = "";
      
        public string? Creator { get; set; }
        [Required]
        public string Description { get; set; } = "";
    
        [Required]
        public string Tags { get; set; } = "";
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd,hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; } = DateTime.Now;

        public List<Comment> Comments = new List<Comment>();


    }
}