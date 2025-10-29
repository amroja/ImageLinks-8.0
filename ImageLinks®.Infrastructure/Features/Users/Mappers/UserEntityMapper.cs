using ImageLinks_.Domain.Models;
using System.Data;

namespace ImageLinks_.Infrastructure
{
    public static class UserEntityMapper
    {
        public static User ToUserEntity(this DataRow row)
        {
            ArgumentNullException.ThrowIfNull(row);

            return new User
            {
                UserId = row["USER_ID"] != DBNull.Value ? Convert.ToInt32(row["USER_ID"]) : 0,
                UserName = row["USER_NAME"].ToString() ?? string.Empty,
                UserPass = row["USER_PASS"].ToString() ?? string.Empty
            };
        }

        public static List<User> ToUsersEntities(this DataTable table)
        {
            ArgumentNullException.ThrowIfNull(table);

            return table.AsEnumerable()
                        .Select(r => r.ToUserEntity())
                        .ToList();
        }
    }
}
