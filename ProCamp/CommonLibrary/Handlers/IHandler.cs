using System.Threading.Tasks;

namespace CommonLibrary.Handlers
{
    public interface IHandler<in TParameters, TResult>
    {
        Task<TResult> ExecuteAsync(TParameters parameters);
    }
    
    public interface IHandler<in TParameters>
    {
        Task ExecuteAsync(TParameters parameters);
    }
}