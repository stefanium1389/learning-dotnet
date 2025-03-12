using System;

namespace Shared.Dtos;

public class CommentCreationDto
{
    public string ArrangementId {get; set;}
    public string CommentText {get; set;}
    public double Rating {get; set;}
}
