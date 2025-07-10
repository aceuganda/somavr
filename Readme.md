# SomaVR - VR Platform

SomaVR is an immersive virtual reality training platform built with Unity that enables interactive 360-degree video experiences with integrated assessments.

## Requirements

- Unity 2019.4.28f1 (LTS) or compatible version
- Compatible VR headset (Oculus Quest 2 recommended)
- Minimum 8GB RAM
- Graphics card with VR capability

## VR Platform Support

This version has been optimized for size and targets specific VR platforms:
- Oculus Quest/Quest 2 (primary)
- Other platforms may require additional SDK installation


The platform-specific plugin binaries are not included in this repository to reduce size. They will be automatically restored when you import the required SDKs

For other VR platforms, please install the appropriate SDK:
- SteamVR: Available on Unity Asset Store
- Windows Mixed Reality: Available through Unity XR Plugin Management
- Google VR: Available on Unity Asset Store. 

## Installation

1. Clone this repository
2. Open Unity Hub
3. Add project by selecting the downloaded folder
4. Open project with Unity 2019.4.28f1

## Setting Up Videos

Due to file size limitations and ethical considerations, the training videos used in the COVID-19 IPC and surgical training projects are not included in this repository. To add your own videos:

1. Navigate to `/Assets/Interactive360/Videos/`
2. Add your 360-degree video files (supported formats: .mp4, .mov) Here is an example of one the 360 videos that we used(https://drive.google.com/file/d/1O6v09u_h4Sml4lpK1aFM2iKeBcvPIB_P/view?usp=sharing)
3. Ensure videos are encoded with:
   - H.264 codec
   - Equirectangular projection
   - Recommended resolution: 4K (3840x2160) or higher
   - Stereo or mono format

## Configuration




### Setting Up Questions

1. In Unity Editor, right-click in Project window
2. Select Create > SomaVR > Question Set
3. Fill in the question details:
   - Module ID and Name
   - Questions with timestamps
   - Multiple choice options
   - Correct answers
   - Feedback messages

### Scene Setup

1. Open the main scene in `/Assets/Interactive360/Scenes/MainScene`
2. Ensure the following components are present:
   - VideoPlayer
   - QuestionManager
   - APIService (if using external data collection)

### API Configuration (Optional)

If using the data collection feature:

1. Open `/Assets/Interactive360/Scripts/APIService.cs`
2. Update `API_BASE_URL` with your endpoint
3. Modify data structure if needed

### Usage

1. Press Play in Unity Editor to test
2. Use the VR controllers to:
   - Navigate menus
   - Select answers
   - Control video playback
3. Questions appear automatically at specified timestamps
4. Performance data is collected and stored locally/sent to API

## Citation

If you use this code in your research or project, please cite:

```
TBD
```
