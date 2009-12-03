using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Apoc3D.RenderSystem.Xna
{
    class HlslCode
    {
        const char StrToken = '"';
        
        HlslDeclaration [] decls;

        public HlslDeclaration[] Declarations 
        {
            get { return decls; }
            set { decls = value; }
        }

        public static HlslCode Parse(string code) 
        {
            string[] lines = code.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                bool isInvalid = false;

                int pos = lines[i].IndexOf(':');
                if (pos != -1)
                {
                    string v = lines[i].Substring(0, pos + 1);

                    string type = null;
                    string name = null;

                    string[] v1 = v.Split(null);
                    for (int k = 0; k < v1.Length; k++)
                    {
                        if (v1[k].Length > 0)
                        {
                            if (type == null)
                            {
                                type = v1[k].Trim();
                                if (type == "//")
                                {
                                    isInvalid = true;
                                    break;
                                }
                            }
                            else if (name == null)
                            {
                                name = v1[k].Trim();
                                if (name == "//")
                                {
                                    isInvalid = true;
                                    break;
                                }
                            }
                            else
                            {
                                isInvalid = true;
                                break;
                            }
                        }
                    }

                    if (isInvalid)
                        continue;

                    pos = lines[i].IndexOf("register(", StringComparison.OrdinalIgnoreCase);

                    if (pos != -1)
                    {
                        pos += 9;
                        int pos2 = lines[i].IndexOf(')', pos);

                        if (pos2 > pos) 
                        {
                            string regName = lines[i].Substring(pos, pos2 - pos).Trim();

                            pos = lines[i].IndexOf("//", pos2);

                            string usage = lines[i].Substring(pos + 2);


                        }
                    }
                }
            }
            

            throw new NotImplementedException();
        }
    }
}