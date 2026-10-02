using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    //attatch this script to an empty gameobject
    //this script holds a list of puzzle events in the scene
    //listens to each OnCompleted
    //fires OnAllSolved when every puzzle is done

    //drag each puzzleEvent gameObject in the scene into this list
    public PuzzleEvent[] puzzles;
    //fired when every puzzle in the list is solved
    public UnityEvent OnAllSolved;
    //track when puzzles are solved
    private bool[] solvedStates;
    private bool allSolved = false;

    void Start()
    {
        if(puzzles == null || puzzles.Length == 0)
        {
            Debug.Log("Puzzle manager has no puzzles assigned.");
            return;
        }

        solvedStates = new bool[puzzles.Length];

        //subscribe to each puzzle's OnCompleted and OnReset events
        //we use a local copy of i for the lambda to capture the right index
        for(int i = 0; i < puzzles.Length; i++)
        {
            int index = i;
            puzzles[i].OnCompleted.AddListener(() => OnPuzzleSolved(index));
            puzzles[i].OnReset.AddListener(() => OnPuzzleReset(index));
        }
    }

    //call when a puzzle event fires on completed
    void OnPuzzleSolved (int index)
    {
        solvedStates[index] = true;
        Debug.Log("Puzzle " + index + " solved. Checking all puzzles...");
        CheckAllSolved();
    }
    //calls when a puzzleEvent fires OnReset
    void OnPuzzleReset (int index)
    {
        solvedStates[index] = false;
        allSolved = false;
        Debug.Log($"Puzzle {index} reset.");
    }

    //loops through all solvedStates and fires OnAllSolved
    //if every one is true, AKA completed
    void CheckAllSolved()
    {
        foreach (bool state in solvedStates)
        {
            if(state == false)
            {
                return; //if one of the puzzle states is false or not completed, exit.
            }

            if(!allSolved)
            {
                allSolved = true;
                Debug.Log("All puzzles solved!");
                OnAllSolved?.Invoke();
            }
        }
    }
}
