using System.Runtime.Serialization;

namespace Anjir.Zuhriddin.Items.DataAccess.Models;
public enum SortField
{
    [EnumMember(Value = "itemid")]
    ItemId,

    [EnumMember(Value = "name")]
    Name,

    [EnumMember(Value = "type")]
    Type,

    [EnumMember(Value = "date")]
    Date,
}
