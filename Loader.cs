using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CWHACK_DEOBF
{
    public class Loader
    {
        public static void Init()
        {
            module = new GameObject();
            module.AddComponent<Main>();
            UnityEngine.Object.DontDestroyOnLoad(module);
        }
        private static GameObject module;
    }
}
