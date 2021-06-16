namespace WebApi.Controllers.V1
{
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApi.Models.V1;

    [ApiController]
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1.0")]
    public class ToDoItemsController : ControllerBase
    {
        private static readonly List<ToDoItemResponse> ToDoItems = new List<ToDoItemResponse>()
        {
            new ToDoItemResponse
            {
                Id = "1",
                AccountId = "account_1",
                Name = "Learn .Net 5",
                Description = "Learn .Net 5 by creating a simple CRUD Web API",
                IsComplete = false,
            },
            new ToDoItemResponse
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
        public ActionResult<IEnumerable<ToDoItemResponse>> Get()
        {
            return ToDoItems;
        }

        [HttpGet]
        [Route("accounts/{accountId}/[controller]/{id}")]
        public ActionResult<ToDoItemResponse> Get(string accountId, string id)
        {
            var selectedItem = ToDoItems
                .SingleOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (selectedItem == null)
            {
                return NotFound();
            }

            return selectedItem;
        }

        [HttpPut]
        [Route("accounts/{accountId}/[controller]/{id}")]
        public IActionResult Put(string accountId, string id, ToDoItemRequest request)
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

        [HttpPatch]
        [Route("accounts/{accountId}/[controller]/{id}")]
        public IActionResult Patch(string accountId, string id, JsonPatchDocument<IUpdatableToDoItemDTO> request)
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
        [Route("accounts/{accountId}/[controller]")]
        public ActionResult<ToDoItemResponse> Post(string accountId, ToDoItemRequest request)
        {
            var toDoItem = new ToDoItemResponse
            {
                Id = Guid.NewGuid().ToString(),
                AccountId = accountId,
                Name = request.Name,
                Description = request.Description,
                IsComplete = request.IsComplete,
            };

            ToDoItems.Add(toDoItem);

            return CreatedAtAction(nameof(Post), toDoItem);
        }
    }
}