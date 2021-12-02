using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SlotSave : MonoBehaviour
{
    //variables
    public string persistentPath = "";

    public Button Slot1Butt;
    public Button Slot2Butt;
    public Button Slot3Butt;
    public Button DeleteButton;

    public TextMeshProUGUI slot1Text;
    public TextMeshProUGUI slot2Text;
    public TextMeshProUGUI slot3Text;

    public Button ContinueButt;
    public Button NewButt;
    public Button BackButt;
    public Button OptionsButt;
    public Button ExitButt;

    public bool slot1Exists = false;
    public bool slot2Exists = false;
    public bool slot3Exists = false;

    public bool IsJustStarted = false;
    private bool NoNewButtonSpawn = false;

    public GameObject skyAndFog;
    public GameObject staticScreen;
    public GameObject skyAndFogCrazy;

    private float staticTimer = 10;
    public GameObject MainUI;

    private bool isOptions = false;
    public GameObject OptionsPanel;
    public GameObject ControlsPanel;
    public GameObject CreditsPanel;
    public GameObject MainPanel;
    public GameObject RightMainPanel;
    public Slider SensitivitySlider;
    public Slider VolumeSlider;
    public TextMeshProUGUI SliderDisplay1;
    public TextMeshProUGUI SliderDisplay2;

    public TextMeshProUGUI ControlsBack;
    public TextMeshProUGUI CreditsBack;

    public GameObject MMConfirmationPanel;

    public bool IsDeleting = false;
    public TextMeshProUGUI DeleteButtonText;

    //no destroy object
    GameObject controller;
    NoDestroy destroyScript;

    // Start is called before the first frame update
    void Start()
    {
        CheckForPostProcessingOBJs();

        //grabs main UI
        MainUI = GameObject.Find("MainMenuPanel");

        //grab no destroy object
        controller = GameObject.Find("NoDestroyOBJ");
        destroyScript = controller.GetComponent<NoDestroy>();
        //gets persistant path
        persistentPath = Application.persistentDataPath;

        Slot1Butt.gameObject.SetActive(false);
        Slot2Butt.gameObject.SetActive(false);
        Slot3Butt.gameObject.SetActive(false);
        BackButt.gameObject.SetActive(false);
        DeleteButton.gameObject.SetActive(false);

        //if the files exist change the bool
        //also updated so it shows date of last write to file and the name of scene they are on
        DisplayFileInfo();

        //makes continue button visable if there is an existing data file to continue game on
        if (slot1Exists || slot2Exists || slot3Exists) ContinueButt.gameObject.SetActive(true);
        else ContinueButt.gameObject.SetActive(false);

        //if there are no more save slots available the new game button will not appear
        if (!slot1Exists || !slot2Exists || !slot3Exists) NewButt.gameObject.SetActive(true);
        else NewButt.gameObject.SetActive(false);
    }

    private void DisplayFileInfo()
    {
        if (File.Exists(persistentPath + "/Slot1Data.txt"))
        {
            slot1Exists = true;
            if (System.IO.File.ReadAllLines(persistentPath + "/Slot1Data.txt")[1] == "1") slot1Text.text = "Slot 1-Intro\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot1Data.txt").ToString();
            else if (System.IO.File.ReadAllLines(persistentPath + "/Slot1Data.txt")[1] == "2") slot1Text.text = "Slot 1-Hunt\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot1Data.txt").ToString();
            else if (System.IO.File.ReadAllLines(persistentPath + "/Slot1Data.txt")[1] == "3") slot1Text.text = "Slot 1-Q&A\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot1Data.txt").ToString();
            else if (System.IO.File.ReadAllLines(persistentPath + "/Slot1Data.txt")[1] == "4") slot1Text.text = "Slot 1-End\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot1Data.txt").ToString();
            else slot1Text.text = "Slot 1\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot1Data.txt").ToString();
        }
        if (File.Exists(persistentPath + "/Slot2Data.txt"))
        {
            slot2Exists = true;
            if (System.IO.File.ReadAllLines(persistentPath + "/Slot2Data.txt")[1] == "1") slot2Text.text = "Slot 2-Intro\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot2Data.txt").ToString();
            else if (System.IO.File.ReadAllLines(persistentPath + "/Slot2Data.txt")[1] == "2") slot2Text.text = "Slot 2-Hunt\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot2Data.txt").ToString();
            else if (System.IO.File.ReadAllLines(persistentPath + "/Slot2Data.txt")[1] == "3") slot2Text.text = "Slot 2-Q&A\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot2Data.txt").ToString();
            else if (System.IO.File.ReadAllLines(persistentPath + "/Slot2Data.txt")[1] == "4") slot2Text.text = "Slot 2-End\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot2Data.txt").ToString();
            else slot2Text.text = "Slot 2\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot2Data.txt").ToString();
        }
        if (File.Exists(persistentPath + "/Slot3Data.txt"))
        {
            slot3Exists = true;
            if (System.IO.File.ReadAllLines(persistentPath + "/Slot3Data.txt")[1] == "1") slot3Text.text = "Slot 3-Intro\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot3Data.txt").ToString();
            else if (System.IO.File.ReadAllLines(persistentPath + "/Slot3Data.txt")[1] == "2") slot3Text.text = "Slot 3-Hunt\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot3Data.txt").ToString();
            else if (System.IO.File.ReadAllLines(persistentPath + "/Slot3Data.txt")[1] == "3") slot3Text.text = "Slot 3-Q&A\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot3Data.txt").ToString();
            else if (System.IO.File.ReadAllLines(persistentPath + "/Slot3Data.txt")[1] == "4") slot3Text.text = "Slot 3-End\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot3Data.txt").ToString();
            else slot3Text.text = "Slot 3\n" + System.IO.File.GetLastWriteTime(persistentPath + "/Slot3Data.txt").ToString();
        }
    }

    private void Update()
    {
        if (isOptions) TrackingSlideBars();
        if (slot1Exists && slot2Exists && slot3Exists) NewButt.gameObject.SetActive(false);

        //should keep player from glitching out menu on multiple file creations
        if (NoNewButtonSpawn)
        {
            DeleteButton.gameObject.SetActive(false);
        }

        if (!IsJustStarted) return;
        StaticScreenTimer();
    }

    private void CheckForPostProcessingOBJs()
    {
        //gets sky and fog volumes
        if (GameObject.Find("Sky and Fog Volume"))
        {
            skyAndFog = GameObject.Find("Sky and Fog Volume");
        }
        else
        {
            skyAndFog = null;
        }
        if (GameObject.Find("Sky and Fog Volume 2"))
        {
            staticScreen = GameObject.Find("Sky and Fog Volume 2");
            staticScreen.SetActive(false);
        }
        else
        {
            staticScreen = null;
        }
        if (GameObject.Find("Sky and Fog Volume 3"))
        {
            skyAndFogCrazy = GameObject.Find("Sky and Fog Volume 3");
            skyAndFogCrazy.SetActive(false);
        }
        else
        {
            skyAndFogCrazy = null;
        }
    }

    private void StaticScreenTimer()
    {
        if (IsJustStarted)
        {
            staticTimer -= 1 * Time.deltaTime;
        }
        if (IsJustStarted && staticTimer >= 0)
        {
            //sets main menu ui to starting state
            //turns on
            ContinueButt.gameObject.SetActive(true);
            if(!NoNewButtonSpawn) NewButt.gameObject.SetActive(true);
            OptionsButt.gameObject.SetActive(true);
            ExitButt.gameObject.SetActive(true);


            //turns off
            Slot1Butt.gameObject.SetActive(false);
            Slot2Butt.gameObject.SetActive(false);
            Slot3Butt.gameObject.SetActive(false);
            BackButt.gameObject.SetActive(false);

            //turns off all UI in Panel
            MainUI.SetActive(false);
            RightMainPanel.SetActive(false);

            //changes Postprocessing to glithy one
            skyAndFog.SetActive(false);
            staticScreen.SetActive(true);

            AudioController.PlayFlashBackSound(100);
        }
        if (staticTimer <= 0)
        {
            AudioController.StopSound();
            IsJustStarted = false;
            NoNewButtonSpawn = true;
            skyAndFogCrazy.SetActive(true);
            staticScreen.SetActive(false);
            MainUI.SetActive(true);
            RightMainPanel.SetActive(true);
            NewButt.gameObject.SetActive(false);
            staticTimer = 10;
            AmbientClipController.pitchFloat = 0.5f;
            AmbientClipController.volumeFloat = 1f;
        }
    }

    //This function activates if you click new game button
    //it makes sure only empty slot buttons show up
    public void SelectEmptySlot()
    {
        AudioController.ClickSound();
        //returns out if all files exist
        if (File.Exists(persistentPath + "/Slot1Data.txt") && File.Exists(persistentPath + "/Slot2Data.txt") && File.Exists(persistentPath + "/Slot3Data.txt")) return;

        //updates gui's accordingly
        BackButt.gameObject.SetActive(true);
        NewButt.gameObject.SetActive(false);
        ContinueButt.gameObject.SetActive(false);
        OptionsButt.gameObject.SetActive(false);
        ExitButt.gameObject.SetActive(false);
        if (!File.Exists(persistentPath + "/Slot1Data.txt")) Slot1Butt.gameObject.SetActive(true);
        if (!File.Exists(persistentPath + "/Slot2Data.txt")) Slot2Butt.gameObject.SetActive(true);
        if (!File.Exists(persistentPath + "/Slot3Data.txt")) Slot3Butt.gameObject.SetActive(true);
    }

    //triggers if you click the back button
    public void BackTrigger()
    {
        AudioController.ClickSound();
        if (!isOptions)
        {
            //updates gui
            BackButt.gameObject.SetActive(false);
            Slot1Butt.gameObject.SetActive(false);
            Slot2Butt.gameObject.SetActive(false);
            Slot3Butt.gameObject.SetActive(false);
            DeleteButton.gameObject.SetActive(false);
            //clears static vairable (may not be needed)
            NoDestroy.fileLoaded = "";

            // makes sure when clicking back button that continue button or new game button should appear accodingly
            if (File.Exists(persistentPath + "/Slot1Data.txt") || File.Exists(persistentPath + "/Slot2Data.txt") || File.Exists(persistentPath + "/Slot3Data.txt")) ContinueButt.gameObject.SetActive(true);
            if (!File.Exists(persistentPath + "/Slot1Data.txt") && !NoNewButtonSpawn || !File.Exists(persistentPath + "/Slot2Data.txt") && !NoNewButtonSpawn || !File.Exists(persistentPath + "/Slot3Data.txt") && !NoNewButtonSpawn) NewButt.gameObject.SetActive(true);
            OptionsButt.gameObject.SetActive(true);
            ExitButt.gameObject.SetActive(true);
        }
        else
        {
            
            OptionsPanel.SetActive(false);
            MainPanel.SetActive(true);
            isOptions = false;
        }
        DeleteButtonText.text = "Delete File";
        IsDeleting = false;
    }

    //i made this out of rage ok
    public void ControlCreditUITriggers(Button buttonClicked)
    {
        AudioController.ClickSound();
        if (buttonClicked.GetComponentInChildren<TextMeshProUGUI>().text == "Controls")
        {
            MainPanel.SetActive(false);
            ControlsPanel.SetActive(true);
            OptionsPanel.SetActive(false);
            CreditsPanel.SetActive(false);
            ControlsBack.text = "Back";
            CreditsBack.transform.parent.gameObject.SetActive(false);
        }
        else if (buttonClicked.GetComponentInChildren<TextMeshProUGUI>().text == "Credits")
        {
            MainPanel.SetActive(false);
            ControlsPanel.SetActive(false);
            OptionsPanel.SetActive(false);
            CreditsPanel.SetActive(true);
            CreditsBack.text = "Back";
            ControlsBack.transform.parent.gameObject.SetActive(false);
        }
        else if (buttonClicked.GetComponentInChildren<TextMeshProUGUI>().text == "Back") 
        {

            CreditsPanel.SetActive(false);
            ControlsPanel.SetActive(false);
            MainPanel.SetActive(true);
            ControlsBack.transform.parent.gameObject.SetActive(true);
            CreditsBack.transform.parent.gameObject.SetActive(true);
            ControlsBack.text = "Controls";
            CreditsBack.text = "Credits";
        }
    }
    
    public void DeleteSelected()
    {
        AudioController.ClickSound();
        DeleteButtonText.text = "Delete File";
        if (!IsDeleting)
        {
            DeleteButtonText.text = "Cancel Deletion";
            IsDeleting = true;
            return;
        }else if (IsDeleting)
        {
            DeleteButtonText.text = "Delete File";
            IsDeleting = false;
        }
    }

    //triggers when you select the options button
    public void SelectOptions()
    {
        AudioController.ClickSound();
        isOptions = true;
        OptionsPanel.SetActive(true);
        MainPanel.SetActive(false);
    }

    public void ApplyOptionChanges() // applys changes to files through main menu
    {
        AudioController.ClickSound();
        if (File.Exists(persistentPath + "/Slot1Data.txt"))
        {
            string thispath = persistentPath + "/Slot1Data.txt";
            string newString = "";
            for(int i = 0; i < System.IO.File.ReadAllLines(thispath).Length; i++)
            {
                if (i == 2) newString = newString + SensitivitySlider.value.ToString() + "\n";
                else if (i == 3) 
                {
                    float decNum = VolumeSlider.value/10;
                    newString = newString + decNum.ToString() + "\n";
                } 
                else newString = newString + System.IO.File.ReadAllLines(thispath)[i] + "\n";
            }
            //updates date modified and created, so saving is not considered tampering
            DateTime dateCreated = System.IO.File.GetCreationTime(thispath);
            DateTime dateModified = System.IO.File.GetLastWriteTime(thispath);
            string dateFileCreated = dateCreated.ToString("O").Substring(0, 18);
            string dateFileModified = dateModified.ToString("O").Substring(0, 18);

            //This deletes the old file and replaces it with the updated new file
            System.IO.File.Delete(thispath);
            System.IO.File.WriteAllText(thispath, newString);

            //This helps us know that the game changed the save data file, not the player in the explorer
            System.IO.File.SetCreationTime(thispath, dateCreated);
            System.IO.File.SetLastWriteTime(thispath, dateModified);
        }
        if (File.Exists(persistentPath + "/Slot2Data.txt"))
        {
            string thispath = persistentPath + "/Slot2Data.txt";
            string newString = "";
            for (int i = 0; i < System.IO.File.ReadAllLines(thispath).Length; i++)
            {
                if (i == 2) newString = newString + SensitivitySlider.value.ToString() + "\n";
                else if (i == 3)
                {
                    float decNum = VolumeSlider.value / 10;
                    newString = newString + decNum.ToString() + "\n";
                }
                else newString = newString + System.IO.File.ReadAllLines(thispath)[i] + "\n";
            }
            //updates date modified and created, so saving is not considered tampering
            DateTime dateCreated = System.IO.File.GetCreationTime(thispath);
            DateTime dateModified = System.IO.File.GetLastWriteTime(thispath);
            string dateFileCreated = dateCreated.ToString("O").Substring(0, 18);
            string dateFileModified = dateModified.ToString("O").Substring(0, 18);

            //This deletes the old file and replaces it with the updated new file
            System.IO.File.Delete(thispath);
            System.IO.File.WriteAllText(thispath, newString);

            //This helps us know that the game changed the save data file, not the player in the explorer
            System.IO.File.SetCreationTime(thispath, dateCreated);
            System.IO.File.SetLastWriteTime(thispath, dateModified);
        }
        if (File.Exists(persistentPath + "/Slot3Data.txt"))
        {
            string thispath = persistentPath + "/Slot3Data.txt";
            string newString = "";
            for (int i = 0; i < System.IO.File.ReadAllLines(thispath).Length; i++)
            {
                if (i == 2) newString = newString + SensitivitySlider.value.ToString() + "\n";
                else if (i == 3)
                {
                    float decNum = VolumeSlider.value / 10;
                    newString = newString + decNum.ToString() + "\n";
                }
                else newString = newString + System.IO.File.ReadAllLines(thispath)[i] + "\n";
            }
            //updates date modified and created, so saving is not considered tampering
            DateTime dateCreated = System.IO.File.GetCreationTime(thispath);
            DateTime dateModified = System.IO.File.GetLastWriteTime(thispath);
            string dateFileCreated = dateCreated.ToString("O").Substring(0, 18);
            string dateFileModified = dateModified.ToString("O").Substring(0, 18);

            //This deletes the old file and replaces it with the updated new file
            System.IO.File.Delete(thispath);
            System.IO.File.WriteAllText(thispath, newString);

            //This helps us know that the game changed the save data file, not the player in the explorer
            System.IO.File.SetCreationTime(thispath, dateCreated);
            System.IO.File.SetLastWriteTime(thispath, dateModified);
        }

        OptionsPanel.SetActive(false);
        MainPanel.SetActive(true);
        isOptions = false;
    }

    private void TrackingSlideBars()
    {
        SliderDisplay2.text = SensitivitySlider.value.ToString();
        SliderDisplay1.text = VolumeSlider.value.ToString();
    }

    //function for clicking slot 1
    public void CreateSlot1() // CURRENT TESTING SLOT, TXT FILE CAN BE MANIPULATED
    {
        AudioController.ClickSound();
        //if the file exists changes the static variable and loads the scene
        if (File.Exists(persistentPath + "/Slot1Data.txt"))
        {
            string grabFilePath = persistentPath + "/Slot1Data.txt";

            if (IsDeleting) // if user is trying to delete
            {
                File.Delete(grabFilePath);
                slot1Exists = false;
                Slot1Butt.gameObject.SetActive(false);
                slot1Text.text = "Slot 1";
                DeleteButtonText.text = "Delete File";
                IsDeleting = false;
                return;
            }

            //COMMENTED OUT FOR TESTING PURPOSES
            /*//check for editing file?
            DateTime dateModified = System.IO.File.GetLastWriteTime(grabFilePath);
            string modified = dateModified.ToString("O").Substring(0, 18);
            if (System.IO.File.ReadAllLines(grabFilePath).Length != 9)
            {
                print("file was tampered with");
                print("error 1");
                SceneManager.LoadScene("HellScene");
                AudioController.StopSound();
                NoDestroy.HasBeenTamperedWith = true;
                return;
            }
            else if (System.IO.File.ReadAllLines(grabFilePath)[System.IO.File.ReadAllLines(grabFilePath).Length - 1].Length != 18)
            {
                print("file was tampered with");
                print("error 2");
                SceneManager.LoadScene("HellScene");
                AudioController.StopSound();
                NoDestroy.HasBeenTamperedWith = true;
                return;
            }
            else if(modified != System.IO.File.ReadAllLines(grabFilePath)[System.IO.File.ReadAllLines(grabFilePath).Length - 1].Substring(0, 18))
            {

                print("file was tampered with");
                print("error 3");
                //print(modified);
                //print(System.IO.File.ReadAllLines(grabFilePath)[System.IO.File.ReadAllLines(grabFilePath).Length - 1].Substring(0, 18));
                SceneManager.LoadScene("HellScene");
                AudioController.StopSound();
                NoDestroy.HasBeenTamperedWith = true;
                return;
            }
            else 
            {
                //load scene if good
                NoDestroy.fileLoaded = "Slot1Data.txt";
                int startAttempts = int.Parse(System.IO.File.ReadAllLines(grabFilePath)[6]);
                string sceneString = System.IO.File.ReadAllLines(grabFilePath)[4];
                AudioController.StopSound();
                if (startAttempts == 2) SceneManager.LoadScene("Between");
                else SceneManager.LoadScene(sceneString);
            }*/
            //load scene if good DELETE BELOW ONCE DONE DEVELOPING
            NoDestroy.fileLoaded = "Slot1Data.txt";
            int startAttempts = int.Parse(System.IO.File.ReadAllLines(grabFilePath)[6]);
            string sceneString = System.IO.File.ReadAllLines(grabFilePath)[4];
            AudioController.StopSound();
            if (startAttempts == 2) SceneManager.LoadScene("Between");
            else SceneManager.LoadScene(sceneString);
        }
        //if the file does not exist changes the static variable and loads the scene
        //also creates and writes to the text file
        else if (!File.Exists(persistentPath + "/Slot1Data.txt"))
        {
            //is created first so we can grab the date
            System.IO.File.WriteAllText(persistentPath + "/Slot1Data.txt", "First");
            //get datetime and datemodified of file
            DateTime dateCreated = System.IO.File.GetCreationTime(persistentPath + "/Slot1Data.txt");
            DateTime dateModified = System.IO.File.GetLastWriteTime(persistentPath + "/Slot1Data.txt");

            System.IO.File.Delete(persistentPath + "/Slot1Data.txt");
            //adds to file the datetime and datemodified
            string created = dateCreated.ToString("O").Substring(0, 18);
            string modified = dateModified.ToString("O").Substring(0, 18);

            //write file
            System.IO.File.WriteAllText(persistentPath + "/Slot1Data.txt", "Slot1Data\n1\n3\n1\nGlitchyStart\nBLOOD\n2\n" + created + "\n" + modified);

            //This creates Txt file and updates datetimes to match
            System.IO.File.SetCreationTime(persistentPath, dateCreated);
            System.IO.File.SetLastWriteTime(persistentPath, dateModified);

            //last steps
            Slot1Butt.gameObject.SetActive(false);
            slot1Exists = true;
            NoDestroy.fileLoaded = "Slot1Data.txt";
            IsJustStarted = true; //triggers intial glitch!!
        }
    }

    //function for clicking slot 2
    public void CreateSlot2()
    {
        AudioController.ClickSound();
        if (File.Exists(persistentPath + "/Slot2Data.txt"))
        {
            string grabFilePath = persistentPath + "/Slot2Data.txt";

            if (IsDeleting) // if user is trying to delete
            {
                File.Delete(grabFilePath);
                slot2Exists = false;
                slot2Text.text = "Slot 2";
                Slot2Butt.gameObject.SetActive(false);
                DeleteButtonText.text = "Delete File";
                IsDeleting = false;
                return;
            }

            //check for editing file?
            DateTime dateModified = System.IO.File.GetLastWriteTime(grabFilePath);
            string modified = dateModified.ToString("O").Substring(0, 18);
            if (System.IO.File.ReadAllLines(grabFilePath).Length != 9)
            {
                print("file was tampered with");
                print("error 1");
                SceneManager.LoadScene("HellScene");
                AudioController.StopSound();
                NoDestroy.HasBeenTamperedWith = true;
                return;
            }
            else if (System.IO.File.ReadAllLines(grabFilePath)[System.IO.File.ReadAllLines(grabFilePath).Length - 1].Length != 18)
            {
                print("file was tampered with");
                print("error 2");
                SceneManager.LoadScene("HellScene");
                AudioController.StopSound();
                NoDestroy.HasBeenTamperedWith = true;
                return;
            }
            else if (modified != System.IO.File.ReadAllLines(grabFilePath)[System.IO.File.ReadAllLines(grabFilePath).Length - 1].Substring(0, 18))
            {

                print("file was tampered with");
                print("error 3");
                SceneManager.LoadScene("HellScene");
                AudioController.StopSound();
                NoDestroy.HasBeenTamperedWith = true;
                return;
            }
            else
            {
                //load scene if good
                NoDestroy.fileLoaded = "Slot2Data.txt";
                int startAttempts = int.Parse(System.IO.File.ReadAllLines(grabFilePath)[6]);
                string sceneString = System.IO.File.ReadAllLines(grabFilePath)[4];
                AudioController.StopSound();
                if (startAttempts == 2) SceneManager.LoadScene("Between");
                else SceneManager.LoadScene(sceneString);
            }
        }
        //if the file does not exist changes the static variable and loads the scene
        //also creates and writes to the text file
        else if (!File.Exists(persistentPath + "/Slot2Data.txt"))
        {
            System.IO.File.WriteAllText(persistentPath + "/Slot2Data.txt", "First");
            //get datetime and datemodified of file
            DateTime dateCreated = System.IO.File.GetCreationTime(persistentPath + "/Slot2Data.txt");
            DateTime dateModified = System.IO.File.GetLastWriteTime(persistentPath + "/Slot2Data.txt");

            System.IO.File.Delete(persistentPath + "/Slot2Data.txt");
            //adds to file the datetime and datemodified
            string created = dateCreated.ToString("O").Substring(0, 18);
            string modified = dateModified.ToString("O").Substring(0, 18);

            //write file
            System.IO.File.WriteAllText(persistentPath + "/Slot2Data.txt", "Slot2Data\n1\n3\n1\nGlitchyStart\nStill time to leave.\n2\n" + created + "\n" + modified);

            //This creates Txt file and updates datetimes to match
            System.IO.File.SetCreationTime(persistentPath, dateCreated);
            System.IO.File.SetLastWriteTime(persistentPath, dateModified);

            //last steps
            Slot2Butt.gameObject.SetActive(false);
            slot2Exists = true;
            NoDestroy.fileLoaded = "Slot2Data.txt";
            IsJustStarted = true; //triggers intial glitch!!
        }
    }

    //function for clicking slot 3
    public void CreateSlot3()
    {
        AudioController.ClickSound();
        if (File.Exists(persistentPath + "/Slot3Data.txt"))
        {
            string grabFilePath = persistentPath + "/Slot3Data.txt";

            if (IsDeleting) // if user is trying to delete
            {
                File.Delete(grabFilePath);
                slot3Text.text = "Slot 3";
                slot3Exists = false;
                Slot3Butt.gameObject.SetActive(false);
                DeleteButtonText.text = "Delete File";
                IsDeleting = false;
                return;
            }

            //check for editing file?
            DateTime dateModified = System.IO.File.GetLastWriteTime(grabFilePath);
            string modified = dateModified.ToString("O").Substring(0, 18);
            if (System.IO.File.ReadAllLines(grabFilePath).Length != 9)
            {
                print("file was tampered with");
                print("error 1");
                SceneManager.LoadScene("HellScene");
                AudioController.StopSound();
                NoDestroy.HasBeenTamperedWith = true;
                return;
            }
            else if (System.IO.File.ReadAllLines(grabFilePath)[System.IO.File.ReadAllLines(grabFilePath).Length - 1].Length != 18)
            {
                print("file was tampered with");
                print("error 2");
                SceneManager.LoadScene("HellScene");
                AudioController.StopSound();
                NoDestroy.HasBeenTamperedWith = true;
                return;
            }
            else if (modified != System.IO.File.ReadAllLines(grabFilePath)[System.IO.File.ReadAllLines(grabFilePath).Length - 1].Substring(0, 18))
            {

                print("file was tampered with");
                print("error 3");
                SceneManager.LoadScene("HellScene");
                AudioController.StopSound();
                NoDestroy.HasBeenTamperedWith = true;
                return;
            }
            else
            {
                //load scene if good
                NoDestroy.fileLoaded = "Slot3Data.txt";
                int startAttempts = int.Parse(System.IO.File.ReadAllLines(grabFilePath)[6]);
                string sceneString = System.IO.File.ReadAllLines(grabFilePath)[4];
                AudioController.StopSound();
                if (startAttempts == 2) SceneManager.LoadScene("Between");
                else SceneManager.LoadScene(sceneString);
            }
        }
        //if the file does not exist changes the static variable and loads the scene
        //also creates and writes to the text file
        else if (!File.Exists(persistentPath + "/Slot3Data.txt"))
        {
            System.IO.File.WriteAllText(persistentPath + "/Slot3Data.txt", "First");
            //get datetime and datemodified of file
            DateTime dateCreated = System.IO.File.GetCreationTime(persistentPath + "/Slot3Data.txt");
            DateTime dateModified = System.IO.File.GetLastWriteTime(persistentPath + "/Slot3Data.txt");

            System.IO.File.Delete(persistentPath + "/Slot3Data.txt");
            //adds to file the datetime and datemodified
            string created = dateCreated.ToString("O").Substring(0, 18);
            string modified = dateModified.ToString("O").Substring(0, 18);

            //write file
            System.IO.File.WriteAllText(persistentPath + "/Slot3Data.txt", "Slot3Data\n1\n3\n1\nGlitchyStart\nLeave or Pay\n2\n" + created + "\n" + modified);

            //This creates Txt file and updates datetimes to match
            System.IO.File.SetCreationTime(persistentPath, dateCreated);
            System.IO.File.SetLastWriteTime(persistentPath, dateModified);

            //last steps
            Slot3Butt.gameObject.SetActive(false);
            slot3Exists = true;
            NoDestroy.fileLoaded = "Slot3Data.txt";
            IsJustStarted = true; //triggers intial glitch!!
        }
    }

    //when user clicks continue (same logic as SelectEmptySlot() function)
    public void ContinueTrigger()
    {
        AudioController.ClickSound();
        if (!File.Exists(persistentPath + "/Slot1Data.txt") && !File.Exists(persistentPath + "/Slot2Data.txt") && !File.Exists(persistentPath + "/Slot3Data.txt")) return;

        BackButt.gameObject.SetActive(true);
        DeleteButton.gameObject.SetActive(true);
        NewButt.gameObject.SetActive(false);
        ContinueButt.gameObject.SetActive(false);
        OptionsButt.gameObject.SetActive(false);
        ExitButt.gameObject.SetActive(false);
        if (File.Exists(persistentPath + "/Slot1Data.txt")) Slot1Butt.gameObject.SetActive(true);
        if (File.Exists(persistentPath + "/Slot2Data.txt")) Slot2Butt.gameObject.SetActive(true);
        if (File.Exists(persistentPath + "/Slot3Data.txt")) Slot3Butt.gameObject.SetActive(true);
    }

    public void ExitGame() 
    {
        AudioController.ClickSound();
        MMConfirmationPanel.SetActive(true);
    }
    public void ExitGameConfirm() 
    {
        AudioController.ClickSound();
        print("Exits Game on Build");
        Application.Quit();
    }
    public void CancelExitGame() 
    {
        AudioController.ClickSound();
        MMConfirmationPanel.SetActive(false);
    }
}
