using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellySolver
{
    class Writer
    {
        string filePath;
        TextWriter tw, twt;

        public Writer(string fileSuffix)
        {
            filePath = @"C:\Users\Titou\Desktop\JellySolver\Log\run_" + fileSuffix + ".txt";
            tw = File.CreateText(filePath);
            filePath = @"C:\Users\Titou\Desktop\JellySolver\Log\runtime_" + fileSuffix + ".txt";
            twt = File.CreateText(filePath);
        }

        public void Close()
        {
            tw.Close();
            twt.Close();
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
            tw.WriteLine(message);
        }

        public void Write(string message)
        {
            Console.Write(message);
            tw.Write(message);
        }
            
        public void WriteTimeLine(string message)
        {
            Console.WriteLine(message);
            twt.WriteLine(message);
        }
    }
}
