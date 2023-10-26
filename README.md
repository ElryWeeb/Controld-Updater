# Automatic Unlocator IP Updater

<img src="https://github.com/ElryWeeb/Unlocator_DynIP_App/blob/main/Unlocator/Resources/Images/unlocator.png" width=100></img>

Automatically update your (WLAN) IP from your Android via Unlocker API Key.

<a name="req">Usage Requirements: </a>
- Unlocator --> https://unlocator.com/account/signup/user
- Your API Key --> https://unlocator.com/account/advanced
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
- Open a CMD shell
- Run ```keytool -genkeypair -v -keystore myapp.keystore -alias myapp -keyalg RSA -keysize 2048 -validity 10000```
- Run ```cd ..```
- Run (replace your_password with yours) ```dotnet publish -c Release -f net7.0-android -p:PackageFormat=Apk -p:AndroidKeyStore=true -p:AndroidSigningKeyStore=myapp.keystore -p:AndroidSigningKeyAlias=myapp -p:AndroidSigningKeyPass=your_password -p:AndroidSigningStorePass=your_password```
- The Final APKs are under ```Unlocator_DynIP_App\Unlocator\bin\Release\net7.0-android\publish```
