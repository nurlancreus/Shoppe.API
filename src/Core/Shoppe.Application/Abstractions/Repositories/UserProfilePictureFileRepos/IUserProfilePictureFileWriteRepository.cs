using Shoppe.Domain.Entities.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Application.Abstractions.Repositories.UserProfilePictureFileRepos
{
    public interface IUserProfilePictureFileWriteRepository : IWriteRepository<UserProfileImageFile>
    {
    }
}
