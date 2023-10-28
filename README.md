# Automatic Dyniamic DNS IP Updater

Automatically update your (WLAN) IP from your Android via API.

<a name="req">Usage Requirements: </a>
- Dynamic DNS with Update URL.
- Android Phone with Android 11 or newer
- The APK File

<a name="how">How to Use the App</a>
- Install the App
- Enter your API Key in the Settings
- Click on the Start Button
- Done

<a name="edit">Edit the App and Build it Yourself </a>
- Download the Repository and open it with Visual Studio 2022 with Tools for MAUI
- Wait for it to set up the Project on your PC
- Save and close the VS after you made your Changes
- Go into ```Unlocator_DynIP_App\Unlocator``` and delete the folder bin and obj
- Go into the parent Folder and start ```create_release.bat```
- The Final APKs are under ```Unlocator_DynIP_App\Unlocator\bin\Release\net7.0-android\publish```
