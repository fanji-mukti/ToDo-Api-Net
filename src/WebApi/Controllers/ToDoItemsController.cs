namespace WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WebApi.Models;

    [ApiController]
    [Route("[controller]")]
    public class ToDoItemsController : ControllerBase
    {
        private readonly List<ToDoItem> toDoItems = new List<ToDoItem>()
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
        public ActionResult<IEnumerable<ToDoItem>> Get()
        {
            return this.toDoItems;
        }

        [HttpGet("{id}")]
        public ActionResult<ToDoItem> Get(string id)
        {
            var selectedItem = this.toDoItems
                .SingleOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (selectedItem == null)
            {
                return NotFound();
            }

            return selectedItem;
        }
    }
}