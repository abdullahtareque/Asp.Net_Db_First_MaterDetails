using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvidenceTest.Models.ViewModels
{
    public class CustOrderVM
    {
        public int CustomerID { get; set; }
        
        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "Please enter the customer name.")]
        public string CutomerName { get; set; }

        [Display(Name = "Billing Address")]
        [Required(ErrorMessage = "Please enter the billing address.")]
        public string BillingAddress { get; set; }
        [Display(Name = "Image")]
        [Required(ErrorMessage = "Please select an image.")]
        public string Imagepath { get; set; }


        public int ItemID { get; set; }
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        public int OrderID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}", ApplyFormatInEditMode = true)]
        public DateTime orderDate { get; set; }
        public int OrderNo { get; set; }
        
        [Display(Name = "Order Status")]
        public Nullable<bool> OrderStatus { get; set; }

        public List<Order> Orders { get; set; }
        public Customer Customer { get; set; }

    }
}