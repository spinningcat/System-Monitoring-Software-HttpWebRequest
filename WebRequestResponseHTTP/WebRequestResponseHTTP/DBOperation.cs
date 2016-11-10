using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRequestResponseHTTP
{
    class DBOperation : GenericExceptionClass
    {
        Int32 count = 0;

        public void InsertProcess(Object obj)
        {
            using (SqlConnection sConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                try
                {
                    sConnection.Open();
                }
                catch (Exception e)
                {
                    obj.determineTheExceotion = true;
                    sqlExMsgList.Add(e.GetBaseException().ToString());
                    StackTrace sTrace = new StackTrace();
                    sqlStTraceFrame.Add(sTrace.GetFrame(0).ToString());
                }
                using (SqlCommand sCommand1 = new SqlCommand("[dbo].[ADDTO_ResultTable", sConnection))
                {
                    try
                    {
                        sCommand1.CommandTimeout = 2000;
                        sCommand1.CommandType = CommandType.StoredProcedure;
                        sCommand1.Parameters.AddWithValue("@UniqueString", obj.uniqueString);
                        sCommand1.Parameters.AddWithValue("@ThreadId", obj.threadId);
                        sCommand1.Parameters.AddWithValue("@ThreadName", obj.threadName);
                        sCommand1.Parameters.AddWithValue("@URLAddress", obj.urlAddress);
                        sCommand1.Parameters.AddWithValue("@StatusCode", obj.statusCode);
                        sCommand1.Parameters.AddWithValue("@DateTimeofWebRequest", obj.requestTime);
                        sCommand1.Parameters.AddWithValue("@DateTimeofWebResponse", obj.responseTime);
                        sCommand1.Parameters.AddWithValue("@TimeDifference", obj.theDifference);
                        sCommand1.Parameters.AddWithValue("@ConditionofWebRequest", obj.responseExist);
                        sCommand1.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        obj.determineTheExceotion = true;
                        sqlExMsgList.Add(e.GetBaseException().ToString());
                        StackTrace sTrace = new StackTrace();
                        sqlStTraceFrame.Add(sTrace.GetFrame(0).ToString());
                    }

                }
                using (SqlCommand sCommand2 = new SqlCommand("[dbo].[ADDTO_ExceptionTable", sConnection))
                {
                    try
                    {
                        sCommand2.CommandTimeout = 2000;
                        sCommand2.CommandType = CommandType.StoredProcedure;
                        foreach (var item in exMsgList)
                        {
                            sCommand2.Parameters.Clear();
                            sCommand2.Parameters.AddWithValue("@UniqueString", obj.uniqueString);
                            sCommand2.Parameters.AddWithValue("@ExceptionMEssage", item);
                            sCommand2.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e)
                    {
                        obj.determineTheExceotion = true;
                        sqlExMsgList.Add(e.GetBaseException().ToString());
                        StackTrace sTrace = new StackTrace();
                        sqlStTraceFrame.Add(sTrace.GetFrame(0).ToString());
                    }
                }
                using (SqlCommand sCommand3 = new SqlCommand("[dbo].[ADDTO_StackTraceTable", sConnection))
                {
                    try
                    {
                        sCommand3.CommandTimeout = 2000;
                        sCommand3.CommandType = CommandType.StoredProcedure;
                        foreach (var item in exMsgList)
                        {
                            sCommand3.Parameters.Clear();
                            sCommand3.Parameters.AddWithValue("@UniqueString", obj.uniqueString);
                            sCommand3.Parameters.AddWithValue("@StackTrace", item);
                            sCommand3.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e)
                    {
                        obj.determineTheExceotion = true;
                        sqlExMsgList.Add(e.GetBaseException().ToString());
                        StackTrace sTrace = new StackTrace();
                        sqlStTraceFrame.Add(sTrace.GetFrame(0).ToString());
                    }
                }
                try
                {
                    /* Possible exception of .concat(...) */
                    /* Argument null exception */
                    exMsgList.Concat(sqlExMsgList);
                    stTraceFramee.Concat(sqlStTraceFrame);
                }
                catch(Exception e)
                {
                    obj.determineTheExceotion = true;
                    exMsgList.Add(e.GetBaseException().ToString());
                    StackTrace sTrace = new StackTrace();
                    stTraceFramee.Add(sTrace.GetFrame(0).ToString());
                }

            }
        }
        public DataTable GetDefinition()
        {
            DataTable dTable = new DataTable();
            using (SqlConnection sConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                try
                {
                    sConnection.Open();
                }
                catch (Exception e)
                {
                    sqlExMsgList.Add(e.GetBaseException().ToString());
                    StackTrace sTrace = new StackTrace();
                    sqlStTraceFrame.Add(sTrace.GetFrame(0).ToString());
                }
                using (SqlCommand sCommand = new SqlCommand("[dbo].[GETFROM_definitiontable]", sConnection))
                {
                    using (SqlDataAdapter sDataAdapter = new SqlDataAdapter(sCommand))
                    {
                        try
                        {
                            /* Possible exception .Fill(...) cause */
                            /* Ivalid operation exception */
                            sDataAdapter.Fill(dTable);
                        }
                        catch (Exception e)
                        {
                            sqlExMsgList.Add(e.GetBaseException().ToString());
                            StackTrace sTrace = new StackTrace();
                            sqlStTraceFrame.Add(sTrace.GetFrame(0).ToString());
                        }
                    }
                }
            }

            return dTable;
        }

        public int RowCount(Object obj)
        {
            /* Each thread has its own URLAddress. It is important to check URLAddress is stored in definition table or 
             * not. i am doing that by getting count information. It must be 1. Otherwise i need to stop the thread. */
            using (SqlConnection sConnection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                try
                {
                    sConnection.Open();
                }
                catch(Exception e)
                {
                    sqlExMsgList.Add(e.GetBaseException().ToString());
                    StackTrace sTrace = new StackTrace();
                    sqlStTraceFrame.Add(sTrace.GetFrame(0).ToString());
                }
                using (SqlCommand sCommand = new SqlCommand("[dbo].[COUNT_definitiontable]", sConnection))
                {
                    try
                    {
                        sCommand.CommandType = CommandType.StoredProcedure;
                    }
                    catch(Exception e)
                    {
                        exMsgList.Add(e.GetBaseException().ToString());
                        StackTrace sTrace = new StackTrace();
                        stTraceFramee.Add(sTrace.GetFrame(0).ToString());
                    }
                    sCommand.Parameters.AddWithValue("@URLAddress", obj.urlAddress);
                    try
                    {
                        count = (Int32)sCommand.ExecuteScalar();
                    }
                    catch(Exception e)
                    {
                        exMsgList.Add(e.GetBaseException().ToString());
                        StackTrace sTrace = new StackTrace();
                        stTraceFramee.Add(sTrace.GetFrame(0).ToString());
                    }
                }
            }
            return count;
        }
        public bool Warning(Object obj)
        {
            /* determineTheException is propert of Object class which is false in default. It becomes true 
             * if any sql's failure take place. */
             /* There is file operation class, this class provide a method for writing data to a file. Data will be 
              * stored in DB of in File according to value of determinetheexception.*/
            if(obj.determineTheExceotion == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        

    }
}

