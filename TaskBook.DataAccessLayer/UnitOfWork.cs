﻿using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Text;
using TaskBook.DataAccessLayer.Exceptions;
using TaskBook.DataAccessLayer.Reader;
using TaskBook.DataAccessLayer.Repositories;
using TaskBook.DataAccessLayer.Repositories.Interfaces;

namespace TaskBook.DataAccessLayer
{
    /// <summary>
    /// Provide business transactions and coordinates the writing out of changes and the resolution of concurrency problems
    /// </summary>
    public sealed class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly TaskBookDbContext _dbContext;
        private IProjectRepository _projectRepository;
        private IProjectUsersRepository _projectUsersRepository;
        private ITaskRepository _taskRepository;
        private IReaderRepository _readerRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">Database context</param>
        public UnitOfWork(TaskBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns the reference to the database context object
        /// </summary>
        public TaskBookDbContext DbContext
        {
            get
            {
                return this._dbContext;
            }
        }

        /// <summary>
        /// Returns the reference to the ProjectRepository object
        /// </summary>
        public IProjectRepository ProjectRepository
        {
            get
            {
                if(_projectRepository == null)
                {
                    _projectRepository = new ProjectRepository(_dbContext);
                }
                return _projectRepository;
            }
        }

        /// <summary>
        /// Returns the reference to the ProjectUsersRepository object
        /// </summary>
        public IProjectUsersRepository ProjectUsersRepository
        {
            get
            {
                if(_projectUsersRepository == null)
                {
                    _projectUsersRepository = new ProjectUsersRepository(_dbContext);
                }
                return _projectUsersRepository;
            }
        }

        /// <summary>
        /// Returns the reference to the TaskRepository object
        /// </summary>
        public ITaskRepository TaskRepository
        {
            get
            {
                if(_taskRepository == null)
                {
                    _taskRepository = new TaskRepository(_dbContext);
                }
                return _taskRepository;
            }
        }

        /// <summary>
        /// Returns the reference to the ReaderRepository object
        /// </summary>
        public IReaderRepository ReaderRepository
        {
            get
            {
                if(_readerRepository == null)
                {
                    _readerRepository = new ReaderRepository(_dbContext);
                }
                return _readerRepository;
            }
        }

        /// <summary>
        /// Commits the unit of work
        /// </summary>
        public void Commit()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new DataAccessLayerException("Concurrency", ex);
            }
            catch(DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach(var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach(var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                string errMessage = sb.ToString();

                throw new DataAccessLayerException("Entity Validation Failed - errors follow:\n" + errMessage, ex);

            }
            catch(DbUpdateException ex)
            {
                throw new DataAccessLayerException("Update", ex);
            }
            catch(Exception ex)
            {
                throw new DataAccessLayerException("Exception", ex);
            }
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }
    }
}
