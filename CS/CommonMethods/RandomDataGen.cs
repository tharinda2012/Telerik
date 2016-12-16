using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CS.CommonMethods
{
    class RandomDataGen
    {

    private static Random random = new Random();

        public static string Random_String_Generated(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String ranstring= new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return "Auto--" + ranstring; 
        }
                     
    }
}
