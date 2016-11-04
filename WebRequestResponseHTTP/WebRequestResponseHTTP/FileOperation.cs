using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRequestResponseHTTP
{
    class FileOperation : GenericExceptionClass
    {
        Sequence seq = new Sequence();
        DBOperation DBOpt = new DBOperation();

        /* As i meantioned above  In case of any sql's failure, the methods in that class will be used.
         * determineTheException is boolean value become true if any exception is caught by try{} catch(Exception e){}
         * block. Therefore I will store data into file. */
        public DirectoryInfo CreateFolder()
        {
            DirectoryInfo dInfo = new DirectoryInfo(@"c:\log");
            try
            {
                /* Possible Exceptions that CreateDirectory(..) cause */
                /* IOEception, Unauthorized access exception, Argument exception, Argument null exception
                 * Path too long exception, Directory not found exception, No supported exception */
                dInfo.Create();
            }
            catch(Exception e)
            {
                Exception ex = new Exception();
                StackTrace st = new StackTrace();
                exMsgList.Add(ex.GetBaseException().ToString());
                stTraceFramee.Add(st.GetFrame(0).ToString());
            }
            return dInfo;
        }
        public void CreateFile(Object obj)
        {
            string currentDateInStringFormat = "";
            int listCounter = exMsgList.Count();
            DateTime currentDate = DateTime.Now.Date;
            currentDateInStringFormat = currentDate.ToString("ddmmyyyy");
            DirectoryInfo getInfo = CreateFolder();
            if(obj.determineTheExceotion == true)
            {
                if(!File.Exists(currentDate.ToString()))
                {
                    try
                    {
                        /* Possible exceptions StreamWriter(...) cause */
                        /* Argument exception, Not supported exception, Argument null exception
                        *  Security.SecurityException, File not found exception, IO exception, 
                        * Directory not found exception, Path too long exception, Argument out of range exception.*/
                        using (FileStream fStream = new FileStream(@"" + getInfo + "" + currentDateInStringFormat, FileMode.Append))
                        {
                            /* Possible exceptions StreamWriter(...) cause */
                            /* Argument exception, Argument null exception*/
                            using (StreamWriter sWriter = new StreamWriter(fStream))
                            {
                                foreach (var item in exMsgList)
                                {
                                    sWriter.WriteLine(DateTime.Now);
                                    sWriter.WriteLine(obj.uniqueString);
                                    sWriter.WriteLine(obj.urlAddress);
                                    sWriter.WriteLine("Exception Message Content");
                                    sWriter.WriteLine(item);
                                }
                                Console.WriteLine("-----------");

                                foreach (var item in stTraceFramee)
                                {
                                    sWriter.WriteLine(DateTime.Now);
                                    sWriter.WriteLine(obj.uniqueString);
                                    sWriter.WriteLine(obj.urlAddress);
                                    sWriter.WriteLine("Exception Message Content");
                                    sWriter.WriteLine(item);
                                }
                                Console.WriteLine("-----------");
                            }
                        }
                    }
                    catch(Exception e)
                    {

                    }

                }
            }

        }
    }
}