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
using Apoc3D.Core;

namespace Apoc3D.Graphics.Effects
{
    public class EffectManager : Singleton
    {
        static EffectManager singleton;
        static object syncHelper = new object();

        public static void Initialize(RenderSystem device)
        {
            singleton = new EffectManager();

            EngineConsole.Instance.Write("特效系统启动。", ConsoleMessageType.Information);

            //singleton.RegisterModelEffectType(StandardEffectFactory.Name, new StandardEffectFactory(device));
            //singleton.RegisterModelEffectType(TerrainRenderingEffectFactory.Name, new TerrainRenderingEffectFactory(device));

            //singleton.RegisterModelEffectType(RoadEffectFactory.Name, new RoadEffectFactory(device));
        }


        /// <summary>
        ///   Gets the singleton instance of this class.
        /// </summary>
        public static EffectManager Instance
        {
            get
            {
                return singleton;
            }
        }

        private EffectManager()
        {
            Enabled = true;
        }

        Dictionary<string, Effect> modelFX = new Dictionary<string, Effect>(CaseInsensitiveStringComparer.Instance);
        


        Dictionary<string, EffectFactory> modelFXFac = new Dictionary<string, EffectFactory>(CaseInsensitiveStringComparer.Instance);



        public void RegisterModelEffectType(string name, EffectFactory fac)
        {
            modelFXFac.Add(name, fac);
            EngineConsole.Instance.Write("找到模型特效 " + name, ConsoleMessageType.Information);
        }


        public void UnregisterModelEffectType(string name)
        {
            modelFXFac.Remove(name);
        }


        public Effect GetModelEffect(string name)
        {
            lock (syncHelper)
            {
                return Enabled ? modelFX[name] : null;
            }
        }

        public int ModelEffectCount
        {
            get { return modelFX.Count; }
        }

        public bool HasModelEffect(string name)
        {
            return modelFX.ContainsKey(name);
        }

        public void LoadEffects()
        {
            EffectsLoaded = true;
            foreach (KeyValuePair<string, EffectFactory> e in modelFXFac)
            {
                modelFX.Add(e.Key, e.Value.CreateInstance());
            }

            string msg = "{0} 种特效已经加载。";
            EngineConsole.Instance.Write(
                string.Format(msg, modelFXFac.Count.ToString()), ConsoleMessageType.Information);
        }

        public void UnloadEffects()
        {
            EffectsLoaded = false;
            foreach (KeyValuePair<string, Effect> e in modelFX)
            {
                modelFXFac[e.Key].DestroyInstance(e.Value);
            }
            modelFX.Clear();
        }

        public bool Enabled
        {
            get;
            set;
        }

        public bool EffectsLoaded
        {
            get;
            private set;
        }
        protected override void dispose()
        {
            if (EffectsLoaded)
            {
                UnloadEffects();
            }
            singleton = null;
        }
    }
}
