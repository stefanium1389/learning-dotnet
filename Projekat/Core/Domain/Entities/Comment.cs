using System;

namespace Domain.Entities;

public class Comment
{
    public Guid Id {get; set;}
    public User User {get; set;}
    public Arrangement Arrangement {get; set;}
    public string CommentText {get; set;}
    public double Rating {get; set;}
    public CommentStatus Status {get; set;}
}
