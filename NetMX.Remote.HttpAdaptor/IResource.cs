namespace NetMX.Remote.HttpAdaptor
{
    public interface IResource<T>
    {
        T Get(string url);
        void Put(string url, T instance);
        void Post(string url, T instance);
        void Delete(string url);
    }
}