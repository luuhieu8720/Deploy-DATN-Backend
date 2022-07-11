using DATN_Back_end.Dto.DtoComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public interface ICommentRepository
    {
        Task Create(CommentForm commentForm);
    }
}
