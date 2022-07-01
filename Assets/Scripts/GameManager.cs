using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int wordID;
    public int wordCountInLevel;
    public int wordCount;
    public int whicLevel;
    public int letterCount;
    public int nextWord;
    public int whichWordLetterCount;
    public List<int> lettersToDisplayEdit;

    public string word;
    public string editWord;
    public string playerLetter;
    public string colorName;

    public bool putLine;
    public bool putLetter;
    public bool wordCheckB;
    public bool playB;

    public char[] letters;

    public float colorTimer;

    public InputField inputText;
    public GameLevelEditor gameEditor;
    public TextMeshProUGUI puzzleText;
    public TouchScreenKeyboard keyboard;
    public Button play;
    public Button end;
    public GameObject playPanel;
    public GameObject endPanel;
    public GameObject cameraRot;
    public GameObject keyboardBut;
    public Button keyboardButton;
    public GameObject tuorialText;
    public Camera mainCamera;

    [Header("FINISHWORD")]
    public string finishWord;
    public float finishTimer;

    [Header("TIMER")]
    public GameObject _timerGround;
    public TextMeshProUGUI _timerText;
    public float _gameGroundTimerF;
    public float _gameTextTimerF;
    public int _gameTextTimerI;
    public int deltaI;
    public bool _timerCheck;

    [Header("WINPANEL")]
    public GameObject _winPanel;
    public Button _nextLevel;
    public List<GameObject> _stars;
    public TextMeshProUGUI _starsText;
    public int _starsCount;
    public int _starsTextCount;
    public int cc;

    [Header("FAILPANEL")]
    public GameObject _failPanel;
    public Button _againButton;
    public TextMeshProUGUI _failStarsText;

    [Header("VEHICLE")]
    public GameObject vehicle;
    public GameObject spawnPoint;
    public List<GameObject> vehiclePieces;
    public Material defaultMat;
    public int ff;
    public int editVehicleWordId;
    public string editVehicleWord;
    public char[] editVehicleSeparate;

    [Header("HIGHLIGHT")]
    public Material highLight;
    public float fixTransFloat;
    public float editTransFloat;
    public bool transFloatCheck;

    [Header("ELECTIVE QUESTION")]
    public GameObject quesPanel;
    public Button quesOne;
    public Button quesTwo;
    public TextMeshProUGUI quesOneText;
    public TextMeshProUGUI quesTwoText;
    public int whichQues;
    public int quesLetterCount;
    public int whichLetter;
    public int quesRandom;
    public bool quesCheck;

    private void Start()
    {
        playB = false;
        playPanel.SetActive(true);
        keyboardBut.SetActive(false);
        _nextLevel.onClick.AddListener(() => NextLevelButton());
        _againButton.onClick.AddListener(() => AgainButton());
        play.onClick.AddListener(() => PlayButton());
        end.onClick.AddListener(() => EndLevelButton());
        quesOne.onClick.AddListener(() => Ques(1));
        quesTwo.onClick.AddListener(() => Ques(2));
        keyboardButton.onClick.AddListener(() => KeyboardOpen());
    }

    private void Update()
    {
        if (playB)
        {
            if (wordCheckB)
            {
                WordCheck();
            }
            ColorTimer();
            GameTimer();
            IsTheWordFinished();
            EditHighLgiht();
            if (whicLevel == 0)
            {
                tuorialText.SetActive(true);
            }
            else
            {
                tuorialText.SetActive(false);
            }
        }
        if (whicLevel == gameEditor.levelData.Count)
        {
            endPanel.SetActive(true);
        }
        Ques();
    }

    public void PlayButton()
    {
        DefaultValues();
        wordCount = gameEditor.wordData.Count();
        NextLevel();
        playB = true;
        playPanel.SetActive(false);
        keyboardBut.SetActive(true);
        _timerText.text = "59";
        KeyboardOpen();
    }

    public void EndLevelButton()
    {
        whicLevel = 0;
        wordCountInLevel = 0;
        whichLetter = 0;
        endPanel.SetActive(false);
    }

    public void KeyboardOpen()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false);
    }

    public void KeyboardClose()
    {
        keyboard = null;
    }

    public void Ques()
    {
        quesLetterCount = gameEditor.levelData[whicLevel].trackCount.Length;
        if (whichQues == whicLevel && quesLetterCount == whichLetter)
        {
            quesPanel.SetActive(true);
            //KeyboardClose();
            quesOneText.text = word;
            if (quesCheck)
            {
                quesRandom = Random.Range(0, gameEditor.wordData.Count);
                quesCheck = false;
                if (quesRandom == gameEditor.levelData[whicLevel].trackCount[quesLetterCount])
                {
                    quesCheck = true;
                }
            }
            quesTwoText.text = gameEditor.wordData[quesRandom].trackName;
        }
    }

    public void Ques(int buttonID)
    {
        if (buttonID == 1)
        {
            WinPanel();
            whichQues += 5;
        }
        else if (buttonID == 2)
        {
            FailPanel();
        }
    }

    public void NextLevel()
    {
        KeyboardOpen();
        CheckSceneLevelObject();
        DefaultValues();
        wordID = gameEditor.levelData[whicLevel].trackCount[wordCountInLevel];
        lettersToDisplayEdit.Clear();
        for (int b = 0; b < wordCount; b++)
        {
            if (wordID == b)
            {
                word = gameEditor.wordData[b].trackName;
                cameraRot.transform.rotation = Quaternion.Euler(gameEditor.wordData[b].cameraRot.x, gameEditor.wordData[b].cameraRot.y, gameEditor.wordData[b].cameraRot.z);
                mainCamera.orthographicSize = gameEditor.wordData[b].cameraSize;
            }
        }
        letters = word.ToCharArray();
        for (int c = 0; c < letters.Length; c++)
        {
            putLine = true;
            for (int d = 0; d < gameEditor.wordData[wordID].lettersToDisplay.Length; d++)
            {
                if (gameEditor.wordData[wordID].lettersToDisplay[d] == c)
                {
                    puzzleText.text += letters[c].ToString().ToUpper() + " ";
                    putLine = false;
                }
            }
            if (putLine)
            {
                puzzleText.text += "_ ";
            }
        }
        for (int e = 0; e < gameEditor.wordData[wordID].lettersToDisplay.Length; e++)
        {
            lettersToDisplayEdit.Add(gameEditor.wordData[wordID].lettersToDisplay[e]);
        }
        whichLetter++;
        VehiclePartsPaint();
    }

    public void WordCheck()
    {
        playerLetter = inputText.text;
        //playerLetter = keyboard.text;
        if (playerLetter != "")
        {
            for (int a = 0; a < lettersToDisplayEdit.Count; a++)
            {
                if (lettersToDisplayEdit[a] == letterCount)
                {
                    letterCount++;
                }
            }
            if (letters[letterCount].ToString().ToLower() == playerLetter.ToLower())
            {
                colorName = "Green";
                lettersToDisplayEdit.Add(letterCount);
                letterCount++;
                puzzleText.text = null;
                finishWord = "";
                for (int c = 0; c < letters.Length; c++)
                {
                    putLine = true;
                    for (int d = 0; d < lettersToDisplayEdit.Count; d++)
                    {
                        if (lettersToDisplayEdit[d] == c)
                        {
                            puzzleText.text += letters[c].ToString().ToUpper() + " ";
                            putLine = false;
                            finishWord += letters[c].ToString().ToUpper();
                        }
                    }
                    if (putLine)
                    {
                        puzzleText.text += "_ ";
                    }
                }
                for (int n = 0; n < letters.Length; n++)
                {
                    if (letters[n] == ' ')
                    {
                        whichWordLetterCount = n;
                    }
                }
            }
            else if (letters[letterCount].ToString().ToLower() != playerLetter.ToLower())
            {
                colorName = "Red";
            }
            else
            {
                for (int a = 0; a < lettersToDisplayEdit.Count; a++)
                {
                    if (lettersToDisplayEdit[a] == letterCount)
                    {
                        letterCount++;
                        colorName = "Red";
                    }
                }
            }
            playerLetter = "";
            inputText.text = "";
            //keyboard.text = "";
        }
    }

    public void CheckSceneLevelObject()
    {
        for (int a = 0; a < gameEditor.vehicleData.Count; a++)
        {
            if (gameEditor.vehicleData[a].howLevel >= whicLevel + 1)
            {
                if (vehicle == null)
                {
                    vehicle = Instantiate(gameEditor.vehicleData[a].vehicle, spawnPoint.transform.position, spawnPoint.transform.rotation);
                    for (int b = 0; b < vehicle.transform.childCount; b++)
                    {
                        vehiclePieces.Add(vehicle.transform.GetChild(b).gameObject);
                    }
                    break;
                }
            }
        }
        VehicleParts();
    }

    public void VehicleParts()
    {
        if (ff == whicLevel)
        {
            ff++;
            for (int c = 0; c < gameEditor.levelData[whicLevel].trackCount.Length; c++)
            {
                editVehicleWordId = gameEditor.levelData[whicLevel].trackCount[c];
                editVehicleWord = gameEditor.wordData[editVehicleWordId].trackName;
                editVehicleSeparate = editVehicleWord.ToCharArray();
                editVehicleWord = null;
                for (int e = 0; e < editVehicleSeparate.Length; e++)
                {
                    if (editVehicleSeparate[e] != ' ')
                    {
                        editVehicleWord += editVehicleSeparate[e].ToString();
                    }
                    for (int d = 0; d < vehiclePieces.Count; d++)
                    {
                        if (editVehicleWord.ToLower() == vehiclePieces[d].tag.ToLower())
                        {
                            vehiclePieces[d].SetActive(false);
                        }
                    }
                }
            }
        }
    }

    public void VehiclePartsPaint()
    {
        editWord = "";
        for (int e = 0; e < letters.Length; e++)
        {
            if (letters[e] != ' ')
            {
                editWord += letters[e].ToString();
            }
            for (int d = 0; d < vehiclePieces.Count; d++)
            {
                if (editWord.ToLower() == vehiclePieces[d].tag.ToLower())
                {
                    vehiclePieces[d].SetActive(true);
                    defaultMat = vehiclePieces[d].GetComponent<Renderer>().material;
                    vehiclePieces[d].GetComponent<Renderer>().material = highLight;
                }
            }
        }
    }

    public void IsTheWordFinished()
    {
        if (word == finishWord)
        {
            for (int c = 0; c < vehiclePieces.Count; c++)
            {
                if (vehiclePieces[c].tag.ToLower() == editWord.ToLower())
                {
                    vehiclePieces[c].GetComponent<Renderer>().material = defaultMat;
                }
            }
            wordCheckB = false;
            wordCountInLevel++;
            if (wordCountInLevel < gameEditor.levelData[whicLevel].trackCount.Length)
            {
                NextLevel();
            }
            else
            {
                if (_winPanel.activeInHierarchy == false)
                {
                    finishTimer -= Time.deltaTime;
                    if (finishTimer <= 0)
                    {
                        _timerCheck = false;
                        WinPanel();
                    }
                }
            }
        }
        else if (_gameTextTimerI == 0)
        {
            _timerCheck = false;
            FailPanel();
        }
    }

    public void DefaultValues()
    {
        _gameGroundTimerF = 1f;
        _gameTextTimerF = 1f;
        _gameTextTimerI = 59;
        finishTimer = 1;
        deltaI = 10;
        _starsCount = 3;
        _timerCheck = true;
        wordCheckB = true;
        letterCount = 0;
        cc = 0;
        _timerGround.GetComponent<Image>().color = Color.green;
        puzzleText.text = "";
        playerLetter = "";
        finishWord = "";
        inputText.text = "";
        //keyboard.text = null;
    }

    public void EditHighLgiht()
    {
        fixTransFloat = highLight.GetFloat("_Float4");
        if (fixTransFloat <= 0f)
        {
            transFloatCheck = true;
        }
        else if (fixTransFloat >= 1f)
        {
            transFloatCheck = false;
        }
        if (transFloatCheck)
        {
            editTransFloat += 0.02f;
            highLight.SetFloat("_Float4", editTransFloat);
        }
        else
        {
            editTransFloat -= 0.02f;
            highLight.SetFloat("_Float4", editTransFloat);
        }
    }

    public void WinPanel()
    {
        _winPanel.SetActive(true);
        for (int a = 0; a < _starsCount; a++)
        {
            _stars[a].SetActive(true);
        }
        cc++;
        if (cc == 1)
        {
            _starsTextCount += _starsCount;
            _starsText.text = _starsTextCount.ToString();
        }
        quesPanel.SetActive(false);
    }

    public void FailPanel()
    {
        _failPanel.SetActive(true);
        _failStarsText.text = _starsTextCount.ToString();
        quesPanel.SetActive(false);
    }

    public void AgainButton()
    {
        _failPanel.SetActive(false);
        wordCountInLevel = 0;
        whichLetter = 0;
        NextLevel();
    }

    public void NextLevelButton()
    {
        _winPanel.SetActive(false);
        whicLevel++;
        wordCountInLevel = 0;
        whichLetter = 0;
        NextLevel();
    }

    public void ColorTimer()
    {
        if (colorName == "Red")
        {
            puzzleText.color = Color.red;
            colorTimer += Time.deltaTime;
            if (colorTimer >= 0.5f)
            {
                colorTimer = 0f;
                colorName = "";
            }
        }
        else if (colorName == "Green")
        {
            puzzleText.color = Color.green;
            colorTimer += Time.deltaTime;
            if (colorTimer >= 0.5f)
            {
                colorTimer = 0f;
                colorName = "";
            }
        }
        else
        {
            if (whichWordLetterCount == letterCount)
            {
                puzzleText.color = Color.yellow;
            }
            else
            {
                puzzleText.color = Color.white;
            }
        }
    }

    public void GameTimer()
    {
        if (_timerCheck)
        {
            _gameTextTimerF -= Time.deltaTime;
            _gameGroundTimerF -= Time.deltaTime / deltaI;
            if (_gameTextTimerF <= 0)
            {
                _gameTextTimerI--;
                _timerText.text = _gameTextTimerI.ToString();
                _gameTextTimerF = 1f;
            }
            _timerGround.GetComponent<Image>().fillAmount = _gameGroundTimerF;
            if (_gameTextTimerI == 50)
            {
                deltaI = 20;
                _gameGroundTimerF = 1f;
                _timerGround.GetComponent<Image>().color = Color.yellow;
                _starsCount = 2;
            }
            else if (_gameTextTimerI == 30)
            {
                deltaI = 30;
                _gameGroundTimerF = 1f;
                _timerGround.GetComponent<Image>().color = Color.red;
                _starsCount = 1;
            }
        }
    }
}
