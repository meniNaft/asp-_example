using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace asp__example.Models
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }

        [Required, Display(Name = "שם פרטי")]
        public string FirstName { get; set; }
        
        [Required, Display(Name ="שם משפחה")]
        public string LastName { get; set; }

        [NotMapped, Display(Name = "שם מלא")]
        public string FullName { get => FirstName + " " + LastName;  }

        [Required, Display(Name = "דואר אלקטרוני")]
        [EmailAddress(ErrorMessage = "כתובת לא תקינה")]
        public string EmailAddress { get; set; }

        [Required, Display(Name = "טלפון"), Phone(ErrorMessage ="טלפון לא תקין")]
        public string Phone { get; set; }

        [AllowNull, Display(Name ="תמונות")]
        public List<Image>Images { get; set; }

        [NotMapped, Display(Name = "הוספת תמונה")]
        public IFormFile? setImg
        {
            get => null;
            set
            {
                if (value == null) return;
                AddImage(value);
            }
        }


        [NotMapped, Display(Name = "הוספת תמונות")]
        public IFormFile[]? setImges
        {
            get => null;
            set
            {
                if (value == null) return;          
                foreach (var item in value) AddImage(item);
            }
        }


        public void AddImage(byte[] img)
        {
            if(Images == null)
            {
                Images = new List<Image>();   
            }
            Images.Add(new Image { Img = img, Friend = this });
        }

        public void AddImage(IFormFile img)
        {
            if(img != null)
            {
                MemoryStream memory = new MemoryStream();
                img.CopyTo(memory);
                AddImage(memory.ToArray());
            }
        }
    }
}
