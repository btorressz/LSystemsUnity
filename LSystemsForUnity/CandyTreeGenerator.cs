using UnityEngine;

public class CandyTreeGenerator : MonoBehaviour
{
    // The initial string for the L-system
    private string axiom = "F";

    // The rules for the L-system
    private string rule = "F[+F]F[-F][F]";

    // The length of each segment in the tree
    public float segmentLength = 1f;

    // The angle to turn for each "+" or "-" symbol in the rule
    public float turnAngle = 90f;

    // The maximum depth of the L-system recursion
    public int maxDepth = 3;

    // The prefab for each segment of the tree
    public GameObject segmentPrefab;

    // The position of the next segment to be spawned
    private Vector3 currentPosition;

    // The rotation of the next segment to be spawned
    private Quaternion currentRotation;

    void Start()
    {
        // Initialize the position and rotation
        currentPosition = transform.position;
        currentRotation = transform.rotation;

        // Generate the L-system
        GenerateLSystem(axiom, maxDepth);
    }

    void GenerateLSystem(string input, int depth)
    {
        // If we have reached the maximum depth, exit the recursion
        if (depth == 0) return;

        // Create a new string to hold the output
        string output = "";

        // Iterate over each character in the input string
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            // If the character is an "F", spawn a new segment and move forward
            if (c == 'F')
            {
                Instantiate(segmentPrefab, currentPosition, currentRotation, transform);
                currentPosition += currentRotation * Vector3.up * segmentLength;
            }
            // If the character is a "+", turn to the right
            else if (c == '+')
            {
                currentRotation *= Quaternion.Euler(0f, turnAngle, 0f);
            }
            // If the character is a "-", turn to the left
            else if (c == '-')
            {
                currentRotation *= Quaternion.Euler(0f, -turnAngle, 0f);
            }
            // If the character is a "[", save the current position and rotation
            else if (c == '[')
            {
                Vector3 savedPosition = currentPosition;
                Quaternion savedRotation = currentRotation;
                GenerateLSystem(input.Substring(i + 1), depth - 1);
                currentPosition = savedPosition;
                currentRotation = savedRotation;
            }
            // If the character is a "]", exit the recursion
            else if (c == ']')
            {
                return;
            }
        }

        // Recursively generate the next level of the L-system using the output string
        GenerateLSystem(output, depth - 1);
    }
}