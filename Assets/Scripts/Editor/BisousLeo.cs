using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class BisousLeo : EditorWindow {

    float vitesseBandeauMultipler = 0;
    float delaiBandeau = 0;
    float vitesseInsertion = 0;

    GameObject model;
    Holder holder;

    [MenuItem("Window/LeoEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(BisousLeo));
    }

    private void OnGUI()
    {

        GUILayout.Label("Vitesse du bandeau (multiplicateur)");
        vitesseBandeauMultipler = EditorGUILayout.FloatField("", vitesseBandeauMultipler);


        GUILayout.Label("Delai du bandeau");
        delaiBandeau = EditorGUILayout.FloatField("", delaiBandeau);


        GUILayout.Label("Vitesse de transition");
        vitesseInsertion = EditorGUILayout.FloatField("", vitesseInsertion);

        model = (GameObject)EditorGUILayout.ObjectField("Modele de base", model, typeof(GameObject), true);
        holder = (Holder)EditorGUILayout.ObjectField("Quel holder ?", holder, typeof(Holder), true);

        if (GUILayout.Button("Ajouter a la liste"))
        {
            GameObject instance;
            instance = Instantiate(model);
            Scenette scn = instance.GetComponent<Scenette>();
            scn.speedBandeauMultipler = vitesseBandeauMultipler;
            scn.timeBeforeNext = delaiBandeau;
            scn.mutliplerSpeedEnter = vitesseInsertion;
            holder.scenetteListTemp.Add(instance);
        }

        if (GUILayout.Button("Supprimer la liste"))
        {
            holder.GetComponent<Holder>().scenetteListTemp.Clear();
        }
    }
}
