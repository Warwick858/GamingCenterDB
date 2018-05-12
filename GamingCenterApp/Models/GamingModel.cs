// ******************************************************************************************************************
//  Copyright(C) 2018  James LoForti
//  Contact Info: jamesloforti@gmail.com
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.If not, see<https://www.gnu.org/licenses/>.
//									     ____.           .____             _____  _______   
//									    |    |           |    |    ____   /  |  | \   _  \  
//									    |    |   ______  |    |   /  _ \ /   |  |_/  /_\  \ 
//									/\__|    |  /_____/  |    |__(  <_> )    ^   /\  \_/   \
//									\________|           |_______ \____/\____   |  \_____  /
//									                             \/          |__|        \/ 
//
// ******************************************************************************************************************
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamingCenterApp.Models
{
    public class GamingModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,30}$", ErrorMessage = "First name can only contain 1 to 30 letters, hyphens, or spaces.")]
        [Required(ErrorMessage = "First name is required. ")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,30}$", ErrorMessage = "Last name can only contain 1 to 30 letters, hyphens, or spaces.")]
        [Required(ErrorMessage = "Last name is required. ")]
        public string LastName { get; set; }

        [Display(Name = "User ID")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "User ID length must be between 1 and 30 characters.")]
        [Required(ErrorMessage = "User ID is required. ")]
        public string UserID { get; set; }

        [Display(Name = "Game Type")]
        [Required(ErrorMessage = "Game Type is required. ")]
        public string GameType { get; set; }

        public List<KeyValuePair<string, string>> GameTypeOptions { get; set; }

        [Display(Name = "Item Type")]
        [Required(ErrorMessage = "Item Type is required. ")]
        public string ItemType { get; set; }

        public List<KeyValuePair<string, string>> ItemTypeOptions { get; set; }

        [Display(Name = "UVU Student?")]
        public bool IsUVUStudent { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    } // end class GamingModel
} // end namespace GamingCenterApp.Models