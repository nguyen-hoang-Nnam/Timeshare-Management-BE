﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TimeshareManagement.Models.Models
{
    public class Timeshare
    {
        [Key]
        public int timeshareId { get; set; }
        [Required(ErrorMessage = "Timeshare name is required")]
        public string? timeshareName { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive value")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }
        public string? Detail {  get; set; }
        /*public DateTime ExpirationDate { get; set; }*/
        public DateTime dateTo { get; set; }
        public DateTime dateFrom { get; set; }
        public int? timeshareStatusId { get; set; }
        [ForeignKey("timeshareStatusId")]
        [JsonIgnore]
        public TimeshareStatus? TimeshareStatus { get; set; }
        public int? placeId { get; set; }
        [ForeignKey("placeId")]
        [JsonIgnore]
        public Place? Place { get; set; }
        public string? Id { get; set; }
        [ForeignKey("Id")]
        [JsonIgnore]
        public ApplicationUser? User { get; set; }


        public int CalculatePrice(DateTime startDate, DateTime endDate)
        {
            TimeSpan duration = endDate - startDate;
            int numberOfDays = (int)duration.TotalDays;
            int totalPrice = Price * numberOfDays;

            return totalPrice;
        }
    }
}
