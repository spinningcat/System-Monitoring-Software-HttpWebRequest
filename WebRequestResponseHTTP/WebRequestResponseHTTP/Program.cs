using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebRequestResponseHTTP
{
    class Program : GenericExceptionClass
    {
        static List<Thread> IThreadList = new List<Thread>();
        static DBOperation DBOpt = new DBOperation();
        static DataTable dTable = new DataTable();

        static void Main(string[] args)
        {
            dTable = DBOpt.GetDefinition();
            /* This app. will work in 7/24 basis but sometimes, some kind of problem can be seen.
             * For example thread can die or definitiontable can be empty totally in some way.
             * If any of this secenerio takes place, we just inform use about this accident 
             * and kindly asking to fix that problem before app is terminated. */
            if(dTable.Rows.Count == 0)
            {
                for (int counter = 0; counter < 10; counter++)
                {
                    if(counter == 10)
                    {
                        break;
                    }
                    try
                    {
                        /* possible exception Sleep(...) cause */
                        /* Argument out of range exception  */
                        Thread.Sleep(1000);
                    }
                    catch(Exception e)
                    {
                        Exception ex = new Exception();
                        StackTrace st = new StackTrace();
                        exMsgList.Add(ex.GetBaseException().ToString());
                        stTraceFramee.Add(st.GetFrame(0).ToString());
                    }
                }
            }
            else
            {
                foreach (DataRow dRow in dTable.Rows)
                {
                    Object obj = new Object();
                    obj = AsignInitialValues(dRow, obj);
                    ThreadStart tObjStarter = new ThreadStart(obj.LifeCycle);
                    try
                    {
                        Thread tObj = new Thread(tObjStarter);
                        tObj.Name = obj.urlAddress;
                        if(!IThreadList.Contains(tObj))
                        {
                            IThreadList.Add(tObj);
                        }
                    }
                    catch(Exception e)
                    {
                        Exception ex = new Exception();
                        StackTrace st = new StackTrace();
                        exMsgList.Add(ex.GetBaseException().ToString());
                        stTraceFramee.Add(st.GetFrame(0).ToString());
                    }
                    for (int i = 0; i < IThreadList.Count(); i++)
                    {
                        IThreadList[i].Start();
                    }
                    Thread th = new Thread(CheckNewRecords);
                    th.Start();

                }
            }
        }
        public static Object AsignInitialValues(DataRow dRow, Object obj)
        {
            obj.checkDayFormat = false;
            obj.checkHourFormat = false;
            obj.timeSpanChecker = false;
            obj.determineTheExceotion = false;
            obj.listofTime = new List<List<TimeSpan>>();
            obj.listOfDays = new List<int>();
            obj.urlAddress = dRow["URLAddress"].ToString();
            /* if web request cannot be completed before exceeding that limit, webrequest wont happe. */
            obj.timeoutValue = Convert.ToInt32(dRow["TimeoutValue"]);
            /* SleepTime is a variable that is used in Thread.Sleep(). Each thread has its own sleep time..*/
            obj.sleepTime = Convert.ToInt32(dRow["SleepTime"]);
            /* You can control or perefer to not control the service. It is "bit" value on DB */
            obj.isActive = Convert.ToBoolean(dRow["IsActive"]);
            /* Time range that webrequest will be done */
            obj.hourInformation = dRow["HourInformation"].ToString();
            /* Particular day or days for webrequest */
            obj.daysOfWeekAsNumber = dRow["DaysofWeekAsNumber"].ToString();
            return obj;
        }
        
        public static void CheckNewRecords()
        {
         /* You can insert data to definition table while running. This methods runs with different thread constantly
         * So it checks definitiontable. And when it finds new record that IThreadList doesnt have. First it will take 
         * initialvalues with the help of AsignInitialValue then add new data to IThreadList. And there will be new thread
         * in life cycle. */
            int counter = 0;
            List<string> dataURLList = new List<string>();
            while(true)
            {
                dTable = DBOpt.GetDefinition();
                foreach (Thread th in IThreadList)
                {
                    if(!dataURLList.Contains(th.Name))
                    {
                        dataURLList.Add(th.Name);
                    }
                }
                foreach (DataRow dRow in dTable.Rows)
                {
                    if(!dataURLList.Contains(dRow["URLAddress"].ToString()))
                    {
                        Object obj = new Object(); ;
                        obj = AsignInitialValues(dRow, obj);
                        ThreadStart tObjStarter = new ThreadStart(obj.LifeCycle);
                        try
                        {
                            Thread tObj = new Thread(tObjStarter);
                            tObj.Name = obj.urlAddress;
                            tObj.Start();
                            if(!IThreadList.Contains(tObj))
                            {
                                IThreadList.Add(tObj);
                            }
                        }
                        catch(Exception e)
                        {

                        }
                    }
                }
                {
                    Exception ex = new Exception();
                    StackTrace st = new StackTrace();
                    exMsgList.Add(ex.GetBaseException().ToString());
                    stTraceFramee.Add(st.GetFrame(0).ToString());
                }
            }
        }
    }
}
