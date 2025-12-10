using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Web.Application.Common.Interfaces;
using Web.Application.Common.Models;
using Web.Domain.Entities;

namespace Application.UnitTests.Mappings;

public class MappingTests
{
    private readonly ILoggerFactory? _loggerFactory;
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configuration;
    
    public MappingTests()
    {
        _loggerFactory = LoggerFactory.Create(b => b.AddDebug().SetMinimumLevel(LogLevel.Debug));
        
        _configuration = new MapperConfiguration(configuration =>
            configuration.AddMaps(typeof(IApplicationDbContext).Assembly),
            loggerFactory: _loggerFactory);

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void Mapping_Configuration_IsValid()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Theory]
    [InlineData(typeof(TodoItem), typeof(TodoItemDto))]
    [InlineData(typeof(TodoItemDto), typeof(TodoItem))]
    public void Should_Support_Mapping_From_Source_To_Destination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);
        _mapper!.Map(instance!, source, destination);
    }

    private static object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        return RuntimeHelpers.GetUninitializedObject(type);
    }
}
