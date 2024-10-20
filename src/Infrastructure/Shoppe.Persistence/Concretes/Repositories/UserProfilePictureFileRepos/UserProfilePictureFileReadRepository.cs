using Shoppe.Application.Abstractions.Repositories.UserProfilePictureRepos;
using Shoppe.Domain.Entities.Files;
using Shoppe.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Persistence.Concretes.Repositories.UserProfilePictureFileRepos
{
    public class UserProfilePictureFileReadRepository : ReadRepository<UserProfilePictureFile>, IUserProfilePictureFileReadRepository
    {
        public UserProfilePictureFileReadRepository(ShoppeDbContext context) : base(context)
        {
        }
    }
}
