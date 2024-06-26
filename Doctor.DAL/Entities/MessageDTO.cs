﻿namespace Doctor.DataAcsess.Entities
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string SenderUserName { get; set; }
        public string RecipientId { get; set; }
        public string RecipientUserName { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.Now;
    }
}
