
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using DWall.VCard;
using DWall.VCard.Tests;

namespace Tests.Samples
{

    /* ===================================================================
     * OutlookTests
     * -------------------------------------------------------------------
     * Tests for Outlook-generated vCards.
     * =================================================================== */

    [TestFixture]
    public class OutlookTests
    {

        /// <summary>
        ///     The issuing organization of the certificate embedded
        ///     into the Outlook vCard.
        /// </summary>
        public const string KeyIssuer =
            "CN=Thawte Personal Freemail Issuing CA, O=Thawte Consulting (Pty) Ltd., C=ZA";

        
        /// <summary>
        ///     The subject of the certificate.
        /// </summary>
        public const string KeySubject =
            "E=dave@thoughtproject.com, CN=Thawte Freemail Member";



        #region [ ParseOutlookCertificate ]

        [Test]
        public void ParseOutlookCertificate()
        {

            // 01 BEGIN:VCARD
            // 02 VERSION:2.1
            // 03 N:Pinch;David;John
            // 04 FN:David John Pinch
            // 05 NICKNAME:Dave
            // 06 ORG:Thought Project
            // 07 TITLE:Dictator
            // 08 TEL;WORK;VOICE:800-929-5805
            // 09 TEL;HOME;VOICE:612-269-6017
            // 10 KEY;X509;ENCODING=BASE64:
            // 11    MIICZDCCAc2gAwIBAgIQRMnJkySZCu8S15WhdyuljjANBgkqhkiG9w0BAQUFADBiMQswCQYD
            // 12   VQQGEwJaQTElMCMGA1UEChMcVGhhd3RlIENvbnN1bHRpbmcgKFB0eSkgTHRkLjEsMCoGA1UE
            // 13    AxMjVGhhd3RlIFBlcnNvbmFsIEZyZWVtYWlsIElzc3VpbmcgQ0EwHhcNMDcwNTExMTU0NTI2
            // 14    WhcNMDgwNTEwMTU0NTI2WjBJMR8wHQYDVQQDExZUaGF3dGUgRnJlZW1haWwgTWVtYmVyMSYw
            // 15    JAYJKoZIhvcNAQkBFhdkYXZlQHRob3VnaHRwcm9qZWN0LmNvbTCBnzANBgkqhkiG9w0BAQEF
            // 16    AAOBjQAwgYkCgYEAmLhq0UDsgB8paOBzXCtv9SbwccPYJhJr6f7bK3JO1xkCbKmzoLhCZUVB
            // 17    4zP5bWTBnQYpolA9t1Pbrd29flX90xizEljCuL2uOz4cFc+NoF7h0h5nFvnFVAmzsJmLnCop
            // 18    vp0GD4jy3cpQhBNAoSqQbwjSuqyFtHeVMIbNlU3Y/Y8CAwEAAaM0MDIwIgYDVR0RBBswGYEX
            // 19    ZGF2ZUB0aG91Z2h0cHJvamVjdC5jb20wDAYDVR0TAQH/BAIwADANBgkqhkiG9w0BAQUFAAOB
            // 20    gQCuMF4mUI5NWv0DqblQH/pqJN0eCjj2j4iQhJNTHtfhrS0ETbakgldJCzg5Rv+8V2Dil7gs
            // 21    4zMmwOuDrHVBqWDvF0/hXzMn5KKWEmzCZshVyFJ24IWkIj4t3wOMG21NdSA+zX7TEc3s7oWh
            // 22    zi6q4lcDj3pOzUyDmaEEBYcyWLKXpA==
            // 23
            // 24
            // 25 EMAIL;PREF;INTERNET:dave@thoughtproject.com
            // 26 REV:20070511T163814Z
            // 27 END:VCARD

            vCard card = new vCard(
                new StreamReader(new MemoryStream(SampleCards.OutlookCertificate)));

            // 03 N:Pinch;David;John

            Assert.AreEqual(
                "Pinch",
                card.FamilyName,
                "N at line 3 has a different family name.");

            Assert.AreEqual(
                "David",
                card.GivenName,
                "N at line 3 has a different given name.");

            Assert.AreEqual(
                "John",
                card.AdditionalNames,
                "N at line 3 has a different middle name.");

            Assert.AreEqual(
                1,
                card.Certificates.Count,
                "Only one certificate was expected.");

            // 04 FN:David John Pinch

            Assert.AreEqual(
                "David John Pinch",
                card.FormattedName,
                "FN at line 4 has a different formatted name.");

            // 05 NICKNAME:Dave

            Assert.AreEqual(
                1,
                card.Nicknames.Count,
                "Exactly one nickname is located at line 5.");

            Assert.AreEqual(
                "Dave",
                card.Nicknames[0],
                "NICKNAME at line 5 has a different value.");

            // 06 ORG:Thought Project

            Assert.AreEqual(
                "Thought Project",
                card.Organization,
                "ORG at line 6 has a different value.");

            // 07 TITLE:Dictator

            Assert.AreEqual(
                "Dictator",
                card.Title,
                "TITLE at line 7 has a different value.");

            // 08 TEL;WORK;VOICE:800-929-5805
            // 09 TEL;HOME;VOICE:612-269-6017

            Assert.AreEqual(
                2,
                card.Phones.Count,
                "Two telephone numbers are defined at lines 8 and 9.");

            Assert.IsTrue(
                card.Phones[0].IsWork,
                "TEL at line 8 is a work number.");

            Assert.IsTrue(
                card.Phones[0].IsVoice,
                "TEL at line 8 is a voice number.");

            Assert.AreEqual(
                "800-929-5805",
                card.Phones[0].FullNumber,
                "TEL at line 8 has a different value.");

            // 09 TEL;HOME;VOICE:612-269-6017

            Assert.IsTrue(
                card.Phones[1].IsHome,
                "TEL at line 9 is a home number.");

            Assert.IsTrue(
                card.Phones[1].IsVoice,
                "TEL at line 9 is a voice number.");

            Assert.AreEqual(
                "612-269-6017",
                card.Phones[1].FullNumber,
                "TEL at line 9 has a different value.");

            // 10 KEY;X509;ENCODING=BASE64:

            Assert.AreEqual(
                1,
                card.Certificates.Count,
                "There is one certificate starting on line 10.");

            Assert.AreEqual(
                "X509",
                card.Certificates[0].KeyType,
                "KEY on line 10 has a different key type.");

            // Create an instance of the certificate.

            X509Certificate2 cert =
                new X509Certificate2(card.Certificates[0].Data);

            Assert.AreEqual(
                KeyIssuer,
                cert.Issuer,
                "The key issuer has a different value.");

            Assert.AreEqual(
                KeySubject,
                cert.Subject,
                "The key subject has a different value.");


        }

        #endregion


    }
}
