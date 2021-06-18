namespace WebApi.Controllers.V1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Core.Models;
    using Core.Services;
    using EnsureThat;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using WebApi.Models.V1;

    /// <summary>
    /// Provides CRUD operation for to-do items via Http.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1.0")]
    public class ToDoItemsController : ControllerBase
    {
        private readonly IToDoService toDoService;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoItemsController"/> class.
        /// </summary>
        /// <param name="toDoService">The <see cref="IToDoService"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        public ToDoItemsController(IToDoService toDoService, IMapper mapper)
        {
            this.toDoService = EnsureArg.IsNotNull(toDoService, nameof(toDoService));
            this.mapper = EnsureArg.IsNotNull(mapper, nameof(mapper));
        }

        /// <summary>
        /// Retrieve all to-do item belong to specified account.
        /// </summary>
        /// <param name="accountId">The account id of the to-do item belong.</param>
        /// <returns>A collection of <see cref="ToDoItemResponse"/>.</returns>
        [HttpGet]
        [Route("accounts/{accountId}/[controller]")]
        public async Task<ActionResult<IEnumerable<ToDoItemResponse>>> Get(string accountId)
        {
            var toDoItems = await this.toDoService.RetrieveAsync(accountId).ConfigureAwait(false);
            return this.Ok(this.mapper.Map<IEnumerable<ToDoItem>, List<ToDoItemResponse>>(toDoItems));
        }

        /// <summary>
        /// Retrieve to-do item by id.
        /// </summary>
        /// <param name="accountId">The account id of the to-do item.</param>
        /// <param name="id">The id of the to-do item.</param>
        /// <returns>The <see cref="ToDoItemResponse"/>.</returns>
        [HttpGet]
        [Route("accounts/{accountId}/[controller]/{id}")]
        public async Task<ActionResult<ToDoItemResponse>> Get(string accountId, string id)
        {
            var toDoItem = await this.toDoService.RetrieveAsync(accountId, id).ConfigureAwait(false);

            if (toDoItem == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.mapper.Map<ToDoItemResponse>(toDoItem));
        }

        /// <summary>
        /// Update to-do item.
        /// </summary>
        /// <param name="accountId">The account id of the to-do item to be updated.</param>
        /// <param name="id">The id of the to-do item to be updated.</param>
        /// <param name="request">The update information.</param>
        /// <returns>An <see cref="IActionResult"/> indicating whether the operation is success.</returns>
        [HttpPut]
        [Route("accounts/{accountId}/[controller]/{id}")]
        public async Task<IActionResult> Put(string accountId, string id, ToDoItemRequest request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var selectedItem = await this.toDoService.RetrieveAsync(accountId, id).ConfigureAwait(false);

            if (selectedItem == null)
            {
                return this.NotFound();
            }

            var updatedItem = this.mapper.Map(request, selectedItem);
            await this.toDoService.UpdateAsync(updatedItem).ConfigureAwait(false);

            return this.NoContent();
        }

        /// <summary>
        /// Partial update to-do item.
        /// </summary>
        /// <param name="accountId">The account id of the to-do item to be updated.</param>
        /// <param name="id">The id of the to-do item to be updated.</param>
        /// <param name="request">The properties to be updated.</param>
        /// <returns>The updated <see cref="ToDoItemResponse"/>.</returns>
        [HttpPatch]
        [Route("accounts/{accountId}/[controller]/{id}")]
        public async Task<ActionResult<ToDoItemResponse>> Patch(string accountId, string id, JsonPatchDocument<IUpdatableToDoItemDTO> request)
        {
            var selectedItem = await this.toDoService.RetrieveAsync(accountId, id).ConfigureAwait(false);
            var selectedItemDto = this.mapper.Map<ToDoItemResponse>(selectedItem);

            if (selectedItem == null)
            {
                return this.NotFound();
            }

            request.ApplyTo(selectedItemDto);
            selectedItem = this.mapper.Map(selectedItemDto, selectedItem);

            await this.toDoService.UpdateAsync(selectedItem).ConfigureAwait(false);

            return this.Ok(selectedItem);
        }

        /// <summary>
        /// Create to-do item.
        /// </summary>
        /// <param name="accountId">The account id of the to-do item belong to.</param>
        /// <param name="request">The to-do item information.</param>
        /// <returns>The created <see cref="ToDoItemResponse"/>.</returns>
        [HttpPost]
        [Route("accounts/{accountId}/[controller]")]
        public async Task<ActionResult<ToDoItemResponse>> Post(string accountId, ToDoItemRequest request)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var itemToCreate = this.mapper.Map<ToDoItem>(request);
            itemToCreate.AccountId = accountId;
            var createdItem = await this.toDoService.CreateAsync(itemToCreate).ConfigureAwait(false);

            return this.CreatedAtAction(nameof(this.Post), createdItem);
        }
    }
}