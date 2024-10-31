using Cosmos.System.FileSystem;
using Cosmos.System.Graphics.Fonts;
using System.Collections.Generic;
using System;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Kernel;
using WinttOS.Core.Utils.Sys;
using WinttOS.Core;
using System.IO;
using WinttOS.wSystem.GUI;
using Cosmos.System.Graphics;

namespace WinttOS.wSystem.Filesystem
{
    internal class BootResourceLoader
    {

        private static BootResourceLoader instance = new();

        public static void LoadResources()
        {
            ShellUtils.PrintTaskResult("Loading", ShellTaskResult.DOING, "System Fonts");

            ShellUtils.MoveCursorUp();

            List<Disk> disks = GlobalData.FileSystem.GetDisks();

            int found = 0;

            foreach (Disk disk in disks)
            {
                foreach (var part in disk.Partitions)
                {
                    if (part.HasFileSystem)
                    {
                        if (File.Exists(part.RootPath + @"\boot\.resources\fonts\zap-ext-light18.psf"))
                        {
                            try
                            {
                                Logger.DoBootLog("[OK] Found 'zap-ext-light18.psf'");
                                Logger.DoBootLog("[Info] Loading resource");
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.DOING, "System Fonts");

                                byte[] font = File.ReadAllBytes(part.RootPath + @"\boot\.resources\fonts\zap-ext-light18.psf");
                                Core.Utils.Sys.Files.Fonts.Font18 = PCScreenFont.LoadFont(font);
                                found++;
                                ShellUtils.MoveCursorUp();
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.OK, "System Fonts");
                                Logger.DoBootLog("[OK] 'zap-ext-light18.psf' loaded");
                            }
                            catch (ArgumentException e)
                            {
                                Logger.DoBootLog("[Error] Cannot load resource -> " + e.Message);
                                ShellUtils.MoveCursorUp();
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "System Fonts: Invalid file");
                                _ = new KernelPanic("Kernel load failed: Invalid resource file", instance);
                            }
                            catch (Exception e)
                            {
                                ShellUtils.MoveCursorUp();
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "System Fonts: " + e.Message);
                                Logger.DoBootLog("[Error] Cannot load resource -> " + e.Message);
                                _ = new KernelPanic("Kernel load failed: Invalid resource file", instance);
                            }
                        }
                        if (File.Exists(part.RootPath + @"\boot\.resources\gui\wallpapers\bg0.bmp"))
                        {
                            try
                            {
                                Logger.DoBootLog("[OK] Found 'bg0.bmp'");
                                Logger.DoBootLog("[Info] Loading resource");
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.DOING, "Images 'bg0.bmp'");
                                byte[] image = File.ReadAllBytes(part.RootPath + @"\boot\.resources\gui\wallpapers\bg0.bmp");
                                GUI.Files.Bg0 = new Bitmap(image);
                                found++;
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.OK, "Images 'bg0.bmp'");
                                Logger.DoBootLog("[OK] 'bg0.bmp' loaded");
                            }
                            catch (ArgumentException e)
                            {
                                Logger.DoBootLog("[Error] Cannot load resource -> " + e.Message);
                                ShellUtils.MoveCursorUp();
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "Images: 'bg0.bmp' Invalid file");
                                _ = new KernelPanic("Kernel load failed: Invalid resource file", instance);
                            }
                            catch (Exception e)
                            {
                                ShellUtils.MoveCursorUp();
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "Images 'bg0.bmp': " + e.Message);
                                Logger.DoBootLog("[Error] Cannot load resource -> " + e.Message);
                                _ = new KernelPanic("Kernel load failed: Invalid resource file", instance);
                            }
                        }
                        if (File.Exists(part.RootPath + @"\boot\.resources\gui\cursor.bmp"))
                        {
                            try
                            {
                                Logger.DoBootLog("[OK] Found 'cursor.bmp'");
                                Logger.DoBootLog("[Info] Loading resource");
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.DOING, "Images 'cursor.bmp'");
                                byte[] image = File.ReadAllBytes(part.RootPath + @"\boot\.resources\gui\cursor.bmp");
                                GUI.Files.CursorImage = new Bitmap(image);
                                found++;
                                ShellUtils.MoveCursorUp();
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.OK, "Images 'cursor.bmp'");
                                Logger.DoBootLog("[OK] 'cursor.bmp' loaded");
                            }
                            catch (ArgumentException e)
                            {
                                Logger.DoBootLog("[Error] Cannot load resource -> " + e.Message);
                                ShellUtils.MoveCursorUp();
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "Images: 'cursor.bmp' Invalid file");
                                _ = new KernelPanic("Kernel load failed: Invalid resource file", instance);
                            }
                            catch (Exception e)
                            {
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "Images 'cursor.bmp': " + e.Message);
                                ShellUtils.MoveCursorUp();
                                Logger.DoBootLog("[Error] Cannot load resource -> " + e.Message);
                                _ = new KernelPanic("Kernel load failed: Invalid resource file", instance);
                            }
                        }
                    }
                }
                if (found >= 3)
                    break;
            }

            if (found < 3)
            {
                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "Resources not found!");
                _ = new KernelPanic("Kernel load failed: Cannot find resource file", instance);
            }
        }
    }
}
