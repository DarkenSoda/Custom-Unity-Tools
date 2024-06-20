using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace DarkenSoda.CustomTools.Scripts.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string label;
        public bool playModeOnly;

        public ButtonAttribute() : this("") { }

        public ButtonAttribute(bool playModeOnly) : this("", playModeOnly) { }

        public ButtonAttribute(string label, bool playModeOnly = false)
        {
            this.label = label;
            this.playModeOnly = playModeOnly;
        }
    }
}
