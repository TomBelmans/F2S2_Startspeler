namespace Kassa.Data
{
    public static class DatabaseConnection
    {
        static MySqlConnection? databaseConnection = null;
        public static MySqlConnection getDBConnection()
        {
            if (databaseConnection == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["StartspelercompanionConnection"].ConnectionString;
                databaseConnection = new MySqlConnection(connectionString);
            }
            return databaseConnection;
        }
    }
}
