using System;
using System.IO;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string str = "";
            
                for (int i=1; i<=136; i++)
                {
                    str += $"insert into SeatSetting(SeatId,ShowTimeId,SeatStatus) values ({i},2,0)\n";
                }
            
            File.WriteAllText("out.txt", str);
        }
    }
}
