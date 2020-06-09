using System.Threading.Tasks;

namespace CommonLibrary.Cqrs
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }
    
    public interface ICommand<in TParameters>
    {
        Task ExecuteAsync(TParameters parameters);
    }
    
    public interface ICommand<in TParameters, TResult>
    {
        Task<TResult> ExecuteAsync(TParameters parameters);
    }
}