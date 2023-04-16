namespace OkrsEntreprise.Framework
{
    public interface ICurrentContextProvider<T>
    {
        T GetCurrentUser();
      
    }
     
}
