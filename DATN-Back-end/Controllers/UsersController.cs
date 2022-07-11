using DATN_Back_end.Dto.DtoUser;
using DATN_Back_end.Models;
using DATN_Back_end.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Controllers
{
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private IRepository<User> repository;

        private IUserRepository userRepository;

        public UsersController(IRepository<User> repository, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.repository = repository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<List<UserItem>> Get() => await userRepository.GetAll();

        [HttpGet("manager")]
        public async Task<List<UserItem>> GetUserByDepartment(Guid id) => await userRepository.GetUserByDepartmentId(id);

        [HttpGet("{id}")]
        public async Task<UserDetail> Get(Guid id) => await userRepository.Get(id);

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task Create([FromBody] UserFormCreate userForm) => await userRepository.Create(userForm);

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task Delete(Guid id) => await repository.Delete(id);

        [HttpPut("{id}")]
        public async Task Update(Guid id, [FromBody] UserFormUpdate userForm) => await userRepository.Update(id, userForm);
    }
}
