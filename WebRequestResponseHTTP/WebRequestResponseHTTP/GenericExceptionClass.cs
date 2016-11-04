using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRequestResponseHTTP
{
    abstract class GenericExceptionClass
    {
        /* The aim of using abstract class is to define structures for collection all exceptions at once.
         * The way for doing this is to implement this class in the other classes. I used concurrentBag here
         * The reason using of this structure is because application is multithreaded. ConcurrentBag is a safe
         * structure against problems thread cause such as deadlock and race condition. So briefly if you use 
         * concurrentbag you dont have to use lock mechaism explicitly. It is already implemented. */
        /* This app. have few classes. The methods particular library contains can cause an exception.
         * You have to catch if any exceptions occur with try{} catch(Exception e) block. As natural flow of this 
         * application, those exceptions have to keep in somewhere. In that point, our concurrentbag structures
         * roll in.*/
        /* One more point tho. We need to define two more lists for sql's failure. The reason of that, taking into 
         * sql's failure consideration, this app. has to offer another mechanism such as storing datas in the file.
         * If everything goes well, datas will be stored in the database otherwise it will be stored in the file 
         * as well as sql's exception data. */

        /* The reason of using protected is to allow class to access this property in same assembly. These structures are
         * actually common property of other classes. So one instance for all classes is enough in my opinion. Hence i define 
         * those as static. */

        protected static ConcurrentBag<string> exMsgList = new ConcurrentBag<string>();
        protected static ConcurrentBag<string> stTraceFramee = new ConcurrentBag<string>();
        protected static ConcurrentBag<string> sqlExMsgList = new ConcurrentBag<string>();
        protected static ConcurrentBag<string> sqlStTraceFrame = new ConcurrentBag<string>();

    }
}
