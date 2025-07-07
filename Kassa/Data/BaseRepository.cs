namespace Kassa.Data
{
    public abstract class BaseRepository
    {
        protected string ConnectionString { get; }

        public BaseRepository()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["StartspelercompanionConnection"].ConnectionString;
        }
    }
}
