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
    class Item
    {
        public Item()
        {

        }

        //Set and Get properties
        public int Item_ID { get; set; }
        public int Invent_id { get; set; }
        public string Itemsize { get; set; }
        public string Color { get; set; }
        public decimal Curr_price { get; set; }
        public int Qoh { get; set; }

        //Parse CSV line
        public bool ParseCSVline(string aLine)
        {
            try
            {
                string[] fields = aLine.Split(',');
                this.Item_ID = int.Parse(fields[0]);
                this.Invent_id = int.Parse(fields[1]);
                this.Itemsize = fields[2];
                this.Color = fields[3];
                this.Curr_price = decimal.Parse(fields[4]);
                this.Qoh = int.Parse(fields[5]);
                return true;  //if everything parsed, return true
            }
            catch (Exception ex)
            {
                return false;  //if a parse failed, return false
            }
        } //end of barseCSVline 

        //See if an item is in the database.
        //Pass in the database connection as db.
        public bool IsInDatabase(OdbcConnection db)
        {
            String sql = "SELECT * FROM item WHERE item_ID=?";
            OdbcCommand Command = new OdbcCommand(sql, db);

            Command.Parameters.Add("@ID", OdbcType.Int).Value = this.Item_ID;

            //If query returns a row - there is already an item in the table.
            if (Command.ExecuteReader().HasRows)
                return true;
            else
                return false;
        }  //end of IsInDatabase

        //Add a row to the database passed in as db
        public bool AddRow(OdbcConnection db)
        {
            String sql = "INSERT INTO item "
                       + "(item_id, invent_id, itemsize, color, curr_price, qoh) "
                       + "VALUES( ?, ?, ?, ?, ?, ?)";
            OdbcCommand Command = new OdbcCommand(sql, db);
            Command.Parameters.Add("@ID", OdbcType.Int).Value = this.Item_ID;
            Command.Parameters.Add("@INVID", OdbcType.Int).Value = this.Invent_id;
            Command.Parameters.Add("@SZ", OdbcType.VarChar).Value = this.Itemsize.Trim();
            Command.Parameters.Add("@COL", OdbcType.VarChar).Value = this.Color.Trim();
            Command.Parameters.Add("@PR", OdbcType.Double).Value = (double)this.Curr_price;
            Command.Parameters.Add("@QOH", OdbcType.Int).Value = this.Qoh;

            int result = Command.ExecuteNonQuery();  //Returns 1 if successful
            if (result > 0)
                return true;  //Was successful in adding
            else
                return false;  //failed to add
        } //end of AddRow


        

        //Update a row to the database passed in as db
        public bool UpdateRow(OdbcConnection db)
        {
            String sql = "UPDATE item "
                       + "SET itemsize=?, "
                       + "color=?, "
                       + "curr_price=?, "
                       + "qoh=? "
                       + "WHERE item_id=?";
            OdbcCommand Command = new OdbcCommand(sql, db);

            Command.Parameters.Add("@SZ", OdbcType.VarChar).Value = this.Itemsize.Trim();
            Command.Parameters.Add("@COL", OdbcType.VarChar).Value = this.Color.Trim();
            Command.Parameters.Add("@PR", OdbcType.Double).Value = (double)this.Curr_price;
            Command.Parameters.Add("@QOH", OdbcType.Int).Value = this.Qoh;
            Command.Parameters.Add("@ID", OdbcType.Int).Value = this.Item_ID;

            int result = Command.ExecuteNonQuery();  //Returns 1 if successful
            if (result > 0)
                return true;  //Was successful in updating
            else
                return false;  //failed to update
        }

        //Delete a row from the database passed in as db
        public bool DeleteRow(OdbcConnection db)
        {
            String sql = "DELETE FROM item WHERE item_id=?";

            OdbcCommand Command = new OdbcCommand(sql, db);

            Command.Parameters.Add("@ID", OdbcType.Int).Value = this.Item_ID;

            int result = Command.ExecuteNonQuery();  //Returns 1 if successful
            if (result > 0)
                return true;  //Was successful in deleting
            else
                return false;  //failed to delete
        }


        //Used to parse an XML file pointed to by XmlReader
        public bool parseXML(XmlReader f)
        {
            try
            {
                this.Item_ID = int.Parse(f.GetAttribute("item_id"));
                this.Invent_id = int.Parse(f.GetAttribute("invent_id"));
                this.Itemsize = f.GetAttribute("itemsize");
                this.Color = f.GetAttribute("color");
                this.Curr_price = decimal.Parse(f.GetAttribute("curr_price"));
                this.Qoh = int.Parse(f.GetAttribute("qoh"));
            }
            catch (Exception ex)
            {
                
                return false;
            }
            return true;
        }

       

        //Used to parse an XML file pointed to by XmlReader
        public bool parseXMLForId(XmlReader f)
        {
            try
            {
                this.Item_ID = int.Parse(f.GetAttribute("item_id"));
            }
            catch (Exception ex)
            {
                
                return false;
            }
            return true;
        }
        //Get this item from the XML file and
        //add this item to the database passed in as db
        public bool XMLAdd(XmlReader f, OdbcConnection db)
        {
            if (!this.parseXML(f))  //parse the item from "f"
            {
                Error.AddErrorLog(db, "Could not parse the XML data from the file",this.Item_ID);
                Console.WriteLine(String.Format("Item {0} not ADDED: An input value was an improper datatype", this.Item_ID));
                return false;  //Leave if the parse failed
            }

            if (this.Item_ID < 1) {
                Error.AddErrorLog(db, "Item id was less than 1.",this.Item_ID);
                Console.WriteLine(String.Format("Item {0} not Added: Item id was less than 1.", this.Item_ID));
                return false;
            }
            if (this.Curr_price < 0)
            {
                Error.AddErrorLog(db, "Current price was negative.",this.Item_ID);
                Console.WriteLine(String.Format("Item {0} not Added: Current price was negative.", this.Item_ID));
                return false;
            }
            if (this.Qoh < 0)
            {
                Error.AddErrorLog(db, "Quantity on hand was negative.",this.Item_ID);
                Console.WriteLine(String.Format("Item {0} not Added: Quantity on hand was negative.", this.Item_ID));
                return false;
            }

            //Is it in database?  Check that it is NOT.
                if (!this.IsInDatabase(db))
                {
                    //if not, add it
                    if (this.AddRow(db))
                        return true;
                    else
                    {
                        Error.AddErrorLog(db, "Tried to send an SQL command, but it failed for some reason.",this.Item_ID);
                        Console.WriteLine(String.Format("Item {0} not ADDED: It failed for some reason", this.Item_ID));

                        return false; //if something went wrong
                    }
                }
                else
                {
                    Error.AddErrorLog(db, "Tried to do an ADD, but the item already was in the database.", this.Item_ID);
                    Console.WriteLine(String.Format("{0} already in the database, can't insert.", this.Item_ID));

                    return false;  //already in DB
                }
        }

        //Get this item from the XML file and
        //update this item to the database passed in as db
        public bool XMLUpdate(XmlReader f, OdbcConnection db)
        {
            if (!this.parseXML(f))  //parse the item from "f"
            {
                Error.AddErrorLog(db, "Could not parse the XML data from the file.", this.Item_ID);
                Console.WriteLine(String.Format("Item {0} not UPDATED: Failed to update item", this.Item_ID));

                return false;  //Leave if the parse failed
            }



            if (this.Item_ID < 1)
            {
                Error.AddErrorLog(db, "Item id was less than 1.", this.Item_ID);
                Console.WriteLine(String.Format("Item {0} not Updated: Item id was less than 1.", this.Item_ID));
                return false;
            }
            if (this.Curr_price < 0)
            {
                Error.AddErrorLog(db, "Current price was negative.", this.Item_ID);
                Console.WriteLine(String.Format("Item {0} not Updated: Current price was negative.", this.Item_ID));
                return false;
            }
            if (this.Qoh < 0)
            {
                Error.AddErrorLog(db, "Quantity on hand was negative.", this.Item_ID);
                Console.WriteLine(String.Format("Item {0} not Updated: Quantity on hand was negative.", this.Item_ID));
                return false;
            }
            //Is it in database?  Check that it is NOT.
            if (this.IsInDatabase(db))
            {
                //if not, add it
                if (this.UpdateRow(db))
                    return true;
                else
                {
                    Error.AddErrorLog(db, "Tried to send an SQL command, but it failed for some reason.", this.Item_ID);
                    Console.WriteLine(String.Format("Item {0} not UPDATED: Failed to update item", this.Item_ID));

                    return false; //if something went wrong
                }
            }
            else
            {
                Error.AddErrorLog(db, "Tried to do an ADD, but the item already was in the database.", this.Item_ID);
                Console.WriteLine(String.Format("{0} already in the database, can't insert.", this.Item_ID));

                return false;  //already in DB
            }
        }

        //Get this item from the XML file and
        //delete this item from the database passed in as db
        public bool XMLDelete(XmlReader f, OdbcConnection db)
        {
            this.Item_ID = int.Parse(f.GetAttribute("item_id"));

            if (!this.parseXMLForId(f))  //parse the item from "f"
            {
                Error.AddErrorLog(db, "Could not parse the XML data from the file.", this.Item_ID);
                Console.WriteLine(String.Format("Item {0} not deleted: Failed to parse data", this.Item_ID));

                return false;  //Leave if the parse failed
            }
            


            //Is it in database?  Check that it is NOT.
            if (this.IsInDatabase(db))
            {
                //if not, add it
                if (this.DeleteRow(db))
                    return true;
                else
                {
                    Error.AddErrorLog(db, "Tried to send an SQL command, but it failed for some reason.", this.Item_ID);
                    Console.WriteLine(String.Format("Item {0} not Deleted: Failed to delete item", this.Item_ID));

                    return false; //if something went wrong
                }
            }
            else
            {
                Error.AddErrorLog(db, "Tried to do a DELETE, but the item wasn’t there.", this.Item_ID);
                Console.WriteLine(String.Format("{0} not in database, can't delete.", this.Item_ID));

                return false;  //already in DB
            }
        }



    }
}
