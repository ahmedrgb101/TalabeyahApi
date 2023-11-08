using System.ComponentModel;

namespace TalabeyahTaskApi.Domain.Common;
public enum FileType
{
    [Description(".jpg,.png,.jpeg")]
    Image,
    [Description(".pdf,.doc,.docx,.xlsx,.xlx")]
    File
}