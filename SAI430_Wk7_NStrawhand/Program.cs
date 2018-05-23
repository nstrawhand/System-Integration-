using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
/*
 * SAI430 Week 7 Lab
 * Nathan Stawhand
 * Reports Class 
 */
namespace SAI430_Wk7_NStrawhand
{
    class Program
    {
        static void Main(string[] args)
        {
            //Connect to database
            //Connection string needed to talk to MySQL on local machine
            string conString = "Driver={MySQL ODBC 5.3 ANSI Driver};"
                 + "Server=localhost;Port=3306;"
                 + "Database=sai430_db;"
                 + "uid=root;pwd=";
            OdbcConnection connection = new OdbcConnection(conString);
            connection.Open();

            //Open input file
            //Set where the file comes from
            string filepath = @"C:\Users\Nathan\Desktop\Pictures\School CLasses and Things\SAI 430\Labs\Lab Files For DB\";
            string filename = @"TestUpdate.xml";
            //Open XML reader – name it “theFile”
            XmlReader theFile = XmlReader.Create(filepath + filename);

            //Loop through file and add to database. Read will return FALSE when there are no more lines to read.
            
            while (theFile.Read())
            {
                //Create an object to use each time through the loop
                Item theItem = new Item();

                //Check each node in the XML file to see what it is: ADD, UPDATE, or DELETE
                
                if (theFile.Name.Equals("ADD"))
                {
                    theItem.XMLAdd(theFile, connection);
                }
                else if (theFile.Name.Equals("UPDATE"))
                {
                    theItem.XMLUpdate(theFile, connection);
                }
                else if (theFile.Name.Equals("DELETE"))
                {
                    theItem.XMLDelete(theFile, connection);
                }

            }  //end of while loop


            Reports.AllInventory(connection, filepath);
            Reports.ErrorLog(connection, filepath);
            Reports.Reorder(connection, filepath);

            connection.Close();
            theFile.Close();

            Console.WriteLine("\n\n\nPress ENTER to continue");
            Console.ReadLine();
        }
    }
}
