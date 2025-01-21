using System;
using System.Threading.Tasks;
using ECApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using static Testing;

namespace ECApp.IntegrationTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        //Arrange
        var userName001 = "testUser001";
        var userName002NotExist = "testUser002";
        
        //Act
        await AddAsync(new Users()
        {
            Id = Guid.NewGuid(),
            Name = userName001,
            CreatedTime = DateTimeOffset.UtcNow,
            Creator = "paul"
        });
        
        //Assertion
        var queryUserResult = await QueryAsync<Users>(a => a.Name == userName001);
        queryUserResult.Should().HaveCount(1);
        
        queryUserResult = await QueryAsync<Users>(a => a.Name == userName002NotExist);
        queryUserResult.Should().BeEmpty();
    }


    [TearDown]
    public async Task TearDown()
    {
        await ResetState();
    }
}