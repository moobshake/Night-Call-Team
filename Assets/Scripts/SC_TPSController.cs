using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(CharacterController))]

public class SC_TPSController : MonoBehaviour
{
    public float speed = 7.5f;
    public float initialSpeed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;
    public Button toggleTreatment;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;

    GameObject prompt;
    GameObject energy;
    GameObject clock;
    GameObject foodSpawner;
    GameObject vitalPreview;
    GameObject treatmentMenu;
    GameObject wellnessCount;
    GameObject deathCount;

    Button[] treatmentOptions;
    string[] optionArray;
    public TextAsset treatmentFile;
    private Dictionary<string, List<string>> treatments;
    private Dictionary<string, List<string>> treatmentProgress;
    string currentPatient;

    private bool isTreated = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        prompt = GameObject.FindGameObjectWithTag("Prompt");
        energy = GameObject.FindGameObjectWithTag("Energy");
        clock = GameObject.FindGameObjectWithTag("Clock");
        foodSpawner = GameObject.FindGameObjectWithTag("FoodSpawner");
        vitalPreview = GameObject.FindGameObjectWithTag("Vitals");
        treatmentMenu = GameObject.FindGameObjectWithTag("TreatmentMenu");
        wellnessCount = GameObject.FindGameObjectWithTag("WellnessCount");
        deathCount = GameObject.FindGameObjectWithTag("DeathCount");

        treatmentProgress = new Dictionary<string, List<string>>();
        InstantiateTM();
        toggleTreatment.gameObject.SetActive(false);
        treatmentMenu.transform.localScale = new Vector3(0,0,0);
        HideVitals();
    }

    void Update()
    {
        if (clock.GetComponent<Clock>().timerIsRunning)
        {
            if (characterController.isGrounded)
            {
                // We are grounded, so recalculate move direction based on axes
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);
                float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
                float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
                moveDirection = (forward * curSpeedX) + (right * curSpeedY);

                if (Input.GetButton("Jump") && canMove)
                {
                    moveDirection.y = jumpSpeed;
                }
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= gravity * Time.deltaTime;

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (canMove)
            {
                rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
                rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
                playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
                transform.eulerAngles = new Vector2(0, rotation.y);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "NapBed")
        {
            prompt.GetComponent<Prompt>().promptText = "Press E to rest!";
            prompt.GetComponent<Prompt>().isPromptUpdated = false;
        }
        if (other.tag == "Patient")
        {
            prompt.GetComponent<Prompt>().promptText = "Collided with patient";
            prompt.GetComponent<Prompt>().isPromptUpdated = false;

            PatientInfo patient = other.gameObject.GetComponent<PatientInfo>();
            ShowVitals(patient.Condition);
            if(!treatmentProgress.ContainsKey(patient.Name)){
                treatmentProgress.Add(patient.Name, treatments[patient.Condition]);
            }

            currentPatient = patient.Name;
            toggleTreatment.GetComponentInChildren<TMP_Text>().text = "Treat "+ patient.Name;
            toggleTreatment.gameObject.SetActive(true);

            isTreated = false;

        }
        if (other.tag == "Food")
        {
            prompt.GetComponent<Prompt>().promptText = "Press E to eat food!";
            prompt.GetComponent<Prompt>().isPromptUpdated = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E) && other.tag == "NapBed")
        {
            prompt.GetComponent<Prompt>().promptText = "Resting. Hold down 'E' to regain more health!";
            prompt.GetComponent<Prompt>().isPromptUpdated = false;
            energy.GetComponent<Energy>().energyLevel += 1;
            energy.GetComponent<Energy>().isEnergyUpdated = false;
        }
        if (Input.GetKey(KeyCode.E) && other.tag == "Food")
        {
            prompt.GetComponent<Prompt>().promptText = "Ate food, gain 30 energy!";
            prompt.GetComponent<Prompt>().isPromptUpdated = false;
            energy.GetComponent<Energy>().energyLevel += 30;
            energy.GetComponent<Energy>().isEnergyUpdated = false;
            foodSpawner.GetComponent<FoodSpawner>().isFoodAvailable = false;
            Destroy(other.gameObject);
        }
        if (isTreated && other.tag == "Patient")
        {
            isTreated = false;
            treatmentMenu.transform.localScale = new Vector3(0, 0, 0);
            toggleTreatment.gameObject.SetActive(false);
            HideVitals();
            currentPatient = " ";

            treatmentOptions = treatmentMenu.GetComponentsInChildren<Button>();
            for (int i = 0; i < treatmentOptions.Length; i++)
            {
                var index = i;
                if (!treatmentOptions[index].interactable)
                {
                    treatmentOptions[index].interactable = true;
                }
            }

            wellnessCount.GetComponent<Wellnes>().wellnessLevel += 1;
            wellnessCount.GetComponent<Wellnes>().isWellnessUpdated = false;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Patient"){
            treatmentMenu.transform.localScale = new Vector3(0,0,0);
            toggleTreatment.gameObject.SetActive(false);
            HideVitals();
            currentPatient = " ";

            treatmentOptions = treatmentMenu.GetComponentsInChildren<Button>();
            for(int i = 0; i<treatmentOptions.Length; i++){
                var index = i;
                if(!treatmentOptions[index].interactable){
                    treatmentOptions[index].interactable = true;
                }
            }
        }
    }

    // Treatment Menu
    private void InstantiateTM(){
        Treatments tData = JsonUtility.FromJson<Treatments>(treatmentFile.text);
        treatments = new Dictionary<string, List<string>>();

        foreach(Treatment treatment in tData.treatments){
            List<string> steps = new List<string>(treatment.steps);
            treatments.Add(treatment.condition, steps);
            
            treatmentOptions = treatmentMenu.GetComponentsInChildren<Button>();
            optionArray = treatment.options;

            for(int i = 0; i<treatmentOptions.Length; i++){
                var index = i;
                treatmentOptions[index].onClick.RemoveAllListeners();
                treatmentOptions[index].onClick.AddListener(() => ChooseTreatment(treatment.options[index], treatmentOptions[index]));
                treatmentOptions[index].GetComponentInChildren<TMP_Text>().text = treatment.options[index];
            }
        }
    }

    private void ChooseTreatment(string choice, Button chosenButton){
        ResetListeners();
        List<string> curT = new List<string>(treatmentProgress[currentPatient]);
        string outcome = "";

        if (curT.Contains(choice)) {
            chosenButton.interactable = false;
            curT.Remove(choice);
            outcome = "Correct choice!";
            treatmentProgress[currentPatient] = curT;

            isTreated = true;

        }
        else{
            outcome = "Wrong choice D:";
        }

        if(treatmentProgress[currentPatient].Count == 1){
            outcome = "Congrats " + currentPatient + " has been treated";
            treatmentProgress.Remove(currentPatient);
            currentPatient = "";
        }

        prompt.GetComponent<Prompt>().promptText = outcome;
        prompt.GetComponent<Prompt>().isPromptUpdated = false;
    }

    private void ResetListeners(){
        treatmentOptions = treatmentMenu.GetComponentsInChildren<Button>();
        for(int i = 0; i<treatmentOptions.Length; i++){
            var index = i;
            treatmentOptions[index].onClick.RemoveAllListeners();
            treatmentOptions[index].onClick.AddListener(delegate{ChooseTreatment(optionArray[index], treatmentOptions[index]);});
        }
    }

    // Vitals Menu
    private void ShowVitals(string condition){
        Image[] vitals = vitalPreview.GetComponentsInChildren<Image>();
        if(condition == "High Blood Pressure"){
            vitals[0].gameObject.transform.localScale = new Vector3(1,1,1);
        }else if((condition == "Pneumonia")){
            vitals[1].gameObject.transform.localScale = new Vector3(1,1,1);
        }

    }

    private void HideVitals(){
        Image[] vitals = vitalPreview.GetComponentsInChildren<Image>();
        foreach(Image i in vitals){
            i.gameObject.transform.localScale = new Vector3(0,0,0);
        }
    }

}