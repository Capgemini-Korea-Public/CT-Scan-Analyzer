# CT-Scan-Analyzer
CT-Scan-Analyzer is a Unity-based project that implements a command system for CT scan analysis using advanced technologies such as Voice Recording (VR), Speech-to-Text (STT), Sentence Similarity(SS), and Large Language Models (LLM). This innovative system allows users to interact with CT scan data using voice commands, providing a hands-free and efficient way to analyze medical imaging.

# Project Overview
## Video Sample


https://github.com/user-attachments/assets/3209dbfc-5014-491c-b0cf-58c4c1c0c999

## Key Features
### Voice Recording (VR)

Captures user voice commands for seamless interaction.

more detail : https://github.com/Capgemini-Korea-Public/Unity-VoiceRecorder

### Speech-to-Text (STT)

Converts audio input into text for further processing.

more detail : https://github.com/Capgemini-Korea-Public/Unity-Speech-To-Text

### Sentence Similarity (Text Preprocessing)

Analyzes the STT output to determine the most similar command among "analyze," "rotate," "scaleup," and "initialize" using Sentence Similarity techniques.

Prepares the appropriate prompt based on the identified command

- For "**diagnose**", diagnose the image data.
- For "**rotate**" and "**zoom**", Structures the output as JSON, e.g., {angle: ?, dir: ?}.

more detail : https://github.com/Capgemini-Korea-Public/Unity-Sentence-Similarity


### LLM Processing

Utilizes a Large Language Model to execute the preprocessed commands.

more detail : https://github.com/Capgemini-Korea-Public/Unity-LLM-Module

### Result Postprocessing

- For "**diagnose**", Directly outputs the text result.
- For "**rotate**" and "**zoom**", Parses the JSON output to extract command parameters.
- For "**reset**", reset the camera state value to the initial value.
  
### Command Execution

Performs the requested operation on the CT scan data based on the processed results.

# Getting Started
To use the CT-Scan-Analyzer, follow these steps:

## 1. HuggingFace API Setup
![HuggingFace Setup](https://github.com/user-attachments/assets/f5dabc08-fc79-402b-9c64-3d868e290b9b)

Follow the detailed guide to set up the HuggingFace API **`(complete up to Step 3)`**
- **[HuggingFace API Installation Tutorial](https://thomassimonini.substack.com/p/building-a-smart-robot-ai-using-hugging)**
```csharp
https://github.com/huggingface/unity-api.git
```

## 2. configure your local environment:

1. Create a folder named **.openai** in your user directory:
- For Windows: `C:\Users\<YourUserName>\`
2. Inside the **.openai** folder, create a file named `auth.json`.
3. In `auth.json`, enter your API credentials in the following JSON format:

```json
{
    "api_key": "your api key name",
    "organization": "your organization name"
}
```

## 3. Installing FFmpeg

This package uses FFmpeg to process audio files, so you must install FFmpeg on your computer before using the package.

### For Windows

1. Visit the [FFmpeg Downloads page](https://ffmpeg.org/download.html).
2. Click on **Windows** and then follow the link under **Windows EXE Files** for **Windows builds from gyan.dev**.
3. Download the FFmpeg build, extract the archive, and locate the `ffmpeg.exe` file in the `bin` folder.
4. In your Unity project, create a folder named **Plugins** inside the **Assets** folder.
5. Place `ffmpeg.exe` into the **Assets/Plugins** folder.


## 4. Clone the repository.

## 5. Open the project in Unity.

## 6. Run the application and start interacting with CT scan data using voice commands.

# Contributing
We welcome contributions to the CT-Scan-Analyzer project. Please feel free to submit issues, fork the repository, and send pull requests.

# License
MIT License

# Contact
For any questions or concerns, please open an issue in the GitHub repository.
