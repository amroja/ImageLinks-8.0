using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

public class IdxXXXXX
{
    public decimal DocId { get; set; }

    [Column("DOCN_LINK")]
    public decimal? DocnLink { get; set; }

    [NotMapped]
    public Dictionary<string, object?> Fields { get; set; } = new();

    public T? GetField<T>(string fieldName)
    {
        if (!Fields.TryGetValue(fieldName, out var value) || value == null)
            return default;

        try
        {
            if (value is T typedValue)
                return typedValue;

            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch
        {
            return default;
        }
    }

    public void SetField(string fieldName, object? value)
    {
        Fields[fieldName] = value;
    }

    public bool HasField(string fieldName) => Fields.ContainsKey(fieldName);
    public bool RemoveField(string fieldName) => Fields.Remove(fieldName);

    [NotMapped]
    public IEnumerable<string> FieldNames => Fields.Keys;

    [NotMapped]
    public int FieldCount => Fields.Count;
}
