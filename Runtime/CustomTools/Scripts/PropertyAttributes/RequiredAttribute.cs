using System;
using UnityEngine;

namespace DarkenSoda.CustomTools.Scripts.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class RequiredAttribute : PropertyAttribute { }
}
