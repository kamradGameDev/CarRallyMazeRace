// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("cgJhcb2VHxN4n9BY3sXtGQJ/CkuebcCbxg/SeJ6x4YByokuOq5aeE93OVqGRWVdKQdcM0nEaIfoMpGp/fsxPbH5DSEdkyAbIuUNPT09LTk2gr1rkBf/pHqWGUnlCrZ+9v0nR2xw9IYNclNGelareEMXcMw86KoNWc3capzQGrswz3Gpsc1MKYmoGAIBK9/A98/IJhqAcy5n7FbPt3xG4VyxFSfYHEDnrkPeH1f77hXHyplmB2DcuBNSN3KMq6qqFbOs671hQgvN3MSuieY3UXhePLegarsywjQ2/XJQXwD2cFKx984lyg16gOS4lavlpzE9BTn7MT0RMzE9PTstOSJnjLY0NSWfbjlk9aF2ymITKVKeUafYJXML6Ive9T2AGKUxNT05P");
        private static int[] order = new int[] { 8,9,2,8,7,7,12,8,12,12,13,13,12,13,14 };
        private static int key = 78;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
