namespace WebApi.Controllers.V1
{
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApi.Models;

    [ApiController]
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1.0")]
    public class ToDoItemsController : ControllerBase
    {
        private static readonly List<ToDoItem> ToDoItems = new List<ToDoItem>()
        {
            new ToDoItem
            {
                Id = "1",
                AccountId = "account_1",
                Name = "Learn .Net 5",
                Description = "Learn .Net 5 by creating a simple CRUD Web API",
                IsComplete = false,
            },
            new ToDoItem
            {
                Id = "2",
                AccountId = "account_1",
                Name = "Add Unit Test",
                Description = "Add unit test for the Web API project",
                IsComplete = false,
            },
        };

        [HttpGet]
        [Route("[controller]")]
        public ActionResult<IEnumerable<ToDoItem>> Get()
        {
            return ToDoItems;
        }

        [HttpGet]
        [Route("accounts/{accountId}/[controller]/{id}")]
        public ActionResult<ToDoItem> Get(string accountId, string id)
        {
            var selectedItem = ToDoItems
                .SingleOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (selectedItem == null)
            {
                return NotFound();
            }

            return selectedItem;
        }

        [HttpPut("{id}")]
        [Route("[controller]")]
        public IActionResult Put(string id, ToDoItemRequest request)
        {
            var selectedItem = ToDoItems
                .SingleOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (selectedItem == null)
            {
                return NotFound();
            }

            selectedItem.Name = request.Name;
            selectedItem.Description = request.Description;
            selectedItem.IsComplete = request.IsComplete;

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Route("[controller]")]
        public IActionResult Patch(string id, JsonPatchDocument<IToDoItemRequest> request)
        {
            var selectedItem = ToDoItems
                .SingleOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (selectedItem == null)
            {
                return NotFound();
            }

            request.ApplyTo(selectedItem);

            return Ok(selectedItem);
        }

        [HttpPost]
        [Route("[controller]")]
        public ActionResult<ToDoItem> Post(ToDoItemRequest request)
        {
            var toDoItem = new ToDoItem
            {
                Id = Guid.NewGuid().ToString(),
                AccountId = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                IsComplete = request.IsComplete,
            };

            ToDoItems.Add(toDoItem);

            return CreatedAtAction(nameof(Post), toDoItem);
        }
    }
}