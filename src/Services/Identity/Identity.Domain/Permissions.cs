using System.ComponentModel.DataAnnotations;

namespace Identity.Domain
{
    public enum Permissions : ushort
    {
        NotSet = 0,

        [Display(GroupName = "Customer", Name = "Access to only basic features", Description = "Customer with limited permissions (ordering).")]
        Customer = 1000,

        [Display(GroupName = "Shop Admins", Name = "Can access some shop features", Description = "Employee of the shop and has limited access.")]
        ShopEmployee = 20000,

        [Display(GroupName = "Shop Admins", Name = "Access all shop features", Description = "Owner of a shop with full access for it.")]
        ShopOwner = 21000,

        [Display(GroupName = "Super Admin", Name = "Access All", Description = "Overall Admin with no access limitations.")]
        AccessAll = ushort.MaxValue,

    }
}
