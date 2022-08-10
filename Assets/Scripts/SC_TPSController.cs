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
    GameObject treatmentMenu;
    Button[] treatmentOptions;
    public TextAsset treatmentFile;
    private Dictionary<string, List<string>> treatments;
    string patientCondition;
    List<string> patientTreatment;    



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        prompt = GameObject.FindGameObjectWithTag("Prompt");
        energy = GameObject.FindGameObjectWithTag("Energy");
        treatmentMenu = GameObject.FindGameObjectWithTag("TreatmentMenu");
        InstantiateTM();
        toggleTreatment.gameObject.SetActive(false);
        treatmentMenu.transform.localScale = new Vector3(0,0,0);
    }

    void Update()
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
        MakeChoice();

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
            toggleTreatment.GetComponentInChildren<TMP_Text>().text = "Treat "+ patient.Name;
            toggleTreatment.gameObject.SetActive(true);
            patientCondition = patient.Condition;
            patientTreatment = treatments[patientCondition];

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
    }

    // For Treatment
    private void InstantiateTM(){
        Treatments tData = JsonUtility.FromJson<Treatments>(treatmentFile.text);
        treatments = new Dictionary<string, List<string>>();

        foreach(Treatment treatment in tData.treatments){
            List<string> steps = new List<string>(treatment.steps);
            treatments.Add(treatment.condition, steps);

            treatmentOptions = treatmentMenu.GetComponentsInChildren<Button>();
            for(int i = 0; i<treatmentOptions.Length; i++){
                var index = i;
                treatmentOptions[index].onClick.AddListener(delegate{ChooseTreatment(treatment.options[index]);});
                treatmentOptions[index].GetComponentInChildren<TMP_Text>().text = treatment.options[index];
            }
        }
    }


    // Temp function since I can't click
    private void MakeChoice(){
        if(Input.GetKeyDown(KeyCode.U)){
            ChooseTreatment("Start Oxygen");
        }else if(Input.GetKeyDown(KeyCode.I)){
            print("Clicked 2");
            ChooseTreatment("Start Fluid");
        }else if(Input.GetKeyDown(KeyCode.O)){
            ChooseTreatment("Capillary Blood Glucose");
        }else if(Input.GetKeyDown(KeyCode.P)){
            ChooseTreatment("Administer Medicine");
        }
    }


    private void ChooseTreatment(string choice){

        string outcome = "";
        if(patientTreatment.Count == 0){
            print("Patient has been treated");
        }else{
            if(string.Compare(patientTreatment[0], choice) == 0){
                treatments[patientCondition].RemoveAt(0);
                outcome = "Correct choice!";

                if(patientTreatment.Count == 0){
                    outcome = "You made the right choice, patient is treated!";
                }
            }else{
                outcome = "Wrong choice :(";
            }
        }
            prompt.GetComponent<Prompt>().promptText = outcome;
            prompt.GetComponent<Prompt>().isPromptUpdated = false;
    }
}