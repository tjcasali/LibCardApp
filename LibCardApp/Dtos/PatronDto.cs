using LibCardApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Org.BouncyCastle.Utilities.Encoders;

namespace LibCardApp.Dtos
{
    public class PatronDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public int Zip { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string PType { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        [UIHint("SignaturePad")]
        public byte[] Signature { get; set; }
        [Required]
        public string DateSubmitted { get; set; }
    }
}