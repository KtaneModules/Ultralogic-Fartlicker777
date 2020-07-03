using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class Ultralogic : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable Goose;
    public GameObject[] ABC;
    public GameObject[] BCA;
    public GameObject[] CAB;
    public GameObject[] ABCBlocks;
    public GameObject[] ABOperators;
    public GameObject[] ACOperators;
    public GameObject[] BCOperators;
    public KMSelectable[] OperatorsButClickie;
    public Material[] colores;

    bool[] Funny = {false,false,false};
    int[] Numero = {0,0,0};
    int[] Whatever = {0,1,2,3,4,5}; //AND NAND NOR OR XNOR XOR
    int[] Whatevertwo = {0,1,2,3,4,5}; //AND NAND NOR OR XNOR XOR
    int[] Whateverelected = {0,1,2,3,4,5}; //AND NAND NOR OR XNOR XOR
    bool[] AnswerToisAwesome = {false,false,false};
    int Aids = 0;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;
        Goose.OnInteract += delegate () { GoosePress(); return false; };
        foreach (KMSelectable LunaPlaturion in OperatorsButClickie) {
            LunaPlaturion.OnInteract += delegate () { LunaPlaturionPress(LunaPlaturion); return false; };
        }
    }

    void Start () {
      Aids = UnityEngine.Random.Range(0,8);
      for (int i = 0; i < 3; i++) {
        ABCBlocks[i].GetComponent<MeshRenderer>().material = colores[0];
      }
      switch (Aids) {
        case 0:
        Funny[0] = true;
        break;
        case 1:
        Funny[1] = true;
        break;
        case 2:
        Funny[2] = true;
        break;
        case 3:
        Funny[0] = true;
        Funny[1] = true;
        break;
        case 4:
        Funny[0] = true;
        Funny[2] = true;
        break;
        case 5:
        Funny[1] = true;
        Funny[2] = true;
        break;
        case 6:
        Funny[0] = true;
        Funny[1] = true;
        Funny[2] = true;
        break;
        case 7:
        Funny[0] = false;
        Funny[1] = false;
        Funny[2] = false;
        break;
      }
      for (int i = 0; i < 3; i++) {
        if (Funny[i] == true) {
          ABCBlocks[i].GetComponent<MeshRenderer>().material = colores[1];
        }
      }
      Whatever.Shuffle();
      Whatevertwo.Shuffle();
      Whateverelected.Shuffle();
      Debug.LogFormat("[Ultralogic #{0}] The truth values are {1}, {2}, {3}.", moduleId, Funny[0], Funny[1], Funny[2]);
    }

    void LunaPlaturionPress(KMSelectable LunaPlaturion){
      for (int i = 0; i < 3; i++) {
        if (LunaPlaturion == OperatorsButClickie[i]) {
          Numero[i] += 1;
          if (Numero[i] == 6) {
            Numero[i] = 0;
          }
          if (i == 0) {
            for (int j = 0; j < 6; j++) {
              ABOperators[j].gameObject.SetActive(false);
            }
            ABOperators[Whatever[Numero[0]]].gameObject.SetActive(true);
          }
          else if (i == 1) {
            for (int j = 0; j < 6; j++) {
              ACOperators[j].gameObject.SetActive(false);
            }
            ACOperators[Whatevertwo[Numero[1]]].gameObject.SetActive(true);
          }
          else if (i == 2) {
            for (int j = 0; j < 6; j++) {
              BCOperators[j].gameObject.SetActive(false);
            }
            BCOperators[Whateverelected[Numero[2]]].gameObject.SetActive(true);
          }
        }
      }
    }

    void GoosePress() {
      switch (Whatever[Numero[0]]) { //AB TO C
        case 0:
        if ((Funny[0] == true && Funny[1] == true) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
        case 1:
        if (!(Funny[0] == true && Funny[1] == true) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
        case 2:
        if (!(Funny[0] == true || Funny[1] == true) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
        case 3:
        if ((Funny[0] == true || Funny[1] == true) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
        case 4:
        if (!((Funny[0] == true && Funny[1] == false) || (Funny[0] == false && Funny[1] == true)) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
        case 5:
        if (((Funny[0] == true && Funny[1] == false) || (Funny[0] == false && Funny[1] == true)) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
      }
      switch (Whatever[Numero[1]]) { //AC TO B
        case 0:
        if ((Funny[0] == true && Funny[2] == true) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
        case 1:
        if (!(Funny[0] == true && Funny[2] == true) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
        case 2:
        if (!(Funny[0] == true || Funny[2] == true) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
        case 3:
        if ((Funny[0] == true || Funny[2] == true) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
        case 4:
        if (!((Funny[0] == true && Funny[2] == false) || (Funny[0] == false && Funny[2] == true)) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
        case 5:
        if (((Funny[0] == true && Funny[2] == false) || (Funny[0] == false && Funny[2] == true)) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
      }
      switch (Whatever[Numero[2]]) { //BC TO A
        case 0:
        if ((Funny[1] == true && Funny[2] == true) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
        case 1:
        if (!(Funny[1] == true && Funny[2] == true) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
        case 2:
        if (!(Funny[1] == true || Funny[2] == true) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
        case 3:
        if ((Funny[1] == true || Funny[2] == true) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
        case 4:
        if (!((Funny[1] == true && Funny[2] == false) || (Funny[1] == false && Funny[2] == true)) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
        case 5:
        if (((Funny[1] == true && Funny[2] == false) || (Funny[1] == false && Funny[2] == true)) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
      }
      if (AnswerToisAwesome[0] == true && AnswerToisAwesome[1] == true && AnswerToisAwesome[2] == true) {
        GetComponent<KMBombModule>().HandlePass();
      }
    }
}
