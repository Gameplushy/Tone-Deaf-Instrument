using KeepCoding;
using KModkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RNG = UnityEngine.Random;

public class ToneDeafInstrumentScript : ModuleScript {

	public KMSelectable[] Valves;
	public Animator[] Anims;
	private const string BUTTONPRESS = "isPressed";
	private long tickets = 0;
	private long lookingFor = 0;

	private int valveMat;
	public MeshRenderer[] ValveMats;
	private int baseMat;
	public MeshRenderer[] BaseMats;
	public Material[] mats;

	private ValveFunction[] valveFunctions;
	private int previousValve;
	private static readonly ValveFunction[][] initialStates =
	{
		new ValveFunction[3] { ValveFunction.Higher, ValveFunction.Same, ValveFunction.Lower },
		new ValveFunction[3] { ValveFunction.Same, ValveFunction.Higher, ValveFunction.Lower },
		new ValveFunction[3] { ValveFunction.Lower, ValveFunction.Higher, ValveFunction.Same },
		new ValveFunction[3] { ValveFunction.Higher, ValveFunction.Lower, ValveFunction.Same },
		new ValveFunction[3] { ValveFunction.Lower, ValveFunction.Same, ValveFunction.Higher },
		new ValveFunction[3] { ValveFunction.Same, ValveFunction.Lower, ValveFunction.Higher },
	};

	private Note[] songUsed;
	private int noteIndex=0;
	private static readonly Note[][] songList =
	{
		new Note[]{Note.Bong,Note.Bong,Note.Bam,Note.Bam,Note.Vroom,Note.Bim,Note.Bam,Note.Bam,Note.Boum,Note.Boum,Note.Vroom,Note.Bim,Note.Bam,Note.Dang,Note.Bim,Note.Bim,Note.Pschit,Note.Boum,Note.Bim,Note.Bang,Note.Dong }, //0
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //1
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //2
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //3
		new Note[]{Note.Bing,Note.Bong,Note.Bong,Note.Bong,Note.Bing,Note.Dang,Note.Bing,Note.Bong,Note.Boum,Note.Bong,Note.Bing,Note.Dong,Note.Bong,Note.Bing,Note.Bong,Note.Bong,Note.Bong,Note.Bing,Note.Dang,Note.Bing,Note.Bong,Note.Boum,Note.Bong,Note.Boum,Note.Bong,Note.Bing}, //4
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //5
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //6
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //7
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //8
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //9
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //A
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //B
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //C
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //D
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //E
		new Note[]{Note.Bam,Note.Bam,Note.Vroom,Note.Bam,Note.Bam,Note.Bam,Note.Vroom,Note.Bam,Note.Bang,Note.Bang,Note.Bim,Note.Bang,Note.Bim,Note.Bong,Note.Bing,Note.Bong,Note.Bing,Note.Bing,Note.Pschit,Note.Dong,Note.Bing,Note.Bong,Note.Bim,Note.Bam }, //F
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //G
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //H
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //I
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //J
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //K
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //L
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //M
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //N
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //O
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //P
		new Note[]{Note.Bam,Note.Bong,Note.Bang,Note.Dang,Note.Bing,Note.Ding,Note.Bing,Note.Ding,Note.Bing,Note.Pschit,Note.Dong,Note.Bing,Note.Bim,Note.Bam,Note.Pschit,Note.Bim,Note.Bam,Note.Dang,Note.Bam,Note.Dang,Note.Bam,Note.Boum,Note.Pschit,Note.Bam,Note.Bang,Note.Bam,Note.Ding,Note.Bam,Note.Bim,Note.Bing,Note.Dang}, //Q
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //R
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //S
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //T
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //U
		new Note[]{Note.Bam,Note.Bong,Note.Bam,Note.Vroom,Note.Bam,Note.Bong,Note.Bam,Note.Vroom,Note.Pschit,Note.Boum,Note.Bim,Note.Pschit,Note.Bam,Note.Bong,Note.Bam,Note.Vroom,Note.Bam,Note.Bong,Note.Bam,Note.Vroom,Note.Pschit,Note.Boum,Note.Bim,Note.Dong,Note.Ding}, //V
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //W
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //X
		new Note[]{Note.Bam,Note.Bong,Note.Vroom}, //Y
		new Note[]{Note.Bam, Note.Bam, Note.Bam, Note.Bam, Note.Bam, Note.Bam, Note.Bam, Note.Bam, Note.Bam, Note.Bam, Note.Bam, Note.Bam} //Z
	};
	private static readonly string[] songNames =
	{
		"0-Le Dernier Jour Du Disco",
		"",
		"",
		"",
		"4-Romeo And Cinderella",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"F-The Positive And The Negative",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"",
		"Q-Civilization Of Magic",
		"",
		"",
		"",
		"",
		"V-Now, Until The Moment You Die",
		"",
		"",
		"",
		"Z-Look What You Made Me Do"
	};
	private AudioClip solveNoise;
	public AudioClip[] solveSfx;

	private static readonly string[] logVPos = { "upper", "middle", "lower" };

	private int pitch = 4;

	// Use this for initialization
	void Start () {
		valveMat = RNG.Range(0, 3);
		baseMat = RNG.Range(0, 2);
		ValveMats.ForEach(v => v.material = mats[valveMat]);
		BaseMats.ForEach(b => b.material = mats[baseMat]);
		Log("Valves are in {0}.", mats[valveMat].name);
		Log("Bases are in {0}.", mats[baseMat].name);
		valveFunctions = initialStates[valveMat + baseMat * 3];
		Log("Valves initial functions are {0}", valveFunctions);
		int index = Helper.Modulo(Helper.Alphanumeric.IndexOf(Get<KMBombInfo>().GetSerialNumber()[0]) + (Helper.Alphabet.Contains(Get<KMBombInfo>().GetSerialNumber()[1].ToString()) ? 1 : -1), songList.Length);
		songUsed = songList[index];
		solveNoise = solveSfx[index];
		Log("Song used is \"{0}\"", songNames[index]);
		LogAnswer();
		Valves.Assign(onInteract: Push);
	}

    private void Push(int valvePressed)
    {
		ValveFunction? noiseType = valveFunctions[valvePressed];
		bool isSolvingPress = false;
        if (!IsSolved)
        {
			ValveFunction target;
			if (noteIndex == 0) target = ValveFunction.Same;
			else
			{
				int compared = songUsed[noteIndex].CompareTo(songUsed[noteIndex - 1]);
				if (compared == 0) target = ValveFunction.Same;
				else if (compared < 0) target = ValveFunction.Lower;
				else target = ValveFunction.Higher;
			}
			if (valveFunctions[valvePressed] == target)
			{
				Log("You pressed the {0} valve correctly.", logVPos[valvePressed]);
				if (++noteIndex == songUsed.Length) { 
					Solve(); 
					Log("Module solved!");
					isSolvingPress = true;
				}
				else if (noteIndex != 1 && RNG.Range(0,6)==5) { //Breaking mechanic
					noiseType = null;
					Log("The module just broke! Swap the functions of the {0} and {1} valves.", logVPos[previousValve], logVPos[valvePressed]);
					ValveFunction tmp = valveFunctions[previousValve];
					valveFunctions[previousValve] = valveFunctions[valvePressed];
					valveFunctions[valvePressed]=tmp;
				}
				previousValve = valvePressed;
			}
			else
			{
				Log("You pressed the {0} valve when you should have pressed the {1}. Strike and reset!", logVPos[valvePressed],logVPos[valveFunctions.IndexOf(target)]);
				Strike();
				noteIndex = 0;
				valveFunctions = initialStates[valveMat + baseMat * 3]; //TODO : FIX THIS
			}
		}
		StartCoroutine(TheWaitingGame(valvePressed,noiseType,isSolvingPress));
	}

    private IEnumerator TheWaitingGame(int obj, ValveFunction? noiseType, bool isSolving)
    {
		long ticket = tickets++;
		yield return new WaitUntil(()=>ticket==lookingFor && !Anims[obj].GetCurrentAnimatorStateInfo(0).IsName("press"));
		Anims[obj].SetTrigger(BUTTONPRESS);
        switch (noiseType)
        {
			
			case ValveFunction.Higher:
				pitch = Math.Min(9, pitch + 1);
				goto case ValveFunction.Same;
			case ValveFunction.Lower:
				pitch = Math.Max(0, pitch - 1);
				goto case ValveFunction.Same;
			case ValveFunction.Same:
				PlaySound("trump" + pitch);
				break;
			case null:
				PlaySound("fake");
				break;
        }
		yield return new WaitForSeconds(.2f);
		if (isSolving) PlaySound(false, solveNoise);
		lookingFor++;
    }

	private void LogAnswer()
    {
		List<ValveFunction> presses = new List<ValveFunction>();
		for(int i=0;i<songUsed.Length;i++)
        {
			if (presses.IsNullOrEmpty()) presses.Add(ValveFunction.Same);
			else
			{
				ValveFunction target;
				int compared = songUsed[i].CompareTo(songUsed[i - 1]);
				if (compared == 0) target = ValveFunction.Same;
				else if (compared < 0) target = ValveFunction.Lower;
				else target = ValveFunction.Higher;
				presses.Add(target);
			}
        }
		Log("Presses the valves who has these functions, in this order : {0}",presses.Join(","));
    }
}
