﻿using Jitbit.Utils;
using LibCardApp.Models;
using LibCardApp.ViewModels;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using LibCardApp.Controllers.Api;
using Microsoft.Owin.Security.Provider;
using System.Reflection;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.Text;

namespace LibCardApp.Controllers
{
    public class PatronController : Controller
    {
        #region _context declaration
        private ApplicationDbContext _context;

        public PatronController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        #endregion

        /// Index()
        /// If logged in it will return a list that can be edited, if not, it will be Read Only
        public ViewResult Index()
        {
            if (User.IsInRole(RoleName.CanManagePatrons))
                return View("List");
            else
                return View("ReadOnlyList");
        }

        /// New()
        /// Creates a new View Model for patron information to be loaded into
        public ActionResult New()
        {
            var viewModel = new PatronViewModel
            {
                Patron = new Patron(),
            };

            return View("New", viewModel);
        }

        /// Save(Patron patron)
        /// Takes the patron that's currently in the View Model and saves it to the _context.
        /// Returns the ReturnToLibrarian view because the patrons will have the iPad and we don't want
        /// them seeing other patron's information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Patron patron)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new PatronViewModel
                {
                    Patron = patron
                };

                return View("New", viewModel);
            }

            if (patron.Id == 0)
                _context.Patrons.Add(patron);
            else
            {
                var patronInDb = _context.Patrons.Single(p => p.Id == patron.Id);
                    patronInDb.Id = patron.Id;
                    patronInDb.Name = patron.Name;
                    patronInDb.Address = patron.Address;
                    patronInDb.City = patron.City;
                    patronInDb.State = patron.State;
                    patronInDb.Zip = patron.Zip;
                    patronInDb.Email = patron.Email;
                    patronInDb.Phone = patron.Phone;
                    patronInDb.PType = patron.PType;
                    patronInDb.Barcode = patron.Barcode;
                    patronInDb.Signature = patron.Signature;
                    patronInDb.DateSubmitted = patron.DateSubmitted;
            }

            _context.SaveChanges();

            return View("ReturnToLibrarian");
        }

        /// Edit(int id)
        /// This function calls the patron from the _context using the id and pulls the patron
        /// information into the view model. 
        public ActionResult Edit(int id)
        {
            var patron = _context.Patrons.SingleOrDefault(c => c.Id == id);

            if (patron == null)
                return HttpNotFound();

            var viewModel = new PatronViewModel
            {
                Patron = patron,
            };

            return View("Edit", viewModel);
        }


        /// BarcodeEntry(string passedBarcode)
        /// Reads the barcode that was passed in the textbox and adds it to the URL for the Sierra API.
        /// It then auto populates the respective fields in the New and Edit Views
        public ActionResult BarcodeEntry(string passedBarcode)
        {
            var patron = new Patron();

            string result = null;
            string url = "http://www.search.livebrary.com:4500/PATRONAPI/20641001772034/dump";
            WebResponse response = null;
            StreamReader reader = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            response = request.GetResponse();
            reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            result = reader.ReadToEnd();

            int startIndex, endIndex;
            string phone, email, barcode, pType, name, fullAddress;

            //Splits the Sierra API into lines so we can use the <br> tags at the end of each line to end the substrings.
            String[] lines = result.Split('\n');
            foreach (String line in lines)
            {
                //Name
                if (line.Contains("[pn]"))
                {
                    startIndex = line.IndexOf("[pn]=") + 5;
                    endIndex = line.IndexOf("<BR>");

                    name = line.Substring(startIndex, endIndex - startIndex);
                    patron.Name = name;

                }

                //Address
                //Example Address: 100 Broadway Ave$Coram, NY 11727
                //Example Address:
                if (line.Contains("[pa]"))
                {
                    int zip;
                    string streetAddress, city, state;
                    int addressEndIndex, cityEndIndex, zipStartIndex, stateStartIndex;

                    startIndex = line.IndexOf("[pa]=") + 5;
                    endIndex = line.IndexOf("<BR>");

                    fullAddress = line.Substring(startIndex, endIndex - startIndex);

                    addressEndIndex = fullAddress.IndexOf("$");
                    cityEndIndex = fullAddress.IndexOf(",") - 1;

                    streetAddress = fullAddress.Substring(0, addressEndIndex);
                    city = fullAddress.Substring(addressEndIndex + 1, cityEndIndex - addressEndIndex);


                    string fullAddressNoWhiteSpace = fullAddress.Replace(" ", String.Empty);
                    int fullAddressEndIndex = fullAddressNoWhiteSpace.Length;

                    zipStartIndex = fullAddressEndIndex - 5;
                    stateStartIndex = zipStartIndex - 2;

                    zip = Int32.Parse(fullAddressNoWhiteSpace.Substring(zipStartIndex, 5));
                    state = fullAddressNoWhiteSpace.Substring(stateStartIndex, 2);


                    patron.Address = streetAddress;
                    patron.City = city;
                    patron.State = state;
                    patron.Zip = zip;
                }

                //Phone
                if (line.Contains("[pt]"))
                {
                    startIndex = line.IndexOf("[pt]=") + 5;
                    endIndex = line.IndexOf("<BR>");

                    phone = line.Substring(startIndex, endIndex - startIndex);
                    patron.Phone = phone;
                }

                //Email
                if (line.Contains("[pz]"))
                {
                    startIndex = line.IndexOf("[pz]=") + 5;
                    endIndex = line.IndexOf("<BR>");

                    email = line.Substring(startIndex, endIndex - startIndex);
                    patron.Email = email;
                }

                //Barcode
                if (line.Contains("[pb]"))
                {
                    startIndex = line.IndexOf("[pb]=") + 5;
                    endIndex = line.IndexOf("<BR>");

                    barcode = line.Substring(startIndex, endIndex - startIndex);
                    patron.Barcode = barcode;
                }

                //PType
                if (line.Contains("[p47]"))
                {
                    startIndex = line.IndexOf("[p47]=") + 6;
                    endIndex = line.IndexOf("<BR>");

                    pType = "p" + line.Substring(startIndex, endIndex - startIndex);
                    patron.PType = pType;
                }

            }

            if (patron.Email == null)
                patron.Email = "No Email Provided";

            if (patron.Phone == null)
                patron.Phone = "No Phone Provided";

            var viewModel = new PatronViewModel
            {
                Patron = patron,
            };

            return View("New", viewModel);
        }

        #region PDF
        ///ActionResult: PdfGenerator
        ///Use: Takes the id from the List View and pull the respective Patron information and print it onto the pdf template
        ///After it's done it returns the pdf that it's created so it's available to print.
        public ActionResult PdfGenerator(int id)
        {
            var patron = _context.Patrons.SingleOrDefault(c => c.Id == id);

            if (patron == null)
                return HttpNotFound();

            var viewModel = new PatronViewModel
            {
                Patron = patron,
            };

            #region PDF Document Declarations

            string todaysDate = DateTime.Now.ToShortDateString();
            string stringZip = patron.Zip.ToString();

            PdfDocument document = new PdfDocument();
            document.Info.Title = patron.Name;
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            //Get the EZFontResolver.
            EZFontResolver fontResolver = EZFontResolver.Get;
            // Assign it to PDFsharp.
            GlobalFontSettings.FontResolver = fontResolver;

            fontResolver.AddFont("Arial", XFontStyle.Regular, Server.MapPath("~/fonts/ARIAL.TTF"), true, true);
            fontResolver.AddFont("Arial Bold", XFontStyle.Bold, Server.MapPath("~/fonts/ARIALBD.TTF"), true, true);
            fontResolver.AddFont("Garamond", XFontStyle.Regular, Server.MapPath("~/fonts/GARA.TTF"), true, true);
            fontResolver.AddFont("Wingdings", XFontStyle.Regular, Server.MapPath("~/fonts/WINGDING.TTF"), true, true);
            fontResolver.AddFont("Wingdings 2", XFontStyle.Regular, Server.MapPath("~/fonts/WINGDNG2.TTF"), true, true);

            XFont font8Arial = new XFont("Arial", 8);
            XFont font12ArialBold = new XFont("Arial Bold", 12, XFontStyle.Bold);
            XFont font12Garamond = new XFont("Garamond", 12);
            XFont wingdings = new XFont("Wingdings", 12);
            XFont wingdings2 = new XFont("Wingdings 2", 12);


            XPen blackPen = new XPen(XColors.Black, 1.5);
            XPen blackPenThin = new XPen(XColors.Black, 0.5);
            XPen grayPenThin = new XPen(XColors.LightGray, 0.3);
            #endregion

            string logoFilepath = System.Web.HttpContext.Current.Server.MapPath("~/Images/LPLLogoPNG.png");
            XImage lplLogo = XImage.FromFile(logoFilepath);
            gfx.DrawImage(lplLogo, 15, 15, 120, 72);

            tf.DrawString("LPL Library Card", font12ArialBold, XBrushes.Black, new XRect(25, 100, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(blackPen, 25, 115, page.Width - 25, 115);

            #region General (Patron) Information
            tf.DrawString("General Information", font12Garamond, XBrushes.Black, new XRect(25, 125, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(blackPenThin, 25, 140, page.Width - 50, 140);

            tf.DrawString("Today's Date", font8Arial, XBrushes.Black, new XRect(50, 145, page.Width, page.Height), XStringFormats.TopLeft);
            tf.DrawString(todaysDate, font8Arial, XBrushes.Black, new XRect((page.Width / 2 + 2), 145, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 155, page.Width / 2, 155);

            tf.DrawString("Name", font8Arial, XBrushes.Black, new XRect(50, 155, page.Width, page.Height), XStringFormats.TopLeft);
            tf.DrawString(patron.Name, font8Arial, XBrushes.Black, new XRect((page.Width / 2 + 2), 155, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 165, page.Width / 2, 165);

            tf.DrawString("Street Address", font8Arial, XBrushes.Black, new XRect(50, 165, page.Width, page.Height), XStringFormats.TopLeft);
            tf.DrawString(patron.Address, font8Arial, XBrushes.Black, new XRect((page.Width / 2 + 2), 165, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 175, page.Width / 2, 175);

            tf.DrawString("City", font8Arial, XBrushes.Black, new XRect(50, 175, page.Width, page.Height), XStringFormats.TopLeft);
            tf.DrawString(patron.City, font8Arial, XBrushes.Black, new XRect((page.Width / 2 + 2), 175, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 185, page.Width / 2, 185);

            tf.DrawString("State", font8Arial, XBrushes.Black, new XRect(50, 185, page.Width, page.Height), XStringFormats.TopLeft);
            tf.DrawString(patron.State, font8Arial, XBrushes.Black, new XRect((page.Width / 2 + 2), 185, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 195, page.Width / 2, 195);

            tf.DrawString("Zip Code", font8Arial, XBrushes.Black, new XRect(50, 195, page.Width, page.Height), XStringFormats.TopLeft);
            tf.DrawString(stringZip, font8Arial, XBrushes.Black, new XRect((page.Width / 2 + 2), 195, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 205, page.Width / 2, 205);

            tf.DrawString("Email Address", font8Arial, XBrushes.Black, new XRect(50, 205, page.Width, page.Height), XStringFormats.TopLeft);
            tf.DrawString(patron.Email, font8Arial, XBrushes.Black, new XRect((page.Width / 2 + 2), 205, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 215, page.Width / 2, 215);

            tf.DrawString("Phone", font8Arial, XBrushes.Black, new XRect(50, 215, page.Width, page.Height), XStringFormats.TopLeft);
            tf.DrawString(patron.Phone, font8Arial, XBrushes.Black, new XRect((page.Width / 2 + 2), 215, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 225, page.Width / 2, 225);

            tf.DrawString("Your e-mail address will only be used by the library for overdue and other notices", font8Arial, XBrushes.Black, new XRect(50, 230, page.Width, page.Height), XStringFormats.TopLeft);
            #endregion

            #region Age Range
            gfx.DrawString("Age", font12Garamond, XBrushes.Black, new XRect(25, 250, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(blackPenThin, 25, 265, page.Width - 50, 265);

            gfx.DrawString("Adult", font8Arial, XBrushes.Black, new XRect(50, 270, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 280, page.Width / 2, 280);

            gfx.DrawString("Young Adult (Ages 12 - 16)", font8Arial, XBrushes.Black, new XRect(50, 280, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 290, page.Width / 2, 290);

            gfx.DrawString("Child (Birth - Age 11)", font8Arial, XBrushes.Black, new XRect(50, 290, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 290, page.Width / 2, 290);

            switch (patron.PType)
            {
                case "p73":
                case "p75":
                case "p222":
                    gfx.DrawString("R", wingdings2, XBrushes.Black, new XRect((page.Width / 2 + 2), 270, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("o", wingdings, XBrushes.Black, new XRect((page.Width / 2 + 2), 280, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("o", wingdings, XBrushes.Black, new XRect((page.Width / 2 + 2), 290, page.Width, page.Height), XStringFormats.TopLeft);
                    break;

                case "p62":
                case "p65":
                    gfx.DrawString("o", wingdings, XBrushes.Black, new XRect((page.Width / 2 + 2), 270, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("R", wingdings2, XBrushes.Black, new XRect((page.Width / 2 + 2), 280, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("o", wingdings, XBrushes.Black, new XRect((page.Width / 2 + 2), 290, page.Width, page.Height), XStringFormats.TopLeft);
                    break;
                case "p74":
                case "p76":
                    gfx.DrawString("o", wingdings, XBrushes.Black, new XRect((page.Width / 2 + 2), 270, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("o", wingdings, XBrushes.Black, new XRect((page.Width / 2 + 2), 280, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("R", wingdings2, XBrushes.Black, new XRect((page.Width / 2 + 2), 290, page.Width, page.Height), XStringFormats.TopLeft);
                    break;
                default:
                    gfx.DrawString("o", wingdings, XBrushes.Black, new XRect((page.Width / 2 + 2), 270, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("o", wingdings, XBrushes.Black, new XRect((page.Width / 2 + 2), 280, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString("o", wingdings, XBrushes.Black, new XRect((page.Width / 2 + 2), 290, page.Width, page.Height), XStringFormats.TopLeft);
                    break;

            }

            const string signatureText =
                "I am a resident of the Longwood Central School District.I agree to follow all library rules, to promptly \n" +
                "pay all charges for overdue, lost and damaged materials, and to give immediate notice of any change of \n" +
                "address or loss of library card. I understand that I am responsible for all materials checked out on this card.";


            byte[] photoBack = patron.Signature;
            using (MemoryStream ms = new MemoryStream(photoBack))
            {
                var image = RenderImage(patron.Id);
                XImage signatureImage = XImage.FromStream(ms);
                gfx.DrawImage(signatureImage, page.Width / 2 + 5, 365, 50, 25);
            }

            tf.DrawString(signatureText, font8Arial, XBrushes.Black, new XRect(50, 330, page.Width, page.Height), XStringFormats.TopLeft);

            tf.DrawString("Signature", font8Arial, XBrushes.Black, new XRect(50, 375, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 385, page.Width / 2, 385);

            tf.DrawString("Date", font8Arial, XBrushes.Black, new XRect(50, 395, page.Width, page.Height), XStringFormats.TopLeft);
            tf.DrawString(todaysDate, font8Arial, XBrushes.Black, new XRect((page.Width / 2 + 2), 395, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 405, page.Width / 2, 405);

            tf.DrawString("Staff Use Only", font8Arial, XBrushes.Black, new XRect(50, 425, page.Width, page.Height), XStringFormats.TopLeft);

            tf.DrawString("Barcode", font8Arial, XBrushes.Black, new XRect(50, 435, page.Width, page.Height), XStringFormats.TopLeft);
            tf.DrawString(patron.Barcode, font8Arial, XBrushes.Black,
                new XRect((page.Width / 2 + 2), 435, page.Width, page.Height), XStringFormats.TopLeft);
            gfx.DrawLine(grayPenThin, 50, 445, page.Width / 2, 445);

            #endregion

            //Naming Convention: The Patrons Last Name, then the last 4 digits of their Barcode. Ex: Casali2034.pdf
            string filename = patron.Name.Substring(0, patron.Name.IndexOf(",")) + patron.Barcode.Substring(patron.Barcode.Length - 4) + ".pdf";
            string fileSavePath = Server.MapPath("~/");

            document.Save(fileSavePath + filename);

            //return File(fileSavePath + filename, filename);
            return File(fileSavePath + filename, "application/pdf");

        }


        /// Render Image(int id)
        /// This function is called in the PDF Generator Action. It's used to pull the signature
        /// byte array and return it as a file to the memory stream and print out the image onto the PDF.
        public ActionResult RenderImage(int id)
        {
            Patron patron = _context.Patrons.SingleOrDefault(c => c.Id == id);

            byte[] photoBack = patron.Signature;

            return File(photoBack, "image/png");
        }
        #endregion

        #region EmailExport
        /// EmailExportIndex()
        /// This function returns the Email Export View when clicked on either from the Nav Bar
        public ViewResult EmailExportIndex()
        {
             return View("EmailIndex");
        }


        /// ExportCSV()
        /// This function uses the CsvExport class to export all of the emails in the patron list
        public ActionResult ExportCSV()
        {
            var emailExport = new CsvExport();

            foreach (var patron in _context.Patrons)
            {
                if (patron.Email != "No Email Provided")
                {
                    emailExport.AddRow();
                    emailExport["Email"] = patron.Email;
                }
            }
            return File(emailExport.ExportToBytes(), "text/csv", "PatronEmails.csv");
        }
        #endregion
    }

}