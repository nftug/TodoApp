using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Comments.Queries;
using Application.Comments.Models;
using Domain.Comments.Entities;
using MediatR;

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
