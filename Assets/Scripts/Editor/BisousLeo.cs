using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Text;


public class BisousLeo : EditorWindow {

    float vitesseBandeauMultipler = 0;
    float delaiBandeau = 0;
    float vitesseInsertion = 0;

    GameObject model;
    Holder holder;

    DataInput dI;

    string code = "E";
    float x = 0;
    float y = 0;

    int listX = 0;
    int listY = 0;

    int sequence = 0;
    string display;

    List<SequenceInput> sequences = new List<SequenceInput>();


    [MenuItem("Window/LeoEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(BisousLeo));
    }

    private void OnGUI()
    {

        GUILayout.Label("Vitesse du bandeau (multiplicateur)");
        vitesseBandeauMultipler = EditorGUILayout.FloatField("", vitesseBandeauMultipler);


        GUILayout.Label("Delai du bandeau (temps en secondes)");
        delaiBandeau = EditorGUILayout.FloatField("", delaiBandeau);


        GUILayout.Label("Vitesse de transition (multiplicateur) ");
        vitesseInsertion = EditorGUILayout.FloatField("", vitesseInsertion);


        GUILayout.Label("IMPORTANT SINON CA MARCHE PAS , selectionner dans 'Asset' ");
        model = (GameObject)EditorGUILayout.ObjectField("Modele de base", model, typeof(GameObject), true);
        holder = (Holder)EditorGUILayout.ObjectField("Quel holder ?", holder, typeof(Holder), true);

        GUILayout.Label("Code ? (une seule lettre et en majuscule)");
        code = GUILayout.TextField(code);

        GUILayout.Label("Position x (entre 0 (gauche) et 1 (droite))");
        x = EditorGUILayout.FloatField(x);

        GUILayout.Label("Position y (entre 0 (bas) et 1 (haut))");
        y = EditorGUILayout.FloatField(y);

        x = Mathf.Clamp(x, 0, 1);
        y = Mathf.Clamp(y, 0, 1);

        if (GUILayout.Button("Ajouter la frappe dans la liste actuelle"))
        {
            if (sequence >= sequences.Count)
            {
                sequences.Add(new SequenceInput());
            }

            KeyCode c = KeyCode.A;
            for(int i = 97; i < 122; i++)
            {
                if (((KeyCode)i).ToString() == code.ToUpper())
                {
                    c = (KeyCode)i;
                }
            }

            DataInput sI = new DataInput(c, new Vector2(x, y));
            sequences[sequence].dI.Add(sI);
        }

        if (GUILayout.Button("Liste de frappe suivante"))
        {
            sequence++;
            if (sequence >= sequences.Count)
            {
                sequences.Add(new SequenceInput());
            }
        }

        EditorGUILayout.LabelField("Index : " + sequence.ToString());
        foreach(SequenceInput sI in sequences)
        {
            foreach(string s in sI.ToStringAlt())
            {
                EditorGUILayout.LabelField(s);
            }
        }
        

        if (GUILayout.Button("Ajouter a la liste"))
        {
            GameObject instance;
            instance = Instantiate(model);
            Scenette scn = instance.GetComponent<Scenette>();
            scn.sequencesToDo = new List<SequenceInput>(sequences);
            scn.speedBandeauMultipler = vitesseBandeauMultipler;
            scn.timeBeforeNext = delaiBandeau;
            scn.mutliplerSpeedEnter = vitesseInsertion;
            holder.scenetteListTemp.Add(instance);
            sequences = new List<SequenceInput>();
        }

        if (GUILayout.Button("Supprimer la liste"))
        {
            holder.GetComponent<Holder>().scenetteListTemp.Clear();
        }

    }
}
