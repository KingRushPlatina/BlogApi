﻿namespace BlogApi.Models
{
    public class Comment
    {
         public int Id { get; set; }
         public string Text { get; set; }
         
         public DateOnly CreationDate { get; set; }
    }
}