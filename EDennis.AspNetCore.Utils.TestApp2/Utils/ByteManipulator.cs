using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Utils.TestApp2.Utils {

    /// <summary>
    /// This class performs a trivial manipulation to
    /// a byte array in order to produce its byte
    /// complement -- for each byte in the array,
    /// calculate 255 minus the byte value.  This is
    /// used for testing purposes only.
    /// </summary>
    public static class ByteManipulator {

        public static byte[] FlipBytes(byte[] bytes) {
            var flipped = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                flipped[i] = (byte)(255 - (int)bytes[i]);
            return flipped;
        }
    }
}
