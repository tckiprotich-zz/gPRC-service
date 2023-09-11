using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gPRC_API.Data;
using gPRC_API.Models;
using Grpc.Core;

namespace gPRC_API.Services
{
    public class ToDoService : toDoService.toDoServiceBase
    {
        private readonly AppDbContext _context;
        public ToDoService(AppDbContext context)
        {
            _context = context;
            
        }

        public override async Task<createToDoResponse> createToDo (createToDoRequest request, ServerCallContext context)
        {
            if(request.Title == string.Empty || request.Description == string.Empty)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Title cannot be empty"));
            }
            var todo = new ToDOItems
            {
                Title = request.Title,
                Description = request.Description
            };

            await _context.ToDOItems.AddAsync(todo);
            await _context.SaveChangesAsync();

            return await Task.FromResult(new createToDoResponse
            {
                Id = todo.Id,
                
            });
        }
        
    }
}