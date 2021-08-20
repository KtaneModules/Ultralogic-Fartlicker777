using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rnd = UnityEngine.Random;

public class Ultralogic : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   public KMSelectable Goose;
   public GameObject[] ABCBlocks;
   public GameObject[] ACOperators;
   public GameObject[] BCOperators;
   public KMSelectable[] OperatorsButClickie;
   public Material[] colores;

   public SpriteRenderer[] Operators;
   public Sprite[] OperatorSprites;

   public GameObject[] AConnectors;
   public GameObject[] BConnectors;
   public GameObject[] CConnectors;

   private List<int> TMOperators = new List<int> { 0, 1, 2, 3, 4, 5 };        //AND NAND NOR OR XNOR XOR
   private List<int> BLOperators = new List<int> { 0, 1, 2, 3, 4, 5 };        //AND NAND NOR OR XNOR XOR
   private List<int> BROperators = new List<int> { 0, 1, 2, 3, 4, 5 };        //AND NAND NOR OR XNOR XOR
   int[] OperatorTrackers = new int[3];

   bool[] ABCTruthValues = new bool[3];
   bool[] StatementValidities = new bool[3];

   static int moduleIdCounter = 1;
   int moduleId;
   private bool moduleSolved;

   void Awake () {
      moduleId = moduleIdCounter++;
      Goose.OnInteract += delegate () { SubmitPress(); return false; };
      foreach (KMSelectable OperatorButton in OperatorsButClickie) {
         OperatorButton.OnInteract += delegate () { OperatorButtonPress(OperatorButton); return false; };
      }
   }

   void Start () {
      TMOperators.Shuffle();
      BLOperators.Shuffle();
      BROperators.Shuffle();
      for (int i = 0; i < 3; i++) {
         ABCTruthValues[i] = rnd.Range(0, 2) == 1;
         if (ABCTruthValues[i]) {
            ABCBlocks[i].GetComponent<MeshRenderer>().material = colores[1];
         }
         OperatorTrackers[i] = rnd.Range(0, 6);
         switch (i) {
            case 0: Operators[i].GetComponent<SpriteRenderer>().sprite = OperatorSprites[TMOperators[OperatorTrackers[i]]]; break;
            case 1: Operators[i].GetComponent<SpriteRenderer>().sprite = OperatorSprites[BLOperators[OperatorTrackers[i]]]; break;
            case 2: Operators[i].GetComponent<SpriteRenderer>().sprite = OperatorSprites[BROperators[OperatorTrackers[i]]]; break;
         }
      }
      Debug.LogFormat("[Ultralogic #{0}] The truth values are {1}, {2}, {3}.", moduleId, ABCTruthValues[0], ABCTruthValues[1], ABCTruthValues[2]);
   }

   void OperatorButtonPress (KMSelectable OperatorButton) {
      OperatorButton.AddInteractionPunch();
      Audio.PlaySoundAtTransform("PAAAAAAAAAAAAAAAAAAAAAAAUSE", OperatorButton.transform);
      if (moduleSolved) {
         return;
      }
      for (int i = 0; i < 3; i++) {
         if (OperatorButton == OperatorsButClickie[i]) {
            OperatorTrackers[i] = (OperatorTrackers[i] + 1) % 6;
            switch (i) {
               case 0: Operators[i].GetComponent<SpriteRenderer>().sprite = OperatorSprites[TMOperators[OperatorTrackers[i]]]; break;
               case 1: Operators[i].GetComponent<SpriteRenderer>().sprite = OperatorSprites[BLOperators[OperatorTrackers[i]]]; break;
               case 2: Operators[i].GetComponent<SpriteRenderer>().sprite = OperatorSprites[BROperators[OperatorTrackers[i]]]; break;
            }
         }
      }
   }

   void SubmitPress () {
      Goose.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Goose.transform);
      if (moduleSolved) {
         return;
      }
      if (TMOperators[OperatorTrackers[0]] == BLOperators[OperatorTrackers[1]] || TMOperators[OperatorTrackers[0]] == BROperators[OperatorTrackers[2]] || BLOperators[OperatorTrackers[1]] == BROperators[OperatorTrackers[2]]) {
         GetComponent<KMBombModule>().HandleStrike();
         Debug.LogFormat("[Ultralogic #{0}] You cannot have it be the same operator. Strike.", moduleId);
         return;
      }
      string Type = "";
      switch (TMOperators[OperatorTrackers[0]]) { //AB TO C
         case 0:
            Type = "AND";
            break;
         case 1:
            Type = "NAND";
            break;
         case 2:
            Type = "NOR";
            break;
         case 3:
            Type = "OR";
            break;
         case 4:
            Type = "XNOR";
            break;
         case 5:
            Type = "XOR";
            break;
      }
      Debug.LogFormat("[Ultralogic #{0}] AB is set to {1}.", moduleId, Type);
      StatementValidities[0] = CalculateValidities(ABCTruthValues[0], ABCTruthValues[1], Type) == ABCTruthValues[2];
      switch (BLOperators[OperatorTrackers[1]]) { //AC TO B
         case 0:
            Type = "AND";
            break;
         case 1:
            Type = "NAND";
            break;
         case 2:
            Type = "NOR";
            break;
         case 3:
            Type = "OR";
            break;
         case 4:
            Type = "XNOR";
            break;
         case 5:
            Type = "XOR";
            break;
      }
      Debug.LogFormat("[Ultralogic #{0}] AC is set to {1}.", moduleId, Type);
      StatementValidities[1] = CalculateValidities(ABCTruthValues[0], ABCTruthValues[2], Type) == ABCTruthValues[1];
      switch (BROperators[OperatorTrackers[2]]) { //BC TO A
         case 0:
            Type = "AND";
            break;
         case 1:
            Type = "NAND";
            break;
         case 2:
            Type = "NOR";
            break;
         case 3:
            Type = "OR";
            break;
         case 4:
            Type = "XNOR";
            break;
         case 5:
            Type = "XOR";
            break;
      }
      Debug.LogFormat("[Ultralogic #{0}] BC is set to {1}.", moduleId, Type);
      StatementValidities[2] = CalculateValidities(ABCTruthValues[1], ABCTruthValues[2], Type) == ABCTruthValues[0];
      if (StatementValidities[0] && StatementValidities[1] && StatementValidities[2]) {
         if (ABCTruthValues[0]) {
            for (int i = 0; i < AConnectors.Length; i++) {
               AConnectors[i].GetComponent<MeshRenderer>().material = colores[1];
            }
         }
         else {
            for (int i = 0; i < AConnectors.Length; i++) {
               AConnectors[i].GetComponent<MeshRenderer>().material = colores[0];
            }
         }
         if (ABCTruthValues[1]) {
            for (int i = 0; i < BConnectors.Length; i++) {
               BConnectors[i].GetComponent<MeshRenderer>().material = colores[1];
            }
         }
         else {
            for (int i = 0; i < BConnectors.Length; i++) {
               BConnectors[i].GetComponent<MeshRenderer>().material = colores[0];
            }
         }
         if (ABCTruthValues[2]) {
            for (int i = 0; i < CConnectors.Length; i++) {
               CConnectors[i].GetComponent<MeshRenderer>().material = colores[1];
            }
         }
         else {
            for (int i = 0; i < CConnectors.Length; i++) {
               CConnectors[i].GetComponent<MeshRenderer>().material = colores[0];
            }
         }
         GetComponent<KMBombModule>().HandlePass();
         Debug.LogFormat("[Ultralogic #{0}] All operators are set correctly, module disarmed.", moduleId);
         moduleSolved = true;
      }
      else {
         GetComponent<KMBombModule>().HandleStrike();
         Debug.LogFormat("[Ultralogic #{0}] An operator(s) is set incorrectly, strike.", moduleId);
      }
   }

   bool CalculateValidities (bool First, bool Second, string Type) {
      switch (Type) {
         case "AND":
            return First & Second;
         case "NAND":
            return !(First & Second);
         case "OR":
            return First | Second;
         case "NOR":
            return !(First | Second);
         case "XOR":
            return First ^ Second;
         case "XNOR":
            return !(First ^ Second);
         default:
            return true;
      }
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use (!{0} set AB/AC/BC AND/OR/XOR/NAND/NOR/XNOR) to set the corresponding operator. Use !{0} submit to submit.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string command) {
      command = command.ToLower().Trim();
      string[] parameters = command.Split(' ');
      string[] OperatorList = { "and", "nand", "nor", "or", "xnor", "xor" };
      yield return null;
      if (parameters.Length > 3 || (parameters.Length == 1 && parameters[0] != "submit")) {
         yield return "sendtochaterror I don't understand!";
         yield break;
      }
      if (parameters[0] != "set" && parameters[0] != "submit") {
         yield return "sendtochaterror I don't understand!";
         yield break;
      }
      if (parameters[0] == "submit") {
         SubmitPress();
      }
      else if (!OperatorList.Join().Contains(parameters[2])) {
         yield return "sendtochaterror I don't understand!";
         yield break;
      }
      else if (parameters[1] == "ab" || parameters[1] == "ba") {
         while (TMOperators[OperatorTrackers[0]] != Array.IndexOf(OperatorList, parameters[2])) {
            OperatorsButClickie[0].OnInteract();
            yield return new WaitForSeconds(0.1f);
         }
      }
      else if (parameters[1] == "ac" || parameters[1] == "ca") {
         while (BLOperators[OperatorTrackers[1]] != Array.IndexOf(OperatorList, parameters[2])) {
            OperatorsButClickie[1].OnInteract();
            yield return new WaitForSeconds(0.1f);
         }
      }
      else if (parameters[1] == "bc" || parameters[1] == "cb") {
         while (BROperators[OperatorTrackers[2]] != Array.IndexOf(OperatorList, parameters[2])) {
            OperatorsButClickie[2].OnInteract();
            yield return new WaitForSeconds(0.1f);
         }
      }
      else {
         yield return "sendtochaterror Invalid command!";
         yield break;
      }
   }

   IEnumerator TwitchHandleForcedSolve () {
      string Type = "";
      bool Reroll = true;
      while (Reroll) {
         OperatorsButClickie[0].OnInteract();
         switch (TMOperators[OperatorTrackers[0]]) { //AB TO C
            case 0:
               Type = "AND";
               break;
            case 1:
               Type = "NAND";
               break;
            case 2:
               Type = "NOR";
               break;
            case 3:
               Type = "OR";
               break;
            case 4:
               Type = "XNOR";
               break;
            case 5:
               Type = "XOR";
               break;
         }
         if (CalculateValidities(ABCTruthValues[0], ABCTruthValues[1], Type) == ABCTruthValues[2]) {
            Reroll = false;
         }
         yield return new WaitForSecondsRealtime(.1f);
      }
      Reroll = true;
      while (Reroll) {
         OperatorsButClickie[1].OnInteract();
         switch (BLOperators[OperatorTrackers[1]]) {
            case 0:
               Type = "AND";
               break;
            case 1:
               Type = "NAND";
               break;
            case 2:
               Type = "NOR";
               break;
            case 3:
               Type = "OR";
               break;
            case 4:
               Type = "XNOR";
               break;
            case 5:
               Type = "XOR";
               break;
         }
         if (CalculateValidities(ABCTruthValues[0], ABCTruthValues[2], Type) == ABCTruthValues[1] && TMOperators[OperatorTrackers[0]] != BLOperators[OperatorTrackers[1]]) {
            Reroll = false;
         }
         yield return new WaitForSecondsRealtime(.1f);
      }
      Reroll = true;
      while (Reroll) {
         OperatorsButClickie[2].OnInteract();
         switch (BROperators[OperatorTrackers[2]]) { //BC TO A
            case 0:
               Type = "AND";
               break;
            case 1:
               Type = "NAND";
               break;
            case 2:
               Type = "NOR";
               break;
            case 3:
               Type = "OR";
               break;
            case 4:
               Type = "XNOR";
               break;
            case 5:
               Type = "XOR";
               break;
         }
         if (CalculateValidities(ABCTruthValues[1], ABCTruthValues[2], Type) == ABCTruthValues[0] && BROperators[OperatorTrackers[2]] != BLOperators[OperatorTrackers[1]] && BROperators[OperatorTrackers[2]] != TMOperators[OperatorTrackers[0]]) {
            Reroll = false;
         }
         yield return new WaitForSecondsRealtime(.1f);
      }
      Goose.OnInteract();
   }
}
