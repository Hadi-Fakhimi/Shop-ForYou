﻿using Shop.Domain.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserName(this User user)
        {
            if(!string.IsNullOrWhiteSpace(user.FirstName)&& !string.IsNullOrWhiteSpace(user.LastName))
            {
                return $"{user.FirstName} {user.LastName}";
            }

            return user.PhoneNumber;
        }
    }
}
