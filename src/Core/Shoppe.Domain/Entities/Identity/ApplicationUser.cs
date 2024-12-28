using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Shoppe.Domain.Entities.Base;
using Shoppe.Domain.Entities.Files;
using Shoppe.Domain.Entities.Reactions;
using Shoppe.Domain.Entities.Replies;
using Shoppe.Domain.Entities.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppe.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser, IBase
    {
        private readonly ILazyLoader _lazyLoader;

        public ApplicationUser()
        {

        }

        public ApplicationUser(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private ICollection<UserProfileImageFile> _profilePictureFiles = [];


        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public ICollection<Blog> Blogs { get; set; } = [];
        public ICollection<Review> Reviews { get; set; } = [];
        public ICollection<Reply> Replies { get; set; } = [];
        public ICollection<Reaction> Reactions { get; set; } = [];
        public ICollection<Basket> Baskets { get; set; } = [];

        public BillingAddress BillingAddress { get; set; }
        public ShippingAddress ShippingAddress { get; set; }

        //public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = [];
        //public virtual ICollection<ApplicationUserLogin> Logins { get; set; } = [];
        //public virtual ICollection<ApplicationUserToken> Tokens { get; set; } = [];
        //public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = [];

        public ICollection<UserProfileImageFile> ProfilePictureFiles
        {
            get => _lazyLoader.Load(this, ref _profilePictureFiles) ?? [];
            set => _profilePictureFiles = value;
        }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeactivatedAt { get; set; }

    }
}
