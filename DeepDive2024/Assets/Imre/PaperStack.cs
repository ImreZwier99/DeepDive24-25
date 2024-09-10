using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PaperStack : MonoBehaviour
{
    // Public variables to be set in the Unity Editor
    public GameObject prefabToCopy; // Reference to the prefab to instantiate
    public GameObject parentObject; // Reference to the empty GameObject (parent)
    public TextMeshProUGUI counterText; // Reference to the TextMeshProUGUI element for displaying the counter
    public float offsetHeight = 1.0f; // Height offset for each stacked prefab
    public int numberOfStacks = 4; // Number of prefabs to stack initially, set in the editor
    public float textYOffset = 2.0f; // Offset to position the text above the stack

    private int startingCounter; // The dynamic starting value of the counter
    private List<GameObject> stackedPrefabs = new List<GameObject>(); // List to keep track of stacked prefabs
    private List<int> triggerValues; // List of dynamic trigger values

    public int counter; // The counter that counts down

    void Start()
    {
        // Dynamically set startingCounter based on the number of stacks
        startingCounter = numberOfStacks * 5;

        // Initialize the counter with the startingCounter
        counter = startingCounter;

        // Generate dynamic trigger values based on the number of stacks
        triggerValues = new List<int>();
        for (int i = 0; i < numberOfStacks; i++)
        {
            triggerValues.Add(i * 5);
        }

        // Stack the number of prefabs defined by numberOfStacks
        for (int i = 0; i < numberOfStacks; i++)
        {
            StackPrefabAtHeight(i);
        }

        // Update the counter text UI at the start
        UpdateCounterText();
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease the counter when pressing a key (for testing purposes)
        if (Input.GetKeyDown(KeyCode.Space)) // Press Space to simulate decreasing the counter
        {
            DecreaseCounter();
        }

        // Update the position of the counter text above the stack
        UpdateCounterTextPosition();
    }

    // This function decreases the counter and checks if it should remove a stack
    void DecreaseCounter()
    {
        // Decrease the counter by 1
        counter--;

        // Check if the current counter value matches one of the trigger values
        foreach (int value in triggerValues)
        {
            if (counter == value)
            {
                RemoveLastStack();
            }
        }

        // Optional: Ensure the counter doesn't go below 0
        if (counter < 0)
        {
            counter = 0;
        }

        // Update the text when the counter changes
        UpdateCounterText();
    }

    // Method to stack the prefab at a specific height as a child of the parent
    void StackPrefabAtHeight(int index)
    {
        // Calculate the new position for the prefab to stack above the previous one
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + offsetHeight * index, transform.position.z);

        // Set the rotation to 90 degrees along the X-axis
        Quaternion rotation = Quaternion.Euler(90, 0, 0);

        // Instantiate a new copy of the prefab at the new position, with the desired rotation, as a child of the parentObject
        GameObject newStack = Instantiate(prefabToCopy, newPosition, rotation, parentObject.transform);

        // Add the newly created prefab to the list of stacked prefabs
        stackedPrefabs.Add(newStack);
    }

    // Method to remove the last stacked prefab
    void RemoveLastStack()
    {
        if (stackedPrefabs.Count > 0)
        {
            // Get the last prefab in the list
            GameObject lastStack = stackedPrefabs[stackedPrefabs.Count - 1];

            // Remove it from the list
            stackedPrefabs.RemoveAt(stackedPrefabs.Count - 1);

            // Destroy the last stacked prefab
            Destroy(lastStack);
        }
    }

    // Method to update the counter text
    void UpdateCounterText()
    {
        // If counter reaches zero, disable the text element
        if (counter == 0)
        {
            counterText.gameObject.SetActive(false); // Hide the counter when it reaches zero
        }
        else
        {
            // Ensure the counter text is active and update the text
            counterText.gameObject.SetActive(true);
            counterText.text = counter.ToString();
        }
    }

    // Method to update the position of the counter text above the stack
    void UpdateCounterTextPosition()
    {
        if (counterText != null)
        {
            // Position the text element above the top of the stack
            float highestYPosition = transform.position.y + offsetHeight * stackedPrefabs.Count + textYOffset;
            counterText.transform.position = new Vector3(transform.position.x, highestYPosition, transform.position.z);
        }
    }
}
