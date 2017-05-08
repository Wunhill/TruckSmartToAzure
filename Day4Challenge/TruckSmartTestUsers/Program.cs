using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace TruckSmartTestUsers
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var rdr = File.OpenText(AppContext.BaseDirectory + "\\TestUsers.csv"))
            {
                //Read off column names
                rdr.ReadLine();
                while (!rdr.EndOfStream)
                {
                    string[] fields = rdr.ReadLine().Split(',');
                    //fields[0] - Driver name
                    //fields[1] - Login name
                    //fields[2] - Phone
                    //field[3] - Postal Code

                }

            }
        }
    }
}
