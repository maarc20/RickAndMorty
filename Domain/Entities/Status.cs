using System.Runtime.Serialization;

namespace PruebaEurofirms.Domain.Entities
{
    public enum Status
    {[
        EnumMember(Value = "Alive")]
        Alive,
        [EnumMember(Value = "Dead")]
        Dead,
        [EnumMember(Value = "unknown")]
        unknown
    }
}