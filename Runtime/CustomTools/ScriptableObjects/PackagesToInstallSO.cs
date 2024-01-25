using System;
using System.Collections.Generic;
using DarkenSoda.CustomTools.Scripts;
using UnityEngine;

namespace DarkenSoda.CustomTools.ScriptableObjects {
    [CreateAssetMenu(fileName = "PackagesToInstallSO", menuName = "Scriptable Objects/PackagesToInstallSO", order = 50)]
    public class PackagesToInstallSO : ScriptableObject {
        public List<PackageData> Packages;
    }
}