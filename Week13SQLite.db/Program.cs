using Microsoft.Data.Sqlite;

ReadData(CreateConnection());

//InsertCustomer(CreateConnection());

RemoveCustomer(CreateConnection());


static SqliteConnection CreateConnection()
{

    SqliteConnection connection = new SqliteConnection("Data Source=mydb.db");

    try
    {
        connection.Open();
        //Console.WriteLine("DB found.");
    }
    catch
    {
        Console.WriteLine("DB not found.");
    }
    return connection;
}

static void ReadData(SqliteConnection myConnection)
{
    Console.Clear();
    SqliteDataReader reader;
    SqliteCommand command;

    command = myConnection.CreateCommand();

    command.CommandText = "SELECT rowid, * FROM customer";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowId = reader["rowid"].ToString();
        string readerStringFirstName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringDoB = reader.GetString(3);

        Console.WriteLine($"{readerRowId}. Full name {readerStringFirstName} {readerStringLastName}; DoB: {readerStringDoB}");
    }

    myConnection.Close();
}


static void InsertCustomer(SqliteConnection myConnection)
{
    SqliteCommand command;
    string firstName, lastName, DoB;

    Console.WriteLine("Enter first name:");
    firstName = Console.ReadLine();

    Console.WriteLine("Enter last name:");
    lastName = Console.ReadLine();

    Console.WriteLine("Enter date of birth (mm-dd-yyyy):");
    DoB = Console.ReadLine();


    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer (firstName, lastName, dateOfBirth) " +
        $"VALUES ('{firstName}', '{lastName}', '{DoB}')";

    int rowInserted = command.ExecuteNonQuery();
    Console.WriteLine($"Rows inserted: {rowInserted}");

    ReadData(myConnection);
}


static void RemoveCustomer(SqliteConnection myConnection)
{
    SqliteCommand command;

    var pragmaCommand = myConnection.CreateCommand();
    pragmaCommand.CommandText = "PRAGMA foreign_keys = OFF;";
    pragmaCommand.ExecuteNonQuery();
    pragmaCommand.Dispose();

    string IdToDelete;

    Console.WriteLine("Enter an id to delete a customer:");
    IdToDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM customer WHERE rowid = {IdToDelete}";

    int RowRemoved = command.ExecuteNonQuery();

    Console.WriteLine($"{RowRemoved} was removed from the customer table.");

    ReadData(myConnection);


}