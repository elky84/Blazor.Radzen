using Dapper;
using MinimalApi.Domain.Account.Dao;
using System.Data;
using System.Text.Json;

namespace MinimalApi.Common.Dapper;

public class DetailTypeHandler : SqlMapper.TypeHandler<DetailDao>
{
    public override DetailDao? Parse(object value)
    {
        return JsonSerializer.Deserialize<DetailDao>((string)value);
    }

    public override void SetValue(IDbDataParameter parameter, DetailDao? value)
    {
        if (value is null)
        {
            parameter.Value = DBNull.Value;
        }
        else
        {
            parameter.Value = JsonSerializer.Serialize(value);
        }
    }
}