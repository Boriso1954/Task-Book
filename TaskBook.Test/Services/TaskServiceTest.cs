using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.Reader;
using TaskBook.DataAccessLayer.Repositories.Interfaces;
using TaskBook.DomainModel;
using TaskBook.DomainModel.Mapping;
using TaskBook.DomainModel.ViewModels;
using TaskBook.Services;

namespace TaskBook.Test
{
    [TestFixture]
    public sealed class TaskServiceTest
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;
        private Mock<IReaderRepository> _reader;
        private Mock<ITaskRepository> _repository;

        [SetUp]
        public void Init()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _reader = new Mock<IReaderRepository>();
            _repository = new Mock<ITaskRepository>();
        }

        [Test]
        public void TaskService_GetTasksByUserName_Ok()
        {
            // Arrange
            string userName = "User1";
            var tasks = new List<TaskVm>()
            {
                new TaskVm()
                {
                    TaskId = 1
                },
                new TaskVm()
                {
                    TaskId = 2
                }
            };

            _unitOfWork.SetupGet(x => x.ReaderRepository)
                .Returns(_reader.Object)
                .Verifiable("Must get IUnitOfWork.ReaderRepository");

            _reader.Setup(x => x.GetTasksByUserName(It.IsAny<string>()))
                .Returns(tasks.AsQueryable())
                .Verifiable("Must call IReaderRepository.GetTasksByUserName");

            var service = new TaskService(_unitOfWork.Object, _mapper.Object);

            // Act
            var result = service.GetTasksByUserName(userName);

            // Assert
            _unitOfWork.Verify();
            _reader.Verify();
            Assert.AreEqual(tasks.Count(), result.Count());
            Assert.AreEqual(tasks.ElementAt(0).TaskId, result.ElementAt(0).TaskId);
            Assert.AreEqual(tasks.ElementAt(1).TaskId, result.ElementAt(1).TaskId);
        }

        [Test]
        public void TaskService_GetTasks_AllProjects_Ok()
        {
            // Arrange
            var tasks = new List<TaskVm>()
            {
                new TaskVm()
                {
                    ProjectId = 1
                },
                new TaskVm()
                {
                    ProjectId = 2
                }
            };

            _unitOfWork.SetupGet(x => x.ReaderRepository)
               .Returns(_reader.Object)
               .Verifiable("Must get IUnitOfWork.ReaderRepository");

            _reader.Setup(x => x.GetTasks(null))
                .Returns(tasks.AsQueryable())
                .Verifiable("Must call IReaderRepository.GetTasks(), w/o parameter");

            var service = new TaskService(_unitOfWork.Object, _mapper.Object);

            // Act
            var result = service.GetTasks();

            // Assert
            _unitOfWork.Verify();
            _reader.Verify();
            Assert.AreEqual(tasks.Count(), result.Count());
            Assert.AreEqual(tasks.ElementAt(0).ProjectId, result.ElementAt(0).ProjectId);
            Assert.AreEqual(tasks.ElementAt(1).ProjectId, result.ElementAt(1).ProjectId);
        }

        [Test]
        public void TaskService_GetTasks_ByProjectId_Ok()
        {
            // Arrange
            var tasks = new List<TaskVm>()
            {
                new TaskVm()
                {
                    ProjectId = 1
                },
                new TaskVm()
                {
                    ProjectId = 2
                }
            };

            _unitOfWork.SetupGet(x => x.ReaderRepository)
               .Returns(_reader.Object)
               .Verifiable("Must get IUnitOfWork.ReaderRepository");

            _reader.Setup(x => x.GetTasks(1))
                .Returns(tasks.Where(x => x.ProjectId == 1).AsQueryable())
                .Verifiable("Must call IReaderRepository.GetTasks({projectId})");

            var service = new TaskService(_unitOfWork.Object, _mapper.Object);

            // Act
            var result = service.GetTasks(1);

            // Assert
            _unitOfWork.Verify();
            _reader.Verify();
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(tasks.ElementAt(0).ProjectId, result.ElementAt(0).ProjectId);
        }

        [Test]
        public void TaskService_AddTask_Ok()
        {
            // Arrange
            var taskVm = new TaskVm()
            {
                TaskId = 1,
                CreatedBy = "User1",
                AssignedTo = "User2"
            };

            var task = new TbTask()
            {
                Id = taskVm.TaskId
            };

            var models = new List<TbUserRoleVm>()
            {
                new TbUserRoleVm()
                {
                    UserId = "AAAA-BBBB"
                }
            };

            _mapper.Setup(x => x.Map<TaskVm, TbTask>(It.IsAny<TaskVm>()))
                .Returns(task)
                .Verifiable("Must call IMapper.MapMap<TaskVm, TbTask>({taskVm})");

            _unitOfWork.SetupGet(x => x.ReaderRepository)
              .Returns(_reader.Object)
              .Verifiable("Must get IUnitOfWork.ReaderRepository");

            _reader.Setup(x => x.GetUserByUserName(It.IsAny<string>()))
                .Returns(models.AsQueryable())
                .Verifiable("Must call IReaderRepository.GetUserByUserName({userName})");

            _unitOfWork.SetupGet(x => x.TaskRepository)
              .Returns(_repository.Object)
              .Verifiable("Must get IUnitOfWork.TaskRepository");

            _repository.Setup(x => x.Add(task))
                .Verifiable("Must call ITaskRepository.Add({task})");

            _unitOfWork.Setup(x => x.Commit())
                .Verifiable("Must call IUnitOfWork.Commit()");

            var service = new TaskService(_unitOfWork.Object, _mapper.Object);

            // Act
            service.AddTask(taskVm);

            // Assert
            _mapper.Verify();
            _unitOfWork.Verify();
            _reader.Verify(x => x.GetUserByUserName(It.IsAny<string>()), Times.Exactly(2));
            _repository.Verify();
            Assert.AreEqual(models.ElementAt(0).UserId, task.AssignedToId);
            Assert.AreEqual(models.ElementAt(0).UserId, task.CreatedById);
        }

        [Test]
        public void TaskService_UpdateTask_Ok()
        {
            // Arrange
            long taskId = 1;
            var taskVm = new TaskVm()
            {
                TaskId = 1,
                AssignedTo = "User1"
            };

            var task = new TbTask()
            {
                Id = taskVm.TaskId,
                AssignedToId = "0000-1111"
            };

            var models = new List<TbUserRoleVm>()
            {
                new TbUserRoleVm()
                {
                    UserId = "AAAA-BBBB"
                }
            };

            _unitOfWork.SetupGet(x => x.TaskRepository)
              .Returns(_repository.Object)
              .Verifiable("Must get IUnitOfWork.TaskRepository");

            _unitOfWork.SetupGet(x => x.ReaderRepository)
              .Returns(_reader.Object)
              .Verifiable("Must get IUnitOfWork.ReaderRepository");

            _repository.Setup(x => x.GetById(taskId))
                .Returns(task)
                .Verifiable("Must call ITaskRepository.GetById({taskId})");

            _mapper.Setup(x => x.Map<TaskVm, TbTask>(It.IsAny<TaskVm>(), It.IsAny<TbTask>()))
                .Returns(task)
                .Verifiable("Must call IMapper.MapMap<TaskVm, TbTask>({taskVm}, {tbTask})");

            _reader.Setup(x => x.GetUserByUserName(It.IsAny<string>()))
               .Returns(models.AsQueryable())
               .Verifiable("Must call IReaderRepository.GetUserByUserName({userName})");

            _repository.Setup(x => x.Update(task))
               .Verifiable("Must call ITaskRepository.Update({task})");

            _unitOfWork.Setup(x => x.Commit())
                .Verifiable("Must call IUnitOfWork.Commit()");

            var service = new TaskService(_unitOfWork.Object, _mapper.Object);

            // Act
            service.UpdateTask(taskId, taskVm);

            // Assert
            _mapper.Verify();
            _unitOfWork.Verify();
            _reader.Verify();
            _repository.Verify();
            Assert.AreEqual(models.ElementAt(0).UserId, task.AssignedToId);
        }

        [Test]
        public void TaskService_UpdateTask_Id_Invalid_Exception()
        {
            // Arrange
            long taskId = 2;
            var taskVm = new TaskVm()
            {
                TaskId = 1,
            };

            var service = new TaskService(_unitOfWork.Object, _mapper.Object);

            // Act, Assert
            var ex = Assert.Throws<Exception>(() => service.UpdateTask(taskId, taskVm));
            Assert.NotNull(ex);
            Assert.AreEqual(ex.Message, "Task ID conflict.");
        }

        [Test]
        public void TaskService_TaskNotFound_Exception()
        {
            // Arrange
            long taskId = 1;
            var taskVm = new TaskVm()
            {
                TaskId = 1,
                Title = "Task1"
            };

            TbTask task = null;

            _unitOfWork.SetupGet(x => x.TaskRepository)
              .Returns(_repository.Object)
              .Verifiable("Must get IUnitOfWork.TaskRepository");

            _repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(task)
                .Verifiable("Must call ITaskRepository.GetById({taskId})");

            var service = new TaskService(_unitOfWork.Object, _mapper.Object);

            // Act, Assert
            var ex = Assert.Throws<Exception>(() => service.UpdateTask(taskId, taskVm));
            _unitOfWork.Verify();
            _repository.Verify();
            Assert.NotNull(ex);
            Assert.AreEqual(ex.Message, string.Format("Unable to find task '{0}'.", taskVm.Title));
        }

        [Test]
        public void TaskService_DeleteTask_Ok()
        {
            // Arrange
            long taskId = 1;
            var task = new TbTask()
            {
                Id = taskId,
            };

            _unitOfWork.SetupGet(x => x.TaskRepository)
              .Returns(_repository.Object)
              .Verifiable("Must get IUnitOfWork.TaskRepository");

            _repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(task)
                .Verifiable("Must call ITaskRepository.GetById({taskId})");

            _repository.Setup(x => x.Delete(It.IsAny<TbTask>()))
                .Verifiable("Must call ITaskRepository.Delete({task})");

            _unitOfWork.Setup(x => x.Commit())
               .Verifiable("Must call IUnitOfWork.Commit()");

            var service = new TaskService(_unitOfWork.Object, _mapper.Object);

            // Act
            service.DeleteTask(taskId);

            // Assert
            _unitOfWork.Verify();
            _repository.Verify();
        }

        [Test]
        public void TaskService_DeleteTask_TaskNotFound_Exception()
        {
            // Arrange
            long taskId = 1;
            TbTask task = null;

            _unitOfWork.SetupGet(x => x.TaskRepository)
              .Returns(_repository.Object)
              .Verifiable("Must get IUnitOfWork.TaskRepository");

            _repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns(task)
                .Verifiable("Must call ITaskRepository.GetById({taskId})");

            var service = new TaskService(_unitOfWork.Object, _mapper.Object);

            // Act, Assert
            var ex = Assert.Throws<Exception>(() => service.DeleteTask(taskId));
            _unitOfWork.Verify();
            _repository.Verify();
            Assert.NotNull(ex);
            Assert.AreEqual(ex.Message, string.Format("Unable to find task to be deleted. task ID {0}", taskId));
        }

    }
}
