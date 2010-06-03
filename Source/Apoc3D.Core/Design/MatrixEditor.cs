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
using System.ComponentModel;
using System.Reflection;
using System.Text;
using Apoc3D.MathLib;

namespace Apoc3D.Design
{
#if !XBOX
    using System.Drawing.Design;
    using System.Windows.Forms.Design;

#endif
    public class MatrixEditor : UITypeEditor
    {
#if !XBOX
        MatrixEditControl ui;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (ui == null)
            {
                Assembly asm = Assembly.GetEntryAssembly();
                Type tpe = asm.GetType("VirtualBicycle.Ide.DevStringTable", true);
                PropertyInfo stProp = tpe.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);

                ui = new MatrixEditControl((StringTable)stProp.GetValue(null, null));
            }

            //m_ui.SetStates((DockAreas)value);
            ui.Value = (Matrix)value;
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            ui.Service = edSvc;
            edSvc.DropDownControl(ui);

            //ui.Value = null;
            Matrix mat = ui.Value;
            ui.Reset();
            return mat;
            //return m_ui.DockAreas;
        }
        public override bool IsDropDownResizable
        {
            get { return true; }
        }
#endif
    }
}
