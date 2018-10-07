using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Text;


public class BisousLeo : EditorWindow {

    float vitesseBandeauMultipler = 1;
    float delaiBandeau = 1;
    float vitesseInsertion = 1;

    GameObject model;

    DataInput dI;

    string code = "E";
    float x = 0;
    float y = 0;

    int listX = 0;
    int listY = 0;

    int sequence = 0;
    string nameObject = "Leo";

    string textVictory = "Yes !";
    string textDefeat = "Oh no !";
    int textSize = 10;

    List<SequenceInput> sequences = new List<SequenceInput>();

    AudioClip clpSuccess;
    AudioClip clpFail;
    AudioClip clpSuccessScenette;

    animHero anim;

    Sprite sprt;

    GameObject evaluate;


    [MenuItem("Window/LeoEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(BisousLeo));
    }

    private void OnGUI()
    {
        GUILayout.Label("Nom de l'objet");
        nameObject = EditorGUILayout.TextArea(nameObject);
        if (nameObject == "")
        {
            nameObject = "J'ai laissé un nom vide , shame on me";
        }

        GUILayout.Label("Vitesse du bandeau (multiplicateur)");
        vitesseBandeauMultipler = Mathf.Max(0.001f,EditorGUILayout.FloatField("", vitesseBandeauMultipler));

        GUILayout.Label("Texte de victoire");
        textVictory = EditorGUILayout.TextArea(textVictory);

        GUILayout.Label("Texte de defaite");
        textDefeat= EditorGUILayout.TextArea(textDefeat);

        GUILayout.Label("Taille de police");
        textSize = Mathf.Max(10, EditorGUILayout.IntField(textSize));


        GUILayout.Label("Delai du bandeau (temps en secondes)");
        delaiBandeau = Mathf.Max(0.001f, EditorGUILayout.FloatField("", delaiBandeau));


        GUILayout.Label("Vitesse de transition (multiplicateur) ");
        vitesseInsertion = Mathf.Max(0.001f, EditorGUILayout.FloatField("", vitesseInsertion));

        GUILayout.Label("Quel animation ?");
        anim = (animHero)EditorGUILayout.EnumPopup(anim);

        sprt = (Sprite)EditorGUILayout.ObjectField("Quel sprite de fond ? ", sprt, typeof(Sprite), true);

        GUILayout.Label("IMPORTANT SINON CA MARCHE PAS ");
        model = (GameObject)EditorGUILayout.ObjectField("Modele de base", model, typeof(GameObject), true);

        GUILayout.Label("Code ? (une seule lettre et en majuscule)");
        code = GUILayout.TextField(code);

        GUILayout.Label("Position x ");
        x = EditorGUILayout.FloatField(x);

        GUILayout.Label("Position y");
        y = EditorGUILayout.FloatField(y);

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

        if (GUILayout.Button("Supprimer la derniere frappe de la liste actuelle"))
        {
            if (sequences[sequence].dI.Count > 0)
            {
                sequences[sequence].dI.RemoveAt(sequences[sequence].dI.Count - 1);
            }
        }

        if (GUILayout.Button("Liste de frappe suivante"))
        {
            sequence++;
            if (sequence >= sequences.Count)
            {
                sequences.Add(new SequenceInput());
            }
        }

        if (GUILayout.Button("Liste de frappe precedente"))
        {
            sequence = Mathf.Max(0, sequence - 1);
        }

        EditorGUILayout.LabelField("Index : " + sequence.ToString());
        foreach(SequenceInput sI in sequences)
        {
            foreach(string s in sI.ToStringAlt())
            {
                EditorGUILayout.LabelField(s);
            }
        }

        GUILayout.Label("Sons Optionnels (en prendra un par defaut si aucun n'est choisi)");
        clpFail = (AudioClip)EditorGUILayout.ObjectField("Mauvaise touche ", clpFail, typeof(AudioClip), true);
        clpSuccess = (AudioClip)EditorGUILayout.ObjectField("Bonne touche ", clpSuccess, typeof(AudioClip), true);
        clpSuccessScenette = (AudioClip)EditorGUILayout.ObjectField("Fin scenette positive ", clpSuccessScenette, typeof(AudioClip), true);


        if (GUILayout.Button("Créer"))
        {
            GameObject instance;
            instance = Instantiate(model);
            instance.name = nameObject;
            Scenette scn = instance.GetComponent<Scenette>();
            scn.sequencesToDo = new List<SequenceInput>(sequences);
            scn.speedBandeauMultipler = vitesseBandeauMultipler;
            scn.timeBeforeNext = delaiBandeau;
            scn.mutliplerSpeedEnter = vitesseInsertion;
            scn.failClic = clpFail;
            scn.successClic = clpSuccess;
            scn.successScenette = clpSuccessScenette;
            scn.animPourHero = anim;
            scn.backgroundSprite = sprt;
            scn.textSize = textSize;
            scn.textVictory = textVictory;
            scn.textDefeat = textDefeat;
            scn.init();

            sequences = new List<SequenceInput>();
        }
    }
}
