// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace System.Windows.Forms
{
    public static class ControlExtensions
    {
        /// <summary>
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// </summary>
        /// <param name="control">the control for which the update is required</param>
        /// <param name="action">action to be performed on the control</param>
        /// InvokeOnUiThreadIfRequired
        public static void Invoke(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
        public static void Invoke<T>(this Control control, Action<T> action, T arg)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action, arg);
            }
            else
            {
                action.Invoke(arg);
            }
        }
    }
}
