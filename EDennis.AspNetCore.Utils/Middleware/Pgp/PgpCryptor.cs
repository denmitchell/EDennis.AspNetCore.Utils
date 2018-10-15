using Org.BouncyCastle.Bcpg;
using PgpCore;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace EDennis.AspNetCore.Utils.Middleware.Pgp {


    /// <summary>
    /// This class provides methods to compress (gzip) and
    /// encrypt files
    /// </summary>
    public static class PgpCryptor {

        /// <summary>
        /// Encrypts a stream using OpenPGP standards.  Only
        /// the organization holding the corresponding private key
        /// can decrypt the data.
        /// </summary>
        /// <param name="inStream">input stream to encrypt</param>
        /// <param name="outStream">encrypted output stream</param>
        /// <param name="publicKeyName">The name of the public key stored in a database</param>
        /// <param name="compressionType">none,gzip,zlib,bzip2,zip</param>
        public static void Encrypt(Stream inStream, Stream outStream,
                byte[] publicKey,
                CompressionType compressionType = CompressionType.None,
                bool armor = false) {


            var compressionAlgorithmTag = ParseCompressionAlgorithm(compressionType);


            using (var inputStream = new MemoryStream()) {
                if (compressionType == CompressionType.GZip) {
                    Gzip(inStream, inputStream);
                } else {
                    inStream.CopyTo(inputStream);
                }

                inputStream.Seek(0, SeekOrigin.Begin);
                using (var pgp = new PGP()) {
                    using (var publicKeyStream = new MemoryStream(publicKey)) {
                        pgp.CompressionAlgorithm = compressionAlgorithmTag;
                        pgp.EncryptStream(inputStream, outStream, publicKeyStream, armor, true);
                    }
                }
            }
        }


        /// <summary>
        /// Decrypts a stream using OpenPGP standards.  Only
        /// the organization holding the corresponding private key
        /// can decrypt the data.
        /// </summary>
        /// <param name="inStream">input stream to decrypt</param>
        /// <param name="outStream">decrypted output stream</param>
        /// <param name="privateKey">The name of the private key stored in a database</param>
        /// <param name="privateKeyName"></param>
        /// <param name="compressionType">none,gzip,zlib,bzip2,zip</param>
        public static void Decrypt(Stream inStream, Stream outStream,
                byte[] privateKey, string passphrase,
                CompressionType compressionType = CompressionType.None,
                bool armor = false) {

            var compressionAlgorithmTag = ParseCompressionAlgorithm(compressionType);

            using (var pgp = new PGP()) {
                using (var outputStream = new MemoryStream()) {
                    using (var privateKeyStream = new MemoryStream(privateKey)) {
                        pgp.CompressionAlgorithm = compressionAlgorithmTag;
                        pgp.DecryptStream(inStream, outputStream, privateKeyStream, passphrase);
                    }

                    outputStream.Seek(0, SeekOrigin.Begin);

                    if (compressionType == CompressionType.GZip) {
                        UnGzip(outputStream, outStream);
                    } else {
                        outputStream.CopyTo(outStream);
                    }
                }
            }
        }


        private static void Gzip(Stream inStream, Stream outStream) {
            using (var gzipStream = new GZipStream(outStream, CompressionMode.Compress, leaveOpen: true)) {
                inStream.CopyTo(gzipStream);
            }
        }


        private static void UnGzip(Stream inStream, Stream outStream) {
            using (var gzipStream = new GZipStream(inStream, CompressionMode.Decompress, leaveOpen: true)) {
                gzipStream.CopyTo(outStream);
            }
        }

        private static CompressionAlgorithmTag ParseCompressionAlgorithm(CompressionType compressionAlgorithm) {
            CompressionAlgorithmTag compressionAlgorithmTag;

            switch (compressionAlgorithm) {
                case CompressionType.None:
                case CompressionType.GZip:
                    compressionAlgorithmTag = CompressionAlgorithmTag.Uncompressed;
                    break;
                case CompressionType.BZip2:
                    compressionAlgorithmTag = CompressionAlgorithmTag.BZip2;
                    break;
                case CompressionType.Zip:
                    compressionAlgorithmTag = CompressionAlgorithmTag.Zip;
                    break;
                case CompressionType.ZLib:
                    compressionAlgorithmTag = CompressionAlgorithmTag.ZLib;
                    break;
                default:
                    throw new ArgumentException($"{compressionAlgorithm} is not a valid compression algorithm tag");
            }
            return compressionAlgorithmTag;
        }
    }
}