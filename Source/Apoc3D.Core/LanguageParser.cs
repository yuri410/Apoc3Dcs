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
http://www.gnu.org/copyleft/lesser.txt.

-----------------------------------------------------------------------------
*/
using System;
using System.Collections.Generic;
using System.Text;
using Apoc3D;

namespace Apoc3D
{
#if !XBOX
    using System.Windows.Forms;

    public static class LanguageParser
    {
        static StringTable strTable;

        public static void ParseLanguage(StringTable data, Control ctl)
        {
            strTable = data;
            ParseLanguage(ctl);
            strTable = null;
        }

        public static void ParseLanguage(StringTable data, ToolStrip ctl)
        {
            strTable = data;
            for (int i = 0; i < ctl.Items.Count; i++)
                ParseLanguage(ctl.Items[i]);
            strTable = null;
        }

        public static void ParseLanguage(StringTable data, ListView ctl)
        {
            strTable = data;
            ParseLanguage(ctl);
            strTable = null;
        }

        static void ParseLanguage(Control ctl)
        {
            if (!string.IsNullOrEmpty(ctl.Text))
            {
                ctl.Text = strTable[ctl.Text];
            }

            Control.ControlCollection col = ctl.Controls;
            for (int i = 0; i < col.Count; i++)
                ParseLanguage(col[i]);
        }

        static void ParseLanguage(ToolStripItem ctl)
        {
            if (!string.IsNullOrEmpty(ctl.Text))
            {
                ctl.Text = strTable[ctl.Text];
            }

            ToolStripDropDownItem itm = ctl as ToolStripDropDownItem;
            if (itm != null)
                for (int i = 0; i < itm.DropDownItems.Count; i++)
                    ParseLanguage(itm.DropDownItems[i]);

        }
        static void ParseLanguage(ListView lv)
        {
            for (int i = 0; i < lv.Columns.Count; i++)
            {
                lv.Columns[i].Text = strTable[lv.Columns[i].Text];
            }
        }
    }
#endif
}