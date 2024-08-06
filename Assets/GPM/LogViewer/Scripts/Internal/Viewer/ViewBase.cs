﻿namespace Gpm.LogViewer.Internal
{
    using UnityEngine;

    public class ViewBase : MonoBehaviour
    {
        public virtual void InitializeView()
        {
        }

        public virtual void CloseView()
        {
        }

        public virtual void SetOrientation(ScreenOrientation orientataion)
        {
        }

        public virtual void UpdateResolution()
        {
        }
    }

}