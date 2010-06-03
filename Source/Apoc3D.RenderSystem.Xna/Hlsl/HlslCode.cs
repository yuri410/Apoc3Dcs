/*
-----------------------------------------------------------------------------
This source file is part of Apoc3D Engine

Copyright (c) 2009+ Tao Games

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  if not, write to the Free Software Foundation, 
Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA, or go to
http://www.gnu.org/copyleft/gpl.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Apoc3D.RenderSystem.Xna
{
    class HlslCode
    {
        const char StrToken = '"';
        static readonly string CommentToken = "//";
        static readonly string UsageStartToken = "[";
        static readonly string UsageEndToken = "]";

        HlslDeclaration [] decls;

        private HlslCode(HlslDeclaration[] d)
        {
            decls = d;
        }
        

        public HlslDeclaration[] Declarations 
        {
            get { return decls; }
            private set { decls = value; }
        }

        static HlslRegisterType ParseRegister(string reg, out int index) 
        {
            reg = reg.ToLower(CultureInfo.InvariantCulture);

            int digitPos = -1;
            for (int i = reg.Length - 1; i >= 0; i--) 
            {
                if (reg[i] >= '0' && reg[i] <= '9')
                {
                    digitPos = i;
                }
                else break;
            }

            if (digitPos != -1)
            {
                string ns = reg.Substring(digitPos);
                index = int.Parse(ns);

                reg = reg.Substring(0, digitPos);
            }
            else index = 0;

            switch (reg) 
            {
                case "c":
                    return HlslRegisterType.Constant;
                case "s":
                    return HlslRegisterType.Sampler;
            }
            return HlslRegisterType.Unknown;
        }
        static HlslType ParseType(string type) 
        {
            type = type.ToLower(CultureInfo.InvariantCulture);
            return HlslType.Boolean;
        }
        static AutoParamType ParseAutoParam(string type, out int index) 
        {
            type = type.ToLower(CultureInfo.InvariantCulture);

            int digitPos = -1;
            for (int i = type.Length - 1; i >= 0; i--)
            {
                if (type[i] >= '0' && type[i] <= '9')
                {
                    digitPos = i;
                }
                else break;
            }

            if (digitPos != -1)
            {
                string ns = type.Substring(digitPos);
                index = int.Parse(ns);

                type = type.Substring(0, digitPos);
            }
            else index = 0;

            AutoParamType result = (AutoParamType)Enum.Parse(typeof(AutoParamType), type, true);
            if (Enum.IsDefined(typeof(AutoParamType), result))
            {
                return result;
            }
            return AutoParamType.None;
        }

        public static HlslCode Parse(string code) 
        {
            List<HlslDeclaration> declList = new List<HlslDeclaration>();

            string[] lines = code.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                bool isInvalid = false;

                int pos = lines[i].IndexOf(':');
                if (pos != -1)
                {
                    string v = lines[i].Substring(0, pos);

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
                                if (type == CommentToken)
                                {
                                    isInvalid = true;
                                    break;
                                }
                            }
                            else if (name == null)
                            {
                                name = v1[k].Trim();
                                if (name == CommentToken)
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

                            pos = lines[i].IndexOf(CommentToken, pos2);

                            if (pos != -1)
                            {
                                string usage = lines[i].Substring(pos + 2).Trim();
                                if (usage.StartsWith(UsageStartToken))
                                {
                                    pos = usage.IndexOf(UsageEndToken);
                                    if (pos != -1)
                                    {
                                        usage = usage.Substring(1, pos - 1);

                                        HlslDeclaration decl = new HlslDeclaration();

                                        int temp;
                                        decl.Name = name;
                                        
                                        decl.Register = ParseRegister(regName, out temp);
                                        decl.RegisterIndex = temp;

                                        decl.Type = ParseType(type);
                                        decl.Usage = ParseAutoParam(usage, out temp);
                                        decl.Index = temp;

                                        declList.Add(decl);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return new HlslCode(declList.ToArray());
        }
    }
}