using System.Data.OleDb;
using System;


namespace CS.CommonMethods
{
    public class DbAccess
    {
        object[] _resultArray;
        OleDbConnection _conn;
        OleDbDataReader _myreader;
        OleDbCommand _comm;


        public void Create_DBConnection(string providestring)
        {

            _conn = new OleDbConnection(providestring);

        }
                
        
        public void Execute_SQLQuery(string querystring)
        {
            _comm = new OleDbCommand {CommandText = querystring};
            _conn.Open();

            //assigning connection to the command object
            _comm.Connection = _conn;

            _myreader = _comm.ExecuteReader();

        }
              
         /// <summary>
        /// This function returns a String array of the result of the 'select statement' executed by 'Execute_SQLQuery' method.Values can be referenced by Return_Data_In_Array()[index] method.
        /// </summary>
        /// <returns></returns>
        public object[] Return_Data_In_Array()
        {
           while (_myreader.Read())
            {
                                
#pragma warning disable 1587
                ///here an object array is created with the size of the number of fields in the row returned. 
                ///by invoking "reader.GetValues" method we can populate the array with the field values in the data row.
#pragma warning restore 1587

                _resultArray = new object[_myreader.FieldCount];
                _myreader.GetValues(_resultArray); 
            }
            //converting all objects (int,decimal,date etc.) in array to 'string' objects and return         
            return _resultArray != null
                ? Array.ConvertAll(_resultArray, x => x.ToString())
                : new object[] {"No Data returned from SQL query..."};

        }


        public void Close_Connection()
        {
            _conn.Close();
            _myreader.Close();

        }

        
       
        
    }
}

