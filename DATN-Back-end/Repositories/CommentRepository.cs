using DATN_Back_end.Dto.DtoComment;
using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly DataContext dataContext;

        public CommentRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task Create(CommentForm commentForm)
        {
            commentForm.CommentedTime = DateTime.Now;

            await base.Create(commentForm);
        }
    }
}
