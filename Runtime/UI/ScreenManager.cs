﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class ScreenManager<TScreenId> : IScreenManager<TScreenId>
    {
        private readonly Dictionary<TScreenId, IScreen> screensDatabase = new();
        
        private IScreenProvider<TScreenId> provider;
        private Canvas parentCanvas;

        public void Init(IScreenProvider<TScreenId> providerReference, Canvas parentCanvasReference)
        {
            if (providerReference == null)
            {
                throw new Exception("The provider reference is null");
            }
            
            if (parentCanvasReference == null)
            {
                throw new Exception("The parent canvas reference is null");
            }
            
            provider = providerReference;
            parentCanvas = parentCanvasReference;
        }

        public IScreen ShowScreen(TScreenId id)
        {
            if (parentCanvas == null)
            {
                throw new Exception("ParentCanvas is not set. Cannot show screen");
            }
            
            if (provider == null)
            {
                throw new Exception("ScreenProvider is null. Cannot show screen");
            }

            if (screensDatabase.ContainsKey(id))
            {
                throw new Exception("Unable to show screen because is already active");
            }
            
            var screen = provider.GetScreen(id, parentCanvas.transform);
            
            if (screen == null)
            {
                throw new Exception("Screen is null. Cannot show screen");
            }
            
            screensDatabase.Add(id, screen);
            screen.Show();

            return screen;
        }

        public void CloseScreen(TScreenId id)
        {
            if (!screensDatabase.ContainsKey(id))
            {
                throw new Exception("Trying to close a closed screen");
            }

            var screen = screensDatabase[id];
            screensDatabase.Remove(id);
            screen.Close();
        }
        
        public void Clear()
        {
            var screensList = screensDatabase.Keys.ToList();

            for (var i = screensList.Count - 1; i >= 0; i--)
            {
                CloseScreen(screensList[i]);
            }
        }
    }
}
