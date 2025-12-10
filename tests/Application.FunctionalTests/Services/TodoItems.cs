using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Web.Application.Common.Interfaces;
using Web.Application.Common.Models;
using Web.Application.Services;
using Web.Domain.Entities;
using Web.Infrastructure.Data;
using Web.Infrastructure.Repositories;

namespace Application.FunctionalTests.Services;

public class TodoItems : IClassFixture<TestingFactory>
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    private readonly ITodoItemRepository _repository;
    private readonly TodoItemService _service;

    public TodoItems(TestingFactory factory)
    {
        _context = factory.Services.GetRequiredService<ApplicationDbContext>();

        ILoggerFactory loggerFactory = LoggerFactory.Create(b => b.AddDebug().SetMinimumLevel(LogLevel.Debug));
        
        MapperConfiguration _configuration = new MapperConfiguration(cfg =>
            cfg.AddMaps(typeof(IApplicationDbContext).Assembly),
            loggerFactory: loggerFactory);

        _mapper = _configuration.CreateMapper();

        _repository = new TodoItemRepository(_context);

        _service = new TodoItemService(_repository, _mapper);
    }

    [Fact]
    public async Task Should_Create_TodoItem()
    {
        TodoItemDto todoItem = new TodoItemDto
        {
            Title = "Test Todo Item Title."
        };

        int id = await _service.CreateTodoItem(todoItem, TestContext.Current.CancellationToken);

        TodoItem? item = await _context.TodoItems.FindAsync([id], TestContext.Current.CancellationToken);

        Assert.NotNull(item);
        Assert.Equal(item.Title, todoItem.Title);
    }

    [Fact]
    public async Task Should_Delete_TodoItem()
    {
        TodoItemDto todoItem = new TodoItemDto
        {
            Title = "Test Todo Item Title."
        };

        int id = await _service.CreateTodoItem(todoItem, TestContext.Current.CancellationToken);

        await _service.DeleteTodoItem(id, TestContext.Current.CancellationToken);

        TodoItem? item = await _context.TodoItems.FindAsync([id], TestContext.Current.CancellationToken);

        Assert.Null(item);
    }

    [Fact]
    public async Task Should_Update_TodoItem()
    {
        TodoItemDto firstDto = new TodoItemDto
        {
            Title = "Test Todo Item Title."
        };

        int id = await _service.CreateTodoItem(firstDto, TestContext.Current.CancellationToken);

        TodoItem? firtEntry = await _context.TodoItems.FindAsync([id], TestContext.Current.CancellationToken);

        Assert.NotNull(firtEntry);

        TodoItemDto newDto = new TodoItemDto
        {
            Id = firtEntry.Id,
            Title = "Updated Test Todo Item Title."
        };

        await _service.UpdateTodoItem(newDto, TestContext.Current.CancellationToken);

        TodoItem? updatedEntry = await _context.TodoItems.FindAsync([id], TestContext.Current.CancellationToken);

        Assert.Equal(updatedEntry?.Title, newDto.Title);
    }
}
