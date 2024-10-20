using Shoppe.Application.Abstractions.Repositories.UserProfilePictureFileRepos;
using Shoppe.Domain.Entities.Files;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.UserProfilePictureFileRepos
{
    public class UserProfilePictureFileWriteRepository : WriteRepository<UserProfilePictureFile>, IUserProfilePictureFileWriteRepository
    {
        public UserProfilePictureFileWriteRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
