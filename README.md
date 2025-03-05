# CT-Scan-Analyzer
CT-Scan-Analyzer is a Unity-based project that implements a command system for CT scan analysis using advanced technologies such as Audio Recording (AR), Speech-to-Text (STT), Sentence Similarity(SS), and Large Language Models (LLM). This innovative system allows users to interact with CT scan data using voice commands, providing a hands-free and efficient way to analyze medical imaging.

# Project Overview
## Key Features
### Audio Recording (AR)

Captures user voice commands for seamless interaction.

### Speech-to-Text (STT)

Converts audio input into text for further processing.

### Sentence Similarity (Text Preprocessing)

Analyzes the STT output to determine the most similar command among "analyze," "rotate," "scaleup," and "initialize" using Sentence Similarity techniques.

Prepares the appropriate prompt based on the identified command:

For "analyze": Preprocesses the image data.

For "rotate" and "scaleup": Structures the output as JSON, e.g., {angle: ?, dir: ?}.

### LLM Processing

Utilizes a Large Language Model to execute the preprocessed commands.

### Result Postprocessing

For "analyze": Directly outputs the text result.

For "rotate" and "scaleup": Parses the JSON output to extract command parameters.

### Command Execution

Performs the requested operation on the CT scan data based on the processed results.

# Getting Started
To use the CT-Scan-Analyzer, follow these steps:

Clone the repository.

Open the project in Unity.

Ensure all necessary dependencies are installed.

Run the application and start interacting with CT scan data using voice commands.

# Contributing
We welcome contributions to the CT-Scan-Analyzer project. Please feel free to submit issues, fork the repository, and send pull requests.

# License
MIT License

# Contact
For any questions or concerns, please open an issue in the GitHub repository.
