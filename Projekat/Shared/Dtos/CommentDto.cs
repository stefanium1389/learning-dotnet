using System;

namespace Shared.Dtos;

public class CommentDto
{

    public Guid Id {get; set;}
    public UserPreviewDto User {get; set;}
    public ArrangementSmallPreviewDto Arrangement {get; set;}
    public string CommentText {get; set;}
    public double Rating {get; set;}
    public string Status {get; set;}

}
