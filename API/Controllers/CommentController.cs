using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Comments.Queries;
using Application.Comments.Models;
using Domain.Comments.Entities;
using Application.Shared.UseCases;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CommentController
    : DomainControllerBase<Comment, CommentResultDTO, CommentCommand, CommentPatchCommand, CommentQueryParameter>
{
    protected override Commands<Comment, CommentResultDTO, CommentCommand, CommentPatchCommand> Commands => new();

    protected override Queries<Comment, CommentResultDTO> Queries => new();
}
