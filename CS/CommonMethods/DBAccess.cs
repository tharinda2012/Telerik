using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CS.CommonMethods
{
    public class DBAccess
    {
        Object[] resultArray;
        OleDbConnection conn;
        OleDbDataReader myreader;
        OleDbCommand comm;


        public void Create_DBConnection(String providestring)
        {

            conn = new OleDbConnection(providestring);

        }
                
        
        public void Execute_SQLQuery(String querystring)
        {
            comm = new OleDbCommand();
            comm.CommandText = querystring;
            conn.Open();

            //assigning connection to the command object
            comm.Connection = conn;

            myreader = comm.ExecuteReader();

        }
              
         /// <summary>
        /// This function returns a String array of the result of the 'select statement' executed by 'Execute_SQLQuery' method.Values can be referenced by Return_Data_In_Array()[index] method.
        /// </summary>
        /// <returns></returns>
        public Object[] Return_Data_In_Array()
        {
           while (myreader.Read())
            {
                                
                ///here an object array is created with the size of the number of fields in the row returned. 
                ///by invoking "reader.GetValues" method we can populate the array with the field values in the data row.

                resultArray = new Object[myreader.FieldCount];
                myreader.GetValues(resultArray); 
            }
            //converting all objects (int,decimal,date etc.) in array to 'string' objects and return         
             return resultArray!=null ? Array.ConvertAll(resultArray, x => x.ToString()) :new object[] { "No Data returned from SQL query..." };

        }


        public void Close_Connection()
        {
            conn.Close();
            myreader.Close();

        }

        
       
        
    }
}

