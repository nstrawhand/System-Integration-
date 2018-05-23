using System;
using System.Collections.Generic;
using System.Data.Odbc;
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
    public static class Error
    {
        public static bool AddErrorLog(OdbcConnection db, string ErrorMsg, int itemId)
        {
            String sql = "INSERT INTO errorLog "
                       + "(item_id, errorTime, errorMsg) "
                       + "VALUES( ?, ?, ?)";
            OdbcCommand Command = new OdbcCommand(sql, db);
            Command.Parameters.Add("@ID", OdbcType.Int).Value = itemId;
            Command.Parameters.Add("@ErrorTime", OdbcType.DateTime).Value = DateTime.Now;
            Command.Parameters.Add("@ErrorMsg", OdbcType.VarChar).Value = ErrorMsg;
            //Returns 1 if successful
            int result = Command.ExecuteNonQuery();  
            if (result > 0)
                //Was successful in adding
                return true;  
            else
                //failed to add
                return false;  
        } //end of AddRow
    }
}
