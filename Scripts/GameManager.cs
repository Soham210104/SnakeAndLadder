using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] pos;
    public GameObject[] players;
    public float speed = 3.0f;
    int posIndexR = 0; //used to continue from the position where the player stops!
    int posIndex = 0;
    bool isRolling = false;
    bool isRed = true;
    public Text redText;
    public Text yellowText;
    public Text cantMoveText;
    public Text redWon;
    public Text yellowWon;
    int sixR = 0; //to calculate the number of times 6 occurred at red.
    int sixY = 0; //to calculate the number of times 6 occurred at yellow.
    public GameObject particles;
    private bool GameOver = false;
    public AudioSource[] sounds;
    // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;
    public GameObject[] myButton;

    // Reference to sprite renderer to change sprites
    private SpriteRenderer rend;

    public int finalSide = 0;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        this.redText.enabled = true;
        this.yellowText.enabled = false;
        this.cantMoveText.enabled = false;
        this.redWon.enabled = false;
        this.yellowWon.enabled = false;
        particles.GetComponent<ParticleSystem>().Stop();
        this.myButton[0].SetActive(false);
        this.myButton[1].SetActive(false);

    }

    private void OnMouseDown()
    {
        if (!isRolling && !GameOver)
        {
            sounds[2].Stop();
            sounds[1].Stop();
            sounds[0].Play();
            StartCoroutine("RollTheDice");
        }
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
    private IEnumerator RollTheDice()
    {
        isRolling = true;
        int randomDiceSide = 0;

        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        finalSide = randomDiceSide + 1;
        //Debug.Log("The final side is " + finalSide);

        if (finalSide == 6) //Doing this for not moving the player after 6 occurs 3 times in a row in dice
        {
            if (isRed)
            {
                sixR++;
                sixY = 0;
            }
            else
            {
                sixY++;
                sixR = 0;
            }
        }
        else
        {
            sixR = 0;
            sixY = 0;
        }

        if (isRed)
        {
            if (players[0].transform.position == pos[98].transform.position && finalSide > 1)
            {
                isRed = false;
                this.redText.enabled = false;
                this.yellowText.enabled = true;
                isRolling = false;
                yield break; // Skip moving the red player for this turn
            }
            else if (players[0].transform.position == pos[97].transform.position && finalSide > 2)
            {
                isRed = false;
                this.redText.enabled = false;
                this.yellowText.enabled = true;
                isRolling = false;
                yield break; 
            }
            else if (players[0].transform.position == pos[96].transform.position && finalSide > 3)
            {
                isRed = false;
                this.redText.enabled = false;
                this.yellowText.enabled = true;
                isRolling = false;
                yield break;
            }
            else if (players[0].transform.position == pos[95].transform.position && finalSide > 4)
            {
                isRed = false;
                this.redText.enabled = false;
                this.yellowText.enabled = true;
                isRolling = false;
                yield break; 
            }
            else if (players[0].transform.position == pos[94].transform.position && finalSide > 5)
            {
                isRed = false;
                this.redText.enabled = false;
                this.yellowText.enabled = true;
                isRolling = false;
                yield break; // Skip moving the red player for this turn
            }

            StartCoroutine("MoveRedPlayer");

            if (finalSide != 6)
            {
                isRed = false;
                this.redText.enabled = false;
                this.yellowText.enabled = true;
            }
        }
        else
        {
            if (players[1].transform.position == pos[98].transform.position && finalSide > 1)
            {
                isRed = true;
                this.yellowText.enabled = false;
                this.redText.enabled = true;
                isRolling = false;
                yield break; // Skip moving the yellow player for this turn
            }
            else if (players[1].transform.position == pos[97].transform.position && finalSide > 2)
            {
                isRed = true;
                this.yellowText.enabled = false;
                this.redText.enabled = true;
                isRolling = false;
                yield break; // Skip moving the yellow player for this turn
            }
            else if (players[1].transform.position == pos[96].transform.position && finalSide > 3)
            {
                isRed = true;
                this.yellowText.enabled = false;
                this.redText.enabled = true;
                isRolling = false;
                yield break; // Skip moving the yellow player for this turn
            }
            else if (players[1].transform.position == pos[95].transform.position && finalSide > 4)
            {
                isRed = true;
                this.yellowText.enabled = false;
                this.redText.enabled = true;
                isRolling = false;
                yield break; // Skip moving the yellow player for this turn
            }
            else if (players[1].transform.position == pos[94].transform.position && finalSide > 5)
            {
                isRed = true;
                this.yellowText.enabled = false;
                this.redText.enabled = true;
                isRolling = false;
                yield break; // Skip moving the yellow player for this turn
            }

            StartCoroutine("MoveYellowPlayer");

            if (finalSide != 6)
            {
                isRed = true;
                this.yellowText.enabled = false;
                this.redText.enabled = true;
            }
        }

        if (isRed && sixY >= 3)
        {
            isRed = false;
            this.redText.enabled = false;
            this.yellowText.enabled = true;
            isRolling = false;
            yield break; // Skip moving the red player when the dice rolled number 6 is 3 times in a row
        }
        else if (!isRed && sixR >= 3)
        {
            isRed = true;
            this.yellowText.enabled = false;
            this.redText.enabled = true;
            isRolling = false;
            yield break; 
        }

        isRolling = false;
    }

    private IEnumerator MoveRedPlayer()
    {
        int targetIndexR = posIndexR + finalSide;
        if (targetIndexR >= pos.Length)
            targetIndexR = pos.Length - 1;

        int currentIndex = posIndexR;
        //This solves the problem of directly going to the position in non-sequential order
        while (currentIndex < targetIndexR)
        {
            currentIndex++;
            Vector3 targetPosition = pos[currentIndex].transform.position;
            while (players[0].transform.position != targetPosition)
            {
                Vector3 newPosition = Vector3.MoveTowards(players[0].transform.position, targetPosition, speed * Time.deltaTime);
                players[0].transform.position = newPosition;
                yield return null;
            }
        }
        posIndexR = targetIndexR;

        if (players[0].transform.position == pos[3].transform.position)
        {
            players[0].transform.position = pos[24].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndexR = 24;
            isRed = true;
            this.yellowText.enabled = false;
            this.redText.enabled = true;
        }
        else if (players[0].transform.position == pos[28].transform.position)
        {
            players[0].transform.position = pos[73].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndexR = 73;
            isRed = true;
            this.yellowText.enabled = false;
            this.redText.enabled = true;
        }
        else if (players[0].transform.position == pos[20].transform.position)
        {
            players[0].transform.position = pos[38].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndexR = 38;
            isRed = true;
            this.yellowText.enabled = false;
            this.redText.enabled = true;
        }
        else if (players[0].transform.position == pos[42].transform.position)
        {
            players[0].transform.position = pos[75].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndexR = 75;
            isRed = true;
            this.yellowText.enabled = false;
            this.redText.enabled = true;
        }
        else if (players[0].transform.position == pos[62].transform.position)
        {
            players[0].transform.position = pos[79].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndexR = 79;
            isRed = true;
            this.yellowText.enabled = false;
            this.redText.enabled = true;
        }
        else if (players[0].transform.position == pos[70].transform.position)
        {
            players[0].transform.position = pos[88].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndexR = 88;
            isRed = true;
            this.yellowText.enabled = false;
            this.redText.enabled = true;
        }

        if (players[0].transform.position == pos[29].transform.position)
        {
            players[0].transform.position = pos[6].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndexR = 6;
        }
        else if (players[0].transform.position == pos[46].transform.position)
        {
            players[0].transform.position = pos[14].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndexR = 14;
        }
        else if (players[0].transform.position == pos[55].transform.position)
        {
            players[0].transform.position = pos[18].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndexR = 18;
        }
        else if (players[0].transform.position == pos[72].transform.position)
        {
            players[0].transform.position = pos[50].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndexR = 50;
        }
        else if (players[0].transform.position == pos[81].transform.position)
        {
            players[0].transform.position = pos[41].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndexR = 41;
        }
        else if (players[0].transform.position == pos[91].transform.position)
        {
            players[0].transform.position = pos[74].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndexR = 74;
        }
        else if (players[0].transform.position == pos[97].transform.position)
        {
            players[0].transform.position = pos[54].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndexR = 54;
        }

        if (players[0].transform.position == pos[99].transform.position)
        {
            //Debug.Log("Red won");
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Stop();
            sounds[3].Play();
            particles.GetComponent<ParticleSystem>().Play();
            this.myButton[0].SetActive(true);
            this.myButton[1].SetActive(true);
            this.redWon.enabled = true;
            this.yellowWon.enabled = false;
            GameOver = true;
            this.redText.enabled = false;
            this.yellowText.enabled = false;
        }

        yield return null;
    }

    private IEnumerator MoveYellowPlayer()
    {
        int targetIndex = posIndex + finalSide;
        if (targetIndex >= pos.Length)
            targetIndex = pos.Length - 1;

        int currentIndex = posIndex;
        //This solves the problem of directly going to the position in non-sequential order
        while (currentIndex < targetIndex)
        {
            currentIndex++;
            Vector3 targetPosition = pos[currentIndex].transform.position;
            while (players[1].transform.position != targetPosition)
            {
                Vector3 newPosition = Vector3.MoveTowards(players[1].transform.position, targetPosition, speed * Time.deltaTime);
                players[1].transform.position = newPosition;
                yield return null;
            }
        }
        posIndex = targetIndex;

        if (players[1].transform.position == pos[3].transform.position)
        {
            players[1].transform.position = pos[24].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndex = 24;
            isRed = false;
            this.redText.enabled = false;
            this.yellowText.enabled = true;
        }
        else if (players[1].transform.position == pos[28].transform.position)
        {
            players[1].transform.position = pos[73].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndex = 73;
            isRed = false;
            this.redText.enabled = false;
            this.yellowText.enabled = true;
        }
        else if (players[1].transform.position == pos[20].transform.position)
        {
            players[1].transform.position = pos[38].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndex = 38;
            isRed = false;
            this.redText.enabled = false;
            this.yellowText.enabled = true;
        }
        else if (players[1].transform.position == pos[42].transform.position)
        {
            players[1].transform.position = pos[75].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndex = 75;
            isRed = false;
            this.redText.enabled = false;
            this.yellowText.enabled = true;
        }
        else if (players[1].transform.position == pos[62].transform.position)
        {
            players[1].transform.position = pos[79].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndex = 79;
            isRed = false;
            this.redText.enabled = false;
            this.yellowText.enabled = true;
        }
        else if (players[1].transform.position == pos[70].transform.position)
        {
            players[1].transform.position = pos[88].transform.position;
            sounds[0].Stop();
            sounds[2].Stop();
            sounds[1].Play();
            posIndex = 88;
            isRed = false;
            this.redText.enabled = false;
            this.yellowText.enabled = true;
        }

        if (players[1].transform.position == pos[29].transform.position)
        {
            players[1].transform.position = pos[6].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndex = 6;
        }
        else if (players[1].transform.position == pos[46].transform.position)
        {
            players[1].transform.position = pos[14].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndex = 14;
        }
        else if (players[1].transform.position == pos[55].transform.position)
        {
            players[1].transform.position = pos[18].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndex = 18;
        }
        else if (players[1].transform.position == pos[72].transform.position)
        {
            players[1].transform.position = pos[50].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndex = 50;
        }
        else if (players[1].transform.position == pos[81].transform.position)
        {
            players[1].transform.position = pos[41].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndex = 41;
        }
        else if (players[1].transform.position == pos[91].transform.position)
        {
            players[1].transform.position = pos[74].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndex = 74;
        }
        else if (players[1].transform.position == pos[97].transform.position)
        {
            players[1].transform.position = pos[54].transform.position;
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Play();
            posIndex = 54;
        }

        if (players[1].transform.position == pos[99].transform.position)
        {
            //Debug.Log("Yellow won");
            sounds[0].Stop();
            sounds[1].Stop();
            sounds[2].Stop();
            sounds[3].Play();
            particles.GetComponent<ParticleSystem>().Play();
            this.myButton[0].SetActive(true);
            this.myButton[1].SetActive(true);
            this.yellowWon.enabled = true;
            this.redWon.enabled = false;
            this.redText.enabled = false;
            this.yellowText.enabled = false;
            GameOver = true;
        }

        yield return null;
    }
}
