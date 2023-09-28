using System;

namespace WinttOS.Core.Utils.Debugging
{
    public struct MethodCallInfo
    {
        public readonly string MethodFullPath;
        public readonly string MethodSignature;
        public readonly string FileName;
        public readonly uint FileLineNumber;

        public MethodCallInfo(string methodFullPath, string methodSignature, string fileName, uint fileLineNumber)
        {
            MethodFullPath = methodFullPath ?? throw new ArgumentNullException(nameof(methodFullPath));
            MethodSignature = methodSignature ?? throw new ArgumentNullException(nameof(methodSignature));
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            FileLineNumber = fileLineNumber;
        }

        public static bool operator ==(MethodCallInfo left, MethodCallInfo right) =>
            left.MethodFullPath.Equals(right.MethodFullPath) &&
            left.MethodSignature.Equals(right.MethodSignature) &&
            left.FileName.Equals(right.FileName) &&
            left.FileLineNumber.Equals(right.FileLineNumber);

        public static bool operator !=(MethodCallInfo left, MethodCallInfo right) =>
            !(left == right);
    }
}
