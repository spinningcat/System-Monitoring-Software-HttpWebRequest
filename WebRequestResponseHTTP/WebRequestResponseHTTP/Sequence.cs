using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRequestResponseHTTP
{
    class Sequence : GenericExceptionClass
    {
        public char GetCharFromSequence()
        {
            string sequence = "";
            int random = 0;
            char letter = '\0';
            sequence = "0123456789ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghjklmnopqrstuvwxyz";
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            try
            {
                random = rand.Next(0, sequence.Length - 1);
                letter = sequence[random];
            }
            catch (Exception e)
            {
                Exception ex = new Exception();
                StackTrace st = new StackTrace();
                exMsgList.Add(ex.GetBaseException().ToString());
                stTraceFramee.Add(st.GetFrame(0).ToString());
            }   
            return letter;
        }
        public string GetRandomString()
        {
            /* Concat operation of string is costy operation because string is immutable structure.
             * That means each time you concat a string with original string variable, new instance 
             * will be created. Whereas stringbuilder is mutable structure. Creating new instance in 
             * append operation wont be created. */
            StringBuilder sBuilder = new StringBuilder();
            string randString = "";
            try
            {
                sBuilder.Append(GetCharFromSequence().ToString());
            }
            catch(Exception e)
            {
                Exception ex = new Exception();
                StackTrace st = new StackTrace();
                exMsgList.Add(ex.GetBaseException().ToString());
                stTraceFramee.Add(st.GetFrame(0).ToString());
            }
            randString = sBuilder.ToString();
            return randString;
        }
        public List<string> GetIntoList()
        {
            /* Even though duplication on random sequence is last possible, we should check it anyway.
             * This function will guarantee that wont be any reputation with goto statement. Each time 
             * sequence will be check with Contain() method. If List has that sequence it wont be added 
             * but GetRandomString() function will be executed again.*/
            List<string> container = new List<string>();
            generateString: GetRandomString();
            if(container.Contains(GetRandomString()))
            {
                goto generateString;
            }
            else
            {
                container.Add(GetRandomString());
            }
            return container;
        }
        public string GetUniqueString()
        {
            /* Pervious function will guarantee the sequence is totally unique.
             * It is important to have unique sequences because each thread will work 
             * in every minute. It is good amount of squence will be generated.
             * So last function will return string inside list. Each thread will create 
             * only one sequence at a time.*/
            string totallyUnique = "";
            foreach (var item in GetIntoList())
            {
                totallyUnique = item;
            }
            return totallyUnique;
        }

    }
}
