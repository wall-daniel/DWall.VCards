
/* =======================================================================
 * vCard Library for .NET
 * Copyright (c) 2007-2009 David Pinch; http://wwww.thoughtproject.com
 * See LICENSE.TXT for licensing information.
 * ======================================================================= */

using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace DWall.VCard
{

    /// <summary>
    ///     A photo embedded in a vCard.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         You must specify the photo using a path, a byte array,
    ///         or a System.Drawing.Bitmap instance. The class will
    ///         extract the underlying raw bytes for storage into the
    ///         vCard.  You can call <see cref="GetBytes"/> to extract the raw
    ///         bytes (e.g. for transmission from a web page).
    ///     </para>
    /// </remarks>
    [Serializable]
    public class vCardPhoto
    {

        /// <summary>
        ///     The raw bytes of the image data.
        /// </summary>
        /// <remarks>
        ///     The raw bytes can be passed directly to the photo object
        ///     or fetched from a file or remote URL.  A .NET bitmap object
        ///     can also be specified, in which case the constructor
        ///     will load the raw bytes from the bitmap.
        /// </remarks>
        private byte[] data;


        /// <summary>
        ///     The url of the image.
        /// </summary>
        private Uri url;


        /// <summary>
        ///     Loads a photograph from an array of bytes.
        /// </summary>
        /// <param name="buffer">
        ///     An array of bytes containing the raw data from
        ///     any of the supported image formats.
        /// </param>
        public vCardPhoto(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            this.data = (byte[])buffer.Clone();
        }


        /// <summary>
        ///     The URL of the image.
        /// </summary>
        /// <param name="url">
        ///     A URL pointing to an image.
        /// </param>
        public vCardPhoto(Uri url)
        {

            if (url == null)
                throw new ArgumentNullException("url");

            this.url = url;
        }


        /// <summary>
        ///     Creates a new vCard photo from an image file.
        /// </summary>
        /// <param name="path">
        ///     The path to an image of any supported format.
        /// </param>
        public vCardPhoto(string path)
        {

            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            this.url = new Uri(path);

        }


        /// <summary>
        ///     Fetches a linked image asynchronously.
        /// </summary>
        /// <remarks>
        ///     This is a simple utility method for accessing the image
        ///     referenced by the URL.  For asynchronous or advanced
        ///     loading you will need to download the image yourself
        ///     and load the bytes directly into the class.
        /// </remarks>
        /// <seealso cref="IsLoaded"/>
        /// <seealso cref="Url"/>
        public void Fetch()
        {

            // An image can be fetched only if the URL
            // of the image is known.  Otherwise the
            // fetch operation makes no sense.

            if (this.url == null)
                throw new InvalidOperationException();

            // Create a web request object that will handle the
            // specifics of downloading a file from the specified
            // URL.  For example, the URL is a file-based URL, then
            // the CreateDefault method will return a FileWebRequest
            // class.

            WebRequest request =
                WebRequest.CreateDefault(this.url);

            // Start the request.  The request begins when
            // the GetResponse method is invoked.  This is a
            // synchronous (blocking) call (i.e. it will not
            // return until the file is downloaded or an 
            // exception is raised).

            WebResponse response = request.GetResponse();

            using (Stream responseStream = response.GetResponseStream())
            {

                // Allocate space to hold the entire image.

                this.data = new byte[response.ContentLength];

                // The following call will fail if the image
                // size is larger than the capacity of an Int32.
                // This may be treated as a minor issue given
                // the fact that this is a vCard library and
                // such images are expected by humans to be small.
                // No reasonable person would embed a multi-gigabyte
                // image into a vCard.

                responseStream.Read(
                    this.data,
                    0,
                    (int)response.ContentLength);

            }

        }


        /// <summary>
        ///     Returns a copy of the raw bytes of the image.
        /// </summary>
        /// <returns>
        ///     A byte array containing the raw bytes of the image.
        /// </returns>
        /// <remarks>
        ///     A copy of the raw bytes are returned.  Modifying the
        ///     array will not modify the photo.
        /// </remarks>
        public byte[] GetBytes()
        {
            return (byte[])this.data.Clone();
        }


        /// <summary>
        ///     Indicates the bytes of the raw image have
        ///     been loaded by the object.
        /// </summary>
        /// <seealso cref="Fetch"/>
        public bool IsLoaded
        {
            get
            {
                return this.data != null;
            }
        }


        /// <summary>
        ///     The URL of the image.
        /// </summary>
        /// <remarks>
        ///     Changing the URL will automatically invalidate the internal
        ///     image data if previously fetched.
        /// </remarks>
        /// <seealso cref="Fetch"/>
        public Uri Url
        {
            get
            {

                return this.url;
            }
            set
            {

                // This class maintains a byte array containing the raw
                // bytes of the image.  The use can call the Fetch method
                // to load the raw bytes from a remote link.  If the
                // URL is changed (e.g. via this property), then the local
                // cache must be invalidated.

                if (value == null)
                {
                    this.data = null;
                    this.url = null;
                }
                else
                {
                    if (this.url != value)
                    {
                        this.data = null;
                        this.url = value;
                    }
                }
            }
        }

    }

}