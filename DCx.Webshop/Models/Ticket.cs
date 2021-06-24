using DCx.lib.Webshop.Storage.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DCx.Webshop.Models
{
    public class Ticket : MongoEntity
    {
        public int TicketNr { get; set; }
        [Required]
        public string Subject { get; set; }
        //[Required]
        public ProductItem Product { get; set; }
        public List<Communication> Communications { get; set; } = new List<Communication>();
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        private string _createdDateString;
        public string CreatedDateString
        {
            get
            {
                return CreatedDate.ToShortDateString();
            }
            set
            {
                _createdDateString = CreatedDate.ToShortDateString();
            }
        }
        public DateTime LastActionDate { get; set; }
        private string _lastActionDateString;
        public string LastActionDateString
        {
            get
            {
                return LastActionDate.ToShortDateString();
            }
            set
            {
                _lastActionDateString = LastActionDate.ToShortDateString();
            }
        }
        public int State { get; set; }
    }

    public enum State
    {
        Created = 0,
        Open = 1,
        Closed = 2,
        All = 3
    }

    public class MailLinks
    {
        public string TicketLink { get; set; }
        public string AttachmentLink { get; set; }
    }
}
