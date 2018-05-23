
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * SAI430 Week 7 Lab
 * Nathan Stawhand
 * Reports Class 
 */
namespace SAI430_Wk7_NStrawhand
{
    public static class Reports
    {
        public static void AllInventory(OdbcConnection db, string filepath)
        {
            //Get all data
            string theQuery = "SELECT * "
                            + "FROM item, inventory "
                            + "WHERE item.invent_id = inventory.invent_id";

            OdbcDataAdapter DataAdapter = new OdbcDataAdapter(theQuery, db);
            DataSet theData = new DataSet();
            DataAdapter.Fill(theData);
            DataTable theTable = theData.Tables[0];

            //Set the report filename
            string filename = @"AllItemsTest.html";

            //Check to see if directory exists, if not create it.
            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);
            //Open file for output
            TextWriter webPage = new StreamWriter(filepath + filename, false);

            //Page header
            webPage.WriteLine("<html>");
            webPage.WriteLine("<head>");
            webPage.WriteLine("<link rel='stylesheet' type='text/css' href='Stylesheet.css'>");
            webPage.WriteLine("<title>Inventory Report</title");
            webPage.WriteLine("</head>");

            //Page body
            webPage.WriteLine("<body>");

            //Start an output table
            webPage.WriteLine("<h1>Inventory Listing</h1>");
            webPage.WriteLine("<table border=1>");
            //header row of table
            webPage.WriteLine("<tr>"); 
            webPage.WriteLine("<th>ITEM ID</th>");
            webPage.WriteLine("<th>DESCRIPTION</th>");
            webPage.WriteLine("<th>QUANT</th>");
            webPage.WriteLine("<th>SIZE</th>");
            webPage.WriteLine("<th>COLOR</th>");
            webPage.WriteLine("<th>PRICE</th>");
            //end header row
            webPage.WriteLine("</tr>"); 

            //Loop through all data results
            foreach (DataRow dataRow in theTable.Rows)
            {
                webPage.WriteLine("<tr>");
                webPage.WriteLine("<td>{0}</td>", dataRow["item_ID"].ToString());
                webPage.WriteLine("<td>{0}</td>", dataRow["invent_desc"].ToString());
                webPage.WriteLine("<td>{0}</td>", dataRow["qoh"].ToString());
                webPage.WriteLine("<td>{0}</td>", dataRow["itemsize"].ToString());
                webPage.WriteLine("<td>{0}</td>", dataRow["color"].ToString());

                //Use the "C" specifier to format currency.
                webPage.WriteLine("<td>{0}</td>",
            ((decimal)dataRow["curr_price"]).ToString("C"));
                webPage.WriteLine("</tr>");
            }
            //end table
            webPage.WriteLine("</table>");  
            webPage.WriteLine("</body>");
            webPage.WriteLine("</html>");
            //Make sure all characters are in the file.
            webPage.Flush();
            //Closes the file and officially writes it to disk.
            webPage.Close();
        }

        public static void ErrorLog(OdbcConnection db, string filepath)
        {
            //Get all data
            string theQuery = "SELECT * "
                            + "FROM errorlog ";

            OdbcDataAdapter DataAdapter = new OdbcDataAdapter(theQuery, db);
            DataSet theData = new DataSet();
            DataAdapter.Fill(theData);
            DataTable theTable = theData.Tables[0];

            //Set the report filename
            string filename = @"ErrorLogTest.html";

            //Check to see if directory exists, if not create it.
            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);
            //Open file for output
            TextWriter webPage = new StreamWriter(filepath + filename, false);

            //Page header
            webPage.WriteLine("<html>");
            webPage.WriteLine("<head>");
            webPage.WriteLine("<link rel='stylesheet' type='text/css' href='Stylesheet.css'>");
            webPage.WriteLine("<title>Error Log</title");
            webPage.WriteLine("</head>");

            //Page body
            webPage.WriteLine("<body>");

            //Start an output table
            webPage.WriteLine("<h1>Error Log</h1>");
            webPage.WriteLine("<table border=1>");
            //header row of table
            webPage.WriteLine("<tr>"); 
            webPage.WriteLine("<th>ERROR ID</th>");
            webPage.WriteLine("<th>ITEM ID</th>");
            webPage.WriteLine("<th>DATE/TIME</th>");
            webPage.WriteLine("<th>DESCRIPTION</th>");
            //end header row
            webPage.WriteLine("</tr>"); 

            //Loop through all data results
            foreach (DataRow dataRow in theTable.Rows)
            {
                webPage.WriteLine("<tr>");
                webPage.WriteLine("<td>{0}</td>", dataRow["error_id"].ToString());
                webPage.WriteLine("<td>{0}</td>", dataRow["item_id"].ToString());
                webPage.WriteLine("<td>{0}</td>", dataRow["errorTime"].ToString());
                webPage.WriteLine("<td>{0}</td>", dataRow["errorMsg"].ToString());
                webPage.WriteLine("</tr>");
            }
            //end table
            webPage.WriteLine("</table>");
            webPage.WriteLine("</body>");
            webPage.WriteLine("</html>");
            //Make sure all characters are in the file.
            webPage.Flush();
            //Closes the file and officially writes it to disk.
            webPage.Close();
        }

        public static void Reorder(OdbcConnection db, string filepath)
        {
            //Get all data
            string theQuery = "SELECT * "
                            + "FROM item, inventory "
                            + "WHERE item.invent_id = inventory.invent_id and qoh <= 100";

            OdbcDataAdapter DataAdapter = new OdbcDataAdapter(theQuery, db);
            DataSet theData = new DataSet();
            DataAdapter.Fill(theData);
            DataTable theTable = theData.Tables[0];

            //Set the report filename
            string filename = @"ReOrderTest.html";

            //Check to see if directory exists, if not create it.
            if (!Directory.Exists(filepath))
                Directory.CreateDirectory(filepath);
            //Open file for output
            TextWriter webPage = new StreamWriter(filepath + filename, false);

            //Page header
            webPage.WriteLine("<html>");
            webPage.WriteLine("<head>");
            webPage.WriteLine("<link rel='stylesheet' type='text/css' href='Stylesheet.css'>");
            webPage.WriteLine("<title>Inventory Reorder Report</title");
            webPage.WriteLine("</head>");

            //Page body
            webPage.WriteLine("<body>");

            //Start an output table
            webPage.WriteLine("<h1>Inventory Reorder Report</h1>");
            webPage.WriteLine("<table border=1>");
            //header row of table
            webPage.WriteLine("<tr>"); 
            webPage.WriteLine("<th>ITEM ID</th>");
            webPage.WriteLine("<th>DESCRIPTION</th>");
            webPage.WriteLine("<th>QUANT</th>");
            webPage.WriteLine("<th>SIZE</th>");
            webPage.WriteLine("<th>COLOR</th>");
            webPage.WriteLine("<th>PRICE</th>");
            //end header row
            webPage.WriteLine("</tr>"); 

            //Loop through all data results
            foreach (DataRow dataRow in theTable.Rows)
            {
                webPage.WriteLine("<tr>");
                webPage.WriteLine("<td>{0}</td>", dataRow["item_ID"].ToString());
                webPage.WriteLine("<td>{0}</td>", dataRow["invent_desc"].ToString());
                webPage.WriteLine("<td>{0}</td>", dataRow["qoh"].ToString());
                webPage.WriteLine("<td>{0}</td>", dataRow["itemsize"].ToString());
                webPage.WriteLine("<td>{0}</td>", dataRow["color"].ToString());

                //Use the "C" specifier to format price to currency.
                webPage.WriteLine("<td>{0}</td>",
            ((decimal)dataRow["curr_price"]).ToString("C"));
                webPage.WriteLine("</tr>");
            }
            //end table
            webPage.WriteLine("</table>");  
            webPage.WriteLine("</body>");
            webPage.WriteLine("</html>");

            //Make sure all characters are in the file.
            webPage.Flush();
            //Closes the file and officially writes it to disk.
            webPage.Close();
        }

       
    }
}
