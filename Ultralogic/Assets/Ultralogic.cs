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
    private List<int> Whatever = new List<int>{0,1,2,3,4,5}; //AND NAND NOR OR XNOR XOR
    private List<int> Whatevertwo = new List<int>{0,1,2,3,4,5}; //AND NAND NOR OR XNOR XOR
    private List<int> Whateverelected = new List<int>{0,1,2,3,4,5}; //AND NAND NOR OR XNOR XOR
    bool[] AnswerToisAwesome = {false,false,false};
    bool[] HeyINeedToSetABool = {false, false, false};
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
      Audio.PlaySoundAtTransform("PAAAAAAAAAAAAAAAAAAAAAAAUSE",  LunaPlaturion.transform);
      LunaPlaturion.AddInteractionPunch();
      if (moduleSolved == true) {
        return;
      }
      for (int i = 0; i < 3; i++) {
        if (LunaPlaturion == OperatorsButClickie[i]) {
          Numero[i] += 1;
          if (Numero[i] == 6) {
            Numero[i] = 0;
          }
          if (HeyINeedToSetABool[i] == false) {
            HeyINeedToSetABool[i] = true;
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
      Goose.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Goose.transform);
      if (moduleSolved == true) {
        return;
      }
      if (Whatever[Numero[0]] == Whatevertwo[Numero[1]] || Whatever[Numero[0]] == Whateverelected[Numero[2]] || Whatevertwo[Numero[1]] == Whateverelected[Numero[2]]) {
        GetComponent<KMBombModule>().HandleStrike();
        Debug.LogFormat("[Ultralogic #{0}] You cannot have it be the same operator. Strike.", moduleId);
        return;
      }
      for (int i = 0; i < 3; i++) {
        if (HeyINeedToSetABool[i] == false) {
          GetComponent<KMBombModule>().HandleStrike();
          Debug.LogFormat("[Ultralogic #{0}] An operator was not set. how.", moduleId);
          return;
        }
      }
      switch (Whatever[Numero[0]]) { //AB TO C
        case 0:
        Debug.LogFormat("[Ultralogic #{0}] AB is set to AND.", moduleId);
        if ((Funny[0] == true && Funny[1] == true) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
        case 1:
        Debug.LogFormat("[Ultralogic #{0}] AB is set to NAND.", moduleId);
        if (!(Funny[0] == true && Funny[1] == true) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
        case 2:
        Debug.LogFormat("[Ultralogic #{0}] AB is set to NOR.", moduleId);
        if (!(Funny[0] == true || Funny[1] == true) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
        case 3:
        Debug.LogFormat("[Ultralogic #{0}] AB is set to OR.", moduleId);
        if ((Funny[0] == true || Funny[1] == true) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
        case 4:
        Debug.LogFormat("[Ultralogic #{0}] AB is set to XNOR.", moduleId);
        if (!((Funny[0] == true && Funny[1] == false) || (Funny[0] == false && Funny[1] == true)) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
        case 5:
        Debug.LogFormat("[Ultralogic #{0}] AB is set to XOR.", moduleId);
        if (((Funny[0] == true && Funny[1] == false) || (Funny[0] == false && Funny[1] == true)) == Funny[2]) {
          AnswerToisAwesome[0] = true;
        }
        break;
      }
      switch (Whatevertwo[Numero[1]]) { //AC TO B
        case 0:
        Debug.LogFormat("[Ultralogic #{0}] AC is set to AND.", moduleId);
        if ((Funny[0] == true && Funny[2] == true) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
        case 1:
        Debug.LogFormat("[Ultralogic #{0}] AC is set to NAND.", moduleId);
        if (!(Funny[0] == true && Funny[2] == true) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
        case 2:
        Debug.LogFormat("[Ultralogic #{0}] AC is set to NOR.", moduleId);
        if (!(Funny[0] == true || Funny[2] == true) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
        case 3:
        Debug.LogFormat("[Ultralogic #{0}] AC is set to OR.", moduleId);
        if ((Funny[0] == true || Funny[2] == true) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
        case 4:
        Debug.LogFormat("[Ultralogic #{0}] AC is set to XNOR.", moduleId);
        if (!((Funny[0] == true && Funny[2] == false) || (Funny[0] == false && Funny[2] == true)) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
        case 5:
        Debug.LogFormat("[Ultralogic #{0}] AC is set to XOR.", moduleId);
        if (((Funny[0] == true && Funny[2] == false) || (Funny[0] == false && Funny[2] == true)) == Funny[1]) {
          AnswerToisAwesome[1] = true;
        }
        break;
      }
      switch (Whateverelected[Numero[2]]) { //BC TO A
        case 0:
        Debug.LogFormat("[Ultralogic #{0}] BC is set to AND.", moduleId);
        if ((Funny[1] == true && Funny[2] == true) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
        case 1:
        Debug.LogFormat("[Ultralogic #{0}] BC is set to NAND.", moduleId);
        if (!(Funny[1] == true && Funny[2] == true) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
        case 2:
        Debug.LogFormat("[Ultralogic #{0}] BC is set to NOR.", moduleId);
        if (!(Funny[1] == true || Funny[2] == true) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
        case 3:
        Debug.LogFormat("[Ultralogic #{0}] BC is set to OR.", moduleId);
        if ((Funny[1] == true || Funny[2] == true) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
        case 4:
        Debug.LogFormat("[Ultralogic #{0}] BC is set to XNOR.", moduleId);
        if (!((Funny[1] == true && Funny[2] == false) || (Funny[1] == false && Funny[2] == true)) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
        case 5:
        Debug.LogFormat("[Ultralogic #{0}] BC is set to XOR.", moduleId);
        if (((Funny[1] == true && Funny[2] == false) || (Funny[1] == false && Funny[2] == true)) == Funny[0]) {
          AnswerToisAwesome[2] = true;
        }
        break;
      }
      if (AnswerToisAwesome[0] == true && AnswerToisAwesome[1] == true && AnswerToisAwesome[2] == true) {
        if (Funny[0] == true) {
          ABC[0].GetComponent<MeshRenderer>().material = colores[1];
          ABC[2].GetComponent<MeshRenderer>().material = colores[1];
          BCA[0].GetComponent<MeshRenderer>().material = colores[1];
          BCA[2].GetComponent<MeshRenderer>().material = colores[1];
          BCA[3].GetComponent<MeshRenderer>().material = colores[1];
          BCA[10].GetComponent<MeshRenderer>().material = colores[1];
          CAB[0].GetComponent<MeshRenderer>().material = colores[1];
          CAB[1].GetComponent<MeshRenderer>().material = colores[1];
          CAB[2].GetComponent<MeshRenderer>().material = colores[1];
        }
        else {
          ABC[0].GetComponent<MeshRenderer>().material = colores[0];
          ABC[2].GetComponent<MeshRenderer>().material = colores[0];
          BCA[0].GetComponent<MeshRenderer>().material = colores[0];
          BCA[2].GetComponent<MeshRenderer>().material = colores[0];
          BCA[3].GetComponent<MeshRenderer>().material = colores[0];
          BCA[10].GetComponent<MeshRenderer>().material = colores[0];
          CAB[0].GetComponent<MeshRenderer>().material = colores[0];
          CAB[1].GetComponent<MeshRenderer>().material = colores[0];
          CAB[2].GetComponent<MeshRenderer>().material = colores[0];
        }
        if (Funny[1] == true) {
          ABC[1].GetComponent<MeshRenderer>().material = colores[1];
          ABC[3].GetComponent<MeshRenderer>().material = colores[1];
          BCA[1].GetComponent<MeshRenderer>().material = colores[1];
          BCA[4].GetComponent<MeshRenderer>().material = colores[1];
          BCA[9].GetComponent<MeshRenderer>().material = colores[1];
          CAB[7].GetComponent<MeshRenderer>().material = colores[1];
          CAB[8].GetComponent<MeshRenderer>().material = colores[1];
          CAB[9].GetComponent<MeshRenderer>().material = colores[1];
          CAB[10].GetComponent<MeshRenderer>().material = colores[1];
          CAB[11].GetComponent<MeshRenderer>().material = colores[1];
        }
        else {
          ABC[1].GetComponent<MeshRenderer>().material = colores[0];
          ABC[3].GetComponent<MeshRenderer>().material = colores[0];
          BCA[1].GetComponent<MeshRenderer>().material = colores[0];
          BCA[4].GetComponent<MeshRenderer>().material = colores[0];
          BCA[9].GetComponent<MeshRenderer>().material = colores[0];
          CAB[7].GetComponent<MeshRenderer>().material = colores[0];
          CAB[8].GetComponent<MeshRenderer>().material = colores[0];
          CAB[9].GetComponent<MeshRenderer>().material = colores[0];
          CAB[10].GetComponent<MeshRenderer>().material = colores[0];
          CAB[11].GetComponent<MeshRenderer>().material = colores[0];
        }
        if (Funny[2] == true) {
          BCA[5].GetComponent<MeshRenderer>().material = colores[1];
          BCA[6].GetComponent<MeshRenderer>().material = colores[1];
          BCA[7].GetComponent<MeshRenderer>().material = colores[1];
          BCA[8].GetComponent<MeshRenderer>().material = colores[1];
          CAB[3].GetComponent<MeshRenderer>().material = colores[1];
          CAB[4].GetComponent<MeshRenderer>().material = colores[1];
          CAB[5].GetComponent<MeshRenderer>().material = colores[1];
          CAB[6].GetComponent<MeshRenderer>().material = colores[1];
        }
        else {
          BCA[5].GetComponent<MeshRenderer>().material = colores[0];
          BCA[6].GetComponent<MeshRenderer>().material = colores[0];
          BCA[7].GetComponent<MeshRenderer>().material = colores[0];
          BCA[8].GetComponent<MeshRenderer>().material = colores[0];
          CAB[3].GetComponent<MeshRenderer>().material = colores[0];
          CAB[4].GetComponent<MeshRenderer>().material = colores[0];
          CAB[5].GetComponent<MeshRenderer>().material = colores[0];
          CAB[6].GetComponent<MeshRenderer>().material = colores[0];
        }
        Audio.PlaySoundAtTransform("BWAAAAH",  Goose.transform);
        GetComponent<KMBombModule>().HandlePass();
        Debug.LogFormat("[Ultralogic #{0}] All operators are set correctly, module disarmed.", moduleId);
        moduleSolved = true;
      }
      else {
        GetComponent<KMBombModule>().HandleStrike();
        Debug.LogFormat("[Ultralogic #{0}] An operator(s) is set incorrectly, strike.", moduleId);
      }
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use (!{0} set AB/AC/BC AND/OR/XOR/NAND/NOR/XNOR) to set the corresponding operator. Use !{0} submit to submit.";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command){
      command = command.Trim();
      string[] parameters = command.Split(' ');
      if (parameters.Length > 3) {
        yield return "sendtochaterror Too many commands!";
        yield break;
      }
      if (parameters.Length == 1 && parameters[0].ToString().ToLower() != "submit") {
        yield return "sendtochaterror Too little commands!";
        yield break;
      }
      if (parameters[0].ToString().ToLower() != "set" && parameters[0].ToString().ToLower() != "submit") {
        yield return "sendtochaterror Invalid command!";
        yield break;
      }
      else if (parameters[0].ToString().ToLower() == "submit") {
        GoosePress();
        yield break;
      }
      else if (parameters[1].ToString().ToLower() == "ab" || parameters[1].ToString().ToLower() == "ba") {
        switch (parameters[2].ToString().ToLower()) {
          case "and":
          if (HeyINeedToSetABool[0] == false) {
            OperatorsButClickie[0].OnInteract();
          }
          while (Whatever[Numero[0]] != 0) {
            OperatorsButClickie[0].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "nand":
          if (HeyINeedToSetABool[0] == false) {
            OperatorsButClickie[0].OnInteract();
          }
          while (Whatever[Numero[0]] != 1) {
            OperatorsButClickie[0].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "nor":
          if (HeyINeedToSetABool[0] == false) {
            OperatorsButClickie[0].OnInteract();
          }
          while (Whatever[Numero[0]] != 2) {
            OperatorsButClickie[0].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "or":
          if (HeyINeedToSetABool[0] == false) {
            OperatorsButClickie[0].OnInteract();
          }
          while (Whatever[Numero[0]] != 3) {
            OperatorsButClickie[0].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "xnor":
          if (HeyINeedToSetABool[0] == false) {
            OperatorsButClickie[0].OnInteract();
          }
          while (Whatever[Numero[0]] != 4) {
            OperatorsButClickie[0].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "xor":
          if (HeyINeedToSetABool[0] == false) {
            OperatorsButClickie[0].OnInteract();
          }
          while (Whatever[Numero[0]] != 5) {
            OperatorsButClickie[0].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
        }
        yield break;
      }
      else if (parameters[1].ToString().ToLower() == "ac" || parameters[1].ToString().ToLower() == "ca") {
        switch (parameters[2].ToString().ToLower()) {
          case "and":
          if (HeyINeedToSetABool[1] == false) {
            OperatorsButClickie[1].OnInteract();
          }
          while (Whatevertwo[Numero[1]] != 0) {
            OperatorsButClickie[1].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "nand":
          if (HeyINeedToSetABool[1] == false) {
            OperatorsButClickie[1].OnInteract();
          }
          while (Whatevertwo[Numero[1]] != 1) {
            OperatorsButClickie[1].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "nor":
          if (HeyINeedToSetABool[1] == false) {
            OperatorsButClickie[1].OnInteract();
          }
          while (Whatevertwo[Numero[1]] != 2) {
            OperatorsButClickie[1].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "or":
          if (HeyINeedToSetABool[1] == false) {
            OperatorsButClickie[1].OnInteract();
          }
          while (Whatevertwo[Numero[1]] != 3) {
            OperatorsButClickie[1].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "xnor":
          if (HeyINeedToSetABool[1] == false) {
            OperatorsButClickie[1].OnInteract();
          }
          while (Whatevertwo[Numero[1]] != 4) {
            OperatorsButClickie[1].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "xor":
          if (HeyINeedToSetABool[1] == false) {
            OperatorsButClickie[1].OnInteract();
          }
          while (Whatevertwo[Numero[1]] != 5) {
            OperatorsButClickie[1].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
        }
        yield break;
      }
      else if (parameters[1].ToString().ToLower() == "bc" || parameters[1].ToString().ToLower() == "cb") {
        switch (parameters[2].ToString().ToLower()) {
          case "and":
          if (HeyINeedToSetABool[2] == false) {
            OperatorsButClickie[2].OnInteract();
          }
          while (Whateverelected[Numero[2]] != 0) {
            OperatorsButClickie[2].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "nand":
          if (HeyINeedToSetABool[2] == false) {
            OperatorsButClickie[2].OnInteract();
          }
          while (Whateverelected[Numero[2]] != 1) {
            OperatorsButClickie[2].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "nor":
          if (HeyINeedToSetABool[2] == false) {
            OperatorsButClickie[2].OnInteract();
          }
          while (Whateverelected[Numero[2]] != 2) {
            OperatorsButClickie[2].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "or":
          if (HeyINeedToSetABool[2] == false) {
            OperatorsButClickie[2].OnInteract();
          }
          while (Whateverelected[Numero[2]] != 3) {
            OperatorsButClickie[2].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "xnor":
          if (HeyINeedToSetABool[2] == false) {
            OperatorsButClickie[2].OnInteract();
          }
          while (Whateverelected[Numero[2]] != 4) {
            OperatorsButClickie[2].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
          case "xor":
          if (HeyINeedToSetABool[2] == false) {
            OperatorsButClickie[2].OnInteract();
          }
          while (Whateverelected[Numero[2]] != 5) {
            OperatorsButClickie[2].OnInteract();
            yield return new WaitForSeconds(0.1f);
          }
          break;
        }
        yield break;
      }
      else {
        yield return "sendtochaterror Invalid command!";
        yield break;
      }
    }
}
