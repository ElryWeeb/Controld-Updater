# Automatic Unlocator IP Updater

<img src="https://github.com/ElryWeeb/Unlocator_DynIP_App/blob/main/Unlocator/Resources/Images/unlocator.png" width=100></img>

Automatically update your (WLAN) IP from your Android via Unlocker API Key.

<a name="req">Usage Requirements: </a>
- Unlocator Subsrciption --> https://unlocator.com/account/signup/user
- Your API Key --> https://unlocator.com/account/advanced
- Android Phone with Android 10 or higher.
- The APK File from Releases or your own Build.

<a name="how">How to Use the App</a>
- Install the App
- Enter your API Key in the Settings
- Click on the Start Button
- Done

<a name="edit">Edit the App and Build it Yourself </a>
- Download the Repository and open it with Visual Studio 2022 with Tools for MAUI.
- Wait for it to set up the Project on your PC.
- Save and Close the Repository after you made your Changes
- Open a CMD inside the Repository Folder
- Execute this Command: ```dotnet publish -c Release -f net7.0-android -p:PackageFormat=Apk```
- The Final APK is under ```Unlocator_DynIP_App\Unlocator\bin\Release\net7.0-android```
