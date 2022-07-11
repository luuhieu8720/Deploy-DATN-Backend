using DATN_Back_end.Dto.DtoComment;
using DATN_Back_end.Models;
using DATN_Back_end.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Controllers
{
    [Route("api/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly IRepository<Comment> repository;

        private readonly ICommentRepository commentRepository;

        public CommentsController(IRepository<Comment> repository,
            ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
            this.repository = repository;
        }

        [HttpPost]
        public async Task Create([FromBody] CommentForm commentForm) => await commentRepository.Create(commentForm);

        [HttpGet]
        public async Task<List<CommentItem>> Get() => await repository.Get<CommentItem>();

        [HttpGet("{id}")]
        public async Task<CommentDetail> Get(Guid id) => await repository.Get<CommentDetail>(id);

        [HttpPut("{id}")]
        public async Task Update(Guid id, [FromBody] CommentForm commentForm) => await repository.Update(id, commentForm);
    }
}
