using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ButtonLoadData : MonoBehaviour
{
    #region Structs
    struct Article
    {
        public string name;
        public Sprite image;
        public List<string> sizes;
    }
    #endregion //Structs

    #region PublicComponents
    public Button buttonLoadData;
    public Button buttonCapture;
    public InputField textPurchaseOrder;
    public Dropdown dropDownArticles;
    public Dropdown dropdownSizes;
    public UnityEngine.UI.Image imageDebug;
    public Text debugText;
    #endregion //PublicComponents

    #region PrivateVariables
    private PIXEL_FORMAT mPixelFormat = PIXEL_FORMAT.UNKNOWN_FORMAT;
    private bool mAccessCameraImage = true;
    private bool mFormatRegistered = false;
    private string mPurchaseDataPath;
    private string mResourceDataPath;
    private string mAndroidPersistentDatapath;
    private List<Article> mArticles;
    private List<Dropdown.OptionData> mOptiondataArticles;
    private List<Dropdown.OptionData> mOptiondataSizes;
    private List<Dropdown.OptionData> mDefaultOptionArticles;
    private List<Dropdown.OptionData> mDefaultOptionSizes;
    #endregion //PrivateVariables
    // Start is called before the first frame update
    void Start()
    {
        // Initializing article List
        mArticles = new List<Article>();

        //Initializing defaultOptionsList
        mDefaultOptionArticles = new List<Dropdown.OptionData>();
        mDefaultOptionSizes = new List<Dropdown.OptionData>();
        mDefaultOptionArticles.Add(new Dropdown.OptionData("Please Enter Order Number..."));
        mDefaultOptionSizes.Add(new Dropdown.OptionData("Select Article..."));

        // Setting paths
        mPurchaseDataPath = Application.dataPath + "/Resources/Purchases/";
        mAndroidPersistentDatapath = Application.persistentDataPath;
        mResourceDataPath = "Purchases/";

        // Setting listeners
        buttonLoadData.onClick.AddListener(loadData);
        buttonCapture.onClick.AddListener(captureImage);
        dropDownArticles.onValueChanged.AddListener(changeSizes);

        // Set pixel format for camera
        #if UNITY_EDITOR
            mPixelFormat = PIXEL_FORMAT.GRAYSCALE; // Need Grayscale for Editor
        #else
            mPixelFormat = PIXEL_FORMAT.RGB888; // Use RGB888 for mobile
        #endif

        // Register Vuforia callbacks
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaARController.Instance.RegisterOnPauseCallback(OnPause);
        // VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
    }

    // For load data button
    void loadData()
    {
        //Setting data path
        string purchaseOrder = textPurchaseOrder.text;
        string tempDataPath = mPurchaseDataPath + purchaseOrder;
        if (purchaseOrder.Equals(""))
        {
            textPurchaseOrder.text = "Invalid Purchase Order...";

            // Set default entries
            dropDownArticles.ClearOptions();
            dropdownSizes.ClearOptions();
            dropDownArticles.AddOptions(mDefaultOptionArticles);
            dropdownSizes.AddOptions(mDefaultOptionSizes);
            return;
        }

        // Initializing lists
        mArticles = new List<Article>();
        mOptiondataArticles = new List<Dropdown.OptionData>();
        mOptiondataSizes = new List<Dropdown.OptionData>();

        //Getting files in directory
        DirectoryInfo tempData = new DirectoryInfo(tempDataPath);
        addDebugInfo(tempDataPath);
        FileInfo[] files = { new FileInfo("temporary") };
        try
        {
            files = tempData.GetFiles();
        }
        catch (DirectoryNotFoundException exception)
        {
            textPurchaseOrder.text = "Invalid Purchase Order...";

            // Set default entries
            dropDownArticles.ClearOptions();
            dropdownSizes.ClearOptions();
            dropDownArticles.AddOptions(mDefaultOptionArticles);
            dropdownSizes.AddOptions(mDefaultOptionSizes);
            return;
        }
        foreach (FileInfo file in files)
        {
            //Remove names that end with meta
            if (file.Name.EndsWith("meta"))
                continue;

            // Get name of the file
            addDebugInfo(file.Name);
            string optionName = file.Name.Substring(0, file.Name.IndexOf("-"));

            // Getting sizes
            string sizes = file.Name.Substring(file.Name.IndexOf("-") + 1);
            sizes = sizes.Substring(0, sizes.IndexOf("."));

            // Convert sizes from string to list
            List<string> listOfSizes = new List<string>();
            string tempSize = "";
            while (true)
            {
                if (sizes.IndexOf(",") != -1)
                {
                    tempSize = sizes.Substring(0, sizes.IndexOf(","));
                    sizes = sizes.Substring(sizes.IndexOf(",") + 1);
                    listOfSizes.Add(tempSize);
                }
                else
                {
                    tempSize = sizes.Substring(0);
                    listOfSizes.Add(tempSize);
                    break;
                }
            }

            // Assigning data for dropdown list options
            Article tempArticle = new Article();
            tempArticle.name = optionName;
            Texture2D temptexture = Resources.Load(mResourceDataPath + purchaseOrder + "/" + file.Name.Substring(0, file.Name.IndexOf("."))) as Texture2D;
            tempArticle.image = Sprite.Create(temptexture, new Rect(0, 0, temptexture.width, temptexture.height), new Vector2(0.5f, 0.5f));
            tempArticle.sizes = listOfSizes;

            // Add to articles list
            mArticles.Add(tempArticle);
        }

        // Load into options list
        mOptiondataArticles = new List<Dropdown.OptionData>();
        mOptiondataSizes = new List<Dropdown.OptionData>();
        foreach (Article article in mArticles)
        {
            Dropdown.OptionData tempOptionArticle = new Dropdown.OptionData();
            tempOptionArticle.text = article.name;
            tempOptionArticle.image = article.image;
            mOptiondataArticles.Add(tempOptionArticle);
        }
        foreach (string size in mArticles[0].sizes)
        {
            Dropdown.OptionData tempOptionSize = new Dropdown.OptionData();
            tempOptionSize.text = size;
            mOptiondataSizes.Add(tempOptionSize);
        }

        // Assign to dropdowns
        dropDownArticles.ClearOptions();
        dropdownSizes.ClearOptions();
        dropDownArticles.AddOptions(mOptiondataArticles);
        dropdownSizes.AddOptions(mOptiondataSizes);
    }
    void addDebugInfo(string message)
    {
        debugText.text += "\n--" + message;
    }

    // For article dropdown's onchange listener
    void changeSizes(int index)
    {
        // A lot new sizes
        mOptiondataSizes = new List<Dropdown.OptionData>();
        addDebugInfo("New index: " + index.ToString());
        foreach (string size in mArticles[index].sizes)
        {
            Dropdown.OptionData tempOptionSize = new Dropdown.OptionData();
            tempOptionSize.text = size;
            mOptiondataSizes.Add(tempOptionSize);
        }

        // Assign to size dropdown
        dropdownSizes.ClearOptions();
        dropdownSizes.AddOptions(mOptiondataSizes);
    }

    // To capture image
    void captureImage()
    {
        if (mFormatRegistered && mAccessCameraImage)
        {
            Vuforia.Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);
            if (image != null)
            {   
                Texture2D captureTexture = new Texture2D(200, 500);
                image.CopyToTexture(captureTexture, false);
                byte[] pixels = captureTexture.EncodeToJPG();
                //byte[] pixels = image.Pixels;
                if (pixels != null && pixels.Length > 0)
                {
                    string filename = "/SS_test.jpeg";
                    #if UNITY_EDITOR
                        File.WriteAllBytes(mPurchaseDataPath + "outputs/CameraCapture_" + System.DateTime.Now.Year + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Day + ".jpeg", pixels);
                        addDebugInfo(mAndroidPersistentDatapath);
                    #else 
                        string path = mAndroidPersistentDatapath + filename;
                        File.WriteAllBytes(path, pixels);
                        Debug.Log(path);
                        addDebugInfo(path);
                    #endif
                }
            }
        }
    }

    // for vuforia started callback
    private void OnVuforiaStarted()
    {
        // Vuforia has started, now register camera image format  
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            Debug.Log("Successfully registered pixel format " + mPixelFormat.ToString());
            mFormatRegistered = true;
        }
        else
        {
            Debug.LogError(
              "Failed to register pixel format " + mPixelFormat.ToString() +
              "\n the format may be unsupported by your device;" +
              "\n consider using a different pixel format.");

            mFormatRegistered = false;
        }
    }

    // To unregister vuforia's pixel format on pause and re-register it on resume
    void OnPause(bool paused)
    {
        if (paused)
        {
            Debug.Log("App was paused");
            UnregisterFormat();
        }
        else
        {
            Debug.Log("App was resumed");
            RegisterFormat();
        }
    }

    // To register frame format
    void RegisterFormat()
    {
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            Debug.Log("Successfully registered camera pixel format " + mPixelFormat.ToString());
            mFormatRegistered = true;
        }
        else
        {
            Debug.LogError("Failed to register camera pixel format " + mPixelFormat.ToString());
            mFormatRegistered = false;
        }
    }

    // To unregister frame format
    void UnregisterFormat()
    {
        Debug.Log("Unregistering camera pixel format " + mPixelFormat.ToString());
        CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
        mFormatRegistered = false;
    }


}
