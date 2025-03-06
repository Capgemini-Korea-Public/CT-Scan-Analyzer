# **CT Scan Analyzer**

## **Table of Contents**
- [Limitations](#limitations)
- [Example Scene Description](#example-scene-description)
- [How to Use](#how-to-use)
- [MonoBehaviour Script Components](#monobehaviour-script-components)
- [License](#license)

---

## **Overview**

This project demonstrates the functionality of **VTK** for diagnostic imaging and command execution within a Unity application.

It enables **CT scan analysis** of input models in Unity, as well as user-controlled interactions for manipulating CT scans or models.

---

## **Directory Structure**

```
📂 CT-Scan-Analyzer
│-- 📂 Plugins
│   ├── ffmpeg.exe (Required for speech-to-text audio preprocessing)
│-- 📂 Resources
│   ├── HuggingFaceAPIConfig (Requires API key for sentence similarity module)
│   ├── Sentis model (ONNX model required for speech-to-text module)
│-- 📂 StreamingAssets
│   ├── Data files necessary for the demo scene
│-- 📂 Scenes
│   ├── main scene (Demo scene showcasing project features)
│-- 📂 Prefabs
│   ├── Controller
│   ├──── MainController.prefab
│   ├── UI
│   ├──── Canvas.preab
│-- 📂 Modules
│   ├── voice recording
│   ├── speech-to-text
│   ├── llm
│   ├── sentence similarity
```

- **Plugins**
  - To use the **speech-to-text module**, download and manually add `ffmpeg.exe`.
  - This file is used for **audio preprocessing** when executing audio-based commands.

- **Resources**
  - **HuggingFaceAPIConfig**:
    - Requires an **API key** for the **sentence similarity module**.
    - *(If the `sentence similarity model` in `MainController` is set to `Sentis`, an API key is not required.)*
  - **Sentis Model**:
    - Requires a **downloaded ONNX model** for speech-to-text conversion.
    - *(See README – [How to Use (Step 4)](#how-to-use) for download instructions.)*

- **StreamingAssets**
  - Contains **data files necessary for the demo scene**.
  - Copy the contents of this folder to `Assets/StreamingAssets` in your Unity project.

- **Scenes**
  - The **main scene** serves as the **example/demo scene**.
  - *(For feature details, see [Example Scene Description](#example-scene-description).)*

- **Modules**
  - Contains **scripts** for various modules, including:
    - **Voice Recording**
    - **Speech-to-Text (STT)**
    - **LLM (Language Model)**
    - **Sentence Similarity**

---

## **Limitations**

- **OS Support**:
  - The **native plugin is implemented and tested only on Windows**.

- **Sentis Support**:
  - The **Sentis plugin requires Unity 2023.2 or later**.
  - Use **Unity 2023.2 or newer** to ensure compatibility.

- **Built-in Rendering Support**:
  - The **VTK package only works with the Built-in Render Pipeline**.
  - **Unity 6 and later versions are not supported**.

---

## **Example Scene Description**

The following features are available in the **main scene** included with this asset.
The **public variables** of the script are exposed in the **Unity Editor**, allowing users to control various functionalities in the example scene.

### **[MainController.cs]**

#### **Speech-to-Text Model**

- Select **STT model type**:
  - **Sentis**
  - **OpenAI Whisper**
- **Max Audio Duration**:
  - Defines the **time segmentation for long audio files** before converting them into text.
- **STT Converted String Output**:
  - After recording audio and pressing **Enter**, the recognized **text output** will be displayed.

#### **LLM (Large Language Model)**

- Select **LLM model type**:
  - **Sentis**
  - **OpenAI (RESTful API)**
- **Localhost Option**:
  - Requires setting up **Ollama manually** (not yet supported, planned for future updates).
  - Use **RESTful API option** for now.
- **LLM Output String**:
  - Displays **the model’s response** to user queries.

#### **Sentence Similarity Model**

- Select **sentence similarity model type**:
  - **Sentis**
  - **Hugging Face API**

#### **Elements**

- Displays **captured images** for analysis.
- Allows users to specify **whether the current image will be used for further inquiries**.


---

## **How to use**
To use the CT-Scan-Analyzer, follow these steps:

### 1. HuggingFace API Setup
![HuggingFace Setup](https://github.com/user-attachments/assets/f5dabc08-fc79-402b-9c64-3d868e290b9b)

Follow the detailed guide to set up the HuggingFace API **`(complete up to Step 3)`**
- **[HuggingFace API Installation Tutorial](https://thomassimonini.substack.com/p/building-a-smart-robot-ai-using-hugging)**
```csharp
https://github.com/huggingface/unity-api.git
```

### 2. configure your local environment:

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

### 3. Installing FFmpeg

This package uses FFmpeg to process audio files, so you must install FFmpeg on your computer before using the package.

#### For Windows

1. Visit the [FFmpeg Downloads page](https://ffmpeg.org/download.html).
2. Click on **Windows** and then follow the link under **Windows EXE Files** for **Windows builds from gyan.dev**.
3. Download the FFmpeg build, extract the archive, and locate the `ffmpeg.exe` file in the `bin` folder.
4. In your Unity project, create a folder named **Plugins** inside the **Assets** folder.
5. Place `ffmpeg.exe` into the **Assets/Plugins** folder.

### 4. Installing Whisper Sentis Model

To convert audio files using the Sentis model, follow these steps:

1. **Create a Resources Folder**  
- In your project's **Assets** folder, create a folder named exactly `Resources`.

2. **Download the Models and Data**  
- Visit [Sentis Whisper Tiny](https://huggingface.co/unity/sentis-whisper-tiny).
- Under the **Files and versions** tab, download the four ONNX models from the `Assets/Models` folder.
- Also, download the `vocab.json` file from the `Assets/Data` folder.

3. **Place the Files**  
- Move the `four downloaded model files` and the `vocab.json` file into your newly created `Assets/Resources` folder.

Your Sentis model setup is now complete.

### 5. Clone the repository.

### 6. Open the project in Unity.

### 7. Run the application and start interacting with CT scan data using voice commands.
---

## **MonoBehaviour Script Components**
This project is a CT scan analysis system that integrates various modules for user interaction, camera control, data processing, and visualization. Below is an overview of the key scripts and their functionalities.

### Scripts

#### MainController.cs
The central control script that manages the overall system workflow and interactions between different modules.
- Manages interactions with other modules
- Coordinates the overall workflow
- Processes user input and directs it to the appropriate module

#### LLMModule.cs
Handles interactions with the Large Language Model (LLM).
- Communicates with the LLM API
- Sends text input to the LLM and receives responses
- Processes responses and forwards results to other modules

#### SentenceSimilarityController.cs
Performs sentence similarity analysis.
- Calculates the similarity between input text and predefined commands
- Determines and returns the most similar command
- Sends analysis results to the MainController

#### CameraCaptureSystem.cs
Manages the camera capture functionality.
- Controls access to and operation of the device camera
- Captures and saves images
- Forwards captured images for analysis

#### HUDUI.cs
Controls the Head-Up Display (HUD) user interface.
- Displays and updates HUD elements
- Handles user interactions
- Shows system status and results

#### VTKUIController.cs
Controls UI elements related to Visualization Toolkit (VTK).
- Adjusts VTK visualization settings
- Handles VTK-related user input
- Displays VTK rendering results

#### CameraRotationController.cs
Manages camera rotation.
- Processes user input to control camera rotation
- Configures rotation speed and limits
- Interacts with other camera controllers

#### CameraFovController.cs
Controls the camera's Field of View (FOV).
- Provides FOV adjustment functionality
- Implements zoom in/out effects
- Updates rendering based on FOV changes

#### CameraOrthoZoomController.cs
Controls zoom functionality in Orthographic projection mode.
- Implements zoom in/out in orthographic mode
- Adjusts and limits zoom levels
- Updates the viewport according to zoom changes

---

## **License**
MIT License

Copyright (c) 2025 Capgemini-Korea

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.