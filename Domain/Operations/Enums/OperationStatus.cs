using System.Runtime.Serialization;

namespace Domain.Operations.Enums;

public enum OperationStatus
{
    Unknown,
    Created,
    In_Progress,
    Success,
    Failed
}