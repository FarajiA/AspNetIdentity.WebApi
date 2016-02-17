using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AspNetIdentity.WebApi.Entities
{
    public class ConnectedUsers
    {
        [Key]
        public int ConnectedUserID { get; set; }
        [Required]
        public string ConnectionId { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}