using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace QuanLiKhoHang.Extensions
{
    /// <summary>
    /// Extension methods cho permission checking
    /// </summary>
    public static class PermissionExtensions
    {
        /// <summary>
        /// Kiểm tra xem user có permission cụ thể không
        /// </summary>
        public static bool HasPermission(this ClaimsPrincipal user, string permissionName)
        {
            if (user?.Identity?.IsAuthenticated != true)
                return false;

            return user.HasClaim(c => c.Type == "Permission" && c.Value == permissionName);
        }

        /// <summary>
        /// Kiểm tra xem user có bất kỳ permission nào trong danh sách
        /// </summary>
        public static bool HasAnyPermission(this ClaimsPrincipal user, params string[] permissionNames)
        {
            return permissionNames.Any(permission => user.HasPermission(permission));
        }

        /// <summary>
        /// Kiểm tra xem user có tất cả các permission
        /// </summary>
        public static bool HasAllPermissions(this ClaimsPrincipal user, params string[] permissionNames)
        {
            return permissionNames.All(permission => user.HasPermission(permission));
        }
    }
}
