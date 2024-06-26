﻿namespace Doctor.DataAcsess.Entities
{
    public class Message
    {
        public int Id { get; set; } 
        public string SenderId { get; set; }    
        public string SenderUserName { get; set; }  
        public User Sender { get; set; }
        public string RecipientId { get; set; }
        public string RecipientUserName { get; set; }
        public User Recipient { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.Now;
        public bool SenderDeleted { get; set; }
        public bool RecepientDeleted { get; set; }
    }
}
