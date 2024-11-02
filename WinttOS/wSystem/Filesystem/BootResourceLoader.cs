using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Kernel;
using WinttOS.Core.Utils.Sys;

namespace WinttOS.wSystem.Filesystem
{
    internal class BootResourceLoader
    {

        private static readonly BootResourceLoader instance = new();        

        public static void LoadResources()
        {
            ShellUtils.PrintTaskResult("Loading", ShellTaskResult.DOING, "System Resources");

            var resources = new List<Resource>
            {
                new(@"\boot\.resources\fonts\zap-ext-light18.psf", fontData => Files.Fonts.Font18 = PCScreenFont.LoadFont(fontData), "Fonts 'zap-ext-light18.psf'"),
                new(@"\boot\.resources\gui\wallpapers\bg0.bmp", imageData => GUI.Files.Bg0 = new Bitmap(imageData), "Images 'bg0.bmp'"),
                new(@"\boot\.resources\gui\cursor.bmp", imageData => GUI.Files.CursorImage = new Bitmap(imageData), "Images 'cursor.bmp'"),
                new(@"\boot\.resources\gui\icons\48p\app_default.bmp", imageData => GUI.Files.Icons.p48.DefaultApp = new Bitmap(imageData), "Images 'app_default.bmp' 48p"),
                new(@"\boot\.resources\gui\icons\16p\app_default.bmp", imageData => GUI.Files.Icons.p16.DefaultApp = new Bitmap(imageData), "Images 'app_default.bmp' 16p"),
                new(@"\boot\.resources\gui\icons\32p\cross.bmp", imageData => GUI.Files.Icons.p32.Cross = new Bitmap(imageData), "Images 'cross.bmp' 32p"),
            };

            int loadedResources = 0;
            foreach (var disk in GlobalData.FileSystem.GetDisks())
            {
                foreach(var part in disk.Partitions)
                {
                    if(!part.HasFileSystem)
                        continue;
                    foreach(var resource in resources)
                    {
                        string resourcePath = part.RootPath + resource.Path;

                        if(File.Exists(resourcePath))
                        {
                            try
                            {
                                Logger.DoBootLog($"[OK] Found '{resource.Path}'");
                                Logger.DoBootLog($"[Info] Loading resource");
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.DOING, $"Resource '{resource.Description}'");

                                byte[] rawData = File.ReadAllBytes(resourcePath);
                                resource.LoadCallback?.Invoke(rawData);

                                loadedResources++;
                                ShellUtils.MoveCursorUp();
                                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.OK, $"Resource '{resource.Description}'");
                                Logger.DoBootLog($"[OK] '{resource.Path}' loaded");
                            }
                            catch (ArgumentException ex)
                            {
                                HandleResourceLoadingError(ex, resource.Description, "Invalid file");
                            }
                            catch (Exception ex)
                            {
                                HandleResourceLoadingError(ex, resource.Path, ex.Message);
                            }
                        }
                    }
                    if (loadedResources >= resources.Count)
                        break;
                }
                if (loadedResources >= resources.Count)
                    break;
            }

            if(loadedResources < resources.Count)
            {
                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "Resources not found!");
                _ = new KernelPanic("Kernel load failed: Cannot find resource file", instance);
            }
        }

        private static void HandleResourceLoadingError(Exception ex, string resourceName, string error)
        {
            Logger.DoBootLog($"[Error] Cannot load resource '{resourceName}' -> {ex.Message}");
            ShellUtils.MoveCursorUp();
            ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, $"Resource '{resourceName}': {error}");
            _ = new KernelPanic("Kernel load failed: Invalid resource file", instance);
        }
    }
}
