
using System;
using System.Drawing;
using System.IO;
using NUnit.Framework;
using DWall.VCard;

namespace Tests
{
    [TestFixture]
    public class vCardPhotoTests
    {

        /// <summary>
        ///     The URL of an image that is under control of the author
        ///     and is sufficiently small to allow quick download.  If you
        ///     use the vCard library internally w/ extensive unit tests,
        ///     please be considerate and change to an image on your
        ///     local network.  This will save bandwidth costs for the author.
        /// </summary>
        private const string TestPhotoUrl =
            "http://www.thoughtproject.com/Common/Download.gif";

        /// <summary>
        ///     The height of the test image in pixels.
        /// </summary>
        private const int TestPhotoHeight = 16;

        /// <summary>
        ///     The size (in bytes) of the test image.
        /// </summary>
        private const int TestPhotoSize = 579;

        /// <summary>
        ///     The width of the test photo in pixels.
        /// </summary>
        private const int TestPhotoWidth = 16;

        // 

        #region [ Constructor_String ]

        [Test]
        public void Constructor_String()
        {

            // If a filename (string) is passed to the constructor, then
            // the scheme of the URI should be set as file.

            vCardPhoto photo = new vCardPhoto("c:\\fake-picture.gif");

            Assert.IsTrue(
                photo.Url.IsFile);

        }

        #endregion

        #region [ Constructor_String_Empty ]

        [Test]
        public void Constructor_String_Empty()
        {
            Assert.Throws(typeof(ArgumentNullException), () => new vCardPhoto(string.Empty));
        }

        #endregion

        #region [ Constructor_String_Null ]

        [Test]
        public void Constructor_String_Null()
        {
            Assert.Throws(typeof(ArgumentNullException), () => new vCardPhoto((string)null));
        }

        #endregion


    }
}
