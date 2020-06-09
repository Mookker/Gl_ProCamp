using System.Threading.Tasks;

namespace CommonLibrary.Cqrs
{
    public interface IQuery<in TParameters, TResult>
    {
        Task<TResult> ExecuteAsync(TParameters parameters);
    }
    public interface IQuery<TResult>
    {
        Task<TResult> ExecuteAsync();
    }
}