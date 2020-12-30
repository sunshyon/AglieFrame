using AglieFrame.Model;
using System;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var rr = new ResultRoot<string>();
            var a = rr.Code;
            rr.Code = 0;

            Console.WriteLine(a);
        }
    }
}
