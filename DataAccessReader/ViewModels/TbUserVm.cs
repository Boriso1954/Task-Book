﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DataAccessReader.ViewModels
{
    public sealed class TbUserVm
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? ProjectId { get; set; }
        public string ProjectTitle { get; set; }
    }
}
