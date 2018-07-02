# BlinkStick Busylight Client

![BlinkStick Logo](__ressourcen/merch/logo-homepage.png)

A Busylight Client for the [BlinkStick Pro](https://www.blinkstick.com/).

**This is an Alpha version!**


## Included Projects
Included projects:
- BlinkStickBusylightClient


## How to test
### Checkout
Checkout the project with git. Then open then Visual Studio solution. Go to **Extras->NuGet Package Manager->Package Manager Console** to open the **Package Manager Console**. Then type the command *Update-Package* to update the dependencies. If you want to setup it manually, have a look at the **3rd Party Libs** list. Now the build should work.


## 3rd Party Libs
- BlinkStickDotNet.dll


## ToDo
The following tasks are open:
- [x] Keyboard Shortcuts
- [x] Connect / Disconnect monitor
- [x] Change color on dekstop locking / unlocking
- [ ] Save / Restore settings
- [x] Pulse and Blink is not working correct
- [x] Disconnect somtimes not correctly handled

## Known Issues
- Only working with BlinkStick Pro
- Not working with multiple BlinkSticks connected