﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.DTO.Identity
{
    public abstract class UserDTO
    {
        public User User { get; set; }
    }

    public class AddLoginDTO : UserDTO
    {
        public UserLoginInfo UserLoginInfo { get; set; }
    }

    public class PasswordHashDTO : UserDTO
    {
        public string Hash { get; set; }
    }

    public class SetLockoutDTO : UserDTO
    {
        public DateTimeOffset? LockoutTime { get; set; }
    }
}
