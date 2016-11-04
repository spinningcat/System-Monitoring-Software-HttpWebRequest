using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WebRequestResponseHTTP
{
    class Object : GenericExceptionClass
    {
        Sequence seq = new Sequence();
        private HttpWebRequest _webRequest;
        public HttpWebRequest webRequest { get; set; }

        private HttpWebResponse _webResponse;
        public HttpWebResponse webResponse { get; set; }

        private string _uniqueString;
        public string uniqueString { get; set; }

        private int _threadId;
        public int threadId { get; set; }

        private string _threadName;
        public string threadName { get; set; }

        private string _urlAddress;
        public string urlAddress { get; set; }

        private int _statusCode;
        public int statusCode { get; set; }

        private DateTime _requestTime;
        public DateTime requestTime { get; set; }

        private DateTime _responseTime;
        public DateTime responseTime { get; set; }

        private TimeSpan _theDifference;
        public TimeSpan theDifference { get; set; }

        private int _responseExist;
        public int responseExist { get; set; }

        private int _timeoutValue;
        public int timeoutValue { get; set; }

        private int _sleepTime;
        public int sleepTime { get; set; }

        private bool _isActive;
        public bool isActive { get; set; }

        private string _hourInformation;
        public string hourInformation { get; set; }

        private bool _checkHourFormat;
        public bool checkHourFormat { get; set; }

        private string _daysOfWeekAsNumber;
        public string daysOfWeekAsNumber { get; set; }

        private bool _checkDayFormat;
        public bool checkDayFormat { get; set; }

        private List<List<TimeSpan>> _listOfTime;
        public List<List<TimeSpan>> listofTime { get; set; }

        private bool _timeSpanChecker;
        public bool timeSpanChecker { get; set; }

        private List<int> _listOfDays;
        public List<int> listOfDays { get; set; }

        private bool _dayChecker;
        public bool dayChecker { get; set; }

        private bool _determineTheException;
        public bool determineTheExceotion { get; set; }

        public void LifeCycle()
        {
            int counter = 0;
            while (true)
            {
                if (DBOpt.RowCountForField((Object)this) != 0)
                {
                    CheckTheChangeInDefinitionTable();
                    if (isActive == true)
                    {
                        if (CheckHourFormat() && CheckDayFormat())
                        {
                            if (CheckTimeSpanwithCurrentTimeSpan() && CompareDatesAndDecideWhatNextActionWillBe())
                            {
                                uniqueString = seq.GetUniqueString();
                                GenerateWebRequest();
                                GetResponse();
                            }
                            else
                            {

                                uniqueString = seq.GetUniqueString();
                                threadId = Thread.CurrentThread.ManagedThreadId;
                                try
                                {
                                    /* Possible Exception that name cause */
                                    /* Invalid Operation Example */
                                    threadName = Thread.CurrentThread.Name;
                                }
                                catch (Exception e)
                                {
                                    Exception ex = new Exception();
                                    StackTrace st = new StackTrace();
                                    /* https://msdn.microsoft.com/en-us/library/system.exception.getbaseexception(v=vs.110).aspx */
                                    exMsgList.Add(ex.GetBaseException().ToString());
                                    /* https://msdn.microsoft.com/en-us/library/system.diagnostics.stacktrace.getframe(v=vs.110).aspx */
                                    stTraceFramee.Add(st.GetFrame(0).ToString());
                                }
                                urlAddress = urlAddress;
                                statusCode = -99;
                                requestTime = DateTime.Now;
                                responseTime = DateTime.Now;
                                theDifference = TimeSpan.Zero;
                                responseExist = -99;
                            }
                        }
                        else if (!CheckHourFormat() || !CheckDayFormat())
                        {
                            uniqueString = seq.GetUniqueString();
                            threadId = Thread.CurrentThread.ManagedThreadId;
                            try
                            {
                                /* Possible Exception that name cause */
                                /* Invalid Operation Example */
                                threadName = Thread.CurrentThread.Name;
                            }
                            catch (Exception e)
                            {
                                Exception ex = new Exception();
                                StackTrace st = new StackTrace();
                                /* https://msdn.microsoft.com/en-us/library/system.exception.getbaseexception(v=vs.110).aspx */
                                exMsgList.Add(ex.GetBaseException().ToString());
                                /* https://msdn.microsoft.com/en-us/library/system.diagnostics.stacktrace.getframe(v=vs.110).aspx */
                                stTraceFramee.Add(st.GetFrame(0).ToString());
                            }
                            urlAddress = urlAddress;
                            statusCode = -0;
                            requestTime = DateTime.Now;
                            responseTime = DateTime.Now;
                            theDifference = TimeSpan.Zero;
                            responseExist = -1;
                            exMsgList.Add("Check the format");
                            stTraceFramee.Add("Format must be correct.");
                        }
                    }
                    else
                    {
                        uniqueString = seq.GetUniqueString();
                        threadId = Thread.CurrentThread.ManagedThreadId;
                        try
                        {
                            threadName = Thread.CurrentThread.Name;
                        }
                        catch (Exception e)
                        {
                            Exception ex = new Exception();
                            StackTrace st = new StackTrace();
                            exMsgList.Add(ex.GetBaseException().ToString());
                            stTraceFramee.Add(st.GetFrame(0).ToString());
                        }
                        urlAddress = urlAddress;
                        statusCode = -99;
                        requestTime = DateTime.Now;
                        responseTime = DateTime.Now;
                        theDifference = TimeSpan.Zero;
                        responseExist = -99;
                    }
                }
                else
                {
                    if (counter < 10)
                    {
                        uniqueString = seq.GetUniqueString();
                        threadId = Thread.CurrentThread.ManagedThreadId;
                        try
                        {
                            threadName = Thread.CurrentThread.Name;
                        }
                        catch (Exception e)
                        {
                            Exception ex = new Exception();
                            StackTrace st = new StackTrace();
                            exMsgList.Add(ex.GetBaseException().ToString());
                            stTraceFramee.Add(st.GetFrame(0).ToString());
                        }
                        urlAddress = urlAddress;
                        statusCode = -0;
                        requestTime = DateTime.Now;
                        responseTime = DateTime.Now;
                        theDifference = TimeSpan.Zero;
                        responseExist = -1;
                        counter++;
                    }
                    else
                    {
                        Console.WriteLine("Life Cycle is terminated");
                        break;
                    }
                }
                InsertIntoDBorIntoFıle();
                determineTheExceotion = DBOpt.Warning((Object)this);
                exMsgList = new ConcurrentBag<string>();
                stTraceFramee = new ConcurrentBag<string>();
                sqlExMsgList = new ConcurrentBag<string>();
                sqlStTraceFrame = new ConcurrentBag<string>();
                checkDayFormat = false;
                checkHourFormat = false;
                timeSpanChecker = false;
                listofTime = new List<List<TimeSpan>>();
                listOfDays = new List<int>();
                try
                {
                    Thread.Sleep(this.sleepTime);
                }
                catch (Exception e)
                {
                    Exception ex = new Exception();
                    StackTrace st = new StackTrace();
                    exMsgList.Add(ex.GetBaseException().ToString());
                    stTraceFramee.Add(st.GetFrame(0).ToString());
                }
            }
        }
        public void GenerateWebRequest()
        {
            /* Request time is used to show beginning time of web request*/
            requestTime = DateTime.Now;
            threadId = Thread.CurrentThread.ManagedThreadId;
            threadName = urlAddress;
            try
            {
                /*Possible exceptions that .create() method cause*/
                /*NotSupportedException, ArgumentNullException, SecurityException, UniFormatException*/
                webRequest = (HttpWebRequest)WebRequest.Create(urlAddress);
                /*Possible exceptions .Timeout*/
                /*ArgumentOutOfRangeException */
                /* In DB's design, Timeout value is a value for stating a limit how webrequest can last.*/
                webRequest.Timeout = this.timeoutValue;
                /* Global proxy settings will be checked when first webrequest is carrying out. It can take 2-8 secs.
                 * Make this null will save us from wasting our time. MSDN has a good documentation for that*/
                /*https://msdn.microsoft.com/en-us/library/system.net.servicepointmanager(v=vs.110).aspx */
                /* More things to check here they might be useful for some cases */
                /* Possible exceptions .Proxy cause*/
                /* ArgumentNullException, InvalidOperationException, Security.SecurityException*/
                webRequest.Proxy = null;
            }
            catch (Exception e)
            {
                Exception ex = new Exception();
                StackTrace st = new StackTrace();
                exMsgList.Add(ex.GetBaseException().ToString());
                stTraceFramee.Add(st.GetFrame(0).ToString());
                /* StatusCode is used for storing http status code */
                statusCode = 0;
                /* ResponseExist is used to show webrequest carries out or fail.*/
                responseExist = -1;
                /* Response time is used to show when webrequest is done but in this case, there 
                 * wont be any web request. So this is what i did */
                responseTime = requestTime;
            }
        }
        public void GetResponse()
        {
            responseTime = DateTime.Now;
            try
            {
                webResponse = (HttpWebResponse)webRequest.GetResponse();
                /*Possible exception .Subtract(..) cause*/
                /* Argument out of range exception*/
                theDifference = responseTime.Subtract(requestTime);
                /*Possible exception .StatusCode cause*/
                /* Object dispose exception*/
                statusCode = (int)webResponse.StatusCode;
                /*Possible exception .StatusCode cause*/
                /* Object dispose exception*/
                webResponse.Dispose();
            }
            /* This catch is spesific to webRequest exception. The aim here is to get 
             * spesific http status code. */
            catch (WebException we)
            {
                webResponse = (HttpWebResponse)we.Response;
                if (webResponse != null)
                {
                    /* Plenty of Http status code is available for determining the actual problem
                     * such as 404, 500, 502. */
                    statusCode = (int)webResponse.StatusCode;
                    requestTime = responseTime;
                    theDifference = TimeSpan.Zero;
                    StackTrace st = new StackTrace();
                    exMsgList.Add(we.GetBaseException().ToString());
                    stTraceFramee.Add(st.GetFrame(0).ToString());
                }
                else
                {
                    statusCode = 0;
                    responseExist = -1;
                    responseTime = requestTime;
                    theDifference = TimeSpan.Zero;
                    StackTrace st = new StackTrace();
                    exMsgList.Add(we.GetBaseException().ToString());
                    stTraceFramee.Add(st.GetFrame(0).ToString());
                }
            }
            catch (Exception e)
            {
                statusCode = 0;
                responseExist = -1;
                responseTime = requestTime;
                Exception ex = new Exception();
                StackTrace st = new StackTrace();
                exMsgList.Add(ex.GetBaseException().ToString());
                stTraceFramee.Add(st.GetFrame(0).ToString());
            }
        }
        public void CheckTheChangeInDefinitionTable()
        {
            DataTable dTable = new DataTable();
            using (SqlConnection sConnection = new SqlConnection("Data Source=(localdb)MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                try
                {
                    /* Possible exception .open() cause*/
                    /* Invalid operation exception, Sql exception, Configuration.Configuration error exception */
                    sConnection.Open();
                }
                catch (Exception e)
                {
                    Exception ex = new Exception();
                    StackTrace st = new StackTrace();
                    sqlExMsgList.Add(ex.GetBaseException().ToString());
                    stTraceFramee.Add(st.GetFrame(0).ToString());
                }
                using (SqlCommand sCommand = new SqlCommand("[dbo].[GET_definitiontablewiththehelpofURLAddress", sConnection))
                {
                    sCommand.CommandType = CommandType.StoredProcedure;
                    sCommand.Parameters.AddWithValue("@URLAddress", urlAddress);
                    using (SqlDataAdapter sAdapter = new SqlDataAdapter(sCommand))
                    {
                        try
                        {
                            /*Possible Exception .fill(...) cause*/
                            /*Invalid operation exception */
                            sAdapter.Fill(dTable);
                        }
                        catch (Exception e)
                        {
                            Exception ex = new Exception();
                            StackTrace st = new StackTrace();
                            sqlExMsgList.Add(ex.GetBaseException().ToString());
                            stTraceFramee.Add(st.GetFrame(0).ToString());
                        }
                    }
                }
            }
            foreach (DataRow dRow in dTable.Rows)
            {
                try
                {
                    /* Possible exceptions .ToInt32(...) cause */
                    /*Format exception, Invalid cast exception, Overflow exception */
                    timeoutValue = Convert.ToInt32(dRow["TimeoutValue"]);
                    sleepTime = Convert.ToInt32(dRow["SleepTime"]);
                    /* Possible exceptions .ToBoolean(...) cause */
                    /*Format exception, Invalid cast exception */
                    isActive = Convert.ToBoolean(dRow["isActive"]);
                }
                catch (Exception e)
                {
                    Exception ex = new Exception();
                    StackTrace st = new StackTrace();
                    sqlExMsgList.Add(ex.GetBaseException().ToString());
                    stTraceFramee.Add(st.GetFrame(0).ToString());
                }
                hourInformation = dRow["hourinformation"].ToString();
                daysOfWeekAsNumber = dRow["daysofweekasnumber"].ToString();
            }
        }
        public bool CheckHourFormat()
        {
            /* This function construct flexible mechanism for doing webrequest in spesific
             * range of time. Regular Expression comes in handy at this point. Definitiontable has 
             * column hourinformation. Legitimate formate are meantioned below. If you dont follow 
             * format style, there wont be any webrequest and you will see friendly message in
             * database table.*/
            /* Suitable Format */
            /* String could be empty. Webrequest is constantly carried out.*/
            /* 14:00-15:00 is appropriate format. Webrequest is carried out between 14 and 15 */
            /* You can give multipe range by putting pipeline between pair of hour
             * 09:00 - 10:00 14:00-15:00|21:00-22:00 */
            try
            {
                /* Possible exception .regex(..) cause */
                /* Argument exception, Argument null exception */
                Regex hourFormat = new Regex(@"(^$)|^(?:[0-1][0-9]|2[0-3]):[0-5][0-9]-(?:[0-1][0-9]|2[0-3]):[0-5][0-9](\|(?:[0-1][0-9]|2[0-3]):[0-5][0-9]-(?:[0-1][0-9]|2[0-3]):[0-5][0-9])*$");
                /* Possible exception .match(..) cause */
                /* Argument null exception, Regex match timeout exception */
                Match hourFormatMath = hourFormat.Match(hourInformation);
                if (hourFormatMath.Success)
                {
                    checkHourFormat = true;
                }
                else
                {
                    checkHourFormat = false;
                }
            }
            catch (Exception e)
            {
                Exception ex = new Exception();
                StackTrace st = new StackTrace();
                exMsgList.Add(ex.GetBaseException().ToString());
                stTraceFramee.Add(st.GetFrame(0).ToString());
            }
            return checkHourFormat;
        }
        public string[] GetHoursIntoArray()
        {
            string[] sArray = new string[hourInformation.Length];
            if (CheckHourFormat())
            {
                if (hourInformation.Length == 0)
                {
                    sArray = null;
                }
                else
                {
                    sArray = hourInformation.Split('|');
                }

            }
            return sArray;
        }
        public List<List<TimeSpan>> GetThoseHoursIntoList()
        {
            if (GetHoursIntoArray() != null)
            {
                foreach (var item in GetHoursIntoArray())
                {
                    /* Possible exception select(...) cause */
                    /* Argument null exception */
                    try
                    {
                        listofTime.Add(item.Split('-').Select(s => TimeSpan.Parse(s)).ToList());
                    }
                    catch (Exception e)
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
                listofTime = null;
            }
            return listofTime;

        }
        public bool CheckTimeSpanwithCurrentTimeSpan()
        {
            /* First of all i store hourinformation in db.
             * It could be empty or could be 14.00-15:00, 14:00-15:00 | 21:00 -22:00
             * Now we need to be sure whether current time that is in the range of times.*/
            DateTime currentTime = DateTime.Now;
            TimeSpan currentTimeSpan = currentTime.TimeOfDay;
            /* The reason of i define listOfTime as List of List of TimeSpan is about my design.
             * after "split" operation, content of List will be like that
             * List -> [14:00,15:00] or [[14:00, 15:00],[21:00-22:00]]. So it is operated
             * like List[i][0], List[i][1]. */
            listofTime = GetThoseHoursIntoList();
            if (listofTime == null)
            {
                timeSpanChecker = true;
            }
            else
            {
                for (int i = 0; i < listofTime.Count(); i++)
                {
                    if (listofTime[i][0] < currentTimeSpan && listofTime[i][1] > currentTimeSpan)
                    {
                        timeSpanChecker = false;
                    }
                    else
                    {
                        timeSpanChecker = true;
                    }
                }
            }
            return timeSpanChecker;
        }
        public bool CheckDayFormat()
        {
            /* This function construct flexible mechanism for webrequest. 
             * Now You can arrange which day or days webrequest will be done.
             * Definitiontable has a column daysofweekasnumber. In c#, days 
             * are indicated numbers. Sunday is 0, Monday is 1, ...., Saturday is 6. */
            /* Suitable Format */
            /* String could be empty or 1 or 1|2 or 1|2|6.*/
            try
            {
                /* Possible exception .regex(..) cause */
                /* Argument exception, Argument null exception */
                Regex dayFormat = new Regex(@"(^$)|(^([0-6]$)|(?:[0-6]\|[0-6])+)$");
                /* Possible exception .match(..) cause */
                /* Argument null exception, Regex match timeout exception */
                Match dayFormatMatch = dayFormat.Match(hourInformation);
                if (dayFormatMatch.Success)
                {
                    checkDayFormat = true;
                }
                else
                {
                    checkDayFormat = false;
                }
            }
            catch (Exception e)
            {
                Exception ex = new Exception();
                StackTrace st = new StackTrace();
                exMsgList.Add(ex.GetBaseException().ToString());
                stTraceFramee.Add(st.GetFrame(0).ToString());
            }
            return checkDayFormat;
        }
        public List<int> GetDaysIntoList()
        {
            if (CheckDayFormat())
            {
                if (daysOfWeekAsNumber == "")
                {
                    listOfDays = null;
                }
                else
                {

                    /* Possible exception select(...) cause */
                    /* Argument null exception */
                    try
                    {
                        listOfDays = daysOfWeekAsNumber.Split('|').Select(s => Convert.ToInt32(s)).ToList();
                    }
                    catch (Exception e)
                    {
                        Exception ex = new Exception();
                        StackTrace st = new StackTrace();
                        exMsgList.Add(ex.GetBaseException().ToString());
                        stTraceFramee.Add(st.GetFrame(0).ToString());
                    }
                }
            }
            return listOfDays;
        }
        public bool CompareDatesAndDecideWhatNextActionWillBe()
        {
            int currentDayAsNumber = (Int32)DateTime.Now.DayOfWeek;
            listOfDays = GetDaysIntoList();
            if (listOfDays == null)
            {
                dayChecker = true;
            }
            else if (listOfDays.Count() == 0)
            {
                dayChecker = true;
            }
            else if (listOfDays.Contains(currentDayAsNumber))
            {
                dayChecker = true;
            }
            else
            {
                dayChecker = false;
            }
            return dayChecker;
        }
        public void InsertIntoDBorIntoFıle()
        {
            DBOpt.InsertProcess((Object)this);
            fOperation.CreateFile((Object)this);
        }
    }
}
