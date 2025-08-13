namespace user_register.core.Permissions
{
    public class Permission
    {
        public enum Permission_Name
        {
            Stock,
            Catalog,
            Order
        }

        public enum Proses_Name
        {
            Read,
            Create,
            Update,
            Delete
        }
        public static class AllPermission
        {
            public static string GetPermission(Permission_Name permission_name,Proses_Name proses_name)
            {
                return $"Permission.{permission_name.ToString()}.{proses_name.ToString()}";
            }
        }
    }
}
