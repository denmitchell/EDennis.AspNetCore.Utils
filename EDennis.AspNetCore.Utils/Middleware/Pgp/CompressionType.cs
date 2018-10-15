using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Utils.Middleware.Pgp {
    public enum CompressionType {
        None = 0,
        Zip = 1,
        ZLib = 2,
        BZip2 = 3,
        GZip = 4
    }

    static class CompressionTypeExtensions {
        public static CompressionType Parse(this CompressionType compressionType,
                string compressionTypeString) {
            var lower = compressionTypeString.ToLower();

            switch (lower) {
                case "bzip2": return CompressionType.BZip2;
                case "gzip": return CompressionType.GZip;
                case "zip": return CompressionType.Zip;
                case "zlib": return CompressionType.ZLib;
                case "none": return CompressionType.None;
                default: throw new ArgumentOutOfRangeException($"compression type ({compressionTypeString}) cannot map to CompresionType enum, which is one of ZLib, Zip, BZip2, or GZip");
            }
        }
    }

}
