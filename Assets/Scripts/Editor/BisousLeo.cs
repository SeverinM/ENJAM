using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class BisousLeo : EditorWindow {
    

    [MenuItem("Window/LeoEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(BisousLeo));
    }

    private void OnGUI()
    {
        
    }
}
