﻿#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion


using System;

namespace Inventory.Models
{
    public class MenuFolderModel : BaseModel
    {
        static public MenuFolderModel CreateEmpty() => new MenuFolderModel { Id = -1, SequenceNumber = 0, IsEmpty = true };

        public static int CATEGORY_ALL = 1;

        public Guid RowGuid { get; set; }

        public Guid? ParentGuid { get; set; }

        public int SequenceNumber { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }
        public byte[] Thumbnail { get; set; }
    }
}


