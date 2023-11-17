using Microsoft.AspNetCore.Mvc.Rendering;

namespace AquaFlow.Areas.Admin.Pages.Manage
{
    public static class ManageAdminPages
    {
        public static string ManageOrders => "ManageOrders";
        public static string ManageProducts => "ManageProducts";
        public static string ManageUsers => "ManageUsers";

        public static string OrdersNavClass(ViewContext viewContext) => PageNavClass(viewContext, ManageOrders);
        public static string ProductsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ManageProducts);
        public static string UsersNavClass(ViewContext viewContext) => PageNavClass(viewContext, ManageUsers);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
