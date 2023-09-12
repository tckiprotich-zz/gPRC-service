using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gPRC_API.Data;
using gPRC_API.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace gPRC_API.Services
{
    /// <summary>
    /// Service to manage ToDo items.
    /// </summary>
    public class ToDoService : toDoService.toDoServiceBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructor for ToDoService.
        /// </summary>
        /// <param name="context">Database context.</param>
        public ToDoService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new ToDo item.
        /// </summary>
        /// <param name="request">Request containing the details of the ToDo item to be created.</param>
        /// <param name="context">Server call context.</param>
        /// <returns>Response containing the ID of the created ToDo item.</returns>
        public override async Task<createToDoResponse> createToDo (createToDoRequest request, ServerCallContext context)
        {
            // Validation
            if(request.Title == string.Empty || request.Description == string.Empty)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Title cannot be empty"));
            }

            // Create new ToDo item
            var todo = new ToDOItems
            {
                Title = request.Title,
                Description = request.Description
            };

            // Add to database and save changes
            await _context.ToDOItems.AddAsync(todo);
            await _context.SaveChangesAsync();

            // Return response
            return await Task.FromResult(new createToDoResponse
            {
                Id = todo.Id,
            });
        }

        /// <summary>
        /// Reads a specific ToDo item by ID.
        /// </summary>
        /// <param name="request">Request containing the ID of the ToDo item to be read.</param>
        /// <param name="context">Server call context.</param>
        /// <returns>Response containing the details of the ToDo item.</returns>
        public override async Task<readToDosResponse> readToDo(readToDoRequest request, ServerCallContext context)
        {
            // Validation
            if(request.Id <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be greater than 0"));
            }

            // Find ToDo item
            var todo =  await _context.ToDOItems.FirstOrDefaultAsync(x => x.Id == request.Id);

            // Check if ToDo item exists
            if(todo == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"ToDo with Id = {request.Id} is not found"));
            }

            // Return response
            return await Task.FromResult(new readToDosResponse
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description
            });
        }

        /// <summary>
        /// Reads all ToDo items.
        /// </summary>
        /// <param name="request">Request to read all ToDo items.</param>
        /// <param name="context">Server call context.</param>
        /// <returns>Response containing all ToDo items.</returns>
        public override async Task<GetAllResponse> readToDos(GetAllRequest request, ServerCallContext context)
        {            
            var response = new GetAllResponse();
            var todos = await _context.ToDOItems.ToListAsync();

            // Add each ToDo item to the response
            foreach(var todo in todos)
            {
                response.ToDos.Add(new readToDosResponse
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Description = todo.Description
                });
            }

            // Return response
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Updates a specific ToDo item.
        /// </summary>
        /// <param name="request">Request containing the updated details of the ToDo item.</param>
        /// <param name="context">Server call context.</param>
        /// <returns>Response containing the ID of the updated ToDo item.</returns>
        public override async Task<updateToDoResponse> updateToDo(updateToDoRequest request, ServerCallContext context)
        {
           // Validation
           if( request.Id <=0 || request.Title == string.Empty || request.Description == string.Empty)
           {
               throw new RpcException(new Status(StatusCode.InvalidArgument, "You must provide a valid Object"));
           }

           // Find ToDo item
           var todo = await _context.ToDOItems.FirstOrDefaultAsync(x => x.Id == request.Id);

           // Check if ToDo item exists
           if(todo == null)
           {
               throw new RpcException(new Status(StatusCode.NotFound, $"ToDo with Id = {request.Id} is not found"));
           }

           // Update ToDo item
           todo.Title = request.Title;
           todo.Description = request.Description;
                
           // Save changes
           await _context.SaveChangesAsync();

           // Return response
           return await Task.FromResult(new updateToDoResponse
           {
               Id = todo.Id,
           });
        }

        /// <summary>
        /// Deletes a specific ToDo item.
        /// </summary>
        /// <param name="request">Request containing the ID of the ToDo item to be deleted.</param>
        /// <param name="context">Server call context.</param>
        /// <returns>Response containing the ID of the deleted ToDo item.</returns>
        public override async Task<deleteToDoResponse> deleteToDo(deleteToDoRequest request, ServerCallContext context)
        {
            // Validation
            if(request.Id <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Id must be greater than 0"));
            }

            // Find ToDo item
            var todo = await _context.ToDOItems.FirstOrDefaultAsync(x => x.Id == request.Id);

            // Check if ToDo item exists
            if(todo == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"ToDo with Id = {request.Id} is not found"));
            }

            // Remove ToDo item and save changes
            _context.ToDOItems.Remove(todo);
            await _context.SaveChangesAsync();

            // Return response
            return await Task.FromResult(new deleteToDoResponse
            {
                Id = todo.Id
            });
        }
    }
}