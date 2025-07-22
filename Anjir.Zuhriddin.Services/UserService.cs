using Anjir.Core;
using Anjir.Zuhriddin.Items.DataAccess;
using Anjir.Zuhriddin.Items.DataAccess.Models;
using Anjir.Zuhriddin.Items.Services.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anjir.Zuhriddin.Items.Services
{
    public class UserService : IUserService
    {
        private readonly MainContext _context;
        public UserService(MainContext context)
        {
            _context = context;
        }

        public async Task<UserResultViewModel> LoginAsync(LoginUserViewModel model)
        {
            string passwordHash = HeshService.HeshSha256(model.Password);
            User user = await _context.Users.FirstOrDefaultAsync(
                x => x.Email == model.Email && 
                     x.PasswordHash == passwordHash);
            if (user == null)
                throw new Exception("user not fount");
            //Console.WriteLine(model.Email + " ------ " + model.Password);
            var result = new UserResultViewModel()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,   
                UserId = user.UserId
            };
            return result;
        }

        public async Task<UserResultViewModel> RegistrationAsync(RegistrationUserViewModel model)
        {
            string passwordHash = HeshService.HeshSha256(model.Password);
            User user = new User()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = passwordHash                
            };
           // Console.WriteLine(model.Email + " ------ " + model.Password);
            var res = await _context.Users.AddAsync(user);
            _context.SaveChanges();
            user = res.Entity;

            var result = new UserResultViewModel()
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return result;
        }
    }
}
