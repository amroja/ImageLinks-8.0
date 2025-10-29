using Dapper;
using ImageLinks_.Application.Common.Models;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Enums;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageLinks_.Application.Common.Helpers
{
    public static class GeneralFunction
    {
        public static string MaskEmail(string email)
        {
            int atIndex = email.IndexOf('@');
            if (atIndex <= 1)
            {
                return $"****{email.AsSpan(atIndex)}";
            }

            return email[0] + "****" + email[atIndex - 1] + email[atIndex..];
        }

        public static string GeneratePagentatedOracleStatment(string sqlStarter, string targetedSql, PageRequest pageRequest, List<FilterFields> filterFieldsList, Type modelType, IGenericRepository _dal = null)
        {
            string orderBySql = "";
            object valueDataType;
            foreach (FilterFields filterFields in filterFieldsList)
            {
                if (pageRequest.Filters != null)
                {
                    foreach (Filters filter in pageRequest.Filters)
                    {
                        string fileterField = "";

                        object Instance = null;
                        if (modelType != typeof(string))
                        {
                            Instance = Activator.CreateInstance((modelType));
                        }

                        if (filter.Field.Trim() != null)
                        {
                            if (filter.Value != null)
                            {
                                var newValue = filter.Value.ToString();

                                // is string a date string ?
                                if (IsDate(newValue))
                                {
                                    var date = ConvertStringToDate(newValue);

                                    if (date != null)
                                    {
                                        var d = (DateTime)date;
                                        DateTime dt = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
                                        var seconds = ConvertDateToSeconds(dt, _dal);
                                        newValue = seconds.ToString();
                                    }

                                }
                                System.Reflection.PropertyInfo p = null;
                                Type t = null;
                                if (modelType != typeof(string))
                                {
                                    p = Instance.GetType().GetProperty(filter.Field.Trim());
                                    if (p != null)
                                    {
                                        t = p.PropertyType;
                                    }
                                }
                                else if (modelType == typeof(string))
                                {
                                    t = typeof(string);
                                }

                                if (t != null && t.Equals(typeof(Boolean)))
                                {
                                    if (Convert.ToBoolean(filter.Value.ToString()) == true)
                                    {
                                        newValue = "1";
                                    }
                                    else
                                    {
                                        newValue = "0";
                                    }
                                }
                                if (filter.JunctionOperator == null)
                                {
                                    filter.JunctionOperator = string.Empty;
                                }
                                if ((filter.JunctionOperator.ToLower() == "and" || filter.JunctionOperator.ToLower() == "or") && !string.IsNullOrEmpty(filter.Field.Trim()))
                                {

                                }
                                else
                                {
                                    filter.JunctionOperator = "AND";
                                }

                                if (filter.Operator == "contains")
                                {
                                    fileterField = "LIKE LOWER('%" + filter.Value + "%')";
                                }
                                else if (filter.Operator == "eq")
                                {
                                    fileterField = "= '" + newValue + "'";
                                }
                                else if (filter.Operator == "gt")
                                {
                                    fileterField = "> '" + newValue + "'";
                                }
                                else if (filter.Operator == "gte")
                                {
                                    fileterField = ">= '" + newValue + "'";
                                }
                                else if (filter.Operator == "lt")
                                {
                                    fileterField = "= '" + newValue + "'";
                                }
                                else if (filter.Operator == "lte")
                                {
                                    fileterField = "<= '" + newValue + "'";
                                }
                                else if (filter.Operator == "neq")
                                {
                                    fileterField = "!= '" + newValue + "'";
                                }
                                if (filter.Field.Trim() == filterFields.FieldName)
                                {
                                    if (!string.IsNullOrEmpty(filter.Value.ToString()))
                                    {
                                        if (filter.Value != "0")
                                        {
                                            if (filter.Operator == "contains")
                                                targetedSql += " " + filter.JunctionOperator + " LOWER(" + filterFields.TableName + "." + filterFields.DbFiledName + ") " + fileterField;
                                            else
                                                targetedSql += " " + filter.JunctionOperator + " " + filterFields.TableName + "." + filterFields.FieldName + " " + fileterField;

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (pageRequest.SortList != null)
                {
                    if (pageRequest.SortList.Count > 0)
                    {
                        foreach (SortList sortList in pageRequest.SortList)
                        {
                            if (sortList.Property != null)
                            {

                                string property = sortList.Property;
                                string direction = sortList.Direction;
                                if (property == filterFields.FieldName)
                                {
                                    orderBySql += "LOWER(RTRIM(LTRIM(" + filterFields.TableName + ".";
                                    orderBySql += filterFields.DbFiledName + ")))  " + direction + "  ";
                                }
                            }
                        }
                    }
                }
            }
            if (orderBySql == string.Empty)
            {
                orderBySql += "  ORDER BY ";
                orderBySql += filterFieldsList[0].TableName + "." + filterFieldsList[0].DbFiledName;
            }
            else
            {
                orderBySql = "  ORDER BY " + orderBySql;
            }
            string sql = string.Format(@"SELECT * FROM ( SELECT a.*, rownum r  
                                          FROM (SELECT {0},(SELECT COUNT(*) {1}) as totalElements {1}{4}) a  WHERE rownum < (({3} * {2}) + 1 )) WHERE r >= ((({3}-1) * {2}) + 1)", sqlStarter, targetedSql, pageRequest.PageSize, pageRequest.PageNumber + 1, orderBySql);
            return sql;
        }

        public static string GeneratePagentatedSqlStatmentNew(string sqlStarter, string targetedSql, PageRequest pageRequest, List<FilterFields> filterFieldsList, Type modelType, IGenericRepository _dal = null)
        {
            string orderBySql = "";
            foreach (FilterFields filterFields in filterFieldsList)
            {
                if (pageRequest.Filters != null)
                {
                    foreach (Filters filter in pageRequest.Filters)
                    {
                        string fileterField = "";

                        object Instance = null;
                        if (modelType != typeof(string))
                        {
                            Instance = Activator.CreateInstance((modelType));
                        }

                        if (filter.Field.Trim() != null)
                        {
                            if (filter.Value != null)
                            {
                                var newValue = filter.Value.ToString();

                                // is string a date string ?
                                if (IsDate(newValue))
                                {
                                    var date = ConvertStringToDate(newValue);

                                    if (date != null)
                                    {
                                        var d = (DateTime)date;


                                            DateTime dt = new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);

                                            var seconds = ConvertDateToSeconds(dt, _dal);

                                            newValue = seconds.ToString();

                                        

                                    }

                                }
                                System.Reflection.PropertyInfo p = null;
                                Type t = null;
                                if (modelType != typeof(string))
                                {
                                    p = Instance.GetType().GetProperty(filter.Field.Trim());
                                    if (p != null)
                                    {
                                        t = p.PropertyType;
                                    }
                                }
                                else if (modelType == typeof(string))
                                {
                                    t = typeof(string);
                                }

                                if (t.Equals(typeof(Boolean)))
                                {
                                    if (Convert.ToBoolean(filter.Value.ToString()) == true)
                                    {
                                        newValue = "1";
                                    }
                                    else
                                    {
                                        newValue = "0";
                                    }
                                }
                                if (filter.JunctionOperator == null)
                                {
                                    filter.JunctionOperator = string.Empty;
                                }
                                if ((filter.JunctionOperator.ToLower() == "and" || filter.JunctionOperator.ToLower() == "or") && !string.IsNullOrEmpty(filter.Field.Trim()))
                                {

                                }
                                else
                                {
                                    filter.JunctionOperator = "AND";
                                }

                                if (filter.Operator == "contains")
                                {
                                    fileterField = "LIKE '%" + filter.Value + "%'";
                                }
                                else if (filter.Operator == "eq")
                                {
                                    fileterField = "= '" + newValue + "'";
                                }
                                else if (filter.Operator == "gt")
                                {
                                    fileterField = "> '" + newValue + "'";
                                }
                                else if (filter.Operator == "gte")
                                {
                                    fileterField = ">= '" + newValue + "'";
                                }
                                else if (filter.Operator == "lt")
                                {
                                    fileterField = "= '" + newValue + "'";
                                }
                                else if (filter.Operator == "lte")
                                {
                                    fileterField = "<= '" + newValue + "'";
                                }
                                else if (filter.Operator == "neq")
                                {
                                    fileterField = "!= '" + newValue + "'";
                                }
                                if (filter.Field.Trim() == filterFields.FieldName)
                                {
                                    if (!string.IsNullOrEmpty(filter.Value.ToString()))
                                    {
                                        if (filter.Value != "0")
                                            targetedSql += filter.JunctionOperator + " " + filterFields.TableName + "." + filterFields.DbFiledName + " " + fileterField;
                                    }
                                }
                            }
                        }
                    }
                }

                if (pageRequest.SortList != null)
                {
                    if (pageRequest.SortList.Count > 0)
                    {
                        foreach (SortList sortList in pageRequest.SortList)
                        {
                            if (sortList.Property != null)
                            {

                                string property = sortList.Property;
                                string direction = sortList.Direction;
                                if (property == filterFields.FieldName)
                                {
                                    orderBySql += "LOWER(RTRIM(LTRIM(" + filterFields.TableName + ".";
                                    orderBySql += filterFields.DbFiledName + ")))  " + direction + "  ";
                                }
                            }
                        }
                    }
                }
            }
            if (orderBySql == string.Empty)
            {
                orderBySql += "  ORDER BY ";
                orderBySql += filterFieldsList[0].TableName + "." + filterFieldsList[0].DbFiledName;
            }
            else
            {
                orderBySql = "  ORDER BY " + orderBySql;
            }

            string sql = string.Format(@"DECLARE @PageNumber AS INT 
                                  DECLARE @RowsOfPage AS INT
                                  DECLARE @MaxTablePage  AS FLOAT
                                  DECLARE @totalElements  AS FLOAT
                                  SET @PageNumber = {0}
                                  SET @RowsOfPage = {1}
                                  SELECT @MaxTablePage = COUNT(*) {2} 
                                  SET @totalElements = @MaxTablePage
                                  SET @MaxTablePage = CEILING(@MaxTablePage / @RowsOfPage)
                                  WHILE @PageNumber = {0}
                                  BEGIN
                               ", pageRequest.PageNumber, pageRequest.PageSize, targetedSql);
            sql += sqlStarter + " " + targetedSql + " " + orderBySql;
            sql += @" OFFSET(@PageNumber) * @RowsOfPage ROWS
                                  FETCH NEXT @RowsOfPage ROWS ONLY
                                  SET @PageNumber = @PageNumber + 1
                                  END";
            return sql;
        }

        public static DateTime? ConvertStringToDate(string dateString)
        {
            try
            {
                var date = DateTime.ParseExact(dateString, new[] { "dd/MM/yyyy", "dd-MM-yyyy", "d/MM/yyyy", "d/M/yyyy" }, CultureInfo.InvariantCulture);
                return date;

            }
            catch (Exception)
            {
                return null;
            }

        }
        public static bool IsDate(string date)
        {
            string pattern = @"^([0]?[0-9]|[12][0-9]|[3][01])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$";

            Regex rg = new Regex(pattern);

            return rg.IsMatch(date);
        }

        public static long ConvertDateToSeconds(DateTime objDate, IGenericRepository dal)
        {
            return default;
        }

        public static string GetParam(string name, DatabaseProvider provider)
        {
            return provider == DatabaseProvider.SqlServer ? $"@{name}" : $":{name}";
        }

        public static (string WhereClause, DynamicParameters Parameters) BuildWhereClause(
            IEnumerable<Filters>? filters,
            List<FilterFields> filterFieldsList,
            Type modelType,
            DatabaseProvider provider,
            IGenericRepository? dal = null)
        {
            var parameters = new DynamicParameters();
            if (filters == null || !filters.Any())
                return (string.Empty, parameters);

            var whereBuilder = new StringBuilder(" WHERE 1=1 ");

            foreach (var filter in filters)
            {
                if (string.IsNullOrWhiteSpace(filter.Field) || filter.Value == null)
                    continue;

                var fieldMeta = filterFieldsList.FirstOrDefault(f => f.FieldName == filter.Field.Trim());
                if (fieldMeta == null) continue;

                string junction = string.IsNullOrEmpty(filter.JunctionOperator) ? "AND" : filter.JunctionOperator.ToUpper();
                string paramName = $"{filter.Field}_{Guid.NewGuid().ToString("N").Substring(0, 5)}";
                string paramRef = GetParam(paramName, provider);

                object? newValue = filter.Value;

                if (IsDate(filter.Value.ToString()))
                {
                    var date = ConvertStringToDate(filter.Value.ToString());
                    if (date != null)
                    {
                        DateTime dt = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0);
                        newValue = ConvertDateToSeconds(dt, dal);
                    }
                }
                else if (modelType != typeof(string))
                {
                    var prop = modelType.GetProperty(filter.Field.Trim());
                    if (prop != null && prop.PropertyType == typeof(bool))
                    {
                        newValue = Convert.ToBoolean(filter.Value) ? 1 : 0;
                    }
                }

                string op = filter.Operator?.ToLower() ?? "eq";
                string sqlOp = op switch
                {
                    "contains" => $"LIKE {paramRef}",
                    "eq" => $"= {paramRef}",
                    "gt" => $"> {paramRef}",
                    "gte" => $">= {paramRef}",
                    "lt" => $"< {paramRef}",
                    "lte" => $"<= {paramRef}",
                    "neq" => $"!= {paramRef}",
                    _ => $"= {paramRef}"
                };

                whereBuilder.Append($" {junction} {fieldMeta.TableName}.{fieldMeta.DbFiledName} {sqlOp}");

                parameters.Add(paramName,
                    op == "contains" ? $"%{newValue}%" : newValue);
            }

            return (whereBuilder.ToString(), parameters);
        }

        public static string BuildOrderByClause(List<SortList>? sortList, List<FilterFields> filterFieldsList)
        {
            if (sortList == null || sortList.Count == 0)
            {
                var defaultField = filterFieldsList.First();
                return $" ORDER BY {defaultField.TableName}.{defaultField.DbFiledName}";
            }

            var orderBuilder = new StringBuilder(" ORDER BY ");
            foreach (var sort in sortList)
            {
                var fieldMeta = filterFieldsList.FirstOrDefault(f => f.FieldName == sort.Property);
                if (fieldMeta == null) continue;

                orderBuilder.Append($" {fieldMeta.TableName}.{fieldMeta.DbFiledName} {sort.Direction},");
            }

            return orderBuilder.ToString().TrimEnd(',');
        }

        public static (string Sql, string CountSql, DynamicParameters Parameters) BuildPagedSql(
            string sqlStarter,
            string targetedSql,
            PageRequest pageRequest,
            List<FilterFields> filterFieldsList,
            Type modelType,
            DatabaseProvider provider,
            IGenericRepository? dal = null)
        {
            var (whereClause, parameters) = BuildWhereClause(pageRequest.Filters, filterFieldsList, modelType, provider, dal);
            string orderByClause = BuildOrderByClause(pageRequest.SortList, filterFieldsList);

            
            string baseSql = $"{sqlStarter} {targetedSql} {whereClause} {orderByClause}";
            string countSql = $"SELECT COUNT(1) FROM ({baseSql}) AS CountTable";
            parameters.Add("Page", pageRequest.PageNumber);
            parameters.Add("PageSize", pageRequest.PageSize);

            string sql = provider switch
            {
                DatabaseProvider.SqlServer =>
                    $@"{baseSql}
                   OFFSET (@Page - 1) * @PageSize ROWS
                   FETCH NEXT @PageSize ROWS ONLY;",

                DatabaseProvider.Oracle =>
                    $@"{baseSql}
                   OFFSET (:Page - 1) * :PageSize ROWS
                   FETCH NEXT :PageSize ROWS ONLY",

                _ => throw new NotSupportedException("Database provider not supported")
            };

            return (sql, countSql, parameters);
        }
    }
}
