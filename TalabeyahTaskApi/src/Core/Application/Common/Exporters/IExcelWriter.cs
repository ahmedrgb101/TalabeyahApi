namespace TalabeyahTaskApi.Application.Common.Exporters;

public interface IExcelWriter : ITransientService
{
    Stream WriteToStream<T>(IList<T> data);
    Stream WriteToStreamWithImage<T>(IList<T> data);
}