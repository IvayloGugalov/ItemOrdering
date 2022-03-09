using System.ComponentModel.DataAnnotations;

namespace Identity.Permissions
{
    public enum Permissions : ushort
    {
        NotSet = 0,

        [Display(Name = "Customer", Description = "Customer with limited permissions (ordering).")]
        Customer = 1000,

        [Display(Name = "Shop Employee", Description = "Employee of the shop and has limited access.")]
        ShopEmployee = 2000,

        [Display(Name = "Shop Admin", Description = "Owner of a shop with full access for it.")]
        ShopOwner = 2100,

        [Display(Name = "Admin", Description = "Admin with some access limitations.")]
        Admin = ushort.MaxValue / 2,

        [Display(Name = "Super Admin", Description = "Overall Admin with no access limitations.")]
        SuperAdmin = ushort.MaxValue,

    }
}
