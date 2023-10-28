# Controld IP Updater

Update your Controld IP from your Android via API.

<a name="req">Usage Requirements: </a>
- Controld DNS and a Write API Key (https://controld.com/dashboard/api)
- Android Phone with Android 11 or newer
- The APK File

<a name="how">How to Use the App</a>
- Install the App
- Enter your API Key and your Resolver ID for the Device in the Settings#
- Restart the App
- Click on the Update Button
- Done

<a name="edit">Edit the App and Build it Yourself </a>
- Download the Repository and open it with Visual Studio 2022 with Tools for MAUI
- Wait for it to set up the Project on your PC
- Save and close the VS after you made your Changes
- Go into ```ElryControld\ControldUpdater``` and delete the folder bin and obj
- Go into the parent Folder and start ```create_release.bat```
- The Final APKs are under ```ElryControld\ControldUpdater\bin\Release\net7.0-android\publish```
