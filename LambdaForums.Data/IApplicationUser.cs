﻿using LambdaForums.Data.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace LambdaForums.Data
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();

        Task IncrementRating(string id);
        Task Add(ApplicationUser user);
        Task Deactivate(ApplicationUser user);
        Task SetProfileImage(string id, string uri);
        Task BumpRating(string userId, Type type);
    }
}
