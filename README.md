![GitHub Tag](https://img.shields.io/github/v/tag/zimavi/WinttOS?logo=github&label=latest)
# WinttOS (Deprecated)

<b>This project has been suspended due to its inability to be supported. A new project aimed to be better organized and robust is [YukihanaOS](https://github.com/zimavi/Yukihana)

Test C# OS. Latest download [link](https://github.com/zimavi/WinttOS/releases/).
<b>Please use VMware as a virtual machine. I do not recommend you to run this OS on real hardware as it may cause your PC to break down</b>

## How to run on VMWare
You will need to create a drive because the OS needs to have a filesystem available to write and boot.<br>
First of all, you need a second virtual machine with either Windows or Linux installed (I have Arch and show commands for it)<br>
Add an IDE hard drive (edit **VM -> Add -> Hard Disk -> IDE -> Define a size you desire and set "Store virtual disk as a single file"**)<br>
In your virtual machine format the drive as FAT32 (for Linux use the command `mkfs.fat -F32 /dev/sda` and replace `sda` with your drive identifier)<br>
If you want you can mount the disk and place some files/packages inside<br>
Next, in the VM where you want to run WinttOS, add a drive (except, instead of '**Create a new virtual disk**' select '**Use an existing virtual disk**' and select the drive you've formatted)<br>
After that select your hard drive, then click **Advanced**, and make sure that the **Virtual device node** is set to `IDE 0:0` and CD/DVD set to `IDE 1:0`<br>
Now you should be able to start WinttOS
