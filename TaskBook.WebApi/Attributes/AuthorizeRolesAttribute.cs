﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TaskBook.DomainModel;

namespace TaskBook.WebApi.Attributes
{
    /// <summary>
    /// Allows using RoleKey class in authorize attribute
    /// </summary>
    public sealed class AuthorizeRolesAttribute: AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}