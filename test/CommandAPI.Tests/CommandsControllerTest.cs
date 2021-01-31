using System;
using System.Collections.Generic;
using Moq;
using AutoMapper;
using Xunit;
using CommandAPI.Models;
using CommandAPI.Data;
using CommandAPI.Controllers;
using CommandAPI.Profiles;
using CommandAPI.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile profile;
        MapperConfiguration configuration;
        IMapper mapper;
        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            profile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockRepo = null;
            mapper = null;
            configuration = null;
            profile = null;
        }

        private List<Command> GetCommands(int num)
        {
            var cmds = new List<Command>();
            if (num > 0)
            {
                cmds.Add(new Command { Id = 0, HowTo = "How to generate a migration", CLI = "dotnet ef migrations add <Name of Migration>", Platform = ".Net Core EF" });
            }
            return cmds;
        }

        [Fact]
        public void GetCommandItems_Returns200Ok_WhenDbIsEmpty()
        {
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));
            //Object returns an instance of the mocked class
            var controller = new CommandsController(mockRepo.Object, mapper);
            var res = controller.GetAllCommands();

            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void GetCommandItems_ReturnsOneCMD_And200Ok_WhenDbHasOneItem()
        {

            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            //Object returns an instance of the mocked class
            var controller = new CommandsController(mockRepo.Object, mapper);

            var res = controller.GetAllCommands();

            var okResult = res.Result as OkObjectResult;
            var cmds = okResult.Value as List<CommandReadDto>;
            Assert.Single(cmds);
            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void GetCommandItems_ReturnsCorrectObjType_WhenDbHasOneItem()
        {

            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            //Object returns an instance of the mocked class
            var controller = new CommandsController(mockRepo.Object, mapper);

            var res = controller.GetAllCommands();

            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(res);
        }

        [Fact]
        public void GetCommandById_Return404_WhenDbIsEmpty()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetCommandById(0);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCommandById_Return404_WhenIdIsInvalid()
        {
            mockRepo.Setup(repo => repo.GetCommandById(12)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetCommandById(12);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCommandById_ReturnOk200_WhenIdIsValid()
        {
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(GetCommands(1)[0]);
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetCommandById(1);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommandById_ReturnCorrectObjectType_WhenIdIsValid()
        {
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(GetCommands(1)[0]);
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetCommandById(1);

            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        [Fact]
        public void CreateCommand_ReturnsCorrectObjectType_WhenValidOjbectGiven()
        {
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(GetCommands(1)[0]);
            var controller = new CommandsController(mockRepo.Object, mapper);
            var result = controller.CreateCommand(new CommandCreateDto { });
            Assert.IsType<CreatedAtRouteResult>(result);
        }
    }
}