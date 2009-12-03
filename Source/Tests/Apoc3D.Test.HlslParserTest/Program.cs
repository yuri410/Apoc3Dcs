using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apoc3D.RenderSystem.Xna;
using System.IO;

namespace Apoc3D.Test.HlslParserTest
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader(
                @"E:\Desktop\registers.hlsl", Encoding.Default);

            HlslCode test = HlslCode.Parse(sr.ReadToEnd());
            sr.Close();

            HlslDeclaration[] decls = test.Declarations;

            for (int i = 0; i < decls.Length; i++) 
            {
                Console.Write(decls[i].Name);
                Console.Write("  ");
                Console.Write(decls[i].Type);
                Console.Write("  ");
                Console.Write(decls[i].Usage);
                Console.Write("  ");
                Console.Write(decls[i].Index);
                Console.Write("  ");
                Console.Write(decls[i].Register);
                Console.Write("  ");
                Console.Write(decls[i].RegisterIndex);
                Console.WriteLine();
            }
            Console.ReadKey();
            
        }
    }
}
