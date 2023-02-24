# ID Card Reader Thales

## Build

1. Build Visual Studio solution
    ```
    Build > Publish [project name] > Publish
    ```
2. Build Inno Setup installer
    ```
    Open `setup.iss` with Inno Setup.
    Build > Compile
    ```

Installer can be found in `Output` folder.

### Notes:

* Use the inno 5.6.1 instead of innosetup 6.2.1 because currently, 6.2.1 create an installer that is detected as a virus by Windows.
* Install  Microsoft Visual C++ 2002-2003-2005-2008-2010-2012-2013-2022 (All In One) if needed.
