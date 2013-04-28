using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellySolver
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args != null && args.Length > 0)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        string option = args[i].ToLower();
                        switch (option)
                        {
                            case "displaysearch":
                                GlobalConfig.DisplaySearch = true;
                                break;
                            default:
                                throw new Exception("invalid option " + option);
                        }
                    }
                }

                GlobalConfig.Writer = new Writer(DateTime.Now.ToString("yyyyMMddHHmm"));

                //Test();
                new Solver().Run();
                System.Threading.Thread.Sleep(2000);
            }
            finally
            {
                GlobalConfig.Writer.Close();
            }

        }

        static void Test()
        {
            new T_Position().Run();
            new T_Jelly().Run();
            new T_Game().Run();
            new T_Cell().Run();
            new T_Solver().Run();
        }
    }
}
