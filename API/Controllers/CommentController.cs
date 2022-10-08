using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Comments.Queries;
using Domain.Comments.Entities;
using MediatR;
using Domain.Comments.DTOs;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CommentController
    : DomainControllerBase<Comment, CommentResultDTO, CommentCommand, CommentPatchCommand, CommentQueryParameter>
{
    public CommentController(ISender mediator) : base(mediator)
    {
    }
}
